using System;
using System.Windows.Forms;
using login;

namespace MDM_UART_Monitor;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Form2 form = new Form2();
		Application.Run((Form)(object)form);
		if ((int)((Form)form).DialogResult == 1)
		{
			Application.Run((Form)(object)new Form1());
		}
		else
		{
			Application.Exit();
		}
	}
}
