#!/usr/bin/env python3
from __future__ import annotations

from pathlib import Path

REPO_ROOT = Path(__file__).resolve().parents[1]
SERVER_ROOT = REPO_ROOT / "Server Files Private Eterna Guerra Online"
CLIENT_CONFIG = REPO_ROOT / "Eterna Guerra Online" / "config.ini"


def load_env_like(path: Path) -> dict[str, str]:
    data: dict[str, str] = {}
    if not path.exists():
        return data
    for raw in path.read_text(encoding="utf-8", errors="ignore").splitlines():
        line = raw.strip()
        if not line or line.startswith("#") or "=" not in line:
            continue
        key, value = line.split("=", 1)
        data[key.strip()] = value.strip()
    return data


def pick_world_target() -> tuple[str, str]:
    env = load_env_like(SERVER_ROOT / "api" / ".env.example")
    env.update(load_env_like(SERVER_ROOT / "api" / ".env"))
    return env.get("WORLD_IP", "127.0.0.1"), env.get("WORLD_PORT", "5999")


def sync_client_ini(ip: str, port: str) -> None:
    lines = CLIENT_CONFIG.read_text(encoding="utf-8", errors="ignore").splitlines()

    in_server = False
    saw_ip = False
    saw_port = False
    out: list[str] = []

    for line in lines:
        stripped = line.strip()

        if stripped.startswith("[") and stripped.endswith("]"):
            if in_server:
                if not saw_port:
                    out.append(f"PORT={port}")
                if not saw_ip:
                    out.append(f"IP={ip}")
            in_server = stripped.lower() == "[server]"
            out.append(line)
            continue

        if in_server and "=" in line:
            key, _ = line.split("=", 1)
            key_clean = key.strip().upper()
            if key_clean == "PORT":
                out.append(f"PORT={port}")
                saw_port = True
                continue
            if key_clean == "IP":
                out.append(f"IP={ip}")
                saw_ip = True
                continue

        out.append(line)

    if in_server:
        if not saw_port:
            out.append(f"PORT={port}")
        if not saw_ip:
            out.append(f"IP={ip}")

    CLIENT_CONFIG.write_text("\n".join(out) + "\n", encoding="utf-8")


if __name__ == "__main__":
    world_ip, world_port = pick_world_target()
    sync_client_ini(world_ip, world_port)
    print(f"SYNC_OK client_config={CLIENT_CONFIG} ip={world_ip} port={world_port}")
