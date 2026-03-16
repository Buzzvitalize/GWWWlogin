#!/usr/bin/env python3
from __future__ import annotations

import os
import socketserver
from datetime import datetime, timezone

WORLD_HOST = os.getenv("WORLD_IP", "127.0.0.1")
WORLD_PORT = int(os.getenv("WORLD_PORT", "5999"))


class WorldHandler(socketserver.BaseRequestHandler):
    def handle(self) -> None:
        client_ip, client_port = self.client_address
        now = datetime.now(timezone.utc).isoformat()
        print(f"[{now}] Cliente conectado: {client_ip}:{client_port}")

        try:
            data = self.request.recv(1024)
            incoming = data.decode("utf-8", errors="ignore").strip()
            if incoming:
                print(f"[{now}] Payload: {incoming}")
            self.request.sendall(b"SYSTEM_ONLINE\n")
        except ConnectionError:
            return


class ThreadedWorldServer(socketserver.ThreadingTCPServer):
    allow_reuse_address = True


def main() -> None:
    with ThreadedWorldServer((WORLD_HOST, WORLD_PORT), WorldHandler) as server:
        print(f"World Emulator ONLINE on {WORLD_HOST}:{WORLD_PORT}")
        print("Estado: Sistema Online")
        server.serve_forever()


if __name__ == "__main__":
    main()
