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
        os.environ.setdefault(key.strip(), value.strip())


load_env_file(ROOT_DIR / "api" / ".env")
load_env_file(ROOT_DIR / "api" / ".env.example")

WORLD_HOST = os.getenv("WORLD_IP", "127.0.0.1")
WORLD_PORT = int(os.getenv("WORLD_PORT", "5999"))
EMULATOR_PORTS = os.getenv("EMULATOR_PORTS", f"{WORLD_PORT},6000,29000")

EMULATOR_MODE = os.getenv("EMULATOR_MODE", "hybrid")  # hybrid | echo | text
BINARY_REPLY_MODE = os.getenv("EMULATOR_BINARY_REPLY_MODE", "ack")  # ack | echo
BINARY_REPLY_HEX = os.getenv("EMULATOR_BINARY_REPLY_HEX", "47 57 68 7C")

ENABLE_UDP = os.getenv("EMULATOR_ENABLE_UDP", "1") == "1"
UDP_REPLY_HEX = os.getenv("EMULATOR_UDP_REPLY_HEX", "47 57 68 7C")

LOG_PACKETS = os.getenv("EMULATOR_LOG_PACKETS", "1") == "1"
log_raw = os.getenv("EMULATOR_LOG_FILE", "logs/world_emulator_packets.log")


def resolve_log_path(raw: str) -> Path:
    p = Path(raw)
    if p.is_absolute():
        return p

    # Si viene con prefijo redundante "Server Files Private Eterna Guerra Online/..."
    # lo recortamos para evitar duplicar ROOT_DIR en el join final.
    if p.parts and p.parts[0] == ROOT_DIR.name:
        p = Path(*p.parts[1:])

    return ROOT_DIR / p


LOG_FILE = resolve_log_path(log_raw)

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


def pick_payload(raw: str) -> bytes:
    payloads = parse_hex_sequence(raw)
    return payloads[0] if payloads else b""


def send_initial_hello(sock: socket.socket, client: str, listen_port: int) -> None:
    if not SEND_HELLO_ON_CONNECT:
        return

    if EMULATOR_MODE == "text":
        payloads = [HELLO_TEXT.encode("utf-8", errors="ignore")]
    else:
        payloads = parse_hex_sequence(HELLO_SEQUENCE_HEX)

    for payload in payloads:
        if payload:
            sock.sendall(payload)
            append_log(f"[{now_utc()}] SEND_HELLO port={listen_port} {client} len={len(payload)} hex={payload.hex(' ')}")


class WorldHandler(socketserver.BaseRequestHandler):
    def handle(self) -> None:
        client_ip, client_port = self.client_address
        listen_port = self.server.server_address[1]
        client = f"{client_ip}:{client_port}"
        append_log(f"[{now_utc()}] CONNECT port={listen_port} {client}")
        print(f"[{now_utc()}] Cliente conectado: {client} -> local:{listen_port}")

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
                    append_log(f"[{now_utc()}] DISCONNECT port={listen_port} {client}")
                    return

                append_log(f"[{now_utc()}] RECV port={listen_port} {client} len={len(data)} hex={data.hex(' ')}")

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
                            reply = data if BINARY_REPLY_MODE == "echo" else pick_payload(BINARY_REPLY_HEX)
                    except UnicodeDecodeError:
                        reply = data if BINARY_REPLY_MODE == "echo" else pick_payload(BINARY_REPLY_HEX)

                if reply:
                    self.request.sendall(reply)
                    append_log(f"[{now_utc()}] SEND port={listen_port} {client} len={len(reply)}")
                else:
                    append_log(f"[{now_utc()}] SEND_SKIP_EMPTY port={listen_port} {client}")
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


def serve_tcp(host: str, port: int) -> None:
    with ThreadedWorldServer((host, port), WorldHandler) as server:
        print(f"World Emulator TCP ONLINE on {host}:{port}")
        server.serve_forever()


def serve_udp(host: str, port: int) -> None:
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    sock.bind((host, port))
    print(f"World Emulator UDP ONLINE on {host}:{port}")
    ack = pick_payload(UDP_REPLY_HEX)

    while True:
        data, addr = sock.recvfrom(4096)
        client = f"{addr[0]}:{addr[1]}"
        append_log(f"[{now_utc()}] UDP_RECV port={port} {client} len={len(data)} hex={data.hex(' ')}")
        if ack:
            sock.sendto(ack, addr)
            append_log(f"[{now_utc()}] UDP_SEND port={port} {client} len={len(ack)} hex={ack.hex(' ')}")


def main() -> None:
    ports = parse_ports(EMULATOR_PORTS)
    print(
        f"Estado: Sistema Online | host={WORLD_HOST} | ports={ports} | mode={EMULATOR_MODE} | "
        f"udp={ENABLE_UDP} | hello={SEND_HELLO_ON_CONNECT} | binary_reply={BINARY_REPLY_MODE} | log={LOG_FILE}"
    )

    threads: list[threading.Thread] = []
    for p in ports:
        t = threading.Thread(target=serve_tcp, args=(WORLD_HOST, p), daemon=True)
        t.start()
        threads.append(t)

        if ENABLE_UDP:
            u = threading.Thread(target=serve_udp, args=(WORLD_HOST, p), daemon=True)
            u.start()
            threads.append(u)

    for t in threads:
        t.join()


if __name__ == "__main__":
    main()
