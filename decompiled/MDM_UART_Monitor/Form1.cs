using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using MDM_UART_Monitor.Properties;

namespace MDM_UART_Monitor;

public class Form1 : RoundForm
{
	private int garbageCount;

	private const int GarbageThreshold = 20;

	private int searchIndex = -1;

	private List<int> searchResults = new List<int>();

	private readonly ConcurrentQueue<string> receiveQueue = new ConcurrentQueue<string>();

	private readonly StringBuilder displayBuffer = new StringBuilder();

	private const int WM_NCLBUTTONDOWN = 161;

	private const int HTCAPTION = 2;

	private IContainer components;

	private Label label1;

	private Label label2;

	private SerialPort serialPort1;

	private Timer timerReceive;

	private NotifyIcon notifyIcon1;

	private FastColoredTextBox txtLog;

	private Label lblStatus;

	private Label label3;

	private TextBox txtSearch;

	private Button btnFindNext;

	private Button btnFindPrevious;

	private Button btnClearSearch;

	private Label lblSearchCount;

	private MDMButton btnConnect;

	private MDMButton btnDisconnect;

	private MDMButton btnCopyLogs;

	private MDMButton btnClear;

	private PictureBox pictureBox1;

	private MDMButton btnSave;

	private MDMPanel mdmPanel3;

	private TextBox txtSendCommand;

	private MDMPanel mdmPanel4;

	private MDMButton btnMinimizeForm;

	private MDMButton btnMaximizeForm;

	private MDMButton btnCloseForm;

	private PictureBox pbBanner;

	private MDMButton btnSendCommand;

	private MDMPanel TitlePanel;

	private MDMPanel mdmPanel2;

	private MDMPanel mdmPanel1;

	private DarkComboBox cmbPort;

	private DarkComboBox cmbBaud;

	private MDMPanel mdmPanel5;

	private Label label5;

	private Label lblCommandHint;

	private PictureBox picRxWarning;

	private Timer timerRxWarning;

	private TrackBar trackBarFontSize;

	private Label labelLogTextSize;

	private Label label4;

	[DllImport("user32.dll")]
	public static extern bool ReleaseCapture();

	[DllImport("user32.dll")]
	public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

	public Form1()
	{
		InitializeComponent();
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		txtLog.ReadOnly = true;
		txtLog.WordWrap = false;
		txtLog.ShowLineNumbers = false;
		txtLog.AutoIndent = false;
		((Control)txtLog).Text = "";
		((Control)lblStatus).Text = "● Disconnected";
		((Control)lblStatus).ForeColor = Color.Red;
		LoadBaudRate();
		RefreshComPort();
		EnableDrag((Control)(object)pbBanner);
		((Control)btnDisconnect).Enabled = false;
		((Control)lblCommandHint).Visible = string.IsNullOrWhiteSpace(((Control)txtSendCommand).Text);
	}

