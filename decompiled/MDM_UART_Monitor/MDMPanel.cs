using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MDM_UART_Monitor;

public class MDMPanel : Panel
{
	private int borderRadius = 15;

	private int borderSize = 1;

	private Color borderColor = Color.SteelBlue;

	private bool enableShadow;

	private int shadowSize = 8;

	private Color shadowColor = Color.Black;

	private int shadowOpacity = 35;

	private bool enableGradient;

	private Color gradientStartColor = Color.White;

	private Color gradientEndColor = Color.LightSkyBlue;

	private float gradientAngle = 90f;

	private Timer animationTimer;

	private bool hover;

	private int animatedBorderSize = 1;

	private int hoverBorderSize = 3;

	private int animationSpeed = 1;

	[Category("MDM UI")]
	public int HoverBorderSize
	{
		get
		{
			return hoverBorderSize;
		}
		set
		{
			hoverBorderSize = value;
		}
	}

	[Category("MDM UI")]
	public int AnimationSpeed
	{
		get
		{
			return animationSpeed;
		}
		set
		{
			animationSpeed = value;
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
			shadowSize = Math.Max(1, value);
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

	public MDMPanel()
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		((Control)this).DoubleBuffered = true;
		((Control)this).Resize += MDMPanel_Resize;
		animationTimer = new Timer();
		animationTimer.Interval = 15;
		animationTimer.Tick += AnimationTimer_Tick;
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)139282, true);
		((Control)this).UpdateStyles();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Expected O, but got Unknown
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Expected O, but got Unknown
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		if (((Control)this).Width <= 1 || ((Control)this).Height <= 1)
		{
			return;
		}
		if (enableShadow)
		{
			Rectangle rect = new Rectangle(shadowSize, shadowSize, ((Control)this).Width - shadowSize - 1, ((Control)this).Height - shadowSize - 1);
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
		((Control)this).OnPaint(e);
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Rectangle rect2 = Rectangle.Inflate(clientRectangle, -borderSize, -borderSize);
		int num = ((borderSize > 0) ? borderSize : 2);
		e.Graphics.SmoothingMode = (SmoothingMode)4;
		if (borderRadius > 2)
		{
			GraphicsPath roundPath2 = GetRoundPath(clientRectangle, borderRadius);
			try
			{
				GraphicsPath roundPath3 = GetRoundPath(rect2, Math.Max(borderRadius - borderSize, 1));
				try
				{
					Control parent = ((Control)this).Parent;
					Pen val2 = new Pen((parent != null) ? parent.BackColor : SystemColors.Control, (float)num);
					try
					{
						Pen val3 = new Pen(borderColor, (float)animatedBorderSize);
						try
						{
							((Control)this).Region = new Region(roundPath2);
							Brush val4 = (Brush)((!enableGradient) ? ((object)new SolidBrush(((Control)this).BackColor)) : ((object)new LinearGradientBrush(clientRectangle, gradientStartColor, gradientEndColor, gradientAngle)));
							Brush val5 = val4;
							try
							{
								e.Graphics.FillPath(val4, roundPath2);
							}
							finally
							{
								((IDisposable)val5)?.Dispose();
							}
							e.Graphics.DrawPath(val2, roundPath2);
							if (borderSize > 0)
							{
								e.Graphics.DrawPath(val3, roundPath3);
							}
							return;
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
					((IDisposable)roundPath3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)roundPath2)?.Dispose();
			}
		}
		((Control)this).Region = new Region(clientRectangle);
		SolidBrush val6 = new SolidBrush(((Control)this).BackColor);
		try
		{
			e.Graphics.FillRectangle((Brush)(object)val6, clientRectangle);
		}
		finally
		{
			((IDisposable)val6)?.Dispose();
		}
		if (borderSize <= 0)
		{
			return;
		}
		Pen val7 = new Pen(borderColor, (float)borderSize);
		try
		{
			val7.Alignment = (PenAlignment)1;
			e.Graphics.DrawRectangle(val7, 0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
		}
		finally
		{
			((IDisposable)val7)?.Dispose();
		}
	}

	private GraphicsPath GetRoundPath(Rectangle rect, int radius)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		if (rect.Width <= 1 || rect.Height <= 1)
		{
			return val;
		}
		radius = Math.Max(0, radius);
		radius = Math.Min(radius, Math.Min(rect.Width, rect.Height) / 2);
		if (radius == 0)
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

	protected override void OnMouseEnter(EventArgs e)
	{
		hover = true;
		animationTimer.Start();
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		hover = false;
		animationTimer.Start();
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		if (((Control)this).Parent != null)
		{
			((Control)this).Parent.BackColorChanged += Parent_BackColorChanged;
		}
	}

	private void Parent_BackColorChanged(object sender, EventArgs e)
	{
		((Control)this).Invalidate();
	}

	private void UpdateRegion()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		GraphicsPath roundPath = GetRoundPath(((Control)this).ClientRectangle, borderRadius);
		try
		{
			Region region = ((Control)this).Region;
			if (region != null)
			{
				region.Dispose();
			}
			((Control)this).Region = new Region(roundPath);
		}
		finally
		{
			((IDisposable)roundPath)?.Dispose();
		}
	}

	private void MDMPanel_Resize(object sender, EventArgs e)
	{
		if (borderRadius > ((Control)this).Height)
		{
			borderRadius = ((Control)this).Height;
		}
		UpdateRegion();
		((Control)this).Invalidate();
	}

	private void AnimationTimer_Tick(object sender, EventArgs e)
	{
		if (hover)
		{
			if (animatedBorderSize < hoverBorderSize)
			{
				animatedBorderSize += animationSpeed;
			}
			else
			{
				animationTimer.Stop();
			}
		}
		else if (animatedBorderSize > borderSize)
		{
			animatedBorderSize -= animationSpeed;
		}
		else
		{
			animationTimer.Stop();
		}
		((Control)this).Invalidate();
	}
}
