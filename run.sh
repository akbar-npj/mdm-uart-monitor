#!/usr/bin/env bash
# MDM UART Monitor - Standalone Linux Launch Script
set -e

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
cd "$DIR"

exec python3 "$DIR/mdm_monitor/main.py" "$@"
