using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MDM_UART_Monitor;

[DefaultEvent("SelectedIndexChanged")]
public class DarkComboBox : UserControl
{
	private readonly Label lblText;

	private readonly Panel pnlArrow;

	private readonly Form dropDownForm;

	private readonly ListBox listBox;

	private readonly List<object> items = new List<object>();

	private bool droppedDown;

	private int hoverIndex = -1;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ObjectCollection Items => listBox.Items;

	public int SelectedIndex
	{
		get
		{
			return ((ListControl)listBox).SelectedIndex;
		}
		set
		{
			((ListControl)listBox).SelectedIndex = value;
		}
	}

	public object SelectedItem => listBox.SelectedItem;

	public override string Text
	{
		get
		{
			return ((Control)lblText).Text;
		}
		set
		{
			((Control)lblText).Text = value;
		}
	}

	public event EventHandler SelectedIndexChanged;

	public DarkComboBox()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Expected O, but got Unknown
		((Control)this).DoubleBuffered = true;
		((Control)this).Height = 34;
		((Control)this).Width = 180;
		((Control)this).Font = new Font("Segoe UI", 9f);
		((Control)this).BackColor = Color.FromArgb(35, 35, 38);
		lblText = new Label();
		((Control)lblText).Dock = (DockStyle)5;
		((Control)lblText).ForeColor = Color.White;
		((Control)lblText).BackColor = Color.Transparent;
		lblText.TextAlign = (ContentAlignment)16;
		((Control)lblText).Padding = new Padding(10, 0, 0, 0);
		pnlArrow = new Panel();
		((Control)pnlArrow).Dock = (DockStyle)4;
		((Control)pnlArrow).Width = 34;
		((Control)pnlArrow).Cursor = Cursors.Hand;
		((Control)pnlArrow).Paint += new PaintEventHandler(Arrow_Paint);
		listBox = new ListBox();
		listBox.BorderStyle = (BorderStyle)0;
		((Control)listBox).Dock = (DockStyle)5;
		((Control)listBox).BackColor = Color.FromArgb(45, 45, 48);
		((Control)listBox).ForeColor = Color.White;
		((Control)listBox).Font = ((Control)this).Font;
		listBox.IntegralHeight = false;
		listBox.Click += ListBox_Click;
		dropDownForm = new Form();
		dropDownForm.FormBorderStyle = (FormBorderStyle)0;
		dropDownForm.ShowInTaskbar = false;
		dropDownForm.StartPosition = (FormStartPosition)0;
		dropDownForm.TopMost = true;
		((Control)dropDownForm).BackColor = Color.FromArgb(45, 45, 48);
		((Control)dropDownForm).Controls.Add((Control)(object)listBox);
		((Control)this).Controls.Add((Control)(object)lblText);
		((Control)this).Controls.Add((Control)(object)pnlArrow);
		((Control)this).Click += ToggleDropDown;
		((Control)lblText).Click += ToggleDropDown;
		((Control)pnlArrow).Click += ToggleDropDown;
		listBox.DrawMode = (DrawMode)1;
		listBox.ItemHeight = 24;
		listBox.DrawItem += new DrawItemEventHandler(ListBox_DrawItem);
		((Control)listBox).MouseMove += new MouseEventHandler(ListBox_MouseMove);
		((Control)listBox).MouseLeave += ListBox_MouseLeave;
		dropDownForm.Deactivate += DropDownForm_Deactivate;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		e.Graphics.SmoothingMode = (SmoothingMode)4;
		GraphicsPath val = RoundedRect(((Control)this).ClientRectangle, 6);
		try
		{
			SolidBrush val2 = new SolidBrush(((Control)this).BackColor);
			try
			{
				Pen val3 = new Pen(Color.DodgerBlue);
				try
				{
					e.Graphics.FillPath((Brush)(object)val2, val);
					e.Graphics.DrawPath(val3, val);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnParentChanged(EventArgs e)
	{
		((ContainerControl)this).OnParentChanged(e);
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			((Control)val).Move -= ParentForm_Move;
			((Control)val).Move += ParentForm_Move;
		}
	}

	private Form GetParentForm()
	{
		return ((Control)this).FindForm();
	}

	private void ParentForm_Move(object sender, EventArgs e)
	{
		if (droppedDown && ((Control)dropDownForm).Visible)
		{
			Point location = ((Control)this).PointToScreen(new Point(0, ((Control)this).Height));
			dropDownForm.Location = location;
		}
	}

	private void DropDownForm_Deactivate(object sender, EventArgs e)
	{
		if (droppedDown)
		{
			((Control)dropDownForm).Hide();
			droppedDown = false;
		}
	}

	private void Arrow_Paint(object sender, PaintEventArgs e)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		e.Graphics.SmoothingMode = (SmoothingMode)4;
		int num = ((Control)pnlArrow).Width / 2;
		int num2 = ((Control)pnlArrow).Height / 2;
		Point[] array = new Point[3]
		{
			new Point(num - 5, num2 - 2),
			new Point(num, num2 + 3),
			new Point(num + 5, num2 - 2)
		};
		Pen val = new Pen(Color.White, 2f);
		try
		{
			e.Graphics.DrawLines(val, array);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ToggleDropDown(object sender, EventArgs e)
	{
		if (droppedDown)
		{
			CloseDropDown();
			return;
		}
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			dropDownForm.Owner = val;
		}
		Point location = ((Control)this).PointToScreen(new Point(0, ((Control)this).Height));
		dropDownForm.Location = location;
		((Control)dropDownForm).Width = ((Control)this).Width;
		((Control)dropDownForm).Height = Math.Min(200, Math.Max(35, listBox.Items.Count * 24 + 4));
		((Control)dropDownForm).Show();
		droppedDown = true;
	}

	private void CloseDropDown()
	{
		if (droppedDown)
		{
			((Control)dropDownForm).Hide();
			droppedDown = false;
			hoverIndex = -1;
			((Control)listBox).Invalidate();
		}
	}

	private void ListBox_MouseMove(object sender, MouseEventArgs e)
	{
		int num = listBox.IndexFromPoint(e.Location);
		if (num != hoverIndex)
		{
			hoverIndex = num;
			((Control)listBox).Invalidate();
		}
	}

	private void ListBox_MouseLeave(object sender, EventArgs e)
	{
		hoverIndex = -1;
		((Control)listBox).Invalidate();
	}

	private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		if (e.Index >= 0)
		{
			Graphics graphics = e.Graphics;
			bool num = (e.State & 1) == 1;
			bool flag = e.Index == hoverIndex;
			Color color = (num ? Color.FromArgb(0, 122, 204) : ((!flag) ? Color.FromArgb(45, 45, 48) : Color.FromArgb(60, 60, 65)));
			SolidBrush val = new SolidBrush(color);
			try
			{
				graphics.FillRectangle((Brush)(object)val, e.Bounds);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			TextRenderer.DrawText((IDeviceContext)(object)graphics, listBox.Items[e.Index].ToString(), ((Control)listBox).Font, new Rectangle(e.Bounds.X + 8, e.Bounds.Y, e.Bounds.Width - 8, e.Bounds.Height), Color.White, (TextFormatFlags)4);
		}
	}

	private void ListBox_Click(object sender, EventArgs e)
	{
		if (listBox.SelectedItem != null)
		{
			((Control)lblText).Text = listBox.SelectedItem.ToString();
			((Control)dropDownForm).Hide();
			droppedDown = false;
			this.SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
		}
	}

	private GraphicsPath RoundedRect(Rectangle r, int radius)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		int num = radius * 2;
		val.AddArc(r.X, r.Y, num, num, 180f, 90f);
		val.AddArc(r.Right - num, r.Y, num, num, 270f, 90f);
		val.AddArc(r.Right - num, r.Bottom - num, num, num, 0f, 90f);
		val.AddArc(r.X, r.Bottom - num, num, num, 90f, 90f);
		val.CloseFigure();
		return val;
	}
}
