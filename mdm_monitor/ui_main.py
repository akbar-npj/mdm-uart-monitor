"""
Mobile Doctor (MDM) UART Monitor - Main GUI Window in Qt6
Faithful standalone Linux port with all original features and modern dark cyber styling.
"""

import os
import sys
from datetime import datetime
from PySide6.QtCore import Qt, QTimer, Slot
from PySide6.QtGui import (
    QAction, QColor, QFont, QIcon, QKeySequence, QPixmap, QTextCursor,
    QTextDocument
)
from PySide6.QtWidgets import (
    QApplication, QComboBox, QFileDialog, QFrame, QGraphicsDropShadowEffect,
    QGridLayout, QHBoxLayout, QLabel, QLineEdit, QMainWindow, QMessageBox,
    QPlainTextEdit, QPushButton, QSlider, QSplitter, QTextEdit, QVBoxLayout,
    QWidget
)

from .highlighter import UartHighlighter
from .serial_worker import SerialWorker, list_available_serial_ports


class UartTextEdit(QPlainTextEdit):
    """
    Subclassed QPlainTextEdit providing high-performance appending
    and smart auto-scroll behavior.
    """
    def __init__(self, parent=None):
        super().__init__(parent)
        self.setReadOnly(True)
        self.setMaximumBlockCount(100000)  # Safe buffer limit for long-running captures
        self.setLineWrapMode(QPlainTextEdit.LineWrapMode.NoWrap)
        self.auto_scroll = True

        # Track user manual scroll
        self.verticalScrollBar().sliderMoved.connect(self._on_scroll_moved)
        self.verticalScrollBar().valueChanged.connect(self._on_scroll_value_changed)

    def _on_scroll_moved(self, value):
        max_val = self.verticalScrollBar().maximum()
        self.auto_scroll = (value >= max_val - 5)

    def _on_scroll_value_changed(self, value):
        max_val = self.verticalScrollBar().maximum()
        if value >= max_val - 5:
            self.auto_scroll = True

    def append_text_chunk(self, chunk: str):
        cursor = self.textCursor()
        cursor.movePosition(QTextCursor.MoveOperation.End)
        cursor.insertText(chunk)
        if self.auto_scroll:
            self.verticalScrollBar().setValue(self.verticalScrollBar().maximum())


