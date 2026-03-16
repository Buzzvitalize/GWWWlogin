#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
import re
from pathlib import Path

ROOT = Path('Server Files Private Eterna Guerra Online')
DEFAULT_LOG = ROOT / 'logs' / 'world_emulator_packets.log'
DEFAULT_SCRIPT = ROOT / 'emulator' / 'packet_script.json'


def extract_first_binary_recv(log_path: Path) -> tuple[int, str] | None:
    pattern = re.compile(r"RECV\s+port=.*len=(\d+)\s+hex=(.+)$")
    for line in log_path.read_text(encoding='utf-8', errors='replace').splitlines():
        m = pattern.search(line)
        if not m:
            continue
        hex_data = ' '.join(m.group(2).split())
        if not hex_data:
            continue
        try:
            raw = bytes.fromhex(hex_data.replace(' ', ''))
        except ValueError:
            continue
        if len(raw) >= 16:
            return int(m.group(1)), hex_data
    return None


def main() -> int:
    parser = argparse.ArgumentParser(description='Build/refresh emulator packet_script.json from log first binary packet.')
    parser.add_argument('--log', default=str(DEFAULT_LOG))
    parser.add_argument('--out', default=str(DEFAULT_SCRIPT))
    parser.add_argument('--prefix-bytes', type=int, default=4)
    args = parser.parse_args()

    log_path = Path(args.log)
    out_path = Path(args.out)

    if not log_path.exists():
        print(f'ERROR: log not found: {log_path}')
        return 1

    first = extract_first_binary_recv(log_path)
    if first is None:
        print('ERROR: could not find binary RECV packet in log')
        return 1

    length, hex_data = first
    parts = hex_data.split()
    prefix = ' '.join(parts[: max(1, args.prefix_bytes)])

    script = {
        'description': 'Auto-generated from world_emulator log first binary packet.',
        'fallback_reply_hex': '47 57 68 7C',
        'rules': [
            {
                'name': f'auto_first_recv_len_{length}',
                'length': length,
                'starts_with': prefix,
                'reply': prefix,
            }
        ],
    }

    out_path.parent.mkdir(parents=True, exist_ok=True)
    out_path.write_text(json.dumps(script, indent=2) + '\n', encoding='utf-8')
    print(f'WROTE {out_path} using len={length} prefix={prefix}')
    return 0


if __name__ == '__main__':
    raise SystemExit(main())