	private void EnableDrag(Control control)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		control.MouseDown += (MouseEventHandler)delegate(object s, MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Invalid comparison between Unknown and I4
			if ((int)e.Button == 1048576)
			{
				ReleaseCapture();
				SendMessage(((Control)this).Handle, 161, 2, 0);
			}
		};
	}

	private void AppendLog(string text)
	{
		if (!string.IsNullOrEmpty(text))
		{
			txtLog.AppendText(text);
			HighlightEngine.Highlight(txtLog);
			txtLog.GoEnd();
			txtLog.DoCaretVisible();
		}
	}

	private void LoadBaudRate()
	{
		string[] array = new string[8] { "9600", "19200", "38400", "57600", "115200", "230400", "460800", "921600" };
		cmbBaud.Items.Clear();
		ObjectCollection items = cmbBaud.Items;
		object[] array2 = array;
		items.AddRange(array2);
		cmbBaud.SelectedIndex = 4;
	}

	private void RefreshComPort()
	{
		cmbPort.Items.Clear();
		string[] portNames = SerialPort.GetPortNames();
		Array.Sort(portNames);
		ObjectCollection items = cmbPort.Items;
		object[] array = portNames;
		items.AddRange(array);
		if (cmbPort.Items.Count == 0)
		{
			((Control)btnConnect).Enabled = false;
		}
		else
		{
			((Control)btnConnect).Enabled = true;
		}
	}

	private void ConnectPort()
	{
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrWhiteSpace(((Control)cmbPort).Text))
		{
			MessageBox.Show("Please select a COM Port.", "MDM UART Monitor", (MessageBoxButtons)0, (MessageBoxIcon)48);
			((Control)cmbPort).Focus();
			return;
		}
		if (string.IsNullOrWhiteSpace(((Control)cmbBaud).Text))
		{
			MessageBox.Show("Please select a Baud Rate.", "MDM UART Monitor", (MessageBoxButtons)0, (MessageBoxIcon)48);
			((Control)cmbBaud).Focus();
			return;
		}
		try
		{
			if (!serialPort1.IsOpen)
			{
				serialPort1.PortName = ((Control)cmbPort).Text;
				serialPort1.BaudRate = Convert.ToInt32(((Control)cmbBaud).Text);
				serialPort1.DataBits = 8;
				serialPort1.Parity = (Parity)0;
				serialPort1.StopBits = (StopBits)1;
				serialPort1.Open();
				timerReceive.Start();
				((Control)btnConnect).Enabled = false;
				((Control)btnDisconnect).Enabled = true;
				((Control)cmbPort).Enabled = false;
				((Control)cmbBaud).Enabled = false;
				((Control)lblStatus).Text = "● Connected";
				((Control)lblStatus).ForeColor = Color.LimeGreen;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Connection Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
			((Control)lblStatus).Text = "● Connection Failed";
			((Control)lblStatus).ForeColor = Color.OrangeRed;
		}
	}

	private void DisconnectPort()
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (serialPort1.IsOpen)
			{
				timerReceive.Stop();
				serialPort1.Close();
				((Control)btnConnect).Enabled = true;
				((Control)btnDisconnect).Enabled = false;
				((Control)cmbPort).Enabled = true;
				((Control)cmbBaud).Enabled = true;
				((Control)lblStatus).Text = "● Disconnected";
				((Control)lblStatus).ForeColor = Color.Red;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Disconnect Error");
			((Control)lblStatus).ForeColor = Color.OrangeRed;
		}
	}

	private void btnConnect_Click(object sender, EventArgs e)
	{
		ConnectPort();
	}

	private void btnDisconnect_Click(object sender, EventArgs e)
	{
		DisconnectPort();
	}

	private void Form1_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (serialPort1.IsOpen)
		{
			serialPort1.Close();
		}
	}

	private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		string text = serialPort1.ReadExisting();
		if (!string.IsNullOrEmpty(text))
		{
			receiveQueue.Enqueue(text);
		}
	}

	private void btnClear_Click(object sender, EventArgs e)
	{
		txtLog.Clear();
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		if (txtLog.TextLength == 0)
		{
			MessageBox.Show("Log is empty.");
			return;
		}
		SaveFileDialog val = new SaveFileDialog();
		((FileDialog)val).Filter = "Text File (*.txt)|*.txt";
		((FileDialog)val).DefaultExt = "txt";
		((FileDialog)val).FileName = "UART_Log_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
		if ((int)((CommonDialog)val).ShowDialog() == 1)
		{
			File.WriteAllText(((FileDialog)val).FileName, ((Control)txtLog).Text);
			MessageBox.Show("Log saved successfully.");
		}
	}

	private void timerReceive_Tick(object sender, EventArgs e)
	{
		string result;
		while (receiveQueue.TryDequeue(out result))
		{
			if (IsGarbageData(result))
			{
				((Control)picRxWarning).Visible = true;
				((Control)picRxWarning).BringToFront();
				timerRxWarning.Stop();
				timerRxWarning.Start();
			}
			displayBuffer.Append(result);
		}
		if (displayBuffer.Length != 0)
		{
			AppendLog(displayBuffer.ToString());
			displayBuffer.Clear();
		}
	}

	private void cmbPort_Click(object sender, EventArgs e)
	{
		RefreshComPort();
	}

	private void btnCopyLogs_Click(object sender, EventArgs e)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrWhiteSpace(((Control)txtLog).Text))
		{
			MessageBox.Show("No log available to copy.", "", (MessageBoxButtons)0, (MessageBoxIcon)64);
			return;
		}
		Clipboard.SetText(((Control)txtLog).Text);
		MessageBox.Show("Logs copied to clipboard.", "r", (MessageBoxButtons)0, (MessageBoxIcon)64);
	}

	private void txtSearch_TextChanged(object sender, EventArgs e)
	{
		searchResults.Clear();
		searchIndex = -1;
		string text = ((Control)txtSearch).Text;
		if (string.IsNullOrWhiteSpace(text))
		{
			((Control)lblSearchCount).Text = "0/0";
			return;
		}
		int startIndex = 0;
		while (true)
		{
			int num = ((Control)txtLog).Text.IndexOf(text, startIndex, StringComparison.OrdinalIgnoreCase);
			if (num < 0)
			{
				break;
			}
			searchResults.Add(num);
			startIndex = num + text.Length;
		}
		((Control)lblSearchCount).Text = "0/" + searchResults.Count;
	}

	private void btnFindNext_Click(object sender, EventArgs e)
	{
		if (searchResults.Count != 0)
		{
			searchIndex++;
			if (searchIndex >= searchResults.Count)
			{
				searchIndex = 0;
			}
			int selectionStart = searchResults[searchIndex];
			txtLog.SelectionStart = selectionStart;
			txtLog.SelectionLength = ((Control)txtSearch).Text.Length;
			txtLog.DoSelectionVisible();
			((Control)lblSearchCount).Text = searchIndex + 1 + "/" + searchResults.Count;
		}
	}

	private void btnFindPrevious_Click(object sender, EventArgs e)
	{
		if (searchResults.Count != 0)
		{
			searchIndex--;
			if (searchIndex < 0)
			{
				searchIndex = searchResults.Count - 1;
			}
			int selectionStart = searchResults[searchIndex];
			txtLog.SelectionStart = selectionStart;
			txtLog.SelectionLength = ((Control)txtSearch).Text.Length;
			txtLog.DoSelectionVisible();
			((Control)lblSearchCount).Text = searchIndex + 1 + "/" + searchResults.Count;
		}
	}

	private void btnClearSearch_Click(object sender, EventArgs e)
	{
		((TextBoxBase)txtSearch).Clear();
		searchResults.Clear();
		searchIndex = -1;
		((Control)lblSearchCount).Text = "0/0";
		txtLog.SelectionLength = 0;
	}

	private void txtSearch_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			if (e.Shift)
			{
				btnFindPrevious.PerformClick();
			}
			else
			{
				btnFindNext.PerformClick();
			}
			e.SuppressKeyPress = true;
		}
	}

	private void btnMinimizeForm_Click(object sender, EventArgs e)
	{
		((Form)this).WindowState = (FormWindowState)1;
	}

	private void btnMaximizeForm_Click(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)((Form)this).WindowState == 2)
		{
			((Form)this).WindowState = (FormWindowState)0;
		}
		else
		{
			((Form)this).WindowState = (FormWindowState)2;
		}
	}

	private void btnCloseForm_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void btnSendCommand_Click(object sender, EventArgs e)
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrWhiteSpace(((Control)txtSendCommand).Text))
		{
			MessageBox.Show("Please enter a command.", "MDM UART Monitor", (MessageBoxButtons)0, (MessageBoxIcon)48);
			((Control)txtSendCommand).Focus();
			return;
		}
		if (!serialPort1.IsOpen)
		{
			MessageBox.Show("Please connect to a COM Port first.", "MDM UART Monitor", (MessageBoxButtons)0, (MessageBoxIcon)48);
			return;
		}
		try
		{
			serialPort1.Write(((Control)txtSendCommand).Text);
			((TextBoxBase)txtSendCommand).Clear();
			((Control)txtSendCommand).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Send Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void lblCommandHint_Click(object sender, EventArgs e)
	{
		((Control)txtSendCommand).Focus();
	}

	private void txtSendCommand_TextChanged(object sender, EventArgs e)
	{
		((Control)lblCommandHint).Visible = string.IsNullOrWhiteSpace(((Control)txtSendCommand).Text);
	}

	private bool IsGarbageData(string data)
	{
		if (string.IsNullOrEmpty(data))
		{
			return false;
		}
		int num = 0;
		int num2 = 0;
		foreach (char c in data)
		{
			if (c == '?')
			{
				num++;
			}
			if (c < ' ' && c != '\r' && c != '\n' && c != '\t')
			{
				num2++;
			}
		}
		if (num >= 2 && num2 >= 1)
		{
			return true;
		}
		return false;
	}

	private void timerRxWarning_Tick(object sender, EventArgs e)
	{
		timerRxWarning.Stop();
		((Control)picRxWarning).Visible = false;
	}

	private void trackBarFontSize_Scroll(object sender, EventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		((Control)txtLog).Font = new Font(((Control)txtLog).Font.FontFamily, (float)trackBarFontSize.Value, ((Control)txtLog).Font.Style);
		int value = trackBarFontSize.Value;
		((Control)labelLogTextSize).Text = $"{value}";
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Expected O, but got Unknown
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Expected O, but got Unknown
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Expected O, but got Unknown
		//IL_0f4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f59: Expected O, but got Unknown
		//IL_1000: Unknown result type (might be due to invalid IL or missing references)
		//IL_100a: Expected O, but got Unknown
		//IL_10d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_10de: Expected O, but got Unknown
		//IL_11a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b2: Expected O, but got Unknown
		//IL_1305: Unknown result type (might be due to invalid IL or missing references)
		//IL_130f: Expected O, but got Unknown
		//IL_1356: Unknown result type (might be due to invalid IL or missing references)
		//IL_1420: Unknown result type (might be due to invalid IL or missing references)
		//IL_142a: Expected O, but got Unknown
		//IL_14d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_14dd: Expected O, but got Unknown
		//IL_155c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1566: Expected O, but got Unknown
		//IL_1592: Unknown result type (might be due to invalid IL or missing references)
		//IL_159c: Expected O, but got Unknown
		//IL_1633: Unknown result type (might be due to invalid IL or missing references)
		//IL_163d: Expected O, but got Unknown
		//IL_1802: Unknown result type (might be due to invalid IL or missing references)
		//IL_180c: Expected O, but got Unknown
		//IL_19f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_19fc: Expected O, but got Unknown
		//IL_1be2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bec: Expected O, but got Unknown
		//IL_265e: Unknown result type (might be due to invalid IL or missing references)
		//IL_267f: Unknown result type (might be due to invalid IL or missing references)
		//IL_26f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_26fc: Expected O, but got Unknown
		//IL_28be: Unknown result type (might be due to invalid IL or missing references)
		//IL_28c8: Expected O, but got Unknown
		//IL_2952: Unknown result type (might be due to invalid IL or missing references)
		//IL_295c: Expected O, but got Unknown
		//IL_2b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b3f: Expected O, but got Unknown
		//IL_2bf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c00: Expected O, but got Unknown
		//IL_2cf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d00: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Form1));
		serialPort1 = new SerialPort(components);
		timerReceive = new Timer(components);
		notifyIcon1 = new NotifyIcon(components);
		txtSendCommand = new TextBox();
		mdmPanel4 = new MDMPanel();
		lblCommandHint = new Label();
		btnSave = new MDMButton();
		btnCopyLogs = new MDMButton();
		btnClear = new MDMButton();
		btnDisconnect = new MDMButton();
		btnConnect = new MDMButton();
		lblSearchCount = new Label();
		btnClearSearch = new Button();
		btnFindPrevious = new Button();
		btnFindNext = new Button();
		txtLog = new FastColoredTextBox();
		label1 = new Label();
		txtSearch = new TextBox();
		label3 = new Label();
		label2 = new Label();
		lblStatus = new Label();
		btnMaximizeForm = new MDMButton();
		btnCloseForm = new MDMButton();
		btnMinimizeForm = new MDMButton();
		pbBanner = new PictureBox();
		mdmPanel3 = new MDMPanel();
		pictureBox1 = new PictureBox();
		btnSendCommand = new MDMButton();
		TitlePanel = new MDMPanel();
		mdmPanel2 = new MDMPanel();
		mdmPanel1 = new MDMPanel();
		labelLogTextSize = new Label();
		trackBarFontSize = new TrackBar();
		picRxWarning = new PictureBox();
		cmbBaud = new DarkComboBox();
		cmbPort = new DarkComboBox();
		mdmPanel5 = new MDMPanel();
		label5 = new Label();
		timerRxWarning = new Timer(components);
		label4 = new Label();
		((Control)mdmPanel4).SuspendLayout();
		((ISupportInitialize)txtLog).BeginInit();
		((ISupportInitialize)pbBanner).BeginInit();
		((Control)mdmPanel3).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)TitlePanel).SuspendLayout();
		((Control)mdmPanel1).SuspendLayout();
		((ISupportInitialize)trackBarFontSize).BeginInit();
		((ISupportInitialize)picRxWarning).BeginInit();
		((Control)mdmPanel5).SuspendLayout();
		((Control)this).SuspendLayout();
		serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
		timerReceive.Interval = 30;
		timerReceive.Tick += timerReceive_Tick;
		notifyIcon1.Text = "notifyIcon1";
		notifyIcon1.Visible = true;
		((Control)txtSendCommand).Anchor = (AnchorStyles)9;
		((Control)txtSendCommand).BackColor = Color.FromArgb(22, 29, 42);
		((TextBoxBase)txtSendCommand).BorderStyle = (BorderStyle)0;
		((Control)txtSendCommand).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtSendCommand).ForeColor = Color.White;
		((Control)txtSendCommand).Location = new Point(7, 5);
		((TextBoxBase)txtSendCommand).Multiline = true;
		((Control)txtSendCommand).Name = "txtSendCommand";
		((Control)txtSendCommand).Size = new Size(164, 66);
		((Control)txtSendCommand).TabIndex = 28;
		((Control)txtSendCommand).TextChanged += txtSendCommand_TextChanged;
		((Control)mdmPanel4).Anchor = (AnchorStyles)9;
		mdmPanel4.AnimationSpeed = 1;
		((Control)mdmPanel4).BackColor = Color.FromArgb(20, 7, 27);
		mdmPanel4.BorderColor = Color.Indigo;
		mdmPanel4.BorderRadius = 8;
		mdmPanel4.BorderSize = 2;
		((Control)mdmPanel4).Controls.Add((Control)(object)lblCommandHint);
		((Control)mdmPanel4).Controls.Add((Control)(object)txtSendCommand);
		mdmPanel4.EnableGradient = false;
		mdmPanel4.EnableShadow = true;
		mdmPanel4.GradientAngle = 90f;
		mdmPanel4.GradientEndColor = Color.LightSkyBlue;
		mdmPanel4.GradientStartColor = Color.White;
		mdmPanel4.HoverBorderSize = 3;
		((Control)mdmPanel4).Location = new Point(6, 29);
		((Control)mdmPanel4).Name = "mdmPanel4";
		mdmPanel4.ShadowOpacity = 35;
		mdmPanel4.ShadowSize = 8;
		((Control)mdmPanel4).Size = new Size(176, 75);
		((Control)mdmPanel4).TabIndex = 29;
		((Control)lblCommandHint).Anchor = (AnchorStyles)9;
		((Control)lblCommandHint).AutoSize = true;
		((Control)lblCommandHint).BackColor = Color.FromArgb(22, 29, 42);
		((Control)lblCommandHint).Cursor = Cursors.IBeam;
		((Control)lblCommandHint).Font = new Font("Microsoft Sans Serif", 10f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblCommandHint).ForeColor = Color.Gray;
		((Control)lblCommandHint).Location = new Point(11, 5);
		((Control)lblCommandHint).Name = "lblCommandHint";
		((Control)lblCommandHint).Size = new Size(150, 17);
		((Control)lblCommandHint).TabIndex = 37;
		((Control)lblCommandHint).Text = "Type command here...";
		((Control)lblCommandHint).Click += lblCommandHint_Click;
		((Control)btnSave).Anchor = (AnchorStyles)9;
		((Control)btnSave).BackColor = Color.FromArgb(43, 41, 52);
		btnSave.BackgroundColor = Color.FromArgb(43, 41, 52);
		btnSave.BorderColor = Color.FromArgb(53, 51, 62);
		btnSave.BorderRadius = 10;
		btnSave.BorderSize = 2;
		((Control)btnSave).Cursor = Cursors.Hand;
		btnSave.EnableGradient = false;
		btnSave.EnableShadow = true;
		((ButtonBase)btnSave).FlatAppearance.BorderSize = 0;
		((ButtonBase)btnSave).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)btnSave).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)btnSave).FlatStyle = (FlatStyle)0;
		((Control)btnSave).ForeColor = Color.DarkOrange;
		btnSave.GradientAngle = 90f;
		btnSave.GradientEndColor = Color.LightSkyBlue;
		btnSave.GradientStartColor = Color.White;
		btnSave.HoverColor = Color.FromArgb(30, 144, 255);
		((Control)btnSave).Location = new Point(1097, 526);
		((Control)btnSave).Name = "btnSave";
		btnSave.ShadowColor = Color.Black;
		btnSave.ShadowOpacity = 35;
		btnSave.ShadowSize = 8;
		((Control)btnSave).Size = new Size(111, 37);
		((Control)btnSave).TabIndex = 27;
		((Control)btnSave).Text = "Save Logs";
		btnSave.TextColor = Color.DarkOrange;
		((ButtonBase)btnSave).UseVisualStyleBackColor = false;
		((Control)btnSave).Click += btnSave_Click;
		((Control)btnCopyLogs).Anchor = (AnchorStyles)9;
		((Control)btnCopyLogs).BackColor = Color.FromArgb(33, 31, 42);
		btnCopyLogs.BackgroundColor = Color.FromArgb(33, 31, 42);
		btnCopyLogs.BorderColor = Color.FromArgb(43, 41, 62);
		btnCopyLogs.BorderRadius = 10;
		btnCopyLogs.BorderSize = 2;
		((Control)btnCopyLogs).Cursor = Cursors.Hand;
		btnCopyLogs.EnableGradient = false;
		btnCopyLogs.EnableShadow = true;
		((ButtonBase)btnCopyLogs).FlatAppearance.BorderSize = 0;
		((ButtonBase)btnCopyLogs).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)btnCopyLogs).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)btnCopyLogs).FlatStyle = (FlatStyle)0;
		((Control)btnCopyLogs).ForeColor = Color.DarkOrange;
		btnCopyLogs.GradientAngle = 90f;
		btnCopyLogs.GradientEndColor = Color.LightSkyBlue;
		btnCopyLogs.GradientStartColor = Color.White;
		btnCopyLogs.HoverColor = Color.FromArgb(30, 144, 255);
		((Control)btnCopyLogs).Location = new Point(1054, 485);
		((Control)btnCopyLogs).Name = "btnCopyLogs";
		btnCopyLogs.ShadowColor = Color.Black;
		btnCopyLogs.ShadowOpacity = 35;
		btnCopyLogs.ShadowSize = 8;
		((Control)btnCopyLogs).Size = new Size(84, 35);
		((Control)btnCopyLogs).TabIndex = 26;
		((Control)btnCopyLogs).Text = "Copy All";
		btnCopyLogs.TextColor = Color.DarkOrange;
		((ButtonBase)btnCopyLogs).UseVisualStyleBackColor = false;
		((Control)btnCopyLogs).Click += btnCopyLogs_Click;
		((Control)btnClear).Anchor = (AnchorStyles)9;
		((Control)btnClear).BackColor = Color.FromArgb(33, 31, 42);
		btnClear.BackgroundColor = Color.FromArgb(33, 31, 42);
		btnClear.BorderColor = Color.FromArgb(43, 41, 62);
		btnClear.BorderRadius = 10;
		btnClear.BorderSize = 2;
		((Control)btnClear).Cursor = Cursors.Hand;
		btnClear.EnableGradient = false;
		btnClear.EnableShadow = true;
		((ButtonBase)btnClear).FlatAppearance.BorderSize = 0;
		((ButtonBase)btnClear).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)btnClear).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)btnClear).FlatStyle = (FlatStyle)0;
		((Control)btnClear).ForeColor = Color.OrangeRed;
		btnClear.GradientAngle = 90f;
		btnClear.GradientEndColor = Color.LightSkyBlue;
		btnClear.GradientStartColor = Color.White;
		btnClear.HoverColor = Color.FromArgb(30, 144, 255);
		((Control)btnClear).Location = new Point(1156, 485);
		((Control)btnClear).Name = "btnClear";
		btnClear.ShadowColor = Color.Black;
		btnClear.ShadowOpacity = 35;
		btnClear.ShadowSize = 8;
		((Control)btnClear).Size = new Size(86, 35);
		((Control)btnClear).TabIndex = 25;
		((Control)btnClear).Text = "Clear All";
		btnClear.TextColor = Color.OrangeRed;
		((ButtonBase)btnClear).UseVisualStyleBackColor = false;
		((Control)btnClear).Click += btnClear_Click;
		((Control)btnDisconnect).Anchor = (AnchorStyles)9;
		((Control)btnDisconnect).BackColor = Color.FromArgb(18, 27, 41);
		btnDisconnect.BackgroundColor = Color.FromArgb(18, 27, 41);
		btnDisconnect.BorderColor = Color.FromArgb(43, 41, 62);
		btnDisconnect.BorderRadius = 10;
		btnDisconnect.BorderSize = 2;
		((Control)btnDisconnect).Cursor = Cursors.Hand;
		btnDisconnect.EnableGradient = false;
		btnDisconnect.EnableShadow = true;
		((ButtonBase)btnDisconnect).FlatAppearance.BorderSize = 0;
		((ButtonBase)btnDisconnect).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)btnDisconnect).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)btnDisconnect).FlatStyle = (FlatStyle)0;
		((Control)btnDisconnect).ForeColor = Color.Red;
		btnDisconnect.GradientAngle = 90f;
		btnDisconnect.GradientEndColor = Color.LightSkyBlue;
		btnDisconnect.GradientStartColor = Color.White;
		btnDisconnect.HoverColor = Color.FromArgb(30, 144, 255);
		((Control)btnDisconnect).Location = new Point(1054, 176);
		((Control)btnDisconnect).Name = "btnDisconnect";
		btnDisconnect.ShadowColor = Color.Black;
		btnDisconnect.ShadowOpacity = 35;
		btnDisconnect.ShadowSize = 8;
		((Control)btnDisconnect).Size = new Size(93, 35);
		((Control)btnDisconnect).TabIndex = 24;
		((Control)btnDisconnect).Text = "Disconnect";
		btnDisconnect.TextColor = Color.Red;
		((ButtonBase)btnDisconnect).UseVisualStyleBackColor = false;
		((Control)btnDisconnect).Click += btnDisconnect_Click;
		((Control)btnConnect).Anchor = (AnchorStyles)9;
		((Control)btnConnect).BackColor = Color.FromArgb(18, 27, 41);
		btnConnect.BackgroundColor = Color.FromArgb(18, 27, 41);
		btnConnect.BorderColor = Color.FromArgb(43, 41, 62);
		btnConnect.BorderRadius = 10;
		btnConnect.BorderSize = 2;
		((Control)btnConnect).Cursor = Cursors.Hand;
		btnConnect.EnableGradient = false;
		btnConnect.EnableShadow = true;
		((ButtonBase)btnConnect).FlatAppearance.BorderSize = 0;
		((ButtonBase)btnConnect).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)btnConnect).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)btnConnect).FlatStyle = (FlatStyle)0;
		((Control)btnConnect).ForeColor = Color.Cyan;
		btnConnect.GradientAngle = 90f;
		btnConnect.GradientEndColor = Color.FromArgb(18, 27, 41);
		btnConnect.GradientStartColor = Color.Black;
		btnConnect.HoverColor = Color.FromArgb(30, 144, 255);
		((Control)btnConnect).Location = new Point(1153, 176);
		((Control)btnConnect).Name = "btnConnect";
		btnConnect.ShadowColor = Color.Black;
		btnConnect.ShadowOpacity = 35;
		btnConnect.ShadowSize = 8;
		((Control)btnConnect).Size = new Size(93, 35);
		((Control)btnConnect).TabIndex = 23;
		((Control)btnConnect).Text = "Connect";
		btnConnect.TextColor = Color.Cyan;
		((ButtonBase)btnConnect).UseVisualStyleBackColor = false;
		((Control)btnConnect).Click += btnConnect_Click;
		((Control)lblSearchCount).Anchor = (AnchorStyles)9;
		((Control)lblSearchCount).AutoSize = true;
		((Control)lblSearchCount).Font = new Font("Microsoft Sans Serif", 10f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)lblSearchCount).ForeColor = Color.Aquamarine;
		((Control)lblSearchCount).Location = new Point(1213, 261);
		((Control)lblSearchCount).Name = "lblSearchCount";
		((Control)lblSearchCount).Size = new Size(31, 17);
		((Control)lblSearchCount).TabIndex = 19;
		((Control)lblSearchCount).Text = "0/0";
		((Control)btnClearSearch).Anchor = (AnchorStyles)9;
		((Control)btnClearSearch).Cursor = Cursors.Hand;
		((ButtonBase)btnClearSearch).FlatStyle = (FlatStyle)0;
		((Control)btnClearSearch).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)btnClearSearch).ForeColor = Color.DeepPink;
		((Control)btnClearSearch).Location = new Point(1177, 291);
		((Control)btnClearSearch).Name = "btnClearSearch";
		((Control)btnClearSearch).Size = new Size(63, 23);
		((Control)btnClearSearch).TabIndex = 18;
		((Control)btnClearSearch).Text = "✖ Clear";
		((ButtonBase)btnClearSearch).UseVisualStyleBackColor = true;
		((Control)btnClearSearch).Click += btnClearSearch_Click;
		((Control)btnFindPrevious).Anchor = (AnchorStyles)9;
		((Control)btnFindPrevious).Cursor = Cursors.Hand;
		((ButtonBase)btnFindPrevious).FlatStyle = (FlatStyle)0;
		((Control)btnFindPrevious).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)btnFindPrevious).ForeColor = Color.DarkOrange;
		((Control)btnFindPrevious).Location = new Point(1119, 291);
		((Control)btnFindPrevious).Name = "btnFindPrevious";
		((Control)btnFindPrevious).Size = new Size(57, 23);
		((Control)btnFindPrevious).TabIndex = 17;
		((Control)btnFindPrevious).Text = "▲ Prev";
		((ButtonBase)btnFindPrevious).UseVisualStyleBackColor = true;
		((Control)btnFindPrevious).Click += btnFindPrevious_Click;
		((Control)btnFindNext).Anchor = (AnchorStyles)9;
		((Control)btnFindNext).Cursor = Cursors.Hand;
		((ButtonBase)btnFindNext).FlatStyle = (FlatStyle)0;
		((Control)btnFindNext).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)btnFindNext).ForeColor = Color.DarkSeaGreen;
		((Control)btnFindNext).Location = new Point(1057, 291);
		((Control)btnFindNext).Name = "btnFindNext";
		((Control)btnFindNext).Size = new Size(61, 23);
		((Control)btnFindNext).TabIndex = 16;
		((Control)btnFindNext).Text = "▼ Next";
		((ButtonBase)btnFindNext).UseVisualStyleBackColor = true;
		((Control)btnFindNext).Click += btnFindNext_Click;
		((Control)txtLog).Anchor = (AnchorStyles)15;
		txtLog.AutoCompleteBracketsList = new char[10] { '(', ')', '{', '}', '[', ']', '"', '"', '\'', '\'' };
		txtLog.AutoIndent = false;
		txtLog.AutoScrollMinSize = new Size(58, 15);
		txtLog.BackBrush = null;
		((Control)txtLog).BackColor = Color.FromArgb(7, 13, 24);
		txtLog.CharHeight = 15;
		txtLog.CharWidth = 8;
		((Control)txtLog).Cursor = Cursors.IBeam;
		txtLog.DisabledColor = Color.FromArgb(100, 180, 180, 180);
		((Control)txtLog).Font = new Font("Consolas", 10f);
		((Control)txtLog).ForeColor = Color.Moccasin;
		txtLog.IsReplaceMode = false;
		((Control)txtLog).Location = new Point(11, 75);
		((Control)txtLog).Name = "txtLog";
		txtLog.Paddings = new Padding(0);
		txtLog.ReadOnly = true;
		txtLog.SelectionColor = Color.FromArgb(100, 0, 255, 255);
		txtLog.ServiceColors = (ServiceColors)componentResourceManager.GetObject("txtLog.ServiceColors");
		txtLog.ShowLineNumbers = false;
		((Control)txtLog).Size = new Size(1031, 582);
		((Control)txtLog).TabIndex = 9;
		((Control)txtLog).Text = "TextBox";
		txtLog.Zoom = 100;
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label1).ForeColor = Color.LightBlue;
		((Control)label1).Location = new Point(1067, 118);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(58, 13);
		((Control)label1).TabIndex = 1;
		((Control)label1).Text = "Com Port";
		((Control)txtSearch).Anchor = (AnchorStyles)9;
		((Control)txtSearch).BackColor = Color.FromArgb(12, 19, 32);
		((TextBoxBase)txtSearch).BorderStyle = (BorderStyle)0;
		((Control)txtSearch).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtSearch).ForeColor = Color.White;
		((Control)txtSearch).Location = new Point(1086, 261);
		((Control)txtSearch).Name = "txtSearch";
		((Control)txtSearch).Size = new Size(105, 19);
		((Control)txtSearch).TabIndex = 14;
		((Control)txtSearch).TextChanged += txtSearch_TextChanged;
		((Control)txtSearch).KeyDown += new KeyEventHandler(txtSearch_KeyDown);
		((Control)label3).Anchor = (AnchorStyles)9;
		((Control)label3).AutoSize = true;
		((Control)label3).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label3).ForeColor = SystemColors.ActiveBorder;
		((Control)label3).Location = new Point(1094, 224);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(47, 13);
		((Control)label3).TabIndex = 12;
		((Control)label3).Text = "Status:";
		((Control)label2).Anchor = (AnchorStyles)9;
		((Control)label2).AutoSize = true;
		((Control)label2).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label2).ForeColor = Color.LightBlue;
		((Control)label2).Location = new Point(1067, 148);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(67, 13);
		((Control)label2).TabIndex = 3;
		((Control)label2).Text = "Baud Rate";
		((Control)lblStatus).Anchor = (AnchorStyles)9;
		((Control)lblStatus).AutoSize = true;
		((Control)lblStatus).ForeColor = Color.Red;
		((Control)lblStatus).Location = new Point(1141, 224);
		((Control)lblStatus).Name = "lblStatus";
		((Control)lblStatus).Size = new Size(73, 13);
		((Control)lblStatus).TabIndex = 11;
		((Control)lblStatus).Text = "Disconnected";
		((Control)btnMaximizeForm).Anchor = (AnchorStyles)9;
		((Control)btnMaximizeForm).BackColor = Color.Teal;
		btnMaximizeForm.BackgroundColor = Color.Teal;
		btnMaximizeForm.BorderColor = Color.CadetBlue;
		btnMaximizeForm.BorderRadius = 5;
		btnMaximizeForm.BorderSize = 2;
		((Control)btnMaximizeForm).Cursor = Cursors.Hand;
		btnMaximizeForm.EnableGradient = false;
		btnMaximizeForm.EnableShadow = true;
		((ButtonBase)btnMaximizeForm).FlatAppearance.BorderSize = 0;
		((ButtonBase)btnMaximizeForm).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)btnMaximizeForm).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)btnMaximizeForm).FlatStyle = (FlatStyle)0;
		((Control)btnMaximizeForm).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)btnMaximizeForm).ForeColor = Color.White;
		btnMaximizeForm.GradientAngle = 90f;
		btnMaximizeForm.GradientEndColor = Color.LightSkyBlue;
		btnMaximizeForm.GradientStartColor = Color.White;
		btnMaximizeForm.HoverColor = Color.FromArgb(30, 144, 255);
		((Control)btnMaximizeForm).Location = new Point(1158, 4);
		((Control)btnMaximizeForm).Name = "btnMaximizeForm";
		btnMaximizeForm.ShadowColor = Color.Black;
		btnMaximizeForm.ShadowOpacity = 35;
		btnMaximizeForm.ShadowSize = 8;
		((Control)btnMaximizeForm).Size = new Size(44, 28);
		((Control)btnMaximizeForm).TabIndex = 22;
		((Control)btnMaximizeForm).Text = "❐";
		btnMaximizeForm.TextColor = Color.White;
		((ButtonBase)btnMaximizeForm).UseVisualStyleBackColor = false;
		((Control)btnMaximizeForm).Click += btnMaximizeForm_Click;
		((Control)btnCloseForm).Anchor = (AnchorStyles)9;
		((Control)btnCloseForm).BackColor = Color.Red;
		btnCloseForm.BackgroundColor = Color.Red;
		btnCloseForm.BorderColor = Color.OrangeRed;
		btnCloseForm.BorderRadius = 5;
		btnCloseForm.BorderSize = 2;
		((Control)btnCloseForm).Cursor = Cursors.Hand;
		btnCloseForm.EnableGradient = false;
		btnCloseForm.EnableShadow = true;
		((ButtonBase)btnCloseForm).FlatAppearance.BorderSize = 0;
		((ButtonBase)btnCloseForm).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)btnCloseForm).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)btnCloseForm).FlatStyle = (FlatStyle)0;
		((Control)btnCloseForm).Font = new Font("Microsoft Sans Serif", 10f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)btnCloseForm).ForeColor = Color.White;
		btnCloseForm.GradientAngle = 90f;
		btnCloseForm.GradientEndColor = Color.LightSkyBlue;
		btnCloseForm.GradientStartColor = Color.White;
		btnCloseForm.HoverColor = Color.FromArgb(30, 144, 255);
		((Control)btnCloseForm).Location = new Point(1204, 4);
		((Control)btnCloseForm).Name = "btnCloseForm";
		btnCloseForm.ShadowColor = Color.Black;
		btnCloseForm.ShadowOpacity = 35;
		btnCloseForm.ShadowSize = 8;
		((Control)btnCloseForm).Size = new Size(42, 28);
		((Control)btnCloseForm).TabIndex = 21;
		((Control)btnCloseForm).Text = "X";
		btnCloseForm.TextColor = Color.White;
		((ButtonBase)btnCloseForm).UseVisualStyleBackColor = false;
		((Control)btnCloseForm).Click += btnCloseForm_Click;
		((Control)btnMinimizeForm).Anchor = (AnchorStyles)9;
		((Control)btnMinimizeForm).BackColor = Color.SeaGreen;
		btnMinimizeForm.BackgroundColor = Color.SeaGreen;
		btnMinimizeForm.BorderColor = Color.SpringGreen;
		btnMinimizeForm.BorderRadius = 5;
		btnMinimizeForm.BorderSize = 2;
		((Control)btnMinimizeForm).Cursor = Cursors.Hand;
		btnMinimizeForm.EnableGradient = false;
		btnMinimizeForm.EnableShadow = true;
		((ButtonBase)btnMinimizeForm).FlatAppearance.BorderSize = 0;
		((ButtonBase)btnMinimizeForm).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)btnMinimizeForm).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)btnMinimizeForm).FlatStyle = (FlatStyle)0;
		((Control)btnMinimizeForm).Font = new Font("Microsoft Sans Serif", 20f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)btnMinimizeForm).ForeColor = Color.White;
		btnMinimizeForm.GradientAngle = 90f;
		btnMinimizeForm.GradientEndColor = Color.LightSkyBlue;
		btnMinimizeForm.GradientStartColor = Color.White;
		btnMinimizeForm.HoverColor = Color.FromArgb(30, 144, 255);
		((Control)btnMinimizeForm).Location = new Point(1113, 4);
		((Control)btnMinimizeForm).Name = "btnMinimizeForm";
		btnMinimizeForm.ShadowColor = Color.Black;
		btnMinimizeForm.ShadowOpacity = 35;
		btnMinimizeForm.ShadowSize = 8;
		((Control)btnMinimizeForm).Size = new Size(44, 28);
		((Control)btnMinimizeForm).TabIndex = 23;
		((Control)btnMinimizeForm).Text = "-";
		btnMinimizeForm.TextColor = Color.White;
		((ButtonBase)btnMinimizeForm).UseVisualStyleBackColor = false;
		((Control)btnMinimizeForm).Click += btnMinimizeForm_Click;
		((Control)pbBanner).Anchor = (AnchorStyles)13;
		((Control)pbBanner).BackgroundImage = (Image)(object)Resources.Banner;
		((Control)pbBanner).BackgroundImageLayout = (ImageLayout)3;
		((Control)pbBanner).Location = new Point(-8, -24);
		((Control)pbBanner).Name = "pbBanner";
		((Control)pbBanner).Size = new Size(1165, 139);
		pbBanner.TabIndex = 24;
		pbBanner.TabStop = false;
		((Control)mdmPanel3).Anchor = (AnchorStyles)9;
		mdmPanel3.AnimationSpeed = 1;
		((Control)mdmPanel3).BackColor = Color.FromArgb(20, 7, 27);
		mdmPanel3.BorderColor = Color.DodgerBlue;
		mdmPanel3.BorderRadius = 15;
		mdmPanel3.BorderSize = 2;
		((Control)mdmPanel3).Controls.Add((Control)(object)pictureBox1);
		mdmPanel3.EnableGradient = false;
		mdmPanel3.EnableShadow = true;
		mdmPanel3.GradientAngle = 90f;
		mdmPanel3.GradientEndColor = Color.LightSkyBlue;
		mdmPanel3.GradientStartColor = Color.White;
		mdmPanel3.HoverBorderSize = 3;
		((Control)mdmPanel3).Location = new Point(1050, 255);
		((Control)mdmPanel3).Name = "mdmPanel3";
		mdmPanel3.ShadowOpacity = 35;
		mdmPanel3.ShadowSize = 8;
		((Control)mdmPanel3).Size = new Size(159, 31);
		((Control)mdmPanel3).TabIndex = 21;
		((Control)pictureBox1).BackColor = Color.Transparent;
		((Control)pictureBox1).BackgroundImage = (Image)(object)Resources.SearchIcon;
		((Control)pictureBox1).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox1).Location = new Point(7, 3);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(31, 28);
		pictureBox1.TabIndex = 28;
		pictureBox1.TabStop = false;
		((Control)btnSendCommand).Anchor = (AnchorStyles)9;
		((Control)btnSendCommand).BackColor = Color.FromArgb(18, 27, 41);
		btnSendCommand.BackgroundColor = Color.FromArgb(18, 27, 41);
		btnSendCommand.BorderColor = Color.FromArgb(43, 41, 62);
		btnSendCommand.BorderRadius = 10;
		btnSendCommand.BorderSize = 2;
		((Control)btnSendCommand).Cursor = Cursors.Hand;
		btnSendCommand.EnableGradient = false;
		btnSendCommand.EnableShadow = true;
		((ButtonBase)btnSendCommand).FlatAppearance.BorderSize = 0;
		((ButtonBase)btnSendCommand).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)btnSendCommand).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)btnSendCommand).FlatStyle = (FlatStyle)0;
		((Control)btnSendCommand).ForeColor = Color.Cyan;
		btnSendCommand.GradientAngle = 90f;
		btnSendCommand.GradientEndColor = Color.FromArgb(18, 27, 41);
		btnSendCommand.GradientStartColor = Color.Black;
		btnSendCommand.HoverColor = Color.FromArgb(30, 144, 255);
		((Control)btnSendCommand).Location = new Point(10, 110);
		((Control)btnSendCommand).Name = "btnSendCommand";
		btnSendCommand.ShadowColor = Color.Black;
		btnSendCommand.ShadowOpacity = 35;
		btnSendCommand.ShadowSize = 8;
		((Control)btnSendCommand).Size = new Size(170, 31);
		((Control)btnSendCommand).TabIndex = 32;
		((Control)btnSendCommand).Text = "Send";
		btnSendCommand.TextColor = Color.Cyan;
		((ButtonBase)btnSendCommand).UseVisualStyleBackColor = false;
		((Control)btnSendCommand).Click += btnSendCommand_Click;
		((Control)TitlePanel).Anchor = (AnchorStyles)13;
		TitlePanel.AnimationSpeed = 1;
		TitlePanel.BorderColor = Color.Teal;
		TitlePanel.BorderRadius = 5;
		TitlePanel.BorderSize = 1;
		((Control)TitlePanel).Controls.Add((Control)(object)btnMaximizeForm);
		((Control)TitlePanel).Controls.Add((Control)(object)btnCloseForm);
		((Control)TitlePanel).Controls.Add((Control)(object)btnMinimizeForm);
		((Control)TitlePanel).Controls.Add((Control)(object)mdmPanel2);
		((Control)TitlePanel).Controls.Add((Control)(object)pbBanner);
		TitlePanel.EnableGradient = false;
		TitlePanel.EnableShadow = true;
		TitlePanel.GradientAngle = 180f;
		TitlePanel.GradientEndColor = Color.LightSkyBlue;
		TitlePanel.GradientStartColor = Color.FromArgb(43, 21, 32);
		TitlePanel.HoverBorderSize = 3;
		((Control)TitlePanel).Location = new Point(0, 0);
		((Control)TitlePanel).Name = "TitlePanel";
		TitlePanel.ShadowOpacity = 35;
		TitlePanel.ShadowSize = 8;
		((Control)TitlePanel).Size = new Size(1252, 79);
		((Control)TitlePanel).TabIndex = 21;
		((Control)mdmPanel2).Anchor = (AnchorStyles)9;
		mdmPanel2.AnimationSpeed = 1;
		((Control)mdmPanel2).BackColor = Color.FromArgb(11, 7, 17);
		mdmPanel2.BorderColor = Color.SteelBlue;
		mdmPanel2.BorderRadius = 15;
		mdmPanel2.BorderSize = 0;
		mdmPanel2.EnableGradient = false;
		mdmPanel2.EnableShadow = false;
		mdmPanel2.GradientAngle = 90f;
		mdmPanel2.GradientEndColor = Color.LightSkyBlue;
		mdmPanel2.GradientStartColor = Color.White;
		mdmPanel2.HoverBorderSize = 3;
		((Control)mdmPanel2).Location = new Point(1128, 0);
		((Control)mdmPanel2).Name = "mdmPanel2";
		mdmPanel2.ShadowOpacity = 35;
		mdmPanel2.ShadowSize = 8;
		((Control)mdmPanel2).Size = new Size(121, 89);
		((Control)mdmPanel2).TabIndex = 25;
		((Control)mdmPanel1).Anchor = (AnchorStyles)15;
		mdmPanel1.AnimationSpeed = 1;
		((Control)mdmPanel1).BackColor = Color.FromArgb(12, 19, 32);
		mdmPanel1.BorderColor = Color.MediumTurquoise;
		mdmPanel1.BorderRadius = 10;
		mdmPanel1.BorderSize = 1;
		((Control)mdmPanel1).Controls.Add((Control)(object)label4);
		((Control)mdmPanel1).Controls.Add((Control)(object)labelLogTextSize);
		((Control)mdmPanel1).Controls.Add((Control)(object)trackBarFontSize);
		((Control)mdmPanel1).Controls.Add((Control)(object)picRxWarning);
		((Control)mdmPanel1).Controls.Add((Control)(object)cmbBaud);
		((Control)mdmPanel1).Controls.Add((Control)(object)cmbPort);
		((Control)mdmPanel1).Controls.Add((Control)(object)btnSave);
		((Control)mdmPanel1).Controls.Add((Control)(object)btnCopyLogs);
		((Control)mdmPanel1).Controls.Add((Control)(object)btnClear);
		((Control)mdmPanel1).Controls.Add((Control)(object)btnDisconnect);
		((Control)mdmPanel1).Controls.Add((Control)(object)btnConnect);
		((Control)mdmPanel1).Controls.Add((Control)(object)lblSearchCount);
		((Control)mdmPanel1).Controls.Add((Control)(object)btnClearSearch);
		((Control)mdmPanel1).Controls.Add((Control)(object)btnFindPrevious);
		((Control)mdmPanel1).Controls.Add((Control)(object)btnFindNext);
		((Control)mdmPanel1).Controls.Add((Control)(object)txtLog);
		((Control)mdmPanel1).Controls.Add((Control)(object)label1);
		((Control)mdmPanel1).Controls.Add((Control)(object)txtSearch);
		((Control)mdmPanel1).Controls.Add((Control)(object)label3);
		((Control)mdmPanel1).Controls.Add((Control)(object)label2);
		((Control)mdmPanel1).Controls.Add((Control)(object)lblStatus);
		((Control)mdmPanel1).Controls.Add((Control)(object)TitlePanel);
		((Control)mdmPanel1).Controls.Add((Control)(object)mdmPanel3);
		((Control)mdmPanel1).Controls.Add((Control)(object)mdmPanel5);
		mdmPanel1.EnableGradient = false;
		mdmPanel1.EnableShadow = true;
		mdmPanel1.GradientAngle = 90f;
		mdmPanel1.GradientEndColor = Color.FromArgb(23, 21, 32);
		mdmPanel1.GradientStartColor = Color.FromArgb(11, 7, 17);
		mdmPanel1.HoverBorderSize = 3;
		((Control)mdmPanel1).Location = new Point(1, 2);
		((Control)mdmPanel1).Margin = new Padding(5);
		((Control)mdmPanel1).Name = "mdmPanel1";
		((Control)mdmPanel1).Padding = new Padding(8);
		mdmPanel1.ShadowOpacity = 35;
		mdmPanel1.ShadowSize = 8;
		((Control)mdmPanel1).Size = new Size(1250, 666);
		((Control)mdmPanel1).TabIndex = 20;
		((Control)labelLogTextSize).Anchor = (AnchorStyles)9;
		((Control)labelLogTextSize).AutoSize = true;
		((Control)labelLogTextSize).Font = new Font("Microsoft Sans Serif", 10f);
		((Control)labelLogTextSize).ForeColor = Color.Chartreuse;
		((Control)labelLogTextSize).Location = new Point(1201, 581);
		((Control)labelLogTextSize).Name = "labelLogTextSize";
		((Control)labelLogTextSize).Size = new Size(24, 17);
		((Control)labelLogTextSize).TabIndex = 40;
		((Control)labelLogTextSize).Text = "10";
		((Control)trackBarFontSize).Anchor = (AnchorStyles)9;
		((Control)trackBarFontSize).Location = new Point(1059, 601);
		trackBarFontSize.Maximum = 20;
		trackBarFontSize.Minimum = 10;
		((Control)trackBarFontSize).Name = "trackBarFontSize";
		((Control)trackBarFontSize).Size = new Size(174, 45);
		((Control)trackBarFontSize).TabIndex = 38;
		trackBarFontSize.TickStyle = (TickStyle)0;
		trackBarFontSize.Value = 10;
		trackBarFontSize.Scroll += trackBarFontSize_Scroll;
		((Control)picRxWarning).BackgroundImage = (Image)(object)Resources.PBoxWarning;
		((Control)picRxWarning).BackgroundImageLayout = (ImageLayout)3;
		((Control)picRxWarning).Location = new Point(94, 118);
		((Control)picRxWarning).Name = "picRxWarning";
		((Control)picRxWarning).Size = new Size(824, 528);
		picRxWarning.TabIndex = 37;
		picRxWarning.TabStop = false;
		((Control)picRxWarning).Visible = false;
		((Control)cmbBaud).Anchor = (AnchorStyles)9;
		((Control)cmbBaud).BackColor = Color.FromArgb(35, 35, 38);
		((Control)cmbBaud).Font = new Font("Segoe UI", 9f);
		((Control)cmbBaud).Location = new Point(1136, 142);
		((Control)cmbBaud).Name = "cmbBaud";
		cmbBaud.SelectedIndex = -1;
		((Control)cmbBaud).Size = new Size(103, 24);
		((Control)cmbBaud).TabIndex = 34;
		((Control)cmbPort).Anchor = (AnchorStyles)9;
		((Control)cmbPort).BackColor = Color.FromArgb(35, 35, 38);
		((Control)cmbPort).Font = new Font("Segoe UI", 9f);
		((Control)cmbPort).Location = new Point(1136, 112);
		((Control)cmbPort).Name = "cmbPort";
		cmbPort.SelectedIndex = -1;
		((Control)cmbPort).Size = new Size(103, 24);
		((Control)cmbPort).TabIndex = 33;
		((Control)cmbPort).Click += cmbPort_Click;
		((Control)mdmPanel5).Anchor = (AnchorStyles)9;
		mdmPanel5.AnimationSpeed = 1;
		mdmPanel5.BorderColor = Color.Brown;
		mdmPanel5.BorderRadius = 15;
		mdmPanel5.BorderSize = 1;
		((Control)mdmPanel5).Controls.Add((Control)(object)label5);
		((Control)mdmPanel5).Controls.Add((Control)(object)mdmPanel4);
		((Control)mdmPanel5).Controls.Add((Control)(object)btnSendCommand);
		mdmPanel5.EnableGradient = false;
		mdmPanel5.EnableShadow = false;
		mdmPanel5.GradientAngle = 90f;
		mdmPanel5.GradientEndColor = Color.LightSkyBlue;
		mdmPanel5.GradientStartColor = Color.White;
		mdmPanel5.HoverBorderSize = 3;
		((Control)mdmPanel5).Location = new Point(1053, 332);
		((Control)mdmPanel5).Name = "mdmPanel5";
		mdmPanel5.ShadowOpacity = 35;
		mdmPanel5.ShadowSize = 8;
		((Control)mdmPanel5).Size = new Size(190, 147);
		((Control)mdmPanel5).TabIndex = 36;
		((Control)label5).Anchor = (AnchorStyles)9;
		((Control)label5).AutoSize = true;
		((Control)label5).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label5).ForeColor = Color.Sienna;
		((Control)label5).Location = new Point(23, 7);
		((Control)label5).Name = "label5";
		((Control)label5).Size = new Size(136, 20);
		((Control)label5).TabIndex = 37;
		((Control)label5).Text = "Send Command";
		timerRxWarning.Interval = 3000;
		timerRxWarning.Tick += timerRxWarning_Tick;
		((Control)label4).Anchor = (AnchorStyles)9;
		((Control)label4).AutoSize = true;
		((Control)label4).Font = new Font("Microsoft Sans Serif", 10f);
		((Control)label4).ForeColor = Color.Aqua;
		((Control)label4).Location = new Point(1070, 581);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(130, 17);
		((Control)label4).TabIndex = 41;
		((Control)label4).Text = "Set Logs Text Size:";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Black;
		base.BorderRadius = 10;
		((Form)this).ClientSize = new Size(1255, 669);
		((Control)this).Controls.Add((Control)(object)mdmPanel1);
		((Control)this).MinimumSize = new Size(1014, 669);
		((Control)this).Name = "Form1";
		((Control)this).Text = "MDM AI-Powered Mobile Repair Assistant";
		((Form)this).FormClosing += new FormClosingEventHandler(Form1_FormClosing);
		((Form)this).Load += Form1_Load;
		((Control)mdmPanel4).ResumeLayout(false);
		((Control)mdmPanel4).PerformLayout();
		((ISupportInitialize)txtLog).EndInit();
		((ISupportInitialize)pbBanner).EndInit();
		((Control)mdmPanel3).ResumeLayout(false);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)TitlePanel).ResumeLayout(false);
		((Control)mdmPanel1).ResumeLayout(false);
		((Control)mdmPanel1).PerformLayout();
		((ISupportInitialize)trackBarFontSize).EndInit();
		((ISupportInitialize)picRxWarning).EndInit();
		((Control)mdmPanel5).ResumeLayout(false);
		((Control)mdmPanel5).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
