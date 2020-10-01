using NormalWebsite.Models;
using Sage.MMS.SAA.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NormalWebsite.Controllers
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

			Company firstCompany = null;

			foreach (Company company in companies)
			{
				Site site = new Site();
				site.company_id = company.CompanyNumber;
				site.company_name = company.CompanyName;
				sites.Add(site);

				if (firstCompany == null)
				{
					firstCompany = company;		// only for sample logon
				}
			}
			

			// We need to specify a company to use for each subsequent request
			// For the example we just select the first company from the list,
			// however you would more likely display a list of companies and allow
			// the user to select the required company from a list

			SAAClientAPI.ConnectCompany(firstCompany);

			string sessionID = Sage.Common.Contexts.SessionContextValues.SessionID;

			ContextStore.Set(Session, sessionID);

			return View(sites);
		}
    }
}