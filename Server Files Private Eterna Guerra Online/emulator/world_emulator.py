#!/usr/bin/env python3
from __future__ import annotations

import os
import socket
import socketserver
import threading
from datetime import datetime, timezone
from pathlib import Path

ROOT_DIR = Path(__file__).resolve().parents[1]


def load_env_file(path: Path) -> None:
    if not path.exists():
        return
    for raw in path.read_text(encoding="utf-8", errors="ignore").splitlines():
        line = raw.strip()
        if not line or line.startswith("#") or "=" not in line:
            continue
        key, value = line.split("=", 1)
        key = key.strip()
        value = value.strip()
        os.environ.setdefault(key, value)


# Carga configuración compartida con API (si existe).
load_env_file(ROOT_DIR / "api" / ".env")
load_env_file(ROOT_DIR / "api" / ".env.example")
WORLD_HOST = os.getenv("WORLD_IP", "127.0.0.1")
WORLD_PORT = int(os.getenv("WORLD_PORT", "5999"))
EMULATOR_PORTS = os.getenv("EMULATOR_PORTS", f"{WORLD_PORT},6000,29000")

EMULATOR_MODE = os.getenv("EMULATOR_MODE", "hybrid")  # hybrid | echo | text
BINARY_REPLY_MODE = os.getenv("EMULATOR_BINARY_REPLY_MODE", "ack")  # ack | echo
BINARY_REPLY_HEX = os.getenv("EMULATOR_BINARY_REPLY_HEX", "47 57 68 7C")
LOG_PACKETS = os.getenv("EMULATOR_LOG_PACKETS", "1") == "1"

log_raw = os.getenv("EMULATOR_LOG_FILE", str(ROOT_DIR / "logs" / "world_emulator_packets.log"))
LOG_FILE = Path(log_raw)
if not LOG_FILE.is_absolute():
    LOG_FILE = ROOT_DIR / LOG_FILE

SEND_HELLO_ON_CONNECT = os.getenv("EMULATOR_SEND_HELLO_ON_CONNECT", "1") == "1"
HELLO_TEXT = os.getenv("EMULATOR_HELLO_TEXT", "SYSTEM_ONLINE\\n")
HELLO_SEQUENCE_HEX = os.getenv("EMULATOR_HELLO_SEQUENCE_HEX", "47 57 68 7C")  # 'GWh|'


def parse_ports(raw: str) -> list[int]:
    ports: list[int] = []
    for p in raw.split(","):
        p = p.strip()
        if not p:
            continue
        try:
            n = int(p)
        except ValueError:
            continue
        if 1 <= n <= 65535 and n not in ports:
            ports.append(n)
    return ports or [WORLD_PORT]


def now_utc() -> str:
    return datetime.now(timezone.utc).isoformat()


def append_log(line: str) -> None:
    if not LOG_PACKETS:
        return
    LOG_FILE.parent.mkdir(parents=True, exist_ok=True)
    with LOG_FILE.open("a", encoding="utf-8") as fh:
        fh.write(line + "\n")


def parse_hex_sequence(raw: str) -> list[bytes]:
    parts = [p.strip() for p in raw.split(",") if p.strip()]
    out: list[bytes] = []
    for part in parts:
        hex_str = part.replace(" ", "")
        if not hex_str:
            continue
        try:
            out.append(bytes.fromhex(hex_str))
        except ValueError:
            append_log(f"[{now_utc()}] WARN invalid hex sequence: {part}")
    return out


def binary_reply_payload() -> bytes:
    payloads = parse_hex_sequence(BINARY_REPLY_HEX)
    if not payloads:
        return b""
    return payloads[0]


def send_initial_hello(sock: socket.socket, client: str, listen_port: int) -> None:
    if not SEND_HELLO_ON_CONNECT:
        return

    if EMULATOR_MODE == "text":
        payloads = [HELLO_TEXT.encode("utf-8", errors="ignore")]
    elif EMULATOR_MODE == "echo":
        payloads = parse_hex_sequence(HELLO_SEQUENCE_HEX)
    else:
        payloads = parse_hex_sequence(HELLO_SEQUENCE_HEX)

    for payload in payloads:
        if not payload:
            continue
        sock.sendall(payload)
        append_log(
            f"[{now_utc()}] SEND_HELLO port={listen_port} {client} "
            f"len={len(payload)} hex={payload.hex(' ')}"
        )


class WorldHandler(socketserver.BaseRequestHandler):
    def handle(self) -> None:
        client_ip, client_port = self.client_address
        listen_port = self.server.server_address[1]
        client = f"{client_ip}:{client_port}"
        start = now_utc()
        print(f"[{start}] Cliente conectado: {client} -> local:{listen_port}")
        append_log(f"[{start}] CONNECT port={listen_port} {client}")

        self.request.settimeout(10.0)

        try:
            send_initial_hello(self.request, client, listen_port)
        except OSError:
            append_log(f"[{now_utc()}] HELLO_FAIL port={listen_port} {client}")
            return

        while True:
            try:
                data = self.request.recv(4096)
                if not data:
                    end = now_utc()
                    append_log(f"[{end}] DISCONNECT port={listen_port} {client}")
                    return

                stamp = now_utc()
                append_log(
                    f"[{stamp}] RECV port={listen_port} {client} len={len(data)} "
                    f"hex={data.hex(' ')}"
                )

                if EMULATOR_MODE == "echo":
                    reply = data
                elif EMULATOR_MODE == "text":
                    reply = HELLO_TEXT.encode("utf-8", errors="ignore")
                else:
                    try:
                        text = data.decode("utf-8")
                        if all((31 < ord(ch) < 127) or ch in "\r\n\t" for ch in text):
                            reply = HELLO_TEXT.encode("utf-8", errors="ignore")
                        else:
                            reply = data if BINARY_REPLY_MODE == "echo" else binary_reply_payload()
                    except UnicodeDecodeError:
                        reply = data if BINARY_REPLY_MODE == "echo" else binary_reply_payload()

                if reply:
                    self.request.sendall(reply)
                    append_log(f"[{stamp}] SEND port={listen_port} {client} len={len(reply)}")
                else:
                    append_log(f"[{stamp}] SEND_SKIP_EMPTY port={listen_port} {client}")
            except socket.timeout:
                try:
                    self.request.sendall(b"\x00")
                    append_log(f"[{now_utc()}] KEEPALIVE port={listen_port} {client}")
                except OSError:
                    append_log(f"[{now_utc()}] KEEPALIVE_FAIL port={listen_port} {client}")
                    return
            except ConnectionError:
                append_log(f"[{now_utc()}] CONNECTION_ERROR port={listen_port} {client}")
                return


class ThreadedWorldServer(socketserver.ThreadingTCPServer):
    allow_reuse_address = True


def serve_port(host: str, port: int) -> None:
    with ThreadedWorldServer((host, port), WorldHandler) as server:
        print(f"World Emulator ONLINE on {host}:{port}")
        server.serve_forever()


def main() -> None:
    ports = parse_ports(EMULATOR_PORTS)
    print(
        f"Estado: Sistema Online | host={WORLD_HOST} | ports={ports} | mode={EMULATOR_MODE} | "
        f"hello={SEND_HELLO_ON_CONNECT} | binary_reply={BINARY_REPLY_MODE} | log={LOG_FILE}"
    )

    threads: list[threading.Thread] = []
    for p in ports:
        t = threading.Thread(target=serve_port, args=(WORLD_HOST, p), daemon=True)
        t.start()
        threads.append(t)

    for t in threads:
        t.join()


if __name__ == "__main__":
    main()
