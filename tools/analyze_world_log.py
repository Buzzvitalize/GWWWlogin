#!/usr/bin/env python3
from __future__ import annotations

from collections import Counter, defaultdict
from pathlib import Path
import re

LOG = Path('Server Files Private Eterna Guerra Online/logs/world_emulator_packets.log')
if not LOG.exists():
    LOG = Path('logs/world_emulator_packets.log')

EVENTS = {
    'CONNECT', 'SEND_HELLO', 'RECV', 'SEND', 'DISCONNECT', 'KEEPALIVE',
    'HELLO_FAIL', 'CONNECTION_ERROR', 'KEEPALIVE_FAIL', 'SEND_SKIP_EMPTY'
}


def main() -> None:
    if not LOG.exists():
        print('No log file found:', LOG)
        return

    counts = Counter()
    by_client = defaultdict(Counter)
    first_recv_hex: dict[str, str] = {}

    for line in LOG.read_text(encoding='utf-8', errors='replace').splitlines():
        parts = line.split()
        if len(parts) < 2:
            continue
        event = parts[1]
        if event not in EVENTS:
            continue
        counts[event] += 1

        client = None
        for token in reversed(parts):
            if ':' in token and token.count('.') >= 1 and token.replace(':', '').replace('.', '').isdigit() is False:
                # fallback skip weird tokens
                pass
            if token.count(':') == 1 and token.replace('.', '').replace(':', '').isdigit():
                client = token
                break
        if client:
            by_client[client][event] += 1
            if event == 'RECV' and client not in first_recv_hex:
                idx = line.find('hex=')
                if idx != -1:
                    hex_data = line[idx + 4:].strip()
                    first_recv_hex[client] = ' '.join(hex_data.split()[:16])

    print('Log path:', LOG)
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
        print('  EMULATOR_PORTS=5999,6000,29000')


if __name__ == '__main__':
    main()
