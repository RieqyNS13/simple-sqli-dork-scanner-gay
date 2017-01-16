using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Net;
namespace latihanthread
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}
		private readonly object obj = new object();
		delegate void addtextbox(TextBox txt, string data);
		/// <summary>
		/// HOMO GAY
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="data"></param>
		///
		private void textAppend(TextBox txt, string data)
		{
			if (this.InvokeRequired)
			{
				addtextbox cok = gaytxt;
				this.Invoke(cok, new object[] { txt, data });
			}
			else txt.AppendText(data);
		}
		private void gaytxt(TextBox txt, string data)
		{
			txt.AppendText(data);
		}
		private string curl(string url_, string data = null, bool responKode = false)
		{
			ServicePointManager.DefaultConnectionLimit = 2;
			Stream stream;
			HttpWebResponse respon;
			StreamReader mbuh;
			Uri url = new Uri(url_);
			try
			{
				HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
				req.Accept = "*/*";
				req.Timeout = 30000;
				req.ReadWriteTimeout = 30000;
				req.UserAgent = "Mozilla/5.0 (X11; Linux i686) AppleWebKit/536.5 (KHTML, like Gecko) Chrome/19.0.1084.52 Safari/536.5";
				req.AllowAutoRedirect = true;
				if (!string.IsNullOrEmpty(data))
				{
					byte[] postdata = Encoding.UTF8.GetBytes(data);
					req.Method = WebRequestMethods.Http.Post;
					req.ContentType = "application/x-www-form-urlencoded";
					req.ContentLength = postdata.Length;
					stream = (Stream)req.GetRequestStream();
					stream.Write(postdata, 0, postdata.Length);
					stream.Close();
				}
				else
				{
					req.Method = WebRequestMethods.Http.Get;
				}
				////////[gay]
				using (respon = (HttpWebResponse)req.GetResponse())
				{
					string kode = string.Empty;
					if (responKode) kode = "_rieqyns13gay_" + respon.StatusCode.ToString() + "_rieqyns13homo_";
					using (stream = respon.GetResponseStream())
					{
						stream.ReadTimeout = 30000;
						stream.WriteTimeout = 30000;
						mbuh = new StreamReader(stream);
						string source = WebUtility.HtmlDecode(mbuh.ReadToEnd().ToString());
						stream.Flush();
						stream.Close();
						//this.curlFinish = true;
						return source + kode;
					}
				}
			}
			catch (WebException ex)
			{
				try
				{
					string kode = string.Empty;
					respon = (HttpWebResponse)ex.Response;
					if (responKode) kode = "_rieqyns13gay_" + respon.StatusCode.ToString() + "_rieqyns13homo_";
					mbuh = new StreamReader(respon.GetResponseStream());
					string hasil = WebUtility.HtmlDecode(mbuh.ReadToEnd().ToString());
					mbuh.Close();
					respon.Close();
					//this.curlFinish = true;
					return hasil + kode;
				}
				catch (Exception)
				{
					//this.curlFinish = true;
					return "error";
				}
			}
			catch (Exception ex)
			{
				//this.curlFinish = true;
				return ex.ToString();
			}

		}
		private void button1_Click(object sender, EventArgs e)
		{
			Thread cok = new Thread(() =>
			{
				try
				{
					for (int a = 0; a <= 100; a++)
					{
						if (a == 10) throw new Exception("jembot");
						Thread.Sleep(900);
					}
				}
				catch (Exception ee)
				{
					MessageBox.Show(ee.Message);
				}
			});
			cok.Start();
			while (true)
			{
				textAppend(textBox1, cok.IsAlive.ToString() + "\r\n");
				Thread.Sleep(500);
				if (!cok.IsAlive)
				{
					textAppend(textBox1, cok.IsAlive.ToString());
					break;
				}
			}

		}
	}
}
