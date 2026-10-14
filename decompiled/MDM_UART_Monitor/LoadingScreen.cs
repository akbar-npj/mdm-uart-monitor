using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MDM_UART_Monitor.Properties;

namespace MDM_UART_Monitor;

public class LoadingScreen : RoundForm
{
	private int progress;

	private IContainer components;

	private PictureBox pictureBox1;

	private MDMPanel mdmPanel1;

	private MDMPanel PanelProgress;

	private Timer timerLoading;

	public LoadingScreen()
	{
		InitializeComponent();
	}

	private void LoadingScreen_Load(object sender, EventArgs e)
	{
		((Control)PanelProgress).Width = 0;
		progress = 0;
		timerLoading.Start();
	}

	private void timerLoading_Tick(object sender, EventArgs e)
	{
		progress++;
		if (progress > 100)
		{
			progress = 100;
		}
		((Control)PanelProgress).Width = progress * 543 / 100;
		if (progress >= 100)
		{
			timerLoading.Stop();
			((Control)new Form1()).Show();
			((Form)this).Close();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		components = new Container();
		pictureBox1 = new PictureBox();
		mdmPanel1 = new MDMPanel();
		PanelProgress = new MDMPanel();
		timerLoading = new Timer(components);
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)mdmPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)pictureBox1).BackgroundImage = (Image)(object)Resources.MDM_Loading;
		((Control)pictureBox1).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox1).Location = new Point(-1, -8);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(786, 569);
		pictureBox1.TabIndex = 0;
		pictureBox1.TabStop = false;
		mdmPanel1.AnimationSpeed = 1;
		((Control)mdmPanel1).BackColor = Color.FromArgb(7, 13, 24);
		mdmPanel1.BorderColor = Color.SteelBlue;
		mdmPanel1.BorderRadius = 15;
		mdmPanel1.BorderSize = 1;
		((Control)mdmPanel1).Controls.Add((Control)(object)PanelProgress);
		mdmPanel1.EnableGradient = false;
		mdmPanel1.EnableShadow = false;
		mdmPanel1.GradientAngle = 90f;
		mdmPanel1.GradientEndColor = Color.LightSkyBlue;
		mdmPanel1.GradientStartColor = Color.White;
		mdmPanel1.HoverBorderSize = 3;
		((Control)mdmPanel1).Location = new Point(117, 490);
		((Control)mdmPanel1).Name = "mdmPanel1";
		mdmPanel1.ShadowOpacity = 35;
		mdmPanel1.ShadowSize = 8;
		((Control)mdmPanel1).Size = new Size(541, 33);
		((Control)mdmPanel1).TabIndex = 1;
		PanelProgress.AnimationSpeed = 1;
		((Control)PanelProgress).BackColor = Color.FromArgb(7, 13, 24);
		PanelProgress.BorderColor = Color.SteelBlue;
		PanelProgress.BorderRadius = 15;
		PanelProgress.BorderSize = 1;
		PanelProgress.EnableGradient = true;
		PanelProgress.EnableShadow = false;
		PanelProgress.GradientAngle = 90f;
		PanelProgress.GradientEndColor = Color.RoyalBlue;
		PanelProgress.GradientStartColor = Color.DarkTurquoise;
		PanelProgress.HoverBorderSize = 3;
		((Control)PanelProgress).Location = new Point(-1, -1);
		((Control)PanelProgress).Name = "PanelProgress";
		PanelProgress.ShadowOpacity = 35;
		PanelProgress.ShadowSize = 8;
		((Control)PanelProgress).Size = new Size(374, 33);
		((Control)PanelProgress).TabIndex = 2;
		timerLoading.Interval = 20;
		timerLoading.Tick += timerLoading_Tick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(783, 555);
		((Control)this).Controls.Add((Control)(object)mdmPanel1);
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).MinimumSize = new Size(0, 0);
		((Control)this).Name = "LoadingScreen";
		((Control)this).Text = "LoadingScreen";
		((Form)this).Load += LoadingScreen_Load;
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)mdmPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
