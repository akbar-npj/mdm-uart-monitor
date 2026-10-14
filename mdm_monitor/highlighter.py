"""
Mobile Doctor (MDM) UART Monitor - Syntax Highlighting Engine for Qt6
Ported from HighlightEngine.cs with mobile SoC and bootloader keyword patterns.
"""

from PySide6.QtCore import QRegularExpression
from PySide6.QtGui import QColor, QFont, QSyntaxHighlighter, QTextCharFormat
import re


class UartHighlighter(QSyntaxHighlighter):
    # Keyword categories ported directly from HighlightEngine.cs
    ERROR_KEYWORDS = [
        "CHARGING SOURCE NOT DETECTED", "AUTHENTICATION FAILED", "FAILED TO LOAD",
        "FAILED TO READ", "FAILED TO WRITE", "FAILED TO OPEN", "FAILED TO INITIALIZE",
        "FAILED TO START", "FAILED TO VERIFY", "VERIFY FAILED", "BOOT FAILED",
        "NOT DETECTED", "NOT FOUND", "NOT READY", "NOT DONE", "NOT SUPPORTED",
        "ACCESS DENIED", "CRC ERROR", "READ ERROR", "WRITE ERROR", "AUTH FAIL",
        "ERROR", "FAILED", "FAIL", "FAILURE", "FATAL", "PANIC", "EXCEPTION",
        "TIMEOUT", "INVALID", "CORRUPTED", "ABORT", "ABNORMAL", "DENIED",
        "MISMATCH", "OVERFLOW", "UNDERFLOW"
    ]

    WARNING_KEYWORDS = [
        "WARNING", "WARN", "NOT RECOMMENDED", "RETRY", "SKIP", "UNKNOWN",
        "UNSUPPORTED", "DEFAULT", "FALLBACK", "DISABLED", "IGNORE", "MISSING"
    ]

    SUCCESS_KEYWORDS = [
        "AUTHENTICATION COMPLETE", "AUTHENTICATE COMPLETE", "BOOT COMPLETE",
        "VERIFY SUCCESS", "INITIALIZATION COMPLETE", "DEVICE READY",
        "BOOT STATE IS: GREEN", "BOOT STATE: GREEN", "RPMB PROVISIONED",
        "SUCCESS", "COMPLETE", "CONNECTED", "VERIFIED", "ENABLED", "PASS", "OK"
    ]

    SECURITY_KEYWORDS = [
        "SECURE BOOT", "SECBOOT", "AVB", "VBMETA", "RPMB", "FRP", "OEM LOCK",
        "UNLOCK", "LOCKED", "UNLOCKED", "AUTH", "VERIFY", "CERTIFICATE"
    ]

    QUALCOMM_KEYWORDS = [
        "PBL", "SBL", "SBL1", "XBL", "XBL_CONFIG", "ABL", "QSEE", "QHEE",
        "RPM ", "RPMH", "SMEM", "SHRM", "AOP", "SPMI", "TZ", "AVB",
        "VBMETA", "CPUSS", "CPUCP", "XPU", "PIL", "PMIC", "QCOM"
    ]

    MTK_KEYWORDS = [
        "BROM", "PRELOADER", "LK", "LK2", "DA", "PL", "RPMB", "SLA",
        "DAA", "META", "FACTORY", "DOWNLOAD", "BOOTROM"
    ]

    SAMSUNG_KEYWORDS = [
        "ODIN", "LOKE", "PIT", "RPMB", "VBMETA", "PARAM", "EFS", "CSC",
        "BL", "CP", "AP"
    ]

    ANDROID_KEYWORDS = [
        "ANDROID", "BOOT", "KERNEL", "SYSTEM", "RECOVERY", "FASTBOOT",
        "EDL", "ADB", "INIT", "DM-VERITY", "VERITY", "SELINUX", "MAGISK", "ROOT"
    ]

    HARDWARE_KEYWORDS = [
        "PMIC", "DDR", "LPDDR", "UFS", "EMMC", "NAND", "NOR", "USB",
        "TYPEC", "VBUS", "VBAT", "IBAT", "BATTERY", "GPIO", "UART",
        "I2C", "SPI", "ADC", "DAC", "CLOCK", "PLL", "THERMAL", "TSENS",
        "DISPLAY", "TOUCH", "CAMERA", "MODEM", "WIFI", "BT", "BLUETOOTH",
        "GPS", "NFC"
    ]

    PARTITION_KEYWORDS = [
        "BOOT", "BOOT_A", "BOOT_B", "SYSTEM", "SYSTEM_A", "SYSTEM_B",
        "SUPER", "VENDOR", "VENDOR_BOOT", "DTBO", "VBMETA", "VBMETA_SYSTEM",
        "RECOVERY", "USERDATA", "CACHE", "PERSIST", "METADATA", "MODEM",
        "EFS", "PARAM"
    ]

    CHARGING_KEYWORDS = [
        "CHARGER", "CHARGING", "VBAT", "IBAT", "VBUS", "TYPEC", "USB_PD",
        "PD", "QC", "QUICK CHARGE", "POWER DELIVERY", "BATTERY ID"
    ]

    def __init__(self, parent=None):
        super().__init__(parent)
        self.rules = []
        self._setup_rules()

    def _create_format(self, color_hex: str, bold: bool = True) -> QTextCharFormat:
        fmt = QTextCharFormat()
        fmt.setForeground(QColor(color_hex))
        if bold:
            fmt.setFontWeight(QFont.Weight.Bold)
        return fmt

    def _setup_rules(self):
        # Color mapping corresponding to HighlightEngine styles
        styles = [
            (self.ERROR_KEYWORDS, "#FF4D4D"),        # Red / ErrorStyle
            (self.WARNING_KEYWORDS, "#FFA600"),      # Orange / WarningStyle
            (self.SUCCESS_KEYWORDS, "#00E676"),      # LimeGreen / SuccessStyle
            (self.SECURITY_KEYWORDS, "#00E5FF"),     # Cyan / SecurityStyle
            (self.QUALCOMM_KEYWORDS, "#00B0FF"),     # DeepSkyBlue / QualcommStyle
            (self.MTK_KEYWORDS, "#E040FB"),          # Magenta / MTK
            (self.SAMSUNG_KEYWORDS, "#448AFF"),      # DodgerBlue / Samsung
            (self.ANDROID_KEYWORDS, "#69F0AE"),      # SpringGreen / Android
            (self.HARDWARE_KEYWORDS, "#FFD700"),     # Gold / HardwareStyle
            (self.PARTITION_KEYWORDS, "#B388FF"),    # Violet / PartitionStyle
            (self.CHARGING_KEYWORDS, "#FF6E40"),     # Coral / Charging
        ]

        # Compile regular expressions in order of precedence
        for keyword_list, color in styles:
            fmt = self._create_format(color, bold=True)
            # Sort by descending length so longer phrases match first
            sorted_keywords = sorted(keyword_list, key=len, reverse=True)
            for kw in sorted_keywords:
                escaped = re.escape(kw.strip())
                pattern = QRegularExpression(
                    rf"\b{escaped}\b",
                    QRegularExpression.PatternOption.CaseInsensitiveOption
                )
                self.rules.append((pattern, fmt))

    def highlightBlock(self, text: str):
        if not text:
            return

        for pattern, fmt in self.rules:
            match_iterator = pattern.globalMatch(text)
            while match_iterator.hasNext():
                match = match_iterator.next()
                start = match.capturedStart()
                length = match.capturedLength()
                self.setFormat(start, length, fmt)
