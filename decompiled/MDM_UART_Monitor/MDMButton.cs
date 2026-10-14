using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MDM_UART_Monitor;

public class MDMButton : Button
{
	private int borderSize = 2;

	private int borderRadius = 12;

	private Color borderColor = Color.Orange;

	private bool enableGradient;

	private Color gradientStartColor = Color.White;

	private Color gradientEndColor = Color.LightSkyBlue;

	private float gradientAngle = 90f;

	private bool enableShadow = true;

	private int shadowSize = 8;

	private Color shadowColor = Color.Black;

	private int shadowOpacity = 35;

	private Color hoverColor = Color.FromArgb(30, 144, 255);

	private bool isHover;

	[Category("MDM UI")]
	public bool EnableGradient
	{
		get
		{
			return enableGradient;
		}
		set
		{
			enableGradient = value;
			((Control)this).Invalidate();
		}
	}

	[Category("MDM UI")]
	public Color GradientStartColor
	{
		get
		{
			return gradientStartColor;
		}
		set
		{
			gradientStartColor = value;
			((Control)this).Invalidate();
		}
	}

	[Category("MDM UI")]
	public Color GradientEndColor
	{
		get
		{
			return gradientEndColor;
		}
		set
		{
			gradientEndColor = value;
			((Control)this).Invalidate();
		}
	}

	[Category("MDM UI")]
	public float GradientAngle
	{
		get
		{
			return gradientAngle;
		}
		set
		{
			gradientAngle = value;
			((Control)this).Invalidate();
		}
	}

	[Category("MDM UI")]
	public bool EnableShadow
	{
		get
		{
			return enableShadow;
		}
		set
		{
			enableShadow = value;
			((Control)this).Invalidate();
		}
	}

	[Category("MDM UI")]
	public int ShadowSize
	{
		get
		{
			return shadowSize;
		}
		set
		{
			shadowSize = value;
			((Control)this).Invalidate();
		}
	}

	[Category("MDM UI")]
	public Color ShadowColor
	{
		get
		{
			return shadowColor;
		}
		set
		{
			shadowColor = value;
			((Control)this).Invalidate();
		}
	}

	[Category("MDM UI")]
	public int ShadowOpacity
	{
		get
		{
			return shadowOpacity;
		}
		set
		{
			shadowOpacity = Math.Max(0, Math.Min(255, value));
			((Control)this).Invalidate();
		}
	}

	[Category("MDM UI")]
	public int BorderSize
	{
		get
		{
			return borderSize;
		}
		set
		{
			borderSize = value;
			((Control)this).Invalidate();
		}
	}

	[Category("MDM UI")]
	public int BorderRadius
	{
		get
		{
			return borderRadius;
		}
		set
		{
			borderRadius = value;
			((Control)this).Invalidate();
		}
	}

	[Category("MDM UI")]
	public Color BorderColor
	{
		get
		{
			return borderColor;
		}
		set
		{
			borderColor = value;
			((Control)this).Invalidate();
		}
	}

	[Category("MDM UI")]
	public Color BackgroundColor
	{
		get
		{
			return ((Control)this).BackColor;
		}
		set
		{
			((Control)this).BackColor = value;
		}
	}

	[Category("MDM UI")]
	public Color TextColor
	{
		get
		{
			return ((Control)this).ForeColor;
		}
		set
		{
			((Control)this).ForeColor = value;
		}
	}

	[Category("MDM UI")]
	public Color HoverColor
	{
		get
		{
			return hoverColor;
		}
		set
		{
			hoverColor = value;
			((Control)this).Invalidate();
		}
	}

	public MDMButton()
	{
		((Control)this).SetStyle((ControlStyles)141330, true);
		((ButtonBase)this).FlatStyle = (FlatStyle)0;
		((ButtonBase)this).FlatAppearance.BorderSize = 0;
		((ButtonBase)this).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)this).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((Control)this).BackColor = Color.FromArgb(0, 120, 215);
		((Control)this).ForeColor = Color.White;
		((Control)this).Cursor = Cursors.Hand;
		((Control)this).Size = new Size(120, 40);
		((Control)this).Resize += Button_Resize;
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Button)this).OnMouseEnter(e);
		isHover = true;
		((Control)this).Invalidate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Button)this).OnMouseLeave(e);
		isHover = false;
		((Control)this).Invalidate();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		e.Graphics.SmoothingMode = (SmoothingMode)4;
		e.Graphics.PixelOffsetMode = (PixelOffsetMode)2;
		e.Graphics.CompositingQuality = (CompositingQuality)2;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.Width--;
		clientRectangle.Height--;
		Color color = (isHover ? hoverColor : ((Control)this).BackColor);
		if (enableShadow)
		{
			Rectangle rect = clientRectangle;
			rect.Offset(2, 2);
			GraphicsPath roundPath = GetRoundPath(rect, borderRadius);
			try
			{
				SolidBrush val = new SolidBrush(Color.FromArgb(shadowOpacity, shadowColor));
				try
				{
					e.Graphics.FillPath((Brush)(object)val, roundPath);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)roundPath)?.Dispose();
			}
		}
		GraphicsPath roundPath2 = GetRoundPath(clientRectangle, borderRadius);
		try
		{
			((Control)this).Region = new Region(roundPath2);
			if (enableGradient)
			{
				LinearGradientBrush val2 = new LinearGradientBrush(clientRectangle, gradientStartColor, gradientEndColor, gradientAngle);
				try
				{
					e.Graphics.FillPath((Brush)(object)val2, roundPath2);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			else
			{
				SolidBrush val3 = new SolidBrush(color);
				try
				{
					e.Graphics.FillPath((Brush)(object)val3, roundPath2);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			if (borderSize > 0)
			{
				Pen val4 = new Pen(borderColor, (float)borderSize);
				try
				{
					val4.Alignment = (PenAlignment)1;
					e.Graphics.DrawPath(val4, roundPath2);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			TextRenderer.DrawText((IDeviceContext)(object)e.Graphics, ((Control)this).Text, ((Control)this).Font, clientRectangle, ((Control)this).ForeColor, (TextFormatFlags)37);
		}
		finally
		{
			((IDisposable)roundPath2)?.Dispose();
		}
	}

	private GraphicsPath GetRoundPath(Rectangle rect, int radius)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		if (radius <= 1)
		{
			val.AddRectangle(rect);
			return val;
		}
		int num = radius * 2;
		val.StartFigure();
		val.AddArc(rect.X, rect.Y, num, num, 180f, 90f);
		val.AddArc(rect.Right - num, rect.Y, num, num, 270f, 90f);
		val.AddArc(rect.Right - num, rect.Bottom - num, num, num, 0f, 90f);
		val.AddArc(rect.X, rect.Bottom - num, num, num, 90f, 90f);
		val.CloseFigure();
		return val;
	}

	private void Button_Resize(object sender, EventArgs e)
	{
		if (borderRadius > ((Control)this).Height / 2)
		{
			borderRadius = ((Control)this).Height / 2;
		}
		((Control)this).Invalidate();
	}
}
