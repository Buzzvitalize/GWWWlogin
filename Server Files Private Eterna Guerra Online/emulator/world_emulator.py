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


def now_utc() -> str:
    return datetime.now(timezone.utc).isoformat()


def append_log(line: str) -> None:
    if not LOG_PACKETS:
        return
    LOG_FILE.parent.mkdir(parents=True, exist_ok=True)
    with LOG_FILE.open("a", encoding="utf-8") as fh:
        fh.write(line + "\n")


class WorldHandler(socketserver.BaseRequestHandler):
    def handle(self) -> None:
        client_ip, client_port = self.client_address
        start = now_utc()
        print(f"[{start}] Cliente conectado: {client_ip}:{client_port}")
        append_log(f"[{start}] CONNECT {client_ip}:{client_port}")

        self.request.settimeout(20.0)

        while True:
            try:
                data = self.request.recv(4096)
                if not data:
                    end = now_utc()
                    append_log(f"[{end}] DISCONNECT {client_ip}:{client_port}")
                    return

                stamp = now_utc()
                hex_payload = data.hex(" ")
                append_log(f"[{stamp}] RECV {client_ip}:{client_port} len={len(data)} hex={hex_payload}")

                if EMULATOR_MODE == "echo":
                    reply = data
                elif EMULATOR_MODE == "text":
                    reply = b"SYSTEM_ONLINE\n"
                else:
                    # hybrid: si parece texto, responde texto; si no, eco binario
                    try:
                        text = data.decode("utf-8")
                        if all((31 < ord(ch) < 127) or ch in "\r\n\t" for ch in text):
                            reply = b"SYSTEM_ONLINE\n"
                        else:
                            reply = data
                    except UnicodeDecodeError:
                        reply = data

                self.request.sendall(reply)
                append_log(f"[{stamp}] SEND {client_ip}:{client_port} len={len(reply)}")
            except socket.timeout:
                # mantener conexión viva sin cerrarla agresivamente
                keep = b"\x00"
                try:
                    self.request.sendall(keep)
                    append_log(f"[{now_utc()}] KEEPALIVE {client_ip}:{client_port}")
                except OSError:
                    append_log(f"[{now_utc()}] KEEPALIVE_FAIL {client_ip}:{client_port}")
                    return
            except ConnectionError:
                append_log(f"[{now_utc()}] CONNECTION_ERROR {client_ip}:{client_port}")
                return


class ThreadedWorldServer(socketserver.ThreadingTCPServer):
    allow_reuse_address = True


def main() -> None:
    with ThreadedWorldServer((WORLD_HOST, WORLD_PORT), WorldHandler) as server:
        print(f"World Emulator ONLINE on {WORLD_HOST}:{WORLD_PORT}")
        print(f"Estado: Sistema Online | mode={EMULATOR_MODE} | log={LOG_FILE}")
        server.serve_forever()


if __name__ == "__main__":
    main()
