#!/usr/bin/env python3
from __future__ import annotations

from collections import Counter, defaultdict
from pathlib import Path
import re

ROOT = Path('Server Files Private Eterna Guerra Online')
CANDIDATES = [
    ROOT / 'logs' / 'world_emulator_packets.log',
    Path('logs/world_emulator_packets.log'),
]

LOG = CANDIDATES[0]
for c in CANDIDATES:
    if c.exists():
        LOG = c
        break

EVENTS = {
    'CONNECT', 'SEND_HELLO', 'RECV', 'SEND', 'DISCONNECT', 'KEEPALIVE',
    'HELLO_FAIL', 'CONNECTION_ERROR', 'KEEPALIVE_FAIL', 'SEND_SKIP_EMPTY',
    'UDP_RECV', 'UDP_SEND'
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

        m = re.search(r"\b(?:\d{1,3}\.){3}\d{1,3}:\d+\b", line)
        client = m.group(0) if m else None
        if client:
            by_client[client][event] += 1
            if event in {'RECV', 'UDP_RECV'} and client not in first_recv_hex:
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
            f'RECV={c["RECV"]} UDP_RECV={c["UDP_RECV"]} SEND={c["SEND"]} '
            f'UDP_SEND={c["UDP_SEND"]} DISCONNECT={c["DISCONNECT"]} first16={sig}'
        )

    if counts['CONNECT'] == 0 and counts['UDP_RECV'] == 0:
        print('\nHint: client may not be reaching emulator host/ports at all.')
        print('Check config.ini IP/PORT and local firewall rules.')
    elif counts['UDP_RECV'] > 0 and counts['CONNECT'] == 0:
        print('\nHint: client is only pinging via UDP. Keep UDP enabled and tune TCP target port list.')


if __name__ == '__main__':
    main()
