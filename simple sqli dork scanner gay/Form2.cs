using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Web;
namespace simple_sqli_dork_scanner_gay
{
	public partial class Form2 : Form
	{
		public string[] urls=null;
		public bool adaHasil = false;
		public bool nextCok=false;
		public int engine=0;
		private string urlcari;
		public Form2(string url,int engine)
		{
			InitializeComponent();
			this.engine = engine;
			this.urlcari = url;
			webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
			webBrowser1.Navigate(url);

		}
		private MatchCollection match(string start, string end, string var)
		{
			MatchCollection match = Regex.Matches(var, Regex.Escape(start) + "(.*?)" + Regex.Escape(end), RegexOptions.IgnoreCase);
			return match;
		}
		public static List<string> getGoogleUrls(string data)
		{
			List<string> urls = new List<string>();
			List<string> pola = new List<string>();
			List<int> groups = new List<int>();
			pola.Add("<h3\\s+class=\"r\">\\s*<a\\s+href=\"/url\\?q=(.*?)&amp;sa=U&amp;"); groups.Add(1);
			pola.Add("<h3\\s+class=\"r\">\\s*<a\\s+href=\"(.*?)\\/url\\?url=(.*?)&amp;rct=j&amp;frm"); groups.Add(2);
			pola.Add("<h3\\s+class=\"r\">\\s*<a\\s+href=\"(.*?)\" onmousedown=\"return"); groups.Add(1);
			bool ketemu = false;
			int i = 0;
			int jumlah = pola.Count;
			MatchCollection matchs = null;
			while (!ketemu && i < jumlah)
			{
				Regex regex = new Regex(pola[i], RegexOptions.IgnoreCase | RegexOptions.Singleline);
				matchs = regex.Matches(data);
				if (matchs.Count > 0)
				{
					foreach (Match jembot in matchs)
					{
						urls.Add(HttpUtility.UrlDecode(WebUtility.HtmlDecode(jembot.Groups[groups[i]].Value)));
					}
					ketemu = true;
				}
				i++;
			}
			return urls;
		}
		private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			string data = webBrowser1.DocumentText;
			data = Regex.Replace(data, "\r\n|\r|\n", string.Empty);
			
			if (engine == 0)
			{
				StreamWriter writer = new StreamWriter("form2.html");
				writer.Write(data);
				writer.Close();
				if (Regex.IsMatch(data, "<h3\\s+class=\"r\">\\s*<a\\s+href=\"", RegexOptions.IgnoreCase | RegexOptions.Singleline))
				{

					urls = getGoogleUrls(data).ToArray();
					this.adaHasil = true;
					if (Regex.IsMatch(data, "<span\\s+style=\"display:block;margin-left:53px\">", RegexOptions.IgnoreCase | RegexOptions.Singleline)) this.nextCok = true;
					else this.nextCok = false;
					this.Close();
				}
				else if (Regex.IsMatch(data, "<p>Your client does not have permission to get URL <code>|<title>Error 403 \\(Forbidden\\)!!1</title>",RegexOptions.IgnoreCase | RegexOptions.Singleline))
				{
					webBrowser1.Navigate(urlcari);
				}
				else if (!Regex.IsMatch(data, Regex.Escape("<img src=\"/sorry/image?"), RegexOptions.IgnoreCase))
				{
					this.adaHasil = false;
					this.Close();
				}
			}
			else
			{
				if (Regex.IsMatch(data, "<h2><a\\s+href=\"", RegexOptions.IgnoreCase))
				{
					MatchCollection matchs = Regex.Matches(data,"<h2><a\\s+href=\"(.*?)\"\\s+h=\"", RegexOptions.IgnoreCase);
					urls = new string[matchs.Count];

					for (int a = 0; a < matchs.Count; a++)
					{
						urls[a] = WebUtility.HtmlDecode(matchs[a].Groups[1].Value);
					}
					this.adaHasil = true;
					if (Regex.IsMatch(data, "title=\"Next", RegexOptions.IgnoreCase)) this.nextCok = true;
					else this.nextCok = false;
					
					this.Close();
				}
				else if (!Regex.IsMatch(data, Regex.Escape("<form action=\"/challenge"), RegexOptions.IgnoreCase))
				{
					this.adaHasil = false;
					this.Close();
				}
			}
		}
		private void Form2_Load(object sender, EventArgs e)
		{

		}
	}
}
