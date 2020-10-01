using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RemoteWebsite
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

			FindCore200();

//			Sage.Accounting.Application app = new Sage.Accounting.Application();		// force initialisation of probing paths, and add on assemblies
			
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		/// <summary>
		/// Locates and invokes assemblies from the client folder at runtime.
		/// </summary>
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

			if (string.IsNullOrEmpty(path) == false)
			{
				string commonDllAssemblyName = System.IO.Path.Combine(path, "Sage.Common.dll");

				if (System.IO.File.Exists(commonDllAssemblyName))
				{
					System.Reflection.Assembly defaultAssembly = System.Reflection.Assembly.LoadFrom(commonDllAssemblyName);

					{
						Type type = defaultAssembly.GetType("Sage.Common.Utilities.AssemblyResolver");
						MethodInfo method = type.GetMethod("GetResolver");
						object resolver = method.Invoke(null, null);
						PropertyInfo property = type.GetProperty("probingPaths");
						StringCollection paths = (StringCollection) property.GetValue(resolver);
						paths.Add(path);
					}

					{
						Type type = defaultAssembly.GetType("Sage.Common.Utilities.ConfigurationHelper");
						PropertyInfo property = type.GetProperty("ExecutablePaths");
						String[] paths = (String[]) property.GetValue(null);
						List<string> list = new List<string>(paths);

						list.Add(path);
						paths = list.ToArray();

						MethodInfo setter = property.GetSetMethod(true);
						setter.Invoke(null, new Object[] {paths});
					}
					
				}
			}
		}
	}
}
