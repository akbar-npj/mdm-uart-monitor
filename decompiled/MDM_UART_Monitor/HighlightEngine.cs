using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using FastColoredTextBoxNS;

namespace MDM_UART_Monitor;

internal class HighlightEngine
{
	private static readonly TextStyle ErrorStyle = new TextStyle(Brushes.Red, null, (FontStyle)1);

	private static readonly TextStyle WarningStyle = new TextStyle(Brushes.Orange, null, (FontStyle)1);

	private static readonly TextStyle SuccessStyle = new TextStyle(Brushes.LimeGreen, null, (FontStyle)1);

	private static readonly TextStyle QualcommStyle = new TextStyle(Brushes.DeepSkyBlue, null, (FontStyle)1);

	private static readonly TextStyle HardwareStyle = new TextStyle(Brushes.Gold, null, (FontStyle)1);

	private static readonly TextStyle PartitionStyle = new TextStyle(Brushes.Violet, null, (FontStyle)1);

	private static readonly TextStyle SecurityStyle = new TextStyle(Brushes.Cyan, null, (FontStyle)1);

	public static readonly string[] ErrorKeywords = new string[37]
	{
		"CHARGING SOURCE NOT DETECTED", "AUTHENTICATION FAILED", "FAILED TO LOAD", "FAILED TO READ", "FAILED TO WRITE", "FAILED TO OPEN", "FAILED TO INITIALIZE", "FAILED TO START", "FAILED TO VERIFY", "VERIFY FAILED",
		"BOOT FAILED", "NOT DETECTED", "NOT FOUND", "NOT READY", "NOT DONE", "NOT SUPPORTED", "ACCESS DENIED", "CRC ERROR", "READ ERROR", "WRITE ERROR",
		"AUTH FAIL", "ERROR", "FAILED", "FAIL", "FAILURE", "FATAL", "PANIC", "EXCEPTION", "TIMEOUT", "INVALID",
		"CORRUPTED", "ABORT", "ABNORMAL", "DENIED", "MISMATCH", "OVERFLOW", "UNDERFLOW"
	};

	public static readonly string[] WarningKeywords = new string[12]
	{
		"WARNING", "WARN", "NOT RECOMMENDED", "RETRY", "SKIP", "UNKNOWN", "UNSUPPORTED", "DEFAULT", "FALLBACK", "DISABLED",
		"IGNORE", "MISSING"
	};

	public static readonly string[] SuccessKeywords = new string[16]
	{
		"AUTHENTICATION COMPLETE", "AUTHENTICATE COMPLETE", "BOOT COMPLETE", "VERIFY SUCCESS", "INITIALIZATION COMPLETE", "DEVICE READY", "BOOT STATE IS: GREEN", "BOOT STATE: GREEN", "RPMB PROVISIONED", "SUCCESS",
		"COMPLETE", "CONNECTED", "VERIFIED", "ENABLED", "PASS", "OK"
	};

	public static readonly string[] QualcommKeywords = new string[23]
	{
		"PBL", "SBL", "SBL1", "XBL", "XBL_CONFIG", "ABL", "QSEE", "QHEE", "RPM ", "RPMH",
		"SMEM", "SHRM", "AOP", "SPMI", "TZ", "AVB", "VBMETA", "CPUSS", "CPUCP", "XPU",
		"PIL", "PMIC", "QCOM"
	};

	public static readonly string[] MTKKeywords = new string[13]
	{
		"BROM", "PRELOADER", "LK", "LK2", "DA", "PL", "RPMB", "SLA", "DAA", "META",
		"FACTORY", "DOWNLOAD", "BOOTROM"
	};

	public static readonly string[] SamsungKeywords = new string[11]
	{
		"ODIN", "LOKE", "PIT", "RPMB", "VBMETA", "PARAM", "EFS", "CSC", "BL", "CP",
		"AP"
	};

	public static readonly string[] AndroidKeywords = new string[14]
	{
		"ANDROID", "BOOT", "KERNEL", "SYSTEM", "RECOVERY", "FASTBOOT", "EDL", "ADB", "INIT", "DM-VERITY",
		"VERITY", "SELINUX", "MAGISK", "ROOT"
	};

	public static readonly string[] HardwareKeywords = new string[32]
	{
		"PMIC", "DDR", "LPDDR", "UFS", "EMMC", "NAND", "NOR", "USB", "TYPEC", "VBUS",
		"VBAT", "IBAT", "BATTERY", "GPIO", "UART", "I2C", "SPI", "ADC", "DAC", "CLOCK",
		"PLL", "THERMAL", "TSENS", "DISPLAY", "TOUCH", "CAMERA", "MODEM", "WIFI", "BT", "BLUETOOTH",
		"GPS", "NFC"
	};

	public static readonly string[] PartitionKeywords = new string[20]
	{
		"BOOT", "BOOT_A", "BOOT_B", "SYSTEM", "SYSTEM_A", "SYSTEM_B", "SUPER", "VENDOR", "VENDOR_BOOT", "DTBO",
		"VBMETA", "VBMETA_SYSTEM", "RECOVERY", "USERDATA", "CACHE", "PERSIST", "METADATA", "MODEM", "EFS", "PARAM"
	};

	public static readonly string[] ChargingKeywords = new string[12]
	{
		"CHARGER", "CHARGING", "VBAT", "IBAT", "VBUS", "TYPEC", "USB_PD", "PD", "QC", "QUICK CHARGE",
		"POWER DELIVERY", "BATTERY ID"
	};

	public static readonly string[] SecurityKeywords = new string[13]
	{
		"SECURE BOOT", "SECBOOT", "AVB", "VBMETA", "RPMB", "FRP", "OEM LOCK", "UNLOCK", "LOCKED", "UNLOCKED",
		"AUTH", "VERIFY", "CERTIFICATE"
	};

	public static void Highlight(FastColoredTextBox txt)
	{
		Range range = txt.Range;
		range.ClearStyle(ErrorStyle, WarningStyle, SuccessStyle, QualcommStyle, HardwareStyle, PartitionStyle, SecurityStyle);
		ApplyStyle(range, ErrorKeywords, ErrorStyle);
		ApplyStyle(range, WarningKeywords, WarningStyle);
		ApplyStyle(range, SuccessKeywords, SuccessStyle);
		ApplyStyle(range, SecurityKeywords, SecurityStyle);
		ApplyStyle(range, QualcommKeywords, QualcommStyle);
		ApplyStyle(range, HardwareKeywords, HardwareStyle);
		ApplyStyle(range, PartitionKeywords, PartitionStyle);
	}

	private static void ApplyStyle(Range range, string[] keywords, TextStyle style)
	{
		foreach (string item in keywords.OrderByDescending((string x) => x.Length))
		{
			range.SetStyle(style, "\\b" + Regex.Escape(item) + "\\b", RegexOptions.IgnoreCase);
		}
	}
}
