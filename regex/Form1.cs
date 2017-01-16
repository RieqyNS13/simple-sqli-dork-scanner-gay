using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace regex
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (Regex.IsMatch(textBox1.Text, "You have an error in your SQL syntax; check the manual that corresponds to your MySQL server version for the right syntax to use near", RegexOptions.IgnoreCase | RegexOptions.Singleline)) MessageBox.Show("You are gay");
			else MessageBox.Show("jancok");
		}
	}
}
