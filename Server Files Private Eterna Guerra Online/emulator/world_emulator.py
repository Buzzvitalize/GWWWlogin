#!/usr/bin/env python3
from __future__ import annotations

import os
import socket
import socketserver
from datetime import datetime, timezone
from pathlib import Path

WORLD_HOST = os.getenv("WORLD_IP", "127.0.0.1")
WORLD_PORT = int(os.getenv("WORLD_PORT", "5999"))
EMULATOR_MODE = os.getenv("EMULATOR_MODE", "hybrid")  # hybrid | echo | text
LOG_PACKETS = os.getenv("EMULATOR_LOG_PACKETS", "1") == "1"
LOG_FILE = Path(os.getenv("EMULATOR_LOG_FILE", "logs/world_emulator_packets.log"))

# Muchos clientes legacy esperan que el servidor hable primero al conectar.
SEND_HELLO_ON_CONNECT = os.getenv("EMULATOR_SEND_HELLO_ON_CONNECT", "1") == "1"
HELLO_TEXT = os.getenv("EMULATOR_HELLO_TEXT", "SYSTEM_ONLINE\\n")
HELLO_SEQUENCE_HEX = os.getenv("EMULATOR_HELLO_SEQUENCE_HEX", "47 57 68 7C")  # 'GWh|'


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


def send_initial_hello(sock: socket.socket, client: str) -> None:
    if not SEND_HELLO_ON_CONNECT:
        return

    if EMULATOR_MODE == "text":
        payloads = [HELLO_TEXT.encode("utf-8", errors="ignore")]
    elif EMULATOR_MODE == "echo":
        # echo puro: no enviamos saludo inicial por defecto
        payloads = parse_hex_sequence(HELLO_SEQUENCE_HEX)
    else:
        # hybrid: intentamos saludo binario + saludo texto
        payloads = parse_hex_sequence(HELLO_SEQUENCE_HEX)
        payloads.append(HELLO_TEXT.encode("utf-8", errors="ignore"))

    for payload in payloads:
        if not payload:
            continue
        sock.sendall(payload)
        append_log(f"[{now_utc()}] SEND_HELLO {client} len={len(payload)} hex={payload.hex(' ')}")


class WorldHandler(socketserver.BaseRequestHandler):
    def handle(self) -> None:
        client_ip, client_port = self.client_address
        client = f"{client_ip}:{client_port}"
        start = now_utc()
        print(f"[{start}] Cliente conectado: {client}")
        append_log(f"[{start}] CONNECT {client}")

        self.request.settimeout(10.0)

        try:
            send_initial_hello(self.request, client)
        except OSError:
            append_log(f"[{now_utc()}] HELLO_FAIL {client}")
            return

        while True:
            try:
                data = self.request.recv(4096)
                if not data:
                    end = now_utc()
                    append_log(f"[{end}] DISCONNECT {client}")
                    return

                stamp = now_utc()
                hex_payload = data.hex(" ")
                append_log(f"[{stamp}] RECV {client} len={len(data)} hex={hex_payload}")

                if EMULATOR_MODE == "echo":
                    reply = data
                elif EMULATOR_MODE == "text":
                    reply = HELLO_TEXT.encode("utf-8", errors="ignore")
                else:
                    # hybrid: si parece texto, responde texto; si no, eco binario
                    try:
                        text = data.decode("utf-8")
                        if all((31 < ord(ch) < 127) or ch in "\r\n\t" for ch in text):
                            reply = HELLO_TEXT.encode("utf-8", errors="ignore")
                        else:
                            reply = data
                    except UnicodeDecodeError:
                        reply = data

                self.request.sendall(reply)
                append_log(f"[{stamp}] SEND {client} len={len(reply)}")
            except socket.timeout:
                keep = b"\x00"
                try:
                    self.request.sendall(keep)
                    append_log(f"[{now_utc()}] KEEPALIVE {client}")
                except OSError:
                    append_log(f"[{now_utc()}] KEEPALIVE_FAIL {client}")
                    return
            except ConnectionError:
                append_log(f"[{now_utc()}] CONNECTION_ERROR {client}")
                return


class ThreadedWorldServer(socketserver.ThreadingTCPServer):
    allow_reuse_address = True


def main() -> None:
    with ThreadedWorldServer((WORLD_HOST, WORLD_PORT), WorldHandler) as server:
        print(f"World Emulator ONLINE on {WORLD_HOST}:{WORLD_PORT}")
        print(
            f"Estado: Sistema Online | mode={EMULATOR_MODE} | "
            f"hello={SEND_HELLO_ON_CONNECT} | log={LOG_FILE}"
        )
        server.serve_forever()


if __name__ == "__main__":
    main()
