using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NormalClient
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			FindCore200();
			Application.Run(new Form1());
		}

		static void FindCore200()
		{
			string path = string.Empty;

			RegistryKey root = Registry.CurrentUser;

			RegistryKey key = root.OpenSubKey(@"Software\Sage\MMS");

			if (key != null)
			{
				object value = key.GetValue("ClientInstallLocation");
				if (value != null)
					path = value as string;
			}


			System.Reflection.Assembly assy = null;

			if (string.IsNullOrEmpty(path) == false)
			{
				string assyName = System.IO.Path.Combine(path, "Sage.Common.dll");

				assy = System.Reflection.Assembly.LoadFrom(assyName);
			}

			if (assy != null)
			{
				Type type = assy.GetType("Sage.Common.Utilities.AssemblyResolver");

				MethodInfo method = type.GetMethod("GetResolver");
				method.Invoke(null, null);
			}

		}

	}
}
