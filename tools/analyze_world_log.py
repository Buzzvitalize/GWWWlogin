#!/usr/bin/env python3
from __future__ import annotations

from collections import Counter, defaultdict
from pathlib import Path

LOG = Path('logs/world_emulator_packets.log')


def main() -> None:
    if not LOG.exists():
        print('No log file found:', LOG)
        return

    counts = Counter()
    by_client = defaultdict(Counter)

    for line in LOG.read_text(encoding='utf-8', errors='replace').splitlines():
        parts = line.split()
        if len(parts) < 3:
            continue
        event = parts[2]
        counts[event] += 1
        if len(parts) >= 4:
            client = parts[3]
            by_client[client][event] += 1

    print('Global events:')
    for k, v in counts.most_common():
        print(f'  {k}: {v}')

    print('\nPer client summary:')
    for client, c in by_client.items():
        print(f'  {client}: CONNECT={c["CONNECT"]} RECV={c["RECV"]} SEND={c["SEND"]} DISCONNECT={c["DISCONNECT"]} HELLO={c["SEND_HELLO"]}')


if __name__ == '__main__':
    main()
