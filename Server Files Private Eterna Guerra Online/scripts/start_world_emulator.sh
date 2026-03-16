#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/.."
python emulator/world_emulator.py
