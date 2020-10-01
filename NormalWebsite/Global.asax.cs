using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NormalWebsite
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
			// getSage 200 server path from config file
			string path = System.Configuration.ConfigurationManager.AppSettings["Sage200Path"];

			if (string.IsNullOrEmpty(path) == false)
			{
				string commonDllAssemblyName = System.IO.Path.Combine(path, "Sage.Common.dll");

				if (System.IO.File.Exists(commonDllAssemblyName))
				{
					System.Reflection.Assembly defaultAssembly = System.Reflection.Assembly.LoadFrom(commonDllAssemblyName);
					Type type = defaultAssembly.GetType("Sage.Common.Utilities.AssemblyResolver");
					MethodInfo method = type.GetMethod("GetResolver");
					method.Invoke(null, null);
				}
			}
		}
	}
}
