#!/usr/bin/env python3
"""Extrae contexto de login desde assets legacy de GodsWar."""
from __future__ import annotations

import json
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
GAME_DIR = ROOT / "Eterna Guerra Online"
TEXT_DIR = GAME_DIR / "Localization" / "en_us" / "Text"


def parse_tab_dat(path: Path) -> dict[str, str]:
    data: dict[str, str] = {}
    text = path.read_text(encoding="utf-8", errors="replace").replace("\ufeff", "")
    for raw_line in text.splitlines():
        line = raw_line.strip()
        if not line or line.startswith("//"):
            continue
        if "\t" not in line:
            continue
        key, value = line.split("\t", 1)
        data[key.strip()] = value.strip()
    return data


def main() -> None:
    login_info = parse_tab_dat(TEXT_DIR / "LoginInfo.dat")

    payload = {
        "source": {
            "config": str((GAME_DIR / "config.ini").relative_to(ROOT)),
            "login_info": str((TEXT_DIR / "LoginInfo.dat").relative_to(ROOT)),
        },
        "server_defaults": {
            "ip": "67.208.220.189",
            "port": 5999,
            "region": 11,
            "locale": "en_us",
        },
        "copy_blocks": {
            "billboard": login_info.get("BillboardInfo", ""),
            "camp_athens": login_info.get("CampInfo1", ""),
            "camp_sparta": login_info.get("CampInfo2", ""),
            "class_warrior": login_info.get("MetierInfo1", ""),
            "class_fighter": login_info.get("MetierInfo2", ""),
            "class_priest": login_info.get("MetierInfo3", ""),
            "class_mage": login_info.get("MetierInfo4", ""),
        },
    }

    out_file = ROOT / "docs" / "login_context_en_us.json"
    out_file.write_text(json.dumps(payload, ensure_ascii=False, indent=2), encoding="utf-8")
    print(f"Escrito: {out_file.relative_to(ROOT)}")


if __name__ == "__main__":
    main()
