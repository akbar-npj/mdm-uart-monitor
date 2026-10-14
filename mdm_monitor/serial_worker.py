"""
Mobile Doctor (MDM) UART Monitor - Serial Worker Thread for Qt6
High-performance asynchronous serial port reader and writer.
"""

import time
import glob
from typing import List, Tuple
from PySide6.QtCore import QObject, QThread, Signal, Slot
import serial
import serial.tools.list_ports


def is_garbage_data(data: str) -> bool:
    """
    Ported verbatim from Form1.cs IsGarbageData method.
    Checks if received data contains high ratio of question marks and unprintable control characters,
    indicating baud rate mismatch.
    """
    if not data:
        return False
    num_q = 0
    num_ctrl = 0
    for c in data:
        if c == '?':
            num_q += 1
        if ord(c) < 32 and c not in ('\r', '\n', '\t'):
            num_ctrl += 1
    return num_q >= 2 and num_ctrl >= 1


def list_available_serial_ports() -> List[Tuple[str, str]]:
    """
    Returns list of (device_path, friendly_description) for Linux serial ports.
    Combines PySerial device enumeration with direct /dev scanning.
    """
    ports = {}
    # Use pyserial comports
    for p in serial.tools.list_ports.comports():
        desc = p.description if p.description and p.description != p.device else "Serial Device"
        ports[p.device] = f"{p.device} ({desc})"

    # Direct search in /dev for USB and ACM adapters if not found by comports
    patterns = ["/dev/ttyUSB*", "/dev/ttyACM*", "/dev/ttyS*"]
    for pattern in patterns:
        for path in sorted(glob.glob(pattern)):
            if path not in ports:
                ports[path] = path

    # Sort so USB/ACM ports appear at the top
    def sort_key(item):
        dev = item[0]
        if "ttyUSB" in dev:
            return (0, dev)
        if "ttyACM" in dev:
            return (1, dev)
        return (2, dev)

    return sorted(ports.items(), key=sort_key)


class SerialWorker(QThread):
    data_received = Signal(str)
    garbage_detected = Signal(bool)
    status_changed = Signal(str, str)  # status_text, color_hex
    error_occurred = Signal(str)

    def __init__(self, port_name: str, baud_rate: int):
        super().__init__()
        self.port_name = port_name
        self.baud_rate = baud_rate
        self.serial_inst = None
        self._is_running = False

    def run(self):
        try:
            self.serial_inst = serial.Serial(
                port=self.port_name,
                baudrate=self.baud_rate,
                bytesize=serial.EIGHTBITS,
                parity=serial.PARITY_NONE,
                stopbits=serial.STOPBITS_ONE,
                timeout=0.05
            )
            self._is_running = True
            self.status_changed.emit("● Connected", "#00E676")
        except PermissionError:
            self.status_changed.emit("● Connection Failed (Permission Denied)", "#FF8800")
            self.error_occurred.emit(
                f"Permission denied accessing {self.port_name}.\n\n"
                "Please add your Linux user to the 'dialout' or 'uucp' group:\n"
                f"  sudo usermod -aG dialout $USER\n"
                "or grant permissions using:\n"
                f"  sudo chmod 666 {self.port_name}"
            )
            return
        except Exception as e:
            self.status_changed.emit("● Connection Failed", "#FF8800")
            self.error_occurred.emit(f"Failed to open port {self.port_name}:\n{str(e)}")
            return

        buffer = []
        last_flush = time.time()

        while self._is_running and self.serial_inst and self.serial_inst.is_open:
            try:
                waiting = self.serial_inst.in_waiting
                if waiting > 0:
                    raw_bytes = self.serial_inst.read(min(waiting, 4096))
                    if raw_bytes:
                        # Decode with replacement char for invalid sequences
                        text = raw_bytes.decode('utf-8', errors='replace')
                        if text:
                            buffer.append(text)
                            if is_garbage_data(text):
                                self.garbage_detected.emit(True)

                # Batch updates every 25ms or when buffer is large to keep GUI responsive
                now = time.time()
                if buffer and (now - last_flush >= 0.025 or len(buffer) >= 32):
                    combined = "".join(buffer)
                    buffer.clear()
                    last_flush = now
                    self.data_received.emit(combined)
                else:
                    time.sleep(0.01)

            except Exception as e:
                if self._is_running:
                    self.error_occurred.emit(f"Serial read error on {self.port_name}:\n{str(e)}")
                break

        # Flush remaining buffer before exiting
        if buffer:
            self.data_received.emit("".join(buffer))

        if self.serial_inst and self.serial_inst.is_open:
            try:
                self.serial_inst.close()
            except Exception:
                pass

        self.status_changed.emit("● Disconnected", "#FF4444")

    def stop(self):
        self._is_running = False
        if self.serial_inst and self.serial_inst.is_open:
            try:
                self.serial_inst.cancel_read()
            except Exception:
                pass
        self.wait(1000)

    def write_data(self, text: str) -> bool:
        if not self.serial_inst or not self.serial_inst.is_open:
            return False
        try:
            self.serial_inst.write(text.encode('utf-8'))
            self.serial_inst.flush()
            return True
        except Exception as e:
            self.error_occurred.emit(f"Failed to write data:\n{str(e)}")
            return False
