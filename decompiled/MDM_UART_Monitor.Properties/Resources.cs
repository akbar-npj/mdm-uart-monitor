using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace MDM_UART_Monitor.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("MDM_UART_Monitor.Properties.Resources", typeof(Resources).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static Bitmap Banner => (Bitmap)ResourceManager.GetObject("Banner", resourceCulture);

	internal static Bitmap Green_Tick => (Bitmap)ResourceManager.GetObject("Green Tick", resourceCulture);

	internal static Bitmap MDM_Loading => (Bitmap)ResourceManager.GetObject("MDM_Loading", resourceCulture);

	internal static Bitmap MDM_UPI => (Bitmap)ResourceManager.GetObject("MDM UPI", resourceCulture);

	internal static Bitmap PBoxWarning => (Bitmap)ResourceManager.GetObject("PBoxWarning", resourceCulture);

	internal static Bitmap SearchIcon => (Bitmap)ResourceManager.GetObject("SearchIcon", resourceCulture);

	internal static Bitmap UART_Logo => (Bitmap)ResourceManager.GetObject("UART_Logo", resourceCulture);

	internal static Bitmap WhatsApp_icon__2_ => (Bitmap)ResourceManager.GetObject("WhatsApp_icon (2)", resourceCulture);

	internal Resources()
	{
	}
}
