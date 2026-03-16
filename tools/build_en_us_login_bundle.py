#!/usr/bin/env python3
from __future__ import annotations

import json
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
TEXT_DIR = ROOT / "Eterna Guerra Online" / "Localization" / "en_us" / "Text"
OUT_FILE = ROOT / "Server Files Private Eterna Guerra Online" / "api" / "assets" / "en_us_login_bundle.json"


def parse_tab_file(path: Path, encoding: str) -> dict[str, str]:
    raw = path.read_text(encoding=encoding, errors="replace")
    raw = raw.replace("\ufeff", "")
    data: dict[str, str] = {}
    for line in raw.splitlines():
        line = line.strip("\x00 \t\r\n")
        if not line or "\t" not in line:
            continue
        key, value = line.split("\t", 1)
        key = key.replace("\x00", "").strip()
        value = value.replace("\x00", "").strip()
        if key:
            data[key] = value
    return data


def main() -> None:
    login_info = parse_tab_file(TEXT_DIR / "LoginInfo.dat", "utf-8")
    error_info = parse_tab_file(TEXT_DIR / "ErrorMessage.dat", "utf-16")

    bundle = {
        "locale": "en_us",
        "source": {
            "login_info": str((TEXT_DIR / "LoginInfo.dat").relative_to(ROOT)),
            "error_message": str((TEXT_DIR / "ErrorMessage.dat").relative_to(ROOT)),
        },
        "login_copy": {
            "billboard": login_info.get("BillboardInfo", ""),
            "camp_athens": login_info.get("CampInfo1", ""),
            "camp_sparta": login_info.get("CampInfo2", ""),
            "class_warrior": login_info.get("MetierInfo1", ""),
            "class_fighter": login_info.get("MetierInfo2", ""),
            "class_priest": login_info.get("MetierInfo3", ""),
            "class_mage": login_info.get("MetierInfo4", ""),
        },
        "error_messages": error_info,
    }

    OUT_FILE.parent.mkdir(parents=True, exist_ok=True)
    OUT_FILE.write_text(json.dumps(bundle, ensure_ascii=False, indent=2), encoding="utf-8")
    print(f"Escrito: {OUT_FILE.relative_to(ROOT)} ({len(error_info)} errors)")


if __name__ == "__main__":
    main()
