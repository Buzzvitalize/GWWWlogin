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
    first_recv_hex: dict[str, str] = {}

    for line in LOG.read_text(encoding='utf-8', errors='replace').splitlines():
        parts = line.split()
        if len(parts) < 3:
            continue
        event = parts[2]
        counts[event] += 1
        if len(parts) >= 4:
            client = parts[3]
            by_client[client][event] += 1
            if event == 'RECV' and client not in first_recv_hex:
                # guardamos firma de primer paquete (primeros 16 bytes)
                marker = 'hex='
                idx = line.find(marker)
                if idx != -1:
                    hex_data = line[idx + len(marker):].strip()
                    first_recv_hex[client] = ' '.join(hex_data.split()[:16])

    print('Global events:')
    for k, v in counts.most_common():
        print(f'  {k}: {v}')

    print('\nPer client summary:')
    for client, c in by_client.items():
        sig = first_recv_hex.get(client, '-')
        print(
            f'  {client}: CONNECT={c["CONNECT"]} HELLO={c["SEND_HELLO"]} '
            f'RECV={c["RECV"]} SEND={c["SEND"]} DISCONNECT={c["DISCONNECT"]} first16={sig}'
        )

    if counts['RECV'] > 0 and counts['DISCONNECT'] >= counts['RECV']:
        print('\nHint: if clients RECV once then disconnect, use:')
        print('  EMULATOR_MODE=hybrid')
        print('  EMULATOR_BINARY_REPLY_MODE=ack')
        print('  EMULATOR_BINARY_REPLY_HEX=47 57 68 7C')


if __name__ == '__main__':
    main()
