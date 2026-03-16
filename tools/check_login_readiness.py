#!/usr/bin/env python3
from __future__ import annotations

from dataclasses import dataclass
from pathlib import Path
import configparser


REPO_ROOT = Path(__file__).resolve().parents[1]
SERVER_ROOT = REPO_ROOT / "Server Files Private Eterna Guerra Online"
CLIENT_ROOT = REPO_ROOT / "Eterna Guerra Online"


@dataclass
class CheckResult:
    name: str
    ok: bool
    detail: str


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


def collect_env() -> dict[str, str]:
    env = load_env_like(SERVER_ROOT / "api" / ".env.example")
    env.update(load_env_like(SERVER_ROOT / "api" / ".env"))
    return env


def check_client_config(env: dict[str, str]) -> CheckResult:
    cfg_path = CLIENT_ROOT / "config.ini"
    parser = configparser.ConfigParser()
    parser.read(cfg_path, encoding="utf-8")
    world_ip = env.get("WORLD_IP", "127.0.0.1")
    world_port = env.get("WORLD_PORT", "5999")

    ip = parser.get("SERVER", "IP", fallback="")
    port = parser.get("SERVER", "PORT", fallback="")

    ok = (ip == world_ip and port == world_port)
    detail = f"client={ip}:{port} env_login={world_ip}:{world_port}"
    return CheckResult("Client config.ini aligned with login endpoint", ok, detail)


def check_required_files() -> list[CheckResult]:
    files = [
        CLIENT_ROOT / "GodsWar.exe",
        CLIENT_ROOT / "Net.dll",
        CLIENT_ROOT / "Localization" / "en_us" / "UI" / "XML" / "LoginUI.xml",
        SERVER_ROOT / "emulator" / "world_emulator.py",
        SERVER_ROOT / "api" / "app.py",
    ]
    out: list[CheckResult] = []
    for p in files:
        out.append(CheckResult(f"Exists: {p.relative_to(REPO_ROOT)}", p.exists(), ""))
    return out


def check_env_values(env: dict[str, str]) -> list[CheckResult]:
    mode = env.get("EMULATOR_BINARY_REPLY_MODE", "")
    ports = env.get("EMULATOR_PORTS", "")
    game_port = env.get("GAME_SERVER_PORT", "7000")

    has_game_port = game_port in {x.strip() for x in ports.split(",") if x.strip()}

    return [
        CheckResult("EMULATOR_BINARY_REPLY_MODE set", bool(mode), f"mode={mode or '<empty>'}"),
        CheckResult("EMULATOR_BINARY_REPLY_MODE uses mirror_first", mode == "mirror_first", f"mode={mode or '<empty>'}"),
        CheckResult("EMULATOR_PORTS includes GAME_SERVER_PORT", has_game_port, f"ports={ports} game_port={game_port}"),
    ]


def protocol_notice() -> CheckResult:
    detail = (
        "Legacy client still needs exact proprietary socket packet flow for server-list/realm-list. "
        "Current emulator is generic and may still disconnect after first packets until opcode-level replies are replicated."
    )
    return CheckResult("Protocol completeness", False, detail)


def main() -> int:
    env = collect_env()
    results: list[CheckResult] = []
    results.extend(check_required_files())
    results.append(check_client_config(env))
    results.extend(check_env_values(env))
    results.append(protocol_notice())

    failed = 0
    for r in results:
        status = "OK" if r.ok else "MISSING/BLOCKER"
        print(f"[{status}] {r.name}")
        if r.detail:
            print(f"  - {r.detail}")
        if not r.ok:
            failed += 1

    print(f"\nSUMMARY: {len(results)-failed}/{len(results)} checks OK")
    return 1 if failed else 0


if __name__ == "__main__":
    raise SystemExit(main())
