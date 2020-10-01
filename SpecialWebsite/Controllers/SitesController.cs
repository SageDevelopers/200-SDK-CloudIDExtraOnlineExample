using RemoteWebsite.Models;
using Sage.MMS.SAA.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RemoteWebsite.Controllers
{
	/// <summary>
	/// Sites Controller
	/// Attributed to check there is a valid logon
	/// </summary>
	[RequiresValidLogon]
    public class SitesController : Controller
    {
		/// <summary>
		/// GET: Sites Async
		/// </summary>
		/// <returns>View representing Sites</returns>
		public ActionResult Index()
        {
			List<Company> companies = SAAClientAPI.CompaniesGet();

			List<Site> sites = new List<Site>();

			foreach (Company company in companies)
			{
				Site site = new Site();
				site.company_id = company.CompanyNumber;
				site.company_name = company.CompanyName;
				sites.Add(site);

			}

			return View(sites);
		}
    }
}