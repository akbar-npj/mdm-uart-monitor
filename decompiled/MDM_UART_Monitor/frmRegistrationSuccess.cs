using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using MDM_UART_Monitor.Properties;

namespace MDM_UART_Monitor;

public class frmRegistrationSuccess : Form
{
	private IContainer components;

	private PictureBox pictureBox1;

	private PictureBox pictureBox2;

	private Label lblTitle;

	private GroupBox groupBox1;

	private Label label3;

	private RichTextBox rtbInstruction;

	private Label label2;

	private TextBox txtToken;

	private Label label1;

	private Panel panel1;

	private Label label5;

	private Label label4;

	private PictureBox pbWhatsapp;

	private Label label6;

	private Label label7;

	private Label label8;

	public string TokenID
	{
		get
		{
			return ((Control)txtToken).Text;
		}
		set
		{
			((Control)txtToken).Text = value;
		}
	}

	public frmRegistrationSuccess()
	{
		InitializeComponent();
	}

	private void pbWhatsapp_Click(object sender, EventArgs e)
	{
		Process.Start("https://wa.me/7001582885");
	}

	private void frmRegistrationSuccess_Load(object sender, EventArgs e)
	{
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Expected O, but got Unknown
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Expected O, but got Unknown
		//IL_0827: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Expected O, but got Unknown
		//IL_08c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d1: Expected O, but got Unknown
		//IL_09d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dd: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmRegistrationSuccess));
		lblTitle = new Label();
		groupBox1 = new GroupBox();
		label7 = new Label();
		label6 = new Label();
		pbWhatsapp = new PictureBox();
		label3 = new Label();
		rtbInstruction = new RichTextBox();
		label2 = new Label();
		pictureBox1 = new PictureBox();
		txtToken = new TextBox();
		label1 = new Label();
		panel1 = new Panel();
		label5 = new Label();
		label4 = new Label();
		pictureBox2 = new PictureBox();
		label8 = new Label();
		((Control)groupBox1).SuspendLayout();
		((ISupportInitialize)pbWhatsapp).BeginInit();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)panel1).SuspendLayout();
		((ISupportInitialize)pictureBox2).BeginInit();
		((Control)this).SuspendLayout();
		((Control)lblTitle).AutoSize = true;
		((Control)lblTitle).BackColor = Color.Transparent;
		((Control)lblTitle).Font = new Font("Segoe UI", 22f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)lblTitle).ForeColor = Color.Chartreuse;
		((Control)lblTitle).Location = new Point(106, 9);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).Size = new Size(199, 82);
		((Control)lblTitle).TabIndex = 2;
		((Control)lblTitle).Text = "Registration \r\nSuccessful";
		((Control)groupBox1).BackColor = Color.Transparent;
		((Control)groupBox1).Controls.Add((Control)(object)label7);
		((Control)groupBox1).Controls.Add((Control)(object)label6);
		((Control)groupBox1).Controls.Add((Control)(object)pbWhatsapp);
		((Control)groupBox1).Controls.Add((Control)(object)label3);
		((Control)groupBox1).Controls.Add((Control)(object)rtbInstruction);
		((Control)groupBox1).Controls.Add((Control)(object)label2);
		((Control)groupBox1).Controls.Add((Control)(object)pictureBox1);
		((Control)groupBox1).Controls.Add((Control)(object)txtToken);
		((Control)groupBox1).Controls.Add((Control)(object)label1);
		((Control)groupBox1).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)groupBox1).ForeColor = Color.AntiqueWhite;
		((Control)groupBox1).Location = new Point(1, 102);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).Size = new Size(501, 377);
		((Control)groupBox1).TabIndex = 3;
		groupBox1.TabStop = false;
		((Control)groupBox1).Text = "Registration Details";
		((Control)label7).AutoSize = true;
		((Control)label7).Location = new Point(76, 328);
		((Control)label7).Name = "label7";
		((Control)label7).Size = new Size(126, 40);
		((Control)label7).TabIndex = 7;
		((Control)label7).Text = "Whatsapp\r\n+917001582885";
		((Control)label6).AutoSize = true;
		((Control)label6).ForeColor = Color.Crimson;
		((Control)label6).Location = new Point(76, 57);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(133, 20);
		((Control)label6).TabIndex = 6;
		((Control)label6).Text = "Payment Pending";
		((Control)pbWhatsapp).BackColor = Color.Transparent;
		((Control)pbWhatsapp).BackgroundImage = (Image)(object)Resources.WhatsApp_icon__2_;
		((Control)pbWhatsapp).BackgroundImageLayout = (ImageLayout)3;
		((Control)pbWhatsapp).Cursor = Cursors.Hand;
		((Control)pbWhatsapp).Location = new Point(7, 320);
		((Control)pbWhatsapp).Name = "pbWhatsapp";
		((Control)pbWhatsapp).Size = new Size(56, 57);
		pbWhatsapp.TabIndex = 5;
		pbWhatsapp.TabStop = false;
		((Control)pbWhatsapp).Click += pbWhatsapp_Click;
		((Control)label3).AutoSize = true;
		((Control)label3).Font = new Font("Microsoft Sans Serif", 16f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label3).ForeColor = Color.Aqua;
		((Control)label3).Location = new Point(270, 51);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(205, 26);
		((Control)label3).TabIndex = 5;
		((Control)label3).Text = "Payment Instruction";
		((Control)rtbInstruction).BackColor = Color.FromArgb(20, 20, 35);
		((TextBoxBase)rtbInstruction).BorderStyle = (BorderStyle)0;
		((Control)rtbInstruction).Font = new Font("Microsoft Sans Serif", 10f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)rtbInstruction).ForeColor = Color.PaleGoldenrod;
		((Control)rtbInstruction).Location = new Point(246, 99);
		((Control)rtbInstruction).Name = "rtbInstruction";
		((Control)rtbInstruction).Size = new Size(255, 272);
		((Control)rtbInstruction).TabIndex = 4;
		((Control)rtbInstruction).Text = "STEP-1\nScan QR Code\n\nSTEP-2\nPay Rs 500 \n\nSTEP-3\nSend Payment Screenshot\n\nSTEP-4\nCopy And Send Token ID\n\nSTEP-5\nWait for Verification\n\nLogin will be enabled After verification\n";
		((Control)label2).AutoSize = true;
		((Control)label2).Location = new Point(6, 57);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(64, 20);
		((Control)label2).TabIndex = 2;
		((Control)label2).Text = "Status: ";
		((Control)pictureBox1).BackgroundImage = (Image)(object)Resources.MDM_UPI;
		((Control)pictureBox1).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox1).Location = new Point(6, 80);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(241, 237);
		pictureBox1.TabIndex = 0;
		pictureBox1.TabStop = false;
		((Control)txtToken).BackColor = Color.Black;
		((TextBoxBase)txtToken).BorderStyle = (BorderStyle)1;
		((Control)txtToken).ForeColor = Color.White;
		((Control)txtToken).Location = new Point(86, 28);
		((Control)txtToken).Name = "txtToken";
		((TextBoxBase)txtToken).ReadOnly = true;
		((Control)txtToken).Size = new Size(134, 26);
		((Control)txtToken).TabIndex = 1;
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(6, 31);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(74, 20);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "Token ID";
		((Control)panel1).BackColor = Color.FromArgb(20, 20, 40);
		((Control)panel1).Controls.Add((Control)(object)label5);
		((Control)panel1).Controls.Add((Control)(object)label4);
		((Control)panel1).Location = new Point(300, 5);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(200, 100);
		((Control)panel1).TabIndex = 4;
		((Control)label5).AutoSize = true;
		((Control)label5).BackColor = Color.Transparent;
		((Control)label5).Font = new Font("Microsoft Sans Serif", 16f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label5).ForeColor = Color.White;
		((Control)label5).Location = new Point(-5, 60);
		((Control)label5).Name = "label5";
		((Control)label5).Size = new Size(202, 26);
		((Control)label5).TabIndex = 6;
		((Control)label5).Text = " LifeTime Activation";
		((Control)label4).AutoSize = true;
		((Control)label4).BackColor = Color.Transparent;
		((Control)label4).Font = new Font("Microsoft Sans Serif", 32f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label4).ForeColor = Color.Aqua;
		((Control)label4).Location = new Point(23, 11);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(164, 51);
		((Control)label4).TabIndex = 6;
		((Control)label4).Text = "Rs 500";
		((Control)pictureBox2).BackColor = Color.Transparent;
		((Control)pictureBox2).BackgroundImage = (Image)(object)Resources.Green_Tick;
		((Control)pictureBox2).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox2).Location = new Point(12, 9);
		((Control)pictureBox2).Name = "pictureBox2";
		((Control)pictureBox2).Size = new Size(88, 87);
		pictureBox2.TabIndex = 1;
		pictureBox2.TabStop = false;
		((Control)label8).AutoSize = true;
		((Control)label8).Font = new Font("Microsoft Sans Serif", 11f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label8).ForeColor = Color.ForestGreen;
		((Control)label8).Location = new Point(32, 482);
		((Control)label8).Name = "label8";
		((Control)label8).Size = new Size(444, 180);
		((Control)label8).TabIndex = 5;
		((Control)label8).Text = componentResourceManager.GetString("label8.Text");
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Black;
		((Form)this).ClientSize = new Size(504, 673);
		((Control)this).Controls.Add((Control)(object)label8);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)groupBox1);
		((Control)this).Controls.Add((Control)(object)lblTitle);
		((Control)this).Controls.Add((Control)(object)pictureBox2);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmRegistrationSuccess";
		((Form)this).ShowIcon = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Registration Successful";
		((Form)this).TopMost = true;
		((Form)this).Load += frmRegistrationSuccess_Load;
		((Control)groupBox1).ResumeLayout(false);
		((Control)groupBox1).PerformLayout();
		((ISupportInitialize)pbWhatsapp).EndInit();
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)panel1).ResumeLayout(false);
		((Control)panel1).PerformLayout();
		((ISupportInitialize)pictureBox2).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
