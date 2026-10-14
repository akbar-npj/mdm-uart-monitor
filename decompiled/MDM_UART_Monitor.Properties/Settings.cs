using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MDM_UART_Monitor.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)(object)SettingsBase.Synchronized((SettingsBase)(object)new Settings());

	public static Settings Default => defaultInstance;

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Username
	{
		get
		{
			return (string)((SettingsBase)this)["Username"];
		}
		set
		{
			((SettingsBase)this)["Username"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Password
	{
		get
		{
			return (string)((SettingsBase)this)["Password"];
		}
		set
		{
			((SettingsBase)this)["Password"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool AutoLogin
	{
		get
		{
			return (bool)((SettingsBase)this)["AutoLogin"];
		}
		set
		{
			((SettingsBase)this)["AutoLogin"] = value;
		}
	}
}
