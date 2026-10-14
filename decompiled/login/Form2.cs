using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Management;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using MDM_UART_Monitor;
using MDM_UART_Monitor.Properties;

namespace login;

public class Form2 : RoundForm
{
	private static string[] Scopes = new string[1] { SheetsService.Scope.Spreadsheets };

	private static readonly string ApplicationName = "MyLoginApp";

	private const string SpreadsheetId = "12twWLzhnaIxKK3PWlwrOLUoAts-u7MVb_JdnGai1MnU";

	private const string Range = "Login Sheet!A:Y";

	private const string ClientEmail = "mdm-uart-monitor-login@mdm-uart-monitor.iam.gserviceaccount.com";

	private const string PrivateKey = "-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQC6qwvQ5UmUP/aj\n+UiE4Rq98m5VKMNc5+gNwLrLZdtBF9Hspkj4FwXohfZ3Dtg4MvGR9NXuncmzbTBu\ncy5ZIrl8VlkfzCWDkxq2JKe8NASdeVLtRF6HyLDQ9X1IfdUH4K8CEiEkfO0yl6L/\nsHf37CSpJ/nxaIOeH/uqOv8zoKXGm7u9tOSzC3rK8nK2WfAX4r5oRnSOBu56zKBs\nEv3VB0PB86A5BX+cKC6pI72XPoPyoFHtvp0LF77KOUGrlOFjhc/OejqYfc+qZv5r\nl2gAgTESzfx+n8ZIxfSjbXvTH9pg0912QAEle3oxedjFw/Pg7dbXoNY8Hf8sevfB\n27/oPfFNAgMBAAECggEAEvshkHRALxrx8Wp/vUNGfNoR7gbfFqteJUfbtqL80Hmj\nlvSxpshwiVxV23Ve9xf3CGhk+IaC5APOUOzVYaruQX5mQZ14kxB6PtsrUy/W6ExV\n6lpxYk097YGI474n0009fnBpflRVqjdRE77V9BfHAoxWFf4SpBmMdZraKF3FfZRb\npotLJYbtfdcSVVir45wLZcFrcITCZATMfenqNGDRNAu/8z5+8JWZ6l5fywQMtWci\ntpH87iaWPamqf6u8hZF0IkJdrfB+RLhLkAPfPDYT9OkViC8jDyFwKswIcjaHu87t\ni+411hA0woRchjNwnaC5/79eRff22O083jEXhS9o4QKBgQDy5OXuBrF6v5zlX/jm\nC/MGKxKx4D7uNPAiZX7FqE6aVMwYZnUuH2/pVhMOcIWlr06ULJqkPToVIMm6Shf4\ni5H9kIDRPumrrkVkS/b7pHerJwZjVq+rob2WYmpH3cxJFFrnsdpi79Q47lcODsi4\n54LXNu17FgKktbeWiqtcSf91kwKBgQDEvX9NTonTm0sYsTsw6YTPOBmXlEQMOoWA\nEqAzUraafPHnIZr9n5SGemQxZxzCPaPKwAm3CB9C2k23lEOWH6LuENr9KReDXK+T\nCw7H8EUjJ9JCv+ImZbUcUvuz3z0O52ooDu6dvPNc0RN2aOw3jX79JqQQ4QbikuFG\nWMP99d9JnwKBgQCjCU3k5Y0/zVotBKx3DdJEc6kKyHC1R8df7yyW1BBeu0h7oFpL\naUKfJu2HpbOvsOpXv1DVqVtRTz7Y0l8gr96AivxMd6Q1XrapSewwoQbKVVmOWGN2\nGwy84rZgItq6uvUBzHnAKycSR7kbcIT+yDck6nOhSPbhRDBE1jOLsQfODQKBgD1q\nEhUl0Sx3K6gXmyyXmNe+4W5J4DwZAJeMOOoMhegP3MuT1/gYGpsoFaK+V2HjMAAY\nlm/GX0vVLbGC3N4+HfRG2rVwYGCnTD+QkhqjzFE/GwmvMH9f/HtNBDmQFFVUs/Sx\nRJeE5ksxEOxDsIle6CdKKqkCNc4An8X/656ZZAxtAoGAYI+KzV1OUMFgb0NPDwpi\noz9gDoJp9yNZNjj+/mPjb55JzfFJm1f0+JC+C/jQodLREWD/VES6SK1Dp2FyUCJr\nJZVOWVsyhWPW9AIqXpTYa3FtXng8psGTZPALe26EJqfKg7dOnlQQhtAIbiJI1nZX\nSZPiBoc8UvQ1fPUCMyIKMnw=\n-----END PRIVATE KEY-----\n";

	private bool areCredentialsVisible;

	private bool areCredentialsVisible1;

	private IContainer components;

	private TextBox textBoxUsername;

	private TextBox textBoxPassword;

	private TextBox textBoxHWID;

	private TextBox txtEmail;

	private TextBox txtMobile;

	private TextBox txtRegUserName;

	private TextBox txtRegPassword;

	private ProgressBar progressLoading;

	private CheckBox checkBoxRemeberMe;

	private Label label6;

	private Label label4;

	private Label label2;

	private CheckBox chkAgree;

	private Label label1;

	private Label label7;

	private Label lblLoadingStatus;

	private Label labelHWID;

	private Label label5;

	private MDMButton buttonLogin;

	private MDMPanel mdmPanel1;

	private MDMButton btnFormClose;

	private MDMButton btnRegister;

	private MDMPanel mdmPanel2;

	private FastColoredTextBox fastColoredTextBox1;

	private MDMPanel mdmPanel3;

	private MDMPanel mdmPanel5;

	private Label label8;

	private PictureBox pictureBox1;

	private MDMPanel mdmPanel4;

	private Label label3;

	private CheckBox checkBoxAutoLogin;

	private MDMButton btnHWIDCOPY;

	private MDMButton btnToggleVisibility1;

	private MDMButton btnToggleVisibility;

	private CheckBox checkBoxResetHWID;

