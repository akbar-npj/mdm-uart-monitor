using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MDM_UART_Monitor;

public class RoundForm : Form
{
	private const int WM_NCLBUTTONDOWN = 161;

	private const int HTCAPTION = 2;

	public int BorderRadius { get; set; } = 18;

	[DllImport("user32.dll")]
	private static extern bool ReleaseCapture();

	[DllImport("user32.dll")]
	private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

	public RoundForm()
	{
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).DoubleBuffered = true;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).MinimumSize = new Size(1000, 650);
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 132)
		{
			((Form)this).WndProc(ref m);
			if ((int)((Message)(ref m)).Result == 1)
			{
				Point point = ((Control)this).PointToClient(Cursor.Position);
				if (point.X <= 8 && point.Y <= 8)
				{
					((Message)(ref m)).Result = (IntPtr)13;
				}
				else if (point.X >= ((Control)this).Width - 8 && point.Y <= 8)
				{
					((Message)(ref m)).Result = (IntPtr)14;
				}
				else if (point.X <= 8 && point.Y >= ((Control)this).Height - 8)
				{
					((Message)(ref m)).Result = (IntPtr)16;
				}
				else if (point.X >= ((Control)this).Width - 8 && point.Y >= ((Control)this).Height - 8)
				{
					((Message)(ref m)).Result = (IntPtr)17;
				}
				else if (point.X <= 8)
				{
					((Message)(ref m)).Result = (IntPtr)10;
				}
				else if (point.X >= ((Control)this).Width - 8)
				{
					((Message)(ref m)).Result = (IntPtr)11;
				}
				else if (point.Y <= 8)
				{
					((Message)(ref m)).Result = (IntPtr)12;
				}
				else if (point.Y >= ((Control)this).Height - 8)
				{
					((Message)(ref m)).Result = (IntPtr)15;
				}
			}
		}
		else
		{
			((Form)this).WndProc(ref m);
		}
	}

	protected override void OnShown(EventArgs e)
	{
		((Form)this).OnShown(e);
		UpdateRegion();
	}

	protected override void OnResize(EventArgs e)
	{
		((Form)this).OnResize(e);
		UpdateRegion();
	}

	private void UpdateRegion()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		int num = BorderRadius * 2;
		val.StartFigure();
		val.AddArc(0, 0, num, num, 180f, 90f);
		val.AddArc(((Control)this).Width - num, 0, num, num, 270f, 90f);
		val.AddArc(((Control)this).Width - num, ((Control)this).Height - num, num, num, 0f, 90f);
		val.AddArc(0, ((Control)this).Height - num, num, num, 90f, 90f);
		val.CloseFigure();
		((Control)this).Region = new Region(val);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		((Control)this).OnMouseDown(e);
		if ((int)e.Button == 1048576)
		{
			ReleaseCapture();
			SendMessage(((Control)this).Handle, 161, 2, 0);
		}
	}
}
