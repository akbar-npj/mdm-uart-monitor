#!/usr/bin/env python3
"""
Entry point for MDM UART Monitor (Linux Qt6 Standalone Edition)
"""

import sys
import os
from PySide6.QtWidgets import QApplication
from PySide6.QtCore import Qt

# Ensure the package is findable in python path
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))

from mdm_monitor.ui_main import MainWindow


def main():
    # Enable High DPI scaling
    os.environ["QT_AUTO_SCREEN_SCALE_FACTOR"] = "1"

    app = QApplication(sys.argv)
    app.setApplicationName("MDM UART Monitor")
    app.setOrganizationName("MobileDoctor")

    window = MainWindow()
    window.show()

    sys.exit(app.exec())


if __name__ == "__main__":
    main()