	public Form2()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		InitializeComponent();
		LoadSavedCredentials();
		((Control)textBoxUsername).KeyDown += new KeyEventHandler(TextBox_KeyDown);
		((Control)textBoxPassword).KeyDown += new KeyEventHandler(TextBox_KeyDown);
		((Control)textBoxHWID).Text = GetHardwareID();
		textBoxPassword.PasswordChar = '*';
		textBoxUsername.PasswordChar = '*';
	}

	private void LoadSavedCredentials()
	{
		((Control)textBoxUsername).Text = Settings.Default.Username;
		((Control)textBoxPassword).Text = Settings.Default.Password;
		checkBoxRemeberMe.Checked = !string.IsNullOrEmpty(Settings.Default.Username);
		checkBoxAutoLogin.Checked = Settings.Default.AutoLogin;
	}

	private async void Form2_Shown(object sender, EventArgs e)
	{
		if (checkBoxAutoLogin.Checked)
		{
			await Task.Delay(300);
			((Button)buttonLogin).PerformClick();
		}
	}

	private async Task<bool> ResetHWIDAsync(string username, string password, string newHWID)
	{
		IList<IList<object>> list = await GetSheetDataAsync();
		if (list == null || list.Count <= 1)
		{
			return false;
		}
		for (int i = 1; i < list.Count; i++)
		{
			IList<object> list2 = list[i];
			if (list2 != null && list2.Count >= 8)
			{
				string text = list2[0]?.ToString().Trim() ?? "";
				string text2 = list2[2]?.ToString().Trim() ?? "";
				if (!string.IsNullOrWhiteSpace(text2) && text2.Equals(newHWID, StringComparison.OrdinalIgnoreCase) && !text.Equals(username, StringComparison.OrdinalIgnoreCase))
				{
					MessageBox.Show("This computer is already registered with another account.", "HWID Reset", (MessageBoxButtons)0, (MessageBoxIcon)48);
					return false;
				}
			}
		}
		for (int j = 1; j < list.Count; j++)
		{
			IList<object> list3 = list[j];
			if (list3 == null || list3.Count < 8)
			{
				continue;
			}
			string obj = list3[0]?.ToString().Trim() ?? "";
			string text3 = list3[1]?.ToString().Trim() ?? "";
			string text4 = list3[5]?.ToString().Trim() ?? "";
			if (obj.Equals(username, StringComparison.OrdinalIgnoreCase) && !(text3 != password))
			{
				if (text4 != "1")
				{
					MessageBox.Show("Your account is not activated yet.", "HWID Reset", (MessageBoxButtons)0, (MessageBoxIcon)48);
					return false;
				}
				int num = j + 1;
				string range = $"Login Sheet!C{num}";
				List<IList<object>> values = new List<IList<object>>
				{
					new List<object> { newHWID }
				};
				ValueRange body = new ValueRange
				{
					Values = values
				};
				SpreadsheetsResource.ValuesResource.UpdateRequest updateRequest = GetSheetsService().Spreadsheets.Values.Update(body, "12twWLzhnaIxKK3PWlwrOLUoAts-u7MVb_JdnGai1MnU", range);
				updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
				await updateRequest.ExecuteAsync();
				return true;
			}
		}
		return false;
	}

	private async void buttonLogin_Click(object sender, EventArgs e)
	{
		string enteredUsername = ((Control)textBoxUsername).Text.Trim();
		string enteredPassword = ((Control)textBoxPassword).Text.Trim();
		string enteredHardwareID = ((Control)textBoxHWID).Text.Trim();
		try
		{
			if (!NetworkInterface.GetIsNetworkAvailable())
			{
				MessageBox.Show("No internet connection detected. Please check your network and try again.", "Network Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
				return;
			}
			IList<IList<object>> list;
			try
			{
				list = await GetSheetDataAsync();
			}
			catch (GoogleApiException ex)
			{
				MessageBox.Show("Google API Error: " + ex.Message + "\n\nPlease verify your credentials or contact support.", "Authentication Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
				return;
			}
			catch (Exception ex2)
			{
				MessageBox.Show("Unable to connect server.\nError: " + ex2.Message + "\n\nCheck your Time/Date settings & internet or try again later.", "Connection Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
				return;
			}
			bool flag2;
			if (checkBoxResetHWID.Checked)
			{
				bool flag = false;
				foreach (IList<object> item in list)
				{
					if (item != null && item.Count >= 8)
					{
						string obj = item[0]?.ToString().Trim() ?? "";
						string text = item[1]?.ToString().Trim() ?? "";
						if (obj.Equals(enteredUsername, StringComparison.OrdinalIgnoreCase) && text == enteredPassword)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					MessageBox.Show("Invalid username or password.", "Login Failed", (MessageBoxButtons)0, (MessageBoxIcon)16);
					return;
				}
				flag2 = await ResetHWIDAsync(enteredUsername, enteredPassword, enteredHardwareID);
			}
			else
			{
				flag2 = ValidateCredentials(list, enteredUsername, enteredPassword, enteredHardwareID);
			}
			if (flag2)
			{
				if (checkBoxRemeberMe.Checked)
				{
					Settings.Default.Username = enteredUsername;
					Settings.Default.Password = enteredPassword;
					((SettingsBase)Settings.Default).Save();
				}
				((Control)this).Hide();
				((Control)new LoadingScreen()).Show();
			}
			else
			{
				MessageBox.Show("Invalid username, password, or hardware ID!", "Login Failed", (MessageBoxButtons)0, (MessageBoxIcon)16);
			}
		}
		catch (Exception ex3)
		{
			MessageBox.Show("An unexpected error occurred:\n\n" + ex3.Message + "\n\nPlease contact support if the issue persists.", "Critical Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private async Task<IList<IList<object>>> GetSheetDataAsync()
	{
		return (await GetSheetsService().Spreadsheets.Values.Get("12twWLzhnaIxKK3PWlwrOLUoAts-u7MVb_JdnGai1MnU", "Login Sheet!A:Y").ExecuteAsync()).Values;
	}

	private async Task AppendRowAsync(IList<object> rowData)
	{
		SheetsService sheetsService = GetSheetsService();
		ValueRange body = new ValueRange
		{
			Values = new List<IList<object>> { rowData }
		};
		SpreadsheetsResource.ValuesResource.AppendRequest appendRequest = sheetsService.Spreadsheets.Values.Append(body, "12twWLzhnaIxKK3PWlwrOLUoAts-u7MVb_JdnGai1MnU", "Login Sheet!A:H");
		appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
		await appendRequest.ExecuteAsync();
	}

	private async Task<string> GenerateTokenAsync()
	{
		IList<IList<object>> list = await GetSheetDataAsync();
		int result = 1001;
		if (list != null && list.Count > 1)
		{
			IList<object> list2 = list[list.Count - 1];
			if (list2.Count >= 8)
			{
				string text = list2[7].ToString();
				if (text.StartsWith("MDM"))
				{
					int.TryParse(text.Substring(3), out result);
					result++;
				}
			}
		}
		return "MDM" + result;
	}

	private async Task<int> FindExpiredRowAsync()
	{
		IList<IList<object>> list = await GetSheetDataAsync();
		if (list == null || list.Count <= 1)
		{
			return -1;
		}
		for (int i = 1; i < list.Count; i++)
		{
			IList<object> list2 = list[i];
			if (list2.Count >= 8 && !(list2[5].ToString().Trim() != "0") && DateTime.TryParse(list2[6].ToString().Trim(), out var result) && (DateTime.Now - result).TotalHours >= 24.0)
			{
				return i + 1;
			}
		}
		return -1;
	}

	private async Task UpdateExpiredRowAsync(int rowNumber, string token)
	{
		SheetsService sheetsService = GetSheetsService();
		string range = $"Login Sheet!A{rowNumber}:H{rowNumber}";
		List<IList<object>> values = new List<IList<object>>
		{
			new List<object>
			{
				((Control)txtRegUserName).Text.Trim(),
				((Control)txtRegPassword).Text.Trim(),
				((Control)textBoxHWID).Text.Trim(),
				((Control)txtEmail).Text.Trim(),
				((Control)txtMobile).Text.Trim(),
				"0",
				DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
				token
			}
		};
		ValueRange body = new ValueRange
		{
			Values = values
		};
		SpreadsheetsResource.ValuesResource.UpdateRequest updateRequest = sheetsService.Spreadsheets.Values.Update(body, "12twWLzhnaIxKK3PWlwrOLUoAts-u7MVb_JdnGai1MnU", range);
		updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
		await updateRequest.ExecuteAsync();
	}

	private async Task<string> RegisterUserAsync()
	{
		string token = await GenerateTokenAsync();
		IList<object> rowData = new List<object>
		{
			((Control)txtRegUserName).Text.Trim(),
			((Control)txtRegPassword).Text.Trim(),
			((Control)textBoxHWID).Text.Trim(),
			((Control)txtEmail).Text.Trim(),
			((Control)txtMobile).Text.Trim(),
			"0",
			DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt"),
			token
		};
		await AppendRowAsync(rowData);
		return token;
	}

	private async Task<string> CheckRegistrationAsync()
	{
		IList<IList<object>> list = await GetSheetDataAsync();
		if (list == null || list.Count == 0)
		{
			return "OK";
		}
		string value = ((Control)txtRegUserName).Text.Trim();
		string value2 = ((Control)textBoxHWID).Text.Trim();
		string value3 = ((Control)txtEmail).Text.Trim();
		string value4 = ((Control)txtMobile).Text.Trim();
		foreach (IList<object> item in list)
		{
			if (item != null && item.Count != 0)
			{
				string text = ((item.Count <= 0) ? "" : item[0]?.ToString().Trim());
				string text2 = ((item.Count <= 2) ? "" : item[2]?.ToString().Trim());
				string text3 = ((item.Count <= 3) ? "" : item[3]?.ToString().Trim());
				string value5 = ((item.Count <= 4) ? "" : item[4]?.ToString().Trim());
				if (!string.IsNullOrWhiteSpace(value) && text.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					return "USERNAME";
				}
				if (!string.IsNullOrWhiteSpace(value2) && text2.Equals(value2, StringComparison.OrdinalIgnoreCase))
				{
					return "HWID";
				}
				if (!string.IsNullOrWhiteSpace(value3) && text3.Equals(value3, StringComparison.OrdinalIgnoreCase))
				{
					return "EMAIL";
				}
				if (!string.IsNullOrWhiteSpace(value4) && Normalize(value4) == Normalize(value5))
				{
					return "MOBILE";
				}
			}
		}
		return "OK";
	}

	private bool ValidateCredentials(IList<IList<object>> sheetData, string username, string password, string hardwareID)
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		foreach (IList<object> sheetDatum in sheetData)
		{
			if (sheetDatum.Count < 8)
			{
				continue;
			}
			string text = sheetDatum[0].ToString().Trim();
			string text2 = sheetDatum[1].ToString().Trim();
			string text3 = sheetDatum[2].ToString().Trim();
			string text4 = sheetDatum[5].ToString().Trim();
			if (username == text && password == text2 && hardwareID == text3)
			{
				if (text4 == "0")
				{
					MessageBox.Show("Your account is waiting for activation.\n\nPlease complete your payment and send us:\n\n✔ Payment Screenshot\n✔ Token ID\n\nvia WhatsApp.. (Please Don't Call Us)", "Account Not Activated", (MessageBoxButtons)0, (MessageBoxIcon)64);
					return false;
				}
				if (text4 == "1")
				{
					return true;
				}
				MessageBox.Show("Your account status is invalid.\nPlease contact support.", "Login", (MessageBoxButtons)0, (MessageBoxIcon)48);
				return false;
			}
		}
		return false;
	}

	private SheetsService GetSheetsService()
	{
		ServiceAccountCredential httpClientInitializer = new ServiceAccountCredential(new ServiceAccountCredential.Initializer("mdm-uart-monitor-login@mdm-uart-monitor.iam.gserviceaccount.com")
		{
			Scopes = Scopes
		}.FromPrivateKey("-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQC6qwvQ5UmUP/aj\n+UiE4Rq98m5VKMNc5+gNwLrLZdtBF9Hspkj4FwXohfZ3Dtg4MvGR9NXuncmzbTBu\ncy5ZIrl8VlkfzCWDkxq2JKe8NASdeVLtRF6HyLDQ9X1IfdUH4K8CEiEkfO0yl6L/\nsHf37CSpJ/nxaIOeH/uqOv8zoKXGm7u9tOSzC3rK8nK2WfAX4r5oRnSOBu56zKBs\nEv3VB0PB86A5BX+cKC6pI72XPoPyoFHtvp0LF77KOUGrlOFjhc/OejqYfc+qZv5r\nl2gAgTESzfx+n8ZIxfSjbXvTH9pg0912QAEle3oxedjFw/Pg7dbXoNY8Hf8sevfB\n27/oPfFNAgMBAAECggEAEvshkHRALxrx8Wp/vUNGfNoR7gbfFqteJUfbtqL80Hmj\nlvSxpshwiVxV23Ve9xf3CGhk+IaC5APOUOzVYaruQX5mQZ14kxB6PtsrUy/W6ExV\n6lpxYk097YGI474n0009fnBpflRVqjdRE77V9BfHAoxWFf4SpBmMdZraKF3FfZRb\npotLJYbtfdcSVVir45wLZcFrcITCZATMfenqNGDRNAu/8z5+8JWZ6l5fywQMtWci\ntpH87iaWPamqf6u8hZF0IkJdrfB+RLhLkAPfPDYT9OkViC8jDyFwKswIcjaHu87t\ni+411hA0woRchjNwnaC5/79eRff22O083jEXhS9o4QKBgQDy5OXuBrF6v5zlX/jm\nC/MGKxKx4D7uNPAiZX7FqE6aVMwYZnUuH2/pVhMOcIWlr06ULJqkPToVIMm6Shf4\ni5H9kIDRPumrrkVkS/b7pHerJwZjVq+rob2WYmpH3cxJFFrnsdpi79Q47lcODsi4\n54LXNu17FgKktbeWiqtcSf91kwKBgQDEvX9NTonTm0sYsTsw6YTPOBmXlEQMOoWA\nEqAzUraafPHnIZr9n5SGemQxZxzCPaPKwAm3CB9C2k23lEOWH6LuENr9KReDXK+T\nCw7H8EUjJ9JCv+ImZbUcUvuz3z0O52ooDu6dvPNc0RN2aOw3jX79JqQQ4QbikuFG\nWMP99d9JnwKBgQCjCU3k5Y0/zVotBKx3DdJEc6kKyHC1R8df7yyW1BBeu0h7oFpL\naUKfJu2HpbOvsOpXv1DVqVtRTz7Y0l8gr96AivxMd6Q1XrapSewwoQbKVVmOWGN2\nGwy84rZgItq6uvUBzHnAKycSR7kbcIT+yDck6nOhSPbhRDBE1jOLsQfODQKBgD1q\nEhUl0Sx3K6gXmyyXmNe+4W5J4DwZAJeMOOoMhegP3MuT1/gYGpsoFaK+V2HjMAAY\nlm/GX0vVLbGC3N4+HfRG2rVwYGCnTD+QkhqjzFE/GwmvMH9f/HtNBDmQFFVUs/Sx\nRJeE5ksxEOxDsIle6CdKKqkCNc4An8X/656ZZAxtAoGAYI+KzV1OUMFgb0NPDwpi\noz9gDoJp9yNZNjj+/mPjb55JzfFJm1f0+JC+C/jQodLREWD/VES6SK1Dp2FyUCJr\nJZVOWVsyhWPW9AIqXpTYa3FtXng8psGTZPALe26EJqfKg7dOnlQQhtAIbiJI1nZX\nSZPiBoc8UvQ1fPUCMyIKMnw=\n-----END PRIVATE KEY-----\n"));
		return new SheetsService(new BaseClientService.Initializer
		{
			HttpClientInitializer = httpClientInitializer,
			ApplicationName = ApplicationName
		});
	}

	private void TextBox_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			buttonLogin_Click(sender, (EventArgs)(object)e);
			e.Handled = true;
			e.SuppressKeyPress = true;
		}
	}

	private string GetWMI(string wmiClass, string property)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ManagementObjectSearcher val = new ManagementObjectSearcher("SELECT " + property + " FROM " + wmiClass);
			try
			{
				ManagementObjectEnumerator enumerator = val.Get().GetEnumerator();
				try
				{
					if (enumerator.MoveNext())
					{
						return ((ManagementBaseObject)(ManagementObject)enumerator.Current)[property]?.ToString();
					}
				}
				finally
				{
					((IDisposable)enumerator)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch
		{
		}
		return null;
	}

	private bool IsValidID(string id)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			return false;
		}
		id = id.Trim().ToLower();
		if (id != "default string" && id != "to be filled by o.e.m." && id != "unknown" && id != "none")
		{
			return id != "0";
		}
		return false;
	}

	private string Normalize(string value)
	{
		return value.Trim().Replace(" ", "").Replace("-", "")
			.ToUpper();
	}

	private string GetHardwareID()
	{
		try
		{
			string wMI = GetWMI("Win32_BaseBoard", "SerialNumber");
			if (IsValidID(wMI))
			{
				return "B-" + Normalize(wMI);
			}
			string wMI2 = GetWMI("Win32_PhysicalMedia", "SerialNumber");
			if (IsValidID(wMI2))
			{
				return "D-" + Normalize(wMI2);
			}
			wMI2 = GetWMI("Win32_LogicalDisk", "VolumeSerialNumber");
			if (IsValidID(wMI2))
			{
				return "V-" + Normalize(wMI2);
			}
		}
		catch
		{
		}
		return "UNKNOWN-PC";
	}

	private void checkBoxRemeberMe_CheckedChanged(object sender, EventArgs e)
	{
		if (checkBoxRemeberMe.Checked)
		{
			Settings.Default.Username = ((Control)textBoxUsername).Text;
			Settings.Default.Password = ((Control)textBoxPassword).Text;
			((SettingsBase)Settings.Default).Save();
		}
		else
		{
			((ApplicationSettingsBase)Settings.Default).Reset();
		}
	}

	private void btnToggleVisibility_Click(object sender, EventArgs e)
	{
		if (areCredentialsVisible)
		{
			textBoxPassword.PasswordChar = '*';
			((Control)btnToggleVisibility).Text = "Show";
		}
		else
		{
			textBoxPassword.PasswordChar = '\0';
			((Control)btnToggleVisibility).Text = "Hide";
		}
		areCredentialsVisible = !areCredentialsVisible;
	}

	private void btnToggleVisibility1_Click(object sender, EventArgs e)
	{
		if (areCredentialsVisible1)
		{
			textBoxUsername.PasswordChar = '*';
			((Control)btnToggleVisibility1).Text = "Show";
		}
		else
		{
			textBoxUsername.PasswordChar = '\0';
			((Control)btnToggleVisibility1).Text = "Hide";
		}
		areCredentialsVisible1 = !areCredentialsVisible1;
	}

	private void btnHWIDCOPY_Click_1(object sender, EventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(((Control)textBoxHWID).Text))
		{
			Clipboard.SetText(((Control)textBoxHWID).Text);
		}
	}

	private async Task ShowLoading(string text, int progress)
	{
		((Control)lblLoadingStatus).Visible = true;
		((Control)progressLoading).Visible = true;
		((Control)lblLoadingStatus).Text = text;
		progressLoading.Value = progress;
		await Task.Delay(250);
	}

	private async void btnRegister_Click(object sender, EventArgs e)
	{
		await ShowLoading("Connecting To Server...", 10);
		await ShowLoading("Checking Username...", 30);
		await ShowLoading("Uploading Registration...", 80);
		await ShowLoading("Generating Token...", 95);
		if (!chkAgree.Checked)
		{
			MessageBox.Show("Please read and accept the Purchase Agreement.", "Agreement Required", (MessageBoxButtons)0, (MessageBoxIcon)48);
			((Control)chkAgree).Focus();
			((Control)lblLoadingStatus).Visible = false;
			((Control)progressLoading).Visible = false;
			return;
		}
		if (string.IsNullOrWhiteSpace(((Control)txtRegUserName).Text))
		{
			MessageBox.Show("Please enter Username.", "Registration", (MessageBoxButtons)0, (MessageBoxIcon)48);
			((Control)txtRegUserName).Focus();
			((Control)lblLoadingStatus).Visible = false;
			((Control)progressLoading).Visible = false;
			return;
		}
		if (string.IsNullOrWhiteSpace(((Control)txtRegPassword).Text))
		{
			MessageBox.Show("Please enter Password.", "Registration", (MessageBoxButtons)0, (MessageBoxIcon)48);
			((Control)txtRegPassword).Focus();
			((Control)lblLoadingStatus).Visible = false;
			((Control)progressLoading).Visible = false;
			return;
		}
		if (string.IsNullOrWhiteSpace(((Control)txtEmail).Text))
		{
			MessageBox.Show("Please enter Email.", "Registration", (MessageBoxButtons)0, (MessageBoxIcon)48);
			((Control)txtEmail).Focus();
			((Control)lblLoadingStatus).Visible = false;
			((Control)progressLoading).Visible = false;
			return;
		}
		if (string.IsNullOrWhiteSpace(((Control)txtMobile).Text))
		{
			MessageBox.Show("Please enter Mobile Number.", "Registration", (MessageBoxButtons)0, (MessageBoxIcon)48);
			((Control)lblLoadingStatus).Visible = false;
			((Control)progressLoading).Visible = false;
			((Control)txtMobile).Focus();
			return;
		}
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			MessageBox.Show("No internet connection detected.", "Network Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
			((Control)lblLoadingStatus).Visible = false;
			((Control)progressLoading).Visible = false;
			return;
		}
		switch (await CheckRegistrationAsync())
		{
		case "USERNAME":
			MessageBox.Show("This Username is already registered.\n\nPlease choose another Username.", "Registration", (MessageBoxButtons)0, (MessageBoxIcon)48);
			((Control)txtRegUserName).Focus();
			((Control)lblLoadingStatus).Visible = false;
			((Control)progressLoading).Visible = false;
			return;
		case "HWID":
			MessageBox.Show("This computer is already registered.", "Registration", (MessageBoxButtons)0, (MessageBoxIcon)48);
			((Control)lblLoadingStatus).Visible = false;
			((Control)progressLoading).Visible = false;
			return;
		case "EMAIL":
			MessageBox.Show("This Email ID is already registered.", "Registration", (MessageBoxButtons)0, (MessageBoxIcon)48);
			((Control)txtEmail).Focus();
			((Control)lblLoadingStatus).Visible = false;
			((Control)progressLoading).Visible = false;
			return;
		case "MOBILE":
			MessageBox.Show("This Mobile Number is already registered.", "Registration", (MessageBoxButtons)0, (MessageBoxIcon)48);
			((Control)txtMobile).Focus();
			((Control)lblLoadingStatus).Visible = false;
			((Control)progressLoading).Visible = false;
			return;
		}
		int expiredRow = await FindExpiredRowAsync();
		string token;
		if (expiredRow != -1)
		{
			token = (await GetSheetDataAsync())[expiredRow - 1][7].ToString();
			await UpdateExpiredRowAsync(expiredRow, token);
		}
		else
		{
			token = await RegisterUserAsync();
		}
		((Control)lblLoadingStatus).Visible = false;
		((Control)progressLoading).Visible = false;
		((Control)textBoxUsername).Text = ((Control)txtRegUserName).Text;
		((Control)textBoxPassword).Text = ((Control)txtRegPassword).Text;
		((Form)new frmRegistrationSuccess
		{
			TokenID = token
		}).ShowDialog();
		((TextBoxBase)txtRegUserName).Clear();
		((TextBoxBase)txtRegPassword).Clear();
		((TextBoxBase)txtEmail).Clear();
		((TextBoxBase)txtMobile).Clear();
		chkAgree.Checked = false;
		((Control)txtRegUserName).Focus();
	}

	private void btnFormClose_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void checkBoxAutoLogin_CheckedChanged(object sender, EventArgs e)
	{
		Settings.Default.AutoLogin = checkBoxAutoLogin.Checked;
		((SettingsBase)Settings.Default).Save();
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
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Expected O, but got Unknown
		//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e2: Expected O, but got Unknown
		//IL_089d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a7: Expected O, but got Unknown
		//IL_0fb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fba: Expected O, but got Unknown
		//IL_104d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1057: Expected O, but got Unknown
		//IL_116f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1179: Expected O, but got Unknown
		//IL_12b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c1: Expected O, but got Unknown
		//IL_134a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1354: Expected O, but got Unknown
		//IL_13e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ef: Expected O, but got Unknown
		//IL_14ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b5: Expected O, but got Unknown
		//IL_1603: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bba: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc4: Expected O, but got Unknown
		//IL_1c56: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c60: Expected O, but got Unknown
		//IL_1cda: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce4: Expected O, but got Unknown
		//IL_1d5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d68: Expected O, but got Unknown
		//IL_1ddf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de9: Expected O, but got Unknown
		//IL_1ed6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee0: Expected O, but got Unknown
		//IL_1fe6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff0: Expected O, but got Unknown
		//IL_212b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2135: Expected O, but got Unknown
		//IL_21c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d3: Expected O, but got Unknown
		//IL_226a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2274: Expected O, but got Unknown
		//IL_2327: Unknown result type (might be due to invalid IL or missing references)
		//IL_2331: Expected O, but got Unknown
		//IL_23c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_23d2: Expected O, but got Unknown
		//IL_24ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b6: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Form2));
		mdmPanel1 = new MDMPanel();
		mdmPanel5 = new MDMPanel();
		label8 = new Label();
		pictureBox1 = new PictureBox();
		mdmPanel2 = new MDMPanel();
		checkBoxResetHWID = new CheckBox();
		checkBoxAutoLogin = new CheckBox();
		btnHWIDCOPY = new MDMButton();
		btnToggleVisibility1 = new MDMButton();
		btnToggleVisibility = new MDMButton();
		textBoxHWID = new TextBox();
		label1 = new Label();
		labelHWID = new Label();
		buttonLogin = new MDMButton();
		textBoxPassword = new TextBox();
		textBoxUsername = new TextBox();
		checkBoxRemeberMe = new CheckBox();
		label2 = new Label();
		fastColoredTextBox1 = new FastColoredTextBox();
		btnFormClose = new MDMButton();
		mdmPanel3 = new MDMPanel();
		mdmPanel4 = new MDMPanel();
		label3 = new Label();
		txtMobile = new TextBox();
		txtEmail = new TextBox();
		txtRegUserName = new TextBox();
		txtRegPassword = new TextBox();
		progressLoading = new ProgressBar();
		label5 = new Label();
		btnRegister = new MDMButton();
		label4 = new Label();
		label7 = new Label();
		lblLoadingStatus = new Label();
		label6 = new Label();
		chkAgree = new CheckBox();
		((Control)mdmPanel1).SuspendLayout();
		((Control)mdmPanel5).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)mdmPanel2).SuspendLayout();
		((ISupportInitialize)fastColoredTextBox1).BeginInit();
		((Control)mdmPanel3).SuspendLayout();
		((Control)mdmPanel4).SuspendLayout();
		((Control)this).SuspendLayout();
		mdmPanel1.AnimationSpeed = 1;
		mdmPanel1.BorderColor = System.Drawing.Color.Tan;
		mdmPanel1.BorderRadius = 15;
		mdmPanel1.BorderSize = 2;
		((Control)mdmPanel1).Controls.Add((Control)(object)mdmPanel5);
		((Control)mdmPanel1).Controls.Add((Control)(object)pictureBox1);
		((Control)mdmPanel1).Controls.Add((Control)(object)mdmPanel2);
		((Control)mdmPanel1).Controls.Add((Control)(object)fastColoredTextBox1);
		((Control)mdmPanel1).Controls.Add((Control)(object)btnFormClose);
		((Control)mdmPanel1).Controls.Add((Control)(object)mdmPanel3);
		((Control)mdmPanel1).Dock = (DockStyle)5;
		mdmPanel1.EnableGradient = true;
		mdmPanel1.EnableShadow = true;
		mdmPanel1.GradientAngle = 90f;
		mdmPanel1.GradientEndColor = System.Drawing.Color.MediumSlateBlue;
		mdmPanel1.GradientStartColor = SystemColors.ActiveCaption;
		mdmPanel1.HoverBorderSize = 3;
		((Control)mdmPanel1).Location = new Point(0, 0);
		((Control)mdmPanel1).Name = "mdmPanel1";
		mdmPanel1.ShadowOpacity = 35;
		mdmPanel1.ShadowSize = 8;
		((Control)mdmPanel1).Size = new Size(1003, 664);
		((Control)mdmPanel1).TabIndex = 32;
		mdmPanel5.AnimationSpeed = 1;
		((Control)mdmPanel5).BackColor = SystemColors.ControlDark;
		mdmPanel5.BorderColor = System.Drawing.Color.DimGray;
		mdmPanel5.BorderRadius = 15;
		mdmPanel5.BorderSize = 1;
		((Control)mdmPanel5).Controls.Add((Control)(object)label8);
		mdmPanel5.EnableGradient = true;
		mdmPanel5.EnableShadow = false;
		mdmPanel5.GradientAngle = 90f;
		mdmPanel5.GradientEndColor = System.Drawing.Color.DimGray;
		mdmPanel5.GradientStartColor = System.Drawing.Color.Tomato;
		mdmPanel5.HoverBorderSize = 3;
		((Control)mdmPanel5).Location = new Point(696, 12);
		((Control)mdmPanel5).Name = "mdmPanel5";
		mdmPanel5.ShadowOpacity = 35;
		mdmPanel5.ShadowSize = 8;
		((Control)mdmPanel5).Size = new Size(200, 33);
		((Control)mdmPanel5).TabIndex = 35;
		((Control)label8).AutoSize = true;
		((Control)label8).BackColor = System.Drawing.Color.Transparent;
		((Control)label8).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label8).ForeColor = System.Drawing.Color.White;
		((Control)label8).Location = new Point(52, 7);
		((Control)label8).Name = "label8";
		((Control)label8).Size = new Size(87, 20);
		((Control)label8).TabIndex = 0;
		((Control)label8).Text = "Login Here";
		((Control)pictureBox1).BackColor = System.Drawing.Color.Transparent;
		((Control)pictureBox1).BackgroundImage = (Image)(object)Resources.UART_Logo;
		((Control)pictureBox1).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox1).Location = new Point(5, 9);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(318, 332);
		pictureBox1.TabIndex = 47;
		pictureBox1.TabStop = false;
		mdmPanel2.AnimationSpeed = 1;
		mdmPanel2.BorderColor = System.Drawing.Color.AliceBlue;
		mdmPanel2.BorderRadius = 15;
		mdmPanel2.BorderSize = 2;
		((Control)mdmPanel2).Controls.Add((Control)(object)checkBoxResetHWID);
		((Control)mdmPanel2).Controls.Add((Control)(object)checkBoxAutoLogin);
		((Control)mdmPanel2).Controls.Add((Control)(object)btnHWIDCOPY);
		((Control)mdmPanel2).Controls.Add((Control)(object)btnToggleVisibility1);
		((Control)mdmPanel2).Controls.Add((Control)(object)btnToggleVisibility);
		((Control)mdmPanel2).Controls.Add((Control)(object)textBoxHWID);
		((Control)mdmPanel2).Controls.Add((Control)(object)label1);
		((Control)mdmPanel2).Controls.Add((Control)(object)labelHWID);
		((Control)mdmPanel2).Controls.Add((Control)(object)buttonLogin);
		((Control)mdmPanel2).Controls.Add((Control)(object)textBoxPassword);
		((Control)mdmPanel2).Controls.Add((Control)(object)textBoxUsername);
		((Control)mdmPanel2).Controls.Add((Control)(object)checkBoxRemeberMe);
		((Control)mdmPanel2).Controls.Add((Control)(object)label2);
		mdmPanel2.EnableGradient = true;
		mdmPanel2.EnableShadow = false;
		mdmPanel2.GradientAngle = 90f;
		mdmPanel2.GradientEndColor = System.Drawing.Color.LightCyan;
		mdmPanel2.GradientStartColor = SystemColors.ActiveCaption;
		mdmPanel2.HoverBorderSize = 3;
		((Control)mdmPanel2).Location = new Point(639, 12);
		((Control)mdmPanel2).Name = "mdmPanel2";
		mdmPanel2.ShadowOpacity = 35;
		mdmPanel2.ShadowSize = 8;
		((Control)mdmPanel2).Size = new Size(304, 329);
		((Control)mdmPanel2).TabIndex = 36;
		((Control)checkBoxResetHWID).AutoSize = true;
		((Control)checkBoxResetHWID).BackColor = System.Drawing.Color.Transparent;
		((Control)checkBoxResetHWID).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)checkBoxResetHWID).ForeColor = System.Drawing.Color.Black;
		((Control)checkBoxResetHWID).Location = new Point(46, 215);
		((Control)checkBoxResetHWID).Name = "checkBoxResetHWID";
		((Control)checkBoxResetHWID).Size = new Size(119, 24);
		((Control)checkBoxResetHWID).TabIndex = 36;
		((Control)checkBoxResetHWID).Text = "Reset HWID";
		((ButtonBase)checkBoxResetHWID).UseVisualStyleBackColor = false;
		((Control)checkBoxAutoLogin).AutoSize = true;
		((Control)checkBoxAutoLogin).BackColor = System.Drawing.Color.Transparent;
		checkBoxAutoLogin.Checked = true;
		checkBoxAutoLogin.CheckState = (CheckState)1;
		((Control)checkBoxAutoLogin).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)checkBoxAutoLogin).ForeColor = System.Drawing.Color.Black;
		((Control)checkBoxAutoLogin).Location = new Point(46, 261);
		((Control)checkBoxAutoLogin).Name = "checkBoxAutoLogin";
		((Control)checkBoxAutoLogin).Size = new Size(105, 24);
		((Control)checkBoxAutoLogin).TabIndex = 35;
		((Control)checkBoxAutoLogin).Text = "Auto Login";
		((ButtonBase)checkBoxAutoLogin).UseVisualStyleBackColor = false;
		checkBoxAutoLogin.CheckedChanged += checkBoxAutoLogin_CheckedChanged;
		((Control)btnHWIDCOPY).BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
		btnHWIDCOPY.BackgroundColor = System.Drawing.Color.FromArgb(0, 120, 215);
		btnHWIDCOPY.BorderColor = System.Drawing.Color.FromArgb(255, 224, 192);
		btnHWIDCOPY.BorderRadius = 12;
		btnHWIDCOPY.BorderSize = 2;
		((Control)btnHWIDCOPY).Cursor = Cursors.Hand;
		btnHWIDCOPY.EnableGradient = false;
		btnHWIDCOPY.EnableShadow = true;
		((ButtonBase)btnHWIDCOPY).FlatAppearance.BorderSize = 0;
		((ButtonBase)btnHWIDCOPY).FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
		((ButtonBase)btnHWIDCOPY).FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
		((ButtonBase)btnHWIDCOPY).FlatStyle = (FlatStyle)0;
		((Control)btnHWIDCOPY).ForeColor = System.Drawing.Color.White;
		btnHWIDCOPY.GradientAngle = 90f;
		btnHWIDCOPY.GradientEndColor = System.Drawing.Color.LightSkyBlue;
		btnHWIDCOPY.GradientStartColor = System.Drawing.Color.White;
		btnHWIDCOPY.HoverColor = System.Drawing.Color.FromArgb(30, 144, 255);
		((Control)btnHWIDCOPY).Location = new Point(207, 67);
		((Control)btnHWIDCOPY).Name = "btnHWIDCOPY";
		btnHWIDCOPY.ShadowColor = System.Drawing.Color.Black;
		btnHWIDCOPY.ShadowOpacity = 35;
		btnHWIDCOPY.ShadowSize = 8;
		((Control)btnHWIDCOPY).Size = new Size(71, 35);
		((Control)btnHWIDCOPY).TabIndex = 34;
		((Control)btnHWIDCOPY).Text = "Copy";
		btnHWIDCOPY.TextColor = System.Drawing.Color.White;
		((ButtonBase)btnHWIDCOPY).UseVisualStyleBackColor = false;
		((Control)btnHWIDCOPY).Click += btnHWIDCOPY_Click_1;
		((Control)btnToggleVisibility1).BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
		btnToggleVisibility1.BackgroundColor = System.Drawing.Color.FromArgb(0, 120, 215);
		btnToggleVisibility1.BorderColor = System.Drawing.Color.FromArgb(255, 224, 192);
		btnToggleVisibility1.BorderRadius = 12;
		btnToggleVisibility1.BorderSize = 2;
		((Control)btnToggleVisibility1).Cursor = Cursors.Hand;
		btnToggleVisibility1.EnableGradient = false;
		btnToggleVisibility1.EnableShadow = true;
		((ButtonBase)btnToggleVisibility1).FlatAppearance.BorderSize = 0;
		((ButtonBase)btnToggleVisibility1).FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
		((ButtonBase)btnToggleVisibility1).FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
		((ButtonBase)btnToggleVisibility1).FlatStyle = (FlatStyle)0;
		((Control)btnToggleVisibility1).ForeColor = System.Drawing.Color.White;
		btnToggleVisibility1.GradientAngle = 90f;
		btnToggleVisibility1.GradientEndColor = System.Drawing.Color.LightSkyBlue;
		btnToggleVisibility1.GradientStartColor = System.Drawing.Color.White;
		btnToggleVisibility1.HoverColor = System.Drawing.Color.FromArgb(30, 144, 255);
		((Control)btnToggleVisibility1).Location = new Point(207, 124);
		((Control)btnToggleVisibility1).Name = "btnToggleVisibility1";
		btnToggleVisibility1.ShadowColor = System.Drawing.Color.Black;
		btnToggleVisibility1.ShadowOpacity = 35;
		btnToggleVisibility1.ShadowSize = 8;
		((Control)btnToggleVisibility1).Size = new Size(71, 35);
		((Control)btnToggleVisibility1).TabIndex = 33;
		((Control)btnToggleVisibility1).Text = "Show";
		btnToggleVisibility1.TextColor = System.Drawing.Color.White;
		((ButtonBase)btnToggleVisibility1).UseVisualStyleBackColor = false;
		((Control)btnToggleVisibility1).Click += btnToggleVisibility1_Click;
		((Control)btnToggleVisibility).BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
		btnToggleVisibility.BackgroundColor = System.Drawing.Color.FromArgb(0, 120, 215);
		btnToggleVisibility.BorderColor = System.Drawing.Color.FromArgb(255, 224, 192);
		btnToggleVisibility.BorderRadius = 12;
		btnToggleVisibility.BorderSize = 2;
		((Control)btnToggleVisibility).Cursor = Cursors.Hand;
		btnToggleVisibility.EnableGradient = false;
		btnToggleVisibility.EnableShadow = true;
		((ButtonBase)btnToggleVisibility).FlatAppearance.BorderSize = 0;
		((ButtonBase)btnToggleVisibility).FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
		((ButtonBase)btnToggleVisibility).FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
		((ButtonBase)btnToggleVisibility).FlatStyle = (FlatStyle)0;
		((Control)btnToggleVisibility).ForeColor = System.Drawing.Color.White;
		btnToggleVisibility.GradientAngle = 90f;
		btnToggleVisibility.GradientEndColor = System.Drawing.Color.LightSkyBlue;
		btnToggleVisibility.GradientStartColor = System.Drawing.Color.White;
		btnToggleVisibility.HoverColor = System.Drawing.Color.FromArgb(30, 144, 255);
		((Control)btnToggleVisibility).Location = new Point(206, 180);
		((Control)btnToggleVisibility).Name = "btnToggleVisibility";
		btnToggleVisibility.ShadowColor = System.Drawing.Color.Black;
		btnToggleVisibility.ShadowOpacity = 35;
		btnToggleVisibility.ShadowSize = 8;
		((Control)btnToggleVisibility).Size = new Size(71, 35);
		((Control)btnToggleVisibility).TabIndex = 32;
		((Control)btnToggleVisibility).Text = "Show";
		btnToggleVisibility.TextColor = System.Drawing.Color.White;
		((ButtonBase)btnToggleVisibility).UseVisualStyleBackColor = false;
		((Control)btnToggleVisibility).Click += btnToggleVisibility_Click;
		((Control)textBoxHWID).BackColor = System.Drawing.Color.Silver;
		((TextBoxBase)textBoxHWID).BorderStyle = (BorderStyle)1;
		((Control)textBoxHWID).ForeColor = SystemColors.WindowText;
		((Control)textBoxHWID).Location = new Point(33, 73);
		((TextBoxBase)textBoxHWID).Multiline = true;
		((Control)textBoxHWID).Name = "textBoxHWID";
		((TextBoxBase)textBoxHWID).ReadOnly = true;
		((Control)textBoxHWID).RightToLeft = (RightToLeft)0;
		((Control)textBoxHWID).Size = new Size(167, 28);
		((Control)textBoxHWID).TabIndex = 7;
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = System.Drawing.Color.Transparent;
		((Control)label1).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label1).ForeColor = System.Drawing.Color.Black;
		((Control)label1).Location = new Point(74, 107);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(89, 20);
		((Control)label1).TabIndex = 3;
		((Control)label1).Text = "User Name";
		((Control)labelHWID).AutoSize = true;
		((Control)labelHWID).BackColor = System.Drawing.Color.Transparent;
		((Control)labelHWID).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)labelHWID).ForeColor = System.Drawing.Color.Black;
		((Control)labelHWID).Location = new Point(42, 50);
		((Control)labelHWID).Name = "labelHWID";
		((Control)labelHWID).Size = new Size(137, 20);
		((Control)labelHWID).TabIndex = 8;
		((Control)labelHWID).Text = "Your Hardware ID";
		((Control)buttonLogin).BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
		buttonLogin.BackgroundColor = System.Drawing.Color.FromArgb(0, 120, 215);
		buttonLogin.BorderColor = System.Drawing.Color.LightSkyBlue;
		buttonLogin.BorderRadius = 12;
		buttonLogin.BorderSize = 2;
		((Control)buttonLogin).Cursor = Cursors.Hand;
		buttonLogin.EnableGradient = false;
		buttonLogin.EnableShadow = true;
		((ButtonBase)buttonLogin).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonLogin).FlatStyle = (FlatStyle)0;
		((Control)buttonLogin).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)buttonLogin).ForeColor = System.Drawing.Color.Bisque;
		buttonLogin.GradientAngle = 90f;
		buttonLogin.GradientEndColor = System.Drawing.Color.LightSkyBlue;
		buttonLogin.GradientStartColor = System.Drawing.Color.White;
		buttonLogin.HoverColor = System.Drawing.Color.FromArgb(192, 64, 0);
		((Control)buttonLogin).Location = new Point(60, 286);
		((Control)buttonLogin).Name = "buttonLogin";
		buttonLogin.ShadowColor = System.Drawing.Color.Black;
		buttonLogin.ShadowOpacity = 35;
		buttonLogin.ShadowSize = 8;
		((Control)buttonLogin).Size = new Size(192, 36);
		((Control)buttonLogin).TabIndex = 31;
		((Control)buttonLogin).Text = "Login";
		buttonLogin.TextColor = System.Drawing.Color.Bisque;
		((ButtonBase)buttonLogin).UseVisualStyleBackColor = false;
		((Control)buttonLogin).Click += buttonLogin_Click;
		((Control)textBoxPassword).BackColor = System.Drawing.Color.DarkGray;
		((TextBoxBase)textBoxPassword).BorderStyle = (BorderStyle)1;
		((Control)textBoxPassword).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)textBoxPassword).ForeColor = System.Drawing.Color.Black;
		((Control)textBoxPassword).Location = new Point(33, 184);
		((Control)textBoxPassword).Name = "textBoxPassword";
		((Control)textBoxPassword).Size = new Size(167, 26);
		((Control)textBoxPassword).TabIndex = 2;
		((Control)textBoxUsername).BackColor = System.Drawing.Color.DarkGray;
		((TextBoxBase)textBoxUsername).BorderStyle = (BorderStyle)1;
		((Control)textBoxUsername).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)textBoxUsername).Location = new Point(34, 128);
		((Control)textBoxUsername).Name = "textBoxUsername";
		((Control)textBoxUsername).Size = new Size(167, 26);
		((Control)textBoxUsername).TabIndex = 1;
		((Control)checkBoxRemeberMe).AutoSize = true;
		((Control)checkBoxRemeberMe).BackColor = System.Drawing.Color.Transparent;
		checkBoxRemeberMe.Checked = true;
		checkBoxRemeberMe.CheckState = (CheckState)1;
		((Control)checkBoxRemeberMe).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)checkBoxRemeberMe).ForeColor = System.Drawing.Color.Black;
		((Control)checkBoxRemeberMe).Location = new Point(46, 238);
		((Control)checkBoxRemeberMe).Name = "checkBoxRemeberMe";
		((Control)checkBoxRemeberMe).Size = new Size(133, 24);
		((Control)checkBoxRemeberMe).TabIndex = 6;
		((Control)checkBoxRemeberMe).Text = "Remember Me";
		((ButtonBase)checkBoxRemeberMe).UseVisualStyleBackColor = false;
		checkBoxRemeberMe.CheckedChanged += checkBoxRemeberMe_CheckedChanged;
		((Control)label2).AutoSize = true;
		((Control)label2).BackColor = System.Drawing.Color.Transparent;
		((Control)label2).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label2).ForeColor = System.Drawing.Color.Black;
		((Control)label2).Location = new Point(78, 163);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(78, 20);
		((Control)label2).TabIndex = 4;
		((Control)label2).Text = "Password";
		fastColoredTextBox1.AutoCompleteBracketsList = new char[10] { '(', ')', '{', '}', '[', ']', '"', '"', '\'', '\'' };
		fastColoredTextBox1.AutoScrollMinSize = new Size(930, 1722);
		fastColoredTextBox1.BackBrush = null;
		((Control)fastColoredTextBox1).BackColor = System.Drawing.Color.Transparent;
		fastColoredTextBox1.CharHeight = 14;
		fastColoredTextBox1.CharWidth = 8;
		((Control)fastColoredTextBox1).Cursor = Cursors.IBeam;
		fastColoredTextBox1.DisabledColor = System.Drawing.Color.FromArgb(100, 180, 180, 180);
		((Control)fastColoredTextBox1).ForeColor = System.Drawing.Color.Cornsilk;
		fastColoredTextBox1.IsReplaceMode = false;
		((Control)fastColoredTextBox1).Location = new Point(12, 360);
		((Control)fastColoredTextBox1).Name = "fastColoredTextBox1";
		fastColoredTextBox1.Paddings = new Padding(0);
		fastColoredTextBox1.SelectionColor = System.Drawing.Color.FromArgb(60, 0, 0, 255);
		fastColoredTextBox1.ServiceColors = (ServiceColors)componentResourceManager.GetObject("fastColoredTextBox1.ServiceColors");
		fastColoredTextBox1.ShowLineNumbers = false;
		((Control)fastColoredTextBox1).Size = new Size(988, 301);
		((Control)fastColoredTextBox1).TabIndex = 35;
		((Control)fastColoredTextBox1).Text = componentResourceManager.GetString("fastColoredTextBox1.Text");
		fastColoredTextBox1.Zoom = 100;
		((Control)btnFormClose).BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
		btnFormClose.BackgroundColor = System.Drawing.Color.FromArgb(0, 120, 215);
		btnFormClose.BorderColor = System.Drawing.Color.Silver;
		btnFormClose.BorderRadius = 12;
		btnFormClose.BorderSize = 2;
		((Control)btnFormClose).Cursor = Cursors.Hand;
		btnFormClose.EnableGradient = true;
		btnFormClose.EnableShadow = true;
		((ButtonBase)btnFormClose).FlatAppearance.BorderSize = 0;
		((ButtonBase)btnFormClose).FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
		((ButtonBase)btnFormClose).FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
		((ButtonBase)btnFormClose).FlatStyle = (FlatStyle)0;
		((Control)btnFormClose).ForeColor = System.Drawing.Color.White;
		btnFormClose.GradientAngle = 90f;
		btnFormClose.GradientEndColor = System.Drawing.Color.Maroon;
		btnFormClose.GradientStartColor = SystemColors.WindowFrame;
		btnFormClose.HoverColor = System.Drawing.Color.FromArgb(30, 144, 255);
		((Control)btnFormClose).Location = new Point(959, 3);
		((Control)btnFormClose).Name = "btnFormClose";
		btnFormClose.ShadowColor = System.Drawing.Color.Black;
		btnFormClose.ShadowOpacity = 35;
		btnFormClose.ShadowSize = 8;
		((Control)btnFormClose).Size = new Size(41, 36);
		((Control)btnFormClose).TabIndex = 32;
		((Control)btnFormClose).Text = "X";
		btnFormClose.TextColor = System.Drawing.Color.White;
		((ButtonBase)btnFormClose).UseVisualStyleBackColor = false;
		((Control)btnFormClose).Click += btnFormClose_Click;
		mdmPanel3.AnimationSpeed = 1;
		mdmPanel3.BorderColor = System.Drawing.Color.AliceBlue;
		mdmPanel3.BorderRadius = 15;
		mdmPanel3.BorderSize = 2;
		((Control)mdmPanel3).Controls.Add((Control)(object)mdmPanel4);
		((Control)mdmPanel3).Controls.Add((Control)(object)txtMobile);
		((Control)mdmPanel3).Controls.Add((Control)(object)txtEmail);
		((Control)mdmPanel3).Controls.Add((Control)(object)txtRegUserName);
		((Control)mdmPanel3).Controls.Add((Control)(object)txtRegPassword);
		((Control)mdmPanel3).Controls.Add((Control)(object)progressLoading);
		((Control)mdmPanel3).Controls.Add((Control)(object)label5);
		((Control)mdmPanel3).Controls.Add((Control)(object)btnRegister);
		((Control)mdmPanel3).Controls.Add((Control)(object)label4);
		((Control)mdmPanel3).Controls.Add((Control)(object)label7);
		((Control)mdmPanel3).Controls.Add((Control)(object)lblLoadingStatus);
		((Control)mdmPanel3).Controls.Add((Control)(object)label6);
		((Control)mdmPanel3).Controls.Add((Control)(object)chkAgree);
		mdmPanel3.EnableGradient = true;
		mdmPanel3.EnableShadow = false;
		mdmPanel3.GradientAngle = 90f;
		mdmPanel3.GradientEndColor = System.Drawing.Color.LightCyan;
		mdmPanel3.GradientStartColor = SystemColors.ActiveCaption;
		mdmPanel3.HoverBorderSize = 3;
		((Control)mdmPanel3).Location = new Point(329, 12);
		((Control)mdmPanel3).Name = "mdmPanel3";
		mdmPanel3.ShadowOpacity = 35;
		mdmPanel3.ShadowSize = 8;
		((Control)mdmPanel3).Size = new Size(304, 329);
		((Control)mdmPanel3).TabIndex = 37;
		mdmPanel4.AnimationSpeed = 1;
		((Control)mdmPanel4).BackColor = SystemColors.ControlDark;
		mdmPanel4.BorderColor = System.Drawing.Color.DimGray;
		mdmPanel4.BorderRadius = 15;
		mdmPanel4.BorderSize = 1;
		((Control)mdmPanel4).Controls.Add((Control)(object)label3);
		mdmPanel4.EnableGradient = true;
		mdmPanel4.EnableShadow = false;
		mdmPanel4.GradientAngle = 90f;
		mdmPanel4.GradientEndColor = System.Drawing.Color.DimGray;
		mdmPanel4.GradientStartColor = System.Drawing.Color.SpringGreen;
		mdmPanel4.HoverBorderSize = 3;
		((Control)mdmPanel4).Location = new Point(53, 0);
		((Control)mdmPanel4).Name = "mdmPanel4";
		mdmPanel4.ShadowOpacity = 35;
		mdmPanel4.ShadowSize = 8;
		((Control)mdmPanel4).Size = new Size(200, 33);
		((Control)mdmPanel4).TabIndex = 34;
		((Control)label3).AutoSize = true;
		((Control)label3).BackColor = System.Drawing.Color.Transparent;
		((Control)label3).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label3).ForeColor = System.Drawing.Color.White;
		((Control)label3).Location = new Point(49, 6);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(108, 20);
		((Control)label3).TabIndex = 0;
		((Control)label3).Text = "Register Here";
		((Control)txtMobile).BackColor = System.Drawing.Color.DarkGray;
		((TextBoxBase)txtMobile).BorderStyle = (BorderStyle)1;
		((Control)txtMobile).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMobile).Location = new Point(15, 222);
		((Control)txtMobile).Name = "txtMobile";
		((Control)txtMobile).Size = new Size(276, 26);
		((Control)txtMobile).TabIndex = 20;
		((Control)txtEmail).BackColor = System.Drawing.Color.DarkGray;
		((TextBoxBase)txtEmail).BorderStyle = (BorderStyle)1;
		((Control)txtEmail).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtEmail).Location = new Point(15, 173);
		((Control)txtEmail).Name = "txtEmail";
		((Control)txtEmail).Size = new Size(276, 26);
		((Control)txtEmail).TabIndex = 22;
		((Control)txtRegUserName).BackColor = System.Drawing.Color.DarkGray;
		((TextBoxBase)txtRegUserName).BorderStyle = (BorderStyle)1;
		((Control)txtRegUserName).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtRegUserName).Location = new Point(15, 76);
		((Control)txtRegUserName).Name = "txtRegUserName";
		((Control)txtRegUserName).Size = new Size(276, 26);
		((Control)txtRegUserName).TabIndex = 16;
		((Control)txtRegPassword).BackColor = System.Drawing.Color.DarkGray;
		((TextBoxBase)txtRegPassword).BorderStyle = (BorderStyle)1;
		((Control)txtRegPassword).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtRegPassword).Location = new Point(15, 124);
		((Control)txtRegPassword).Name = "txtRegPassword";
		((Control)txtRegPassword).Size = new Size(276, 26);
		((Control)txtRegPassword).TabIndex = 17;
		((Control)progressLoading).ForeColor = System.Drawing.Color.OrangeRed;
		((Control)progressLoading).Location = new Point(134, 299);
		((Control)progressLoading).Name = "progressLoading";
		((Control)progressLoading).Size = new Size(157, 23);
		progressLoading.Style = (ProgressBarStyle)2;
		((Control)progressLoading).TabIndex = 28;
		((Control)progressLoading).Visible = false;
		((Control)label5).AutoSize = true;
		((Control)label5).BackColor = System.Drawing.Color.Transparent;
		((Control)label5).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label5).ForeColor = System.Drawing.Color.Black;
		((Control)label5).Location = new Point(111, 57);
		((Control)label5).Name = "label5";
		((Control)label5).Size = new Size(89, 20);
		((Control)label5).TabIndex = 18;
		((Control)label5).Text = "User Name";
		((Control)btnRegister).BackColor = System.Drawing.Color.Coral;
		btnRegister.BackgroundColor = System.Drawing.Color.Coral;
		btnRegister.BorderColor = System.Drawing.Color.DarkGray;
		btnRegister.BorderRadius = 12;
		btnRegister.BorderSize = 2;
		((Control)btnRegister).Cursor = Cursors.Hand;
		btnRegister.EnableGradient = false;
		btnRegister.EnableShadow = true;
		((ButtonBase)btnRegister).FlatAppearance.BorderSize = 0;
		((ButtonBase)btnRegister).FlatStyle = (FlatStyle)0;
		((Control)btnRegister).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)btnRegister).ForeColor = System.Drawing.Color.Bisque;
		btnRegister.GradientAngle = 90f;
		btnRegister.GradientEndColor = System.Drawing.Color.LightSkyBlue;
		btnRegister.GradientStartColor = System.Drawing.Color.White;
		btnRegister.HoverColor = System.Drawing.Color.FromArgb(192, 64, 0);
		((Control)btnRegister).Location = new Point(15, 290);
		((Control)btnRegister).Name = "btnRegister";
		btnRegister.ShadowColor = System.Drawing.Color.Black;
		btnRegister.ShadowOpacity = 35;
		btnRegister.ShadowSize = 8;
		((Control)btnRegister).Size = new Size(113, 33);
		((Control)btnRegister).TabIndex = 33;
		((Control)btnRegister).Text = "Register";
		btnRegister.TextColor = System.Drawing.Color.Bisque;
		((ButtonBase)btnRegister).UseVisualStyleBackColor = false;
		((Control)btnRegister).Click += btnRegister_Click;
		((Control)label4).AutoSize = true;
		((Control)label4).BackColor = System.Drawing.Color.Transparent;
		((Control)label4).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label4).ForeColor = System.Drawing.Color.Black;
		((Control)label4).Location = new Point(113, 105);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(78, 20);
		((Control)label4).TabIndex = 19;
		((Control)label4).Text = "Password";
		((Control)label7).AutoSize = true;
		((Control)label7).BackColor = System.Drawing.Color.Transparent;
		((Control)label7).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label7).ForeColor = System.Drawing.Color.Black;
		((Control)label7).Location = new Point(120, 154);
		((Control)label7).Name = "label7";
		((Control)label7).Size = new Size(69, 20);
		((Control)label7).TabIndex = 23;
		((Control)label7).Text = "Email ID";
		((Control)lblLoadingStatus).AutoSize = true;
		((Control)lblLoadingStatus).BackColor = System.Drawing.Color.Transparent;
		((Control)lblLoadingStatus).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblLoadingStatus).ForeColor = System.Drawing.Color.Red;
		((Control)lblLoadingStatus).Location = new Point(134, 274);
		((Control)lblLoadingStatus).Name = "lblLoadingStatus";
		((Control)lblLoadingStatus).Size = new Size(90, 20);
		((Control)lblLoadingStatus).TabIndex = 27;
		((Control)lblLoadingStatus).Text = "Initializing...";
		lblLoadingStatus.TextAlign = (ContentAlignment)32;
		((Control)lblLoadingStatus).Visible = false;
		((Control)label6).AutoSize = true;
		((Control)label6).BackColor = System.Drawing.Color.Transparent;
		((Control)label6).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label6).ForeColor = System.Drawing.Color.Black;
		((Control)label6).Location = new Point(112, 203);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(79, 20);
		((Control)label6).TabIndex = 21;
		((Control)label6).Text = "Mobile No";
		((Control)chkAgree).AutoSize = true;
		((Control)chkAgree).BackColor = System.Drawing.Color.Transparent;
		((Control)chkAgree).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkAgree).ForeColor = System.Drawing.Color.Red;
		((Control)chkAgree).Location = new Point(5, 256);
		((Control)chkAgree).Name = "chkAgree";
		((Control)chkAgree).Size = new Size(299, 19);
		((Control)chkAgree).TabIndex = 26;
		((Control)chkAgree).Text = "I have read and agree to the Purchase Agreement.";
		((ButtonBase)chkAgree).UseVisualStyleBackColor = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = System.Drawing.Color.Black;
		((Form)this).ClientSize = new Size(1003, 664);
		((Control)this).Controls.Add((Control)(object)mdmPanel1);
		((Control)this).ForeColor = SystemColors.Desktop;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).MinimumSize = new Size(0, 0);
		((Control)this).Name = "Form2";
		((Control)this).Text = "AI-Powered Mobile Repair Assistant";
		((Form)this).Shown += Form2_Shown;
		((Control)mdmPanel1).ResumeLayout(false);
		((Control)mdmPanel5).ResumeLayout(false);
		((Control)mdmPanel5).PerformLayout();
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)mdmPanel2).ResumeLayout(false);
		((Control)mdmPanel2).PerformLayout();
		((ISupportInitialize)fastColoredTextBox1).EndInit();
		((Control)mdmPanel3).ResumeLayout(false);
		((Control)mdmPanel3).PerformLayout();
		((Control)mdmPanel4).ResumeLayout(false);
		((Control)mdmPanel4).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
