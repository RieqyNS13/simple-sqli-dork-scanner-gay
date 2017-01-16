using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
class asu{
	static void Main(){
		string a=File.ReadAllText("asu.html");
		
		Console.WriteLine(Regex.Escape("&amp;sa=U&amp;"));
	}
}