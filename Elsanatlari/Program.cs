using System;
using System.Windows.Forms;

namespace Elsanatlari
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			SQLitePCL.Batteries.Init();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			Application.Run(new Form1());
		}
	}
}
