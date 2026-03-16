#!/usr/bin/env python3
from __future__ import annotations

import hashlib
import json
from pathlib import Path

REPO_ROOT = Path(__file__).resolve().parents[1]
CLIENT_ROOT = REPO_ROOT / "Eterna Guerra Online"
SERVER_ROOT = REPO_ROOT / "Server Files Private Eterna Guerra Online"

MAP_ROOT = CLIENT_ROOT / "Map"
MONSTER_ROOT = CLIENT_ROOT / "Localization" / "en_us" / "Monster"
OUT_DIR = SERVER_ROOT / "content"
MAP_MANIFEST = OUT_DIR / "maps_manifest.json"
MONSTER_MANIFEST = OUT_DIR / "monsters_manifest.json"


def sha1_file(path: Path) -> str:
    h = hashlib.sha1()
    with path.open("rb") as fh:
        for chunk in iter(lambda: fh.read(1024 * 1024), b""):
            h.update(chunk)
    return h.hexdigest()


def build_manifest(root: Path) -> dict:
    items = []
    for file in sorted(p for p in root.rglob("*") if p.is_file()):
        rel = file.relative_to(root).as_posix()
        items.append(
            {
                "path": rel,
                "size": file.stat().st_size,
                "sha1": sha1_file(file),
            }
        )
    return {
        "source_root": str(root),
        "file_count": len(items),
        "items": items,
    }


def write_json(path: Path, payload: dict) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(json.dumps(payload, indent=2) + "\n", encoding="utf-8")


def main() -> int:
    if not MAP_ROOT.exists():
        print(f"ERROR: missing map root: {MAP_ROOT}")
        return 1
    if not MONSTER_ROOT.exists():
        print(f"ERROR: missing monster root: {MONSTER_ROOT}")
        return 1

    map_manifest = build_manifest(MAP_ROOT)
    monster_manifest = build_manifest(MONSTER_ROOT)

    write_json(MAP_MANIFEST, map_manifest)
    write_json(MONSTER_MANIFEST, monster_manifest)

    print(f"SYNC_OK maps={map_manifest['file_count']} monsters={monster_manifest['file_count']}")
    print(f"WROTE {MAP_MANIFEST}")
    print(f"WROTE {MONSTER_MANIFEST}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