class MainWindow(QMainWindow):
    BAUD_RATES = ["9600", "19200", "38400", "57600", "115200", "230400", "460800", "921600"]

    def __init__(self):
        super().__init__()
        self.setWindowTitle("MDM UART Monitor - Linux Diagnostic Edition")
        self.setMinimumSize(1150, 720)
        self.resize(1280, 800)

        self.worker = None
        self.search_matches = []
        self.current_match_index = -1

        self.asset_dir = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", "assets"))
        self._set_app_icon()

        self._init_ui()
        self._apply_styles()
        self._init_serial_options()

        # Warning hide timer (matches timerRxWarning in original app)
        self.rx_warning_timer = QTimer(self)
        self.rx_warning_timer.setSingleShot(True)
        self.rx_warning_timer.setInterval(3000)
        self.rx_warning_timer.timeout.connect(self._hide_rx_warning)

    def _set_app_icon(self):
        logo_path = os.path.join(self.asset_dir, "UART_Logo.png")
        if os.path.exists(logo_path):
            self.setWindowIcon(QIcon(logo_path))

    def _init_ui(self):
        central = QWidget()
        self.setCentralWidget(central)
        root_layout = QVBoxLayout(central)
        root_layout.setContentsMargins(10, 10, 10, 10)
        root_layout.setSpacing(8)

        # 1. Top Title & Branding Banner
        banner_frame = self._create_banner()
        root_layout.addWidget(banner_frame)

        # 2. Main Content Splitter / Layout
        content_layout = QHBoxLayout()
        content_layout.setSpacing(10)

        # 2a. Left Terminal Panel with Overlay Warning
        terminal_container = self._create_terminal_view()
        content_layout.addWidget(terminal_container, stretch=8)

        # 2b. Right Control Sidebar
        sidebar = self._create_sidebar()
        content_layout.addWidget(sidebar, stretch=3)

        root_layout.addLayout(content_layout)

    def _create_banner(self) -> QFrame:
        frame = QFrame()
        frame.setObjectName("BannerFrame")
        layout = QHBoxLayout(frame)
        layout.setContentsMargins(12, 6, 12, 6)

        banner_img_path = os.path.join(self.asset_dir, "Banner.png")
        logo_img_path = os.path.join(self.asset_dir, "UART_Logo.png")

        if os.path.exists(banner_img_path):
            banner_lbl = QLabel()
            pix = QPixmap(banner_img_path)
            # Scale banner proportionally
            banner_lbl.setPixmap(pix.scaledToHeight(52, Qt.TransformationMode.SmoothTransformation))
            layout.addWidget(banner_lbl)
        else:
            if os.path.exists(logo_img_path):
                logo_lbl = QLabel()
                pix = QPixmap(logo_img_path)
                logo_lbl.setPixmap(pix.scaledToHeight(48, Qt.TransformationMode.SmoothTransformation))
                layout.addWidget(logo_lbl)

            title_lbl = QLabel("MDM UART MONITOR")
            title_lbl.setStyleSheet("font-size: 20px; font-weight: bold; color: #00E5FF; letter-spacing: 2px;")
            layout.addWidget(title_lbl)

        layout.addStretch()

        subtitle = QLabel("Mobile Repair & SoC Diagnostic Terminal  |  Linux Native")
        subtitle.setStyleSheet("font-size: 12px; font-weight: bold; color: #78909C; margin-right: 15px;")
        layout.addWidget(subtitle)

        return frame

    def _create_terminal_view(self) -> QWidget:
        container = QWidget()
        layout = QVBoxLayout(container)
        layout.setContentsMargins(0, 0, 0, 0)
        layout.setSpacing(0)

        # Rx Baud Rate Warning Banner (Ported from picRxWarning & IsGarbageData)
        self.warning_banner = QFrame()
        self.warning_banner.setObjectName("WarningBanner")
        warn_layout = QHBoxLayout(self.warning_banner)
        warn_layout.setContentsMargins(15, 8, 15, 8)

        warn_icon = QLabel("⚠️")
        warn_icon.setStyleSheet("font-size: 18px;")
        warn_layout.addWidget(warn_icon)

        warn_text = QLabel(
            "<b>Rx Warning:</b> High rate of non-printable / garbage bytes detected! "
            "Please check if the selected Baud Rate matches your device UART."
        )
        warn_text.setStyleSheet("color: #FFF3E0; font-size: 12px;")
        warn_layout.addWidget(warn_text)
        warn_layout.addStretch()

        btn_dismiss = QPushButton("✕")
        btn_dismiss.setFixedWidth(26)
        btn_dismiss.setStyleSheet("background: transparent; color: #FFD54F; font-size: 13px; border: none;")
        btn_dismiss.clicked.connect(self._hide_rx_warning)
        warn_layout.addWidget(btn_dismiss)

        self.warning_banner.setVisible(False)
        layout.addWidget(self.warning_banner)

        # Terminal Edit Box
        self.txt_log = UartTextEdit()
        self.txt_log.setObjectName("TxtLog")
        font = QFont("Consolas")
        if not font.exactMatch():
            font = QFont("JetBrains Mono")
        if not font.exactMatch():
            font = QFont("monospace")
        font.setPointSize(10)
        self.txt_log.setFont(font)

        # Attach Qt6 Syntax Highlighter
        self.highlighter = UartHighlighter(self.txt_log.document())

        layout.addWidget(self.txt_log)
        return container

    def _create_sidebar(self) -> QWidget:
        sidebar = QFrame()
        sidebar.setObjectName("SidebarFrame")
        sidebar_layout = QVBoxLayout(sidebar)
        sidebar_layout.setContentsMargins(12, 12, 12, 12)
        sidebar_layout.setSpacing(12)

        # Group 1: Port & Connection
        conn_group = QFrame()
        conn_group.setObjectName("CardGroup")
        conn_layout = QGridLayout(conn_group)
        conn_layout.setContentsMargins(10, 10, 10, 10)
        conn_layout.setSpacing(8)

        lbl_port = QLabel("Com Port:")
        lbl_port.setStyleSheet("color: #90CAF9; font-weight: bold;")
        self.cmb_port = QComboBox()
        self.cmb_port.setObjectName("DarkCombo")

        btn_refresh_ports = QPushButton("⟳")
        btn_refresh_ports.setToolTip("Scan serial ports")
        btn_refresh_ports.setFixedWidth(30)
        btn_refresh_ports.clicked.connect(self._refresh_serial_ports)

        port_box = QHBoxLayout()
        port_box.setSpacing(4)
        port_box.addWidget(self.cmb_port)
        port_box.addWidget(btn_refresh_ports)

        lbl_baud = QLabel("Baud Rate:")
        lbl_baud.setStyleSheet("color: #90CAF9; font-weight: bold;")
        self.cmb_baud = QComboBox()
        self.cmb_baud.setObjectName("DarkCombo")
        self.cmb_baud.addItems(self.BAUD_RATES)
        self.cmb_baud.setCurrentIndex(4)  # 115200

        lbl_status_title = QLabel("Status:")
        lbl_status_title.setStyleSheet("color: #B0BEC5; font-weight: bold;")
        self.lbl_status = QLabel("● Disconnected")
        self.lbl_status.setStyleSheet("color: #FF5252; font-weight: bold;")

        self.btn_connect = QPushButton("Connect")
        self.btn_connect.setObjectName("BtnConnect")
        self.btn_connect.clicked.connect(self._on_connect_clicked)

        self.btn_disconnect = QPushButton("Disconnect")
        self.btn_disconnect.setObjectName("BtnDisconnect")
        self.btn_disconnect.setEnabled(False)
        self.btn_disconnect.clicked.connect(self._on_disconnect_clicked)

        conn_layout.addWidget(lbl_port, 0, 0)
        conn_layout.addLayout(port_box, 0, 1)
        conn_layout.addWidget(lbl_baud, 1, 0)
        conn_layout.addWidget(self.cmb_baud, 1, 1)
        conn_layout.addWidget(lbl_status_title, 2, 0)
        conn_layout.addWidget(self.lbl_status, 2, 1)

        btn_box = QHBoxLayout()
        btn_box.setSpacing(8)
        btn_box.addWidget(self.btn_disconnect)
        btn_box.addWidget(self.btn_connect)
        conn_layout.addLayout(btn_box, 3, 0, 1, 2)

        sidebar_layout.addWidget(conn_group)

        # Group 2: Log Search
        search_group = QFrame()
        search_group.setObjectName("CardGroup")
        search_layout = QVBoxLayout(search_group)
        search_layout.setContentsMargins(10, 10, 10, 10)
        search_layout.setSpacing(8)

        search_hdr = QHBoxLayout()
        lbl_search = QLabel("Search Logs:")
        lbl_search.setStyleSheet("color: #90CAF9; font-weight: bold;")
        self.lbl_search_count = QLabel("0/0")
        self.lbl_search_count.setStyleSheet("color: #7FFFD4; font-weight: bold;")
        search_hdr.addWidget(lbl_search)
        search_hdr.addStretch()
        search_hdr.addWidget(self.lbl_search_count)
        search_layout.addLayout(search_hdr)

        self.txt_search = QLineEdit()
        self.txt_search.setPlaceholderText("Filter / search term...")
        self.txt_search.textChanged.connect(self._on_search_text_changed)
        self.txt_search.returnPressed.connect(self._on_find_next_clicked)
        search_layout.addWidget(self.txt_search)

        search_btns = QHBoxLayout()
        search_btns.setSpacing(6)
        self.btn_find_next = QPushButton("▼ Next")
        self.btn_find_next.setObjectName("BtnSearchNav")
        self.btn_find_next.clicked.connect(self._on_find_next_clicked)

        self.btn_find_prev = QPushButton("▲ Prev")
        self.btn_find_prev.setObjectName("BtnSearchNav")
        self.btn_find_prev.clicked.connect(self._on_find_prev_clicked)

        self.btn_clear_search = QPushButton("✖ Clear")
        self.btn_clear_search.setObjectName("BtnClearSearch")
        self.btn_clear_search.clicked.connect(self._on_clear_search_clicked)

        search_btns.addWidget(self.btn_find_next)
        search_btns.addWidget(self.btn_find_prev)
        search_btns.addWidget(self.btn_clear_search)
        search_layout.addLayout(search_btns)

        sidebar_layout.addWidget(search_group)

        # Group 3: Command Sender
        cmd_group = QFrame()
        cmd_group.setObjectName("CardGroup")
        cmd_layout = QVBoxLayout(cmd_group)
        cmd_layout.setContentsMargins(10, 10, 10, 10)
        cmd_layout.setSpacing(8)

        cmd_hdr = QHBoxLayout()
        lbl_cmd = QLabel("Send Command:")
        lbl_cmd.setStyleSheet("color: #FFB74D; font-weight: bold; font-size: 13px;")
        cmd_hdr.addWidget(lbl_cmd)
        cmd_hdr.addStretch()

        self.cmb_line_ending = QComboBox()
        self.cmb_line_ending.setObjectName("DarkComboMini")
        self.cmb_line_ending.addItems(["CRLF (\\r\\n)", "LF (\\n)", "CR (\\r)", "Raw (None)"])
        cmd_hdr.addWidget(self.cmb_line_ending)
        cmd_layout.addLayout(cmd_hdr)

        self.txt_cmd = QLineEdit()
        self.txt_cmd.setPlaceholderText("Type command here...")
        self.txt_cmd.returnPressed.connect(self._on_send_command_clicked)
        cmd_layout.addWidget(self.txt_cmd)

        self.btn_send = QPushButton("Send")
        self.btn_send.setObjectName("BtnSend")
        self.btn_send.clicked.connect(self._on_send_command_clicked)
        cmd_layout.addWidget(self.btn_send)

        sidebar_layout.addWidget(cmd_group)

        # Group 4: Log Actions & Font Size
        actions_group = QFrame()
        actions_group.setObjectName("CardGroup")
        actions_layout = QVBoxLayout(actions_group)
        actions_layout.setContentsMargins(10, 10, 10, 10)
        actions_layout.setSpacing(8)

        action_btns = QHBoxLayout()
        action_btns.setSpacing(8)

        self.btn_copy = QPushButton("Copy All")
        self.btn_copy.setObjectName("BtnOrange")
        self.btn_copy.clicked.connect(self._on_copy_logs_clicked)

        self.btn_clear = QPushButton("Clear All")
        self.btn_clear.setObjectName("BtnRed")
        self.btn_clear.clicked.connect(self._on_clear_logs_clicked)

        action_btns.addWidget(self.btn_copy)
        action_btns.addWidget(self.btn_clear)
        actions_layout.addLayout(action_btns)

        self.btn_save = QPushButton("Save Logs")
        self.btn_save.setObjectName("BtnOrange")
        self.btn_save.clicked.connect(self._on_save_logs_clicked)
        actions_layout.addWidget(self.btn_save)

        # Font Size Slider
        font_box = QHBoxLayout()
        lbl_font_size = QLabel("Log Text Size:")
        lbl_font_size.setStyleSheet("color: #00E5FF; font-weight: bold;")
        self.lbl_font_val = QLabel("10")
        self.lbl_font_val.setStyleSheet("color: #76FF03; font-weight: bold;")
        font_box.addWidget(lbl_font_size)
        font_box.addStretch()
        font_box.addWidget(self.lbl_font_val)
        actions_layout.addLayout(font_box)

        self.slider_font = QSlider(Qt.Orientation.Horizontal)
        self.slider_font.setRange(8, 22)
        self.slider_font.setValue(10)
        self.slider_font.valueChanged.connect(self._on_font_slider_changed)
        actions_layout.addWidget(self.slider_font)

        sidebar_layout.addWidget(actions_group)
        sidebar_layout.addStretch()

        return sidebar

    def _init_serial_options(self):
        self._refresh_serial_ports()

    def _refresh_serial_ports(self):
        current_text = self.cmb_port.currentText()
        self.cmb_port.clear()
        ports = list_available_serial_ports()
        for dev, display_name in ports:
            self.cmb_port.addItem(display_name, dev)

        # Restore previous selection if possible
        idx = self.cmb_port.findText(current_text)
        if idx >= 0:
            self.cmb_port.setCurrentIndex(idx)
        elif self.cmb_port.count() > 0:
            self.cmb_port.setCurrentIndex(0)

        # Update Connect button state
        has_ports = self.cmb_port.count() > 0
        self.btn_connect.setEnabled(has_ports and (self.worker is None or not self.worker.isRunning()))

    # --- Slot Handlers ---

    @Slot()
    def _on_connect_clicked(self):
        if self.cmb_port.count() == 0:
            QMessageBox.warning(self, "MDM UART Monitor", "Please select or plug in a COM / Serial port.")
            return

        port_device = self.cmb_port.currentData() or self.cmb_port.currentText().split()[0]
        baud_rate = int(self.cmb_baud.currentText())

        self.worker = SerialWorker(port_device, baud_rate)
        self.worker.data_received.connect(self._on_data_received)
        self.worker.garbage_detected.connect(self._on_garbage_detected)
        self.worker.status_changed.connect(self._on_status_changed)
        self.worker.error_occurred.connect(self._on_error_occurred)

        self.btn_connect.setEnabled(False)
        self.btn_disconnect.setEnabled(True)
        self.cmb_port.setEnabled(False)
        self.cmb_baud.setEnabled(False)

        self.worker.start()

    @Slot()
    def _on_disconnect_clicked(self):
        if self.worker:
            self.worker.stop()
            self.worker = None

        self.btn_connect.setEnabled(True)
        self.btn_disconnect.setEnabled(False)
        self.cmb_port.setEnabled(True)
        self.cmb_baud.setEnabled(True)
        self._on_status_changed("● Disconnected", "#FF5252")

    @Slot(str)
    def _on_data_received(self, text: str):
        self.txt_log.append_text_chunk(text)

    @Slot(bool)
    def _on_garbage_detected(self, detected: bool):
        if detected:
            self.warning_banner.setVisible(True)
            self.rx_warning_timer.stop()
            self.rx_warning_timer.start()

    @Slot()
    def _hide_rx_warning(self):
        self.warning_banner.setVisible(False)

    @Slot(str, str)
    def _on_status_changed(self, text: str, color_hex: str):
        self.lbl_status.setText(text)
        self.lbl_status.setStyleSheet(f"color: {color_hex}; font-weight: bold;")
        if "Disconnected" in text or "Failed" in text:
            self.btn_connect.setEnabled(self.cmb_port.count() > 0)
            self.btn_disconnect.setEnabled(False)
            self.cmb_port.setEnabled(True)
            self.cmb_baud.setEnabled(True)

    @Slot(str)
    def _on_error_occurred(self, err_msg: str):
        QMessageBox.critical(self, "UART Connection Error", err_msg)

    @Slot()
    def _on_send_command_clicked(self):
        cmd = self.txt_cmd.text()
        if not cmd:
            QMessageBox.warning(self, "MDM UART Monitor", "Please enter a command.")
            self.txt_cmd.setFocus()
            return

        if not self.worker or not self.worker.isRunning():
            QMessageBox.warning(self, "MDM UART Monitor", "Please connect to a COM / Serial port first.")
            return

        # Add line ending
        le_idx = self.cmb_line_ending.currentIndex()
        if le_idx == 0:
            cmd += "\r\n"
        elif le_idx == 1:
            cmd += "\n"
        elif le_idx == 2:
            cmd += "\r"

        if self.worker.write_data(cmd):
            self.txt_cmd.clear()
            self.txt_cmd.setFocus()

    @Slot()
    def _on_copy_logs_clicked(self):
        text = self.txt_log.toPlainText()
        if not text:
            QMessageBox.information(self, "MDM UART Monitor", "No logs available to copy.")
            return
        QApplication.clipboard().setText(text)
        QMessageBox.information(self, "MDM UART Monitor", "Logs copied to clipboard.")

    @Slot()
    def _on_clear_logs_clicked(self):
        self.txt_log.clear()
        self.search_matches.clear()
        self.current_match_index = -1
        self.lbl_search_count.setText("0/0")

    @Slot()
    def _on_save_logs_clicked(self):
        text = self.txt_log.toPlainText()
        if not text:
            QMessageBox.information(self, "MDM UART Monitor", "Log is empty.")
            return

        default_filename = f"UART_Log_{datetime.now().strftime('%Y%m%d_%H%M%S')}.txt"
        file_path, _ = QFileDialog.getSaveFileName(
            self,
            "Save UART Log",
            default_filename,
            "Text Files (*.txt);;All Files (*)"
        )
        if file_path:
            try:
                with open(file_path, "w", encoding="utf-8") as f:
                    f.write(text)
                QMessageBox.information(self, "MDM UART Monitor", f"Log saved successfully to:\n{file_path}")
            except Exception as e:
                QMessageBox.critical(self, "Save Error", f"Failed to save log:\n{str(e)}")

    @Slot(int)
    def _on_font_slider_changed(self, value: int):
        self.lbl_font_val.setText(str(value))
        font = self.txt_log.font()
        font.setPointSize(value)
        self.txt_log.setFont(font)

    # --- Search Implementation ---

    @Slot(str)
    def _on_search_text_changed(self, query: str):
        self.search_matches.clear()
        self.current_match_index = -1
        if not query:
            self.lbl_search_count.setText("0/0")
            return

        doc = self.txt_log.document()
        cursor = QTextCursor(doc)
        while True:
            cursor = doc.find(query, cursor, QTextDocument.FindFlag(0))
            if cursor.isNull():
                break
            self.search_matches.append(cursor.selectionStart())

        count = len(self.search_matches)
        self.lbl_search_count.setText(f"0/{count}")

    @Slot()
    def _on_find_next_clicked(self):
        if not self.search_matches:
            return
        self.current_match_index = (self.current_match_index + 1) % len(self.search_matches)
        self._highlight_current_match()

    @Slot()
    def _on_find_prev_clicked(self):
        if not self.search_matches:
            return
        self.current_match_index = (self.current_match_index - 1) % len(self.search_matches)
        self._highlight_current_match()

    def _highlight_current_match(self):
        if self.current_match_index < 0 or self.current_match_index >= len(self.search_matches):
            return
        pos = self.search_matches[self.current_match_index]
        term_len = len(self.txt_search.text())

        cursor = self.txt_log.textCursor()
        cursor.setPosition(pos)
        cursor.setPosition(pos + term_len, QTextCursor.MoveMode.KeepAnchor)
        self.txt_log.setTextCursor(cursor)
        self.txt_log.ensureCursorVisible()

        self.lbl_search_count.setText(f"{self.current_match_index + 1}/{len(self.search_matches)}")

    @Slot()
    def _on_clear_search_clicked(self):
        self.txt_search.clear()
        self.search_matches.clear()
        self.current_match_index = -1
        self.lbl_search_count.setText("0/0")
        cursor = self.txt_log.textCursor()
        cursor.clearSelection()
        self.txt_log.setTextCursor(cursor)

    def closeEvent(self, event):
        if self.worker:
            self.worker.stop()
            self.worker = None
        event.accept()

    def _apply_styles(self):
        self.setStyleSheet("""
            QMainWindow {
                background-color: #060A12;
            }
            #BannerFrame {
                background-color: #0A111E;
                border: 1px solid #1E385C;
                border-radius: 8px;
            }
            #SidebarFrame {
                background-color: #0B1322;
                border: 1px solid #1C3352;
                border-radius: 8px;
            }
            #CardGroup {
                background-color: #0E182A;
                border: 1px solid #1A2E4A;
                border-radius: 6px;
            }
            #WarningBanner {
                background-color: #8C2A00;
                border-bottom: 2px solid #FF9800;
                border-radius: 4px;
            }
            #TxtLog {
                background-color: #070D18;
                color: #ECEFF1;
                border: 1px solid #1A2E4A;
                border-radius: 6px;
                selection-background-color: #007ACC;
                selection-color: #FFFFFF;
            }
            QLineEdit {
                background-color: #121F35;
                color: #FFFFFF;
                border: 1px solid #23426A;
                border-radius: 4px;
                padding: 5px 8px;
                font-size: 12px;
            }
            QLineEdit:focus {
                border: 1px solid #00B0FF;
            }
            QComboBox {
                background-color: #121F35;
                color: #FFFFFF;
                border: 1px solid #23426A;
                border-radius: 4px;
                padding: 4px 8px;
                font-size: 12px;
            }
            QComboBox::drop-down {
                border: none;
                width: 20px;
            }
            QComboBox QAbstractItemView {
                background-color: #0F1A2D;
                color: #FFFFFF;
                selection-background-color: #007ACC;
                border: 1px solid #23426A;
            }
            QPushButton {
                background-color: #16263F;
                color: #ECEFF1;
                border: 1px solid #2A4872;
                border-radius: 5px;
                padding: 6px 12px;
                font-weight: bold;
                font-size: 12px;
            }
            QPushButton:hover {
                background-color: #1E385C;
                border: 1px solid #00B0FF;
            }
            QPushButton:pressed {
                background-color: #0E1A2C;
            }
            QPushButton:disabled {
                background-color: #0A111C;
                color: #546E7A;
                border: 1px solid #162438;
            }
            #BtnConnect {
                background-color: #0A3238;
                color: #00E5FF;
                border: 1px solid #00ACC1;
            }
            #BtnConnect:hover {
                background-color: #0E4750;
                border: 1px solid #18FFFF;
            }
            #BtnDisconnect {
                background-color: #381216;
                color: #FF5252;
                border: 1px solid #D32F2F;
            }
            #BtnDisconnect:hover {
                background-color: #4E181F;
                border: 1px solid #FF1744;
            }
            #BtnSend {
                background-color: #113626;
                color: #69F0AE;
                border: 1px solid #00C853;
            }
            #BtnSend:hover {
                background-color: #164D36;
                border: 1px solid #00E676;
            }
            #BtnOrange {
                color: #FFB74D;
                border: 1px solid #E65100;
            }
            #BtnOrange:hover {
                background-color: #3E1E08;
                border: 1px solid #FF9800;
            }
            #BtnRed {
                color: #FF7043;
                border: 1px solid #BF360C;
            }
            #BtnRed:hover {
                background-color: #3E1308;
                border: 1px solid #FF5722;
            }
            #BtnSearchNav {
                color: #80CBC4;
            }
            #BtnClearSearch {
                color: #F48FB1;
            }
            QSlider::groove:horizontal {
                height: 6px;
                background: #142238;
                border-radius: 3px;
            }
            QSlider::sub-page:horizontal {
                background: #00B0FF;
                border-radius: 3px;
            }
            QSlider::handle:horizontal {
                background: #FFFFFF;
                border: 1px solid #00B0FF;
                width: 14px;
                margin-top: -4px;
                margin-bottom: -4px;
                border-radius: 7px;
            }
            QScrollBar:vertical {
                border: none;
                background: #080E1A;
                width: 10px;
                margin: 0px;
            }
            QScrollBar::handle:vertical {
                background: #1C3352;
                min-height: 25px;
                border-radius: 5px;
            }
            QScrollBar::handle:vertical:hover {
                background: #00B0FF;
            }
            QScrollBar::add-line:vertical, QScrollBar::sub-line:vertical {
                height: 0px;
            }
        """)
