using RemoteWebsite.Infrastructure;
using Sage.MMS.SAA.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RemoteWebsite.Controllers
{
	[RequiresValidSession]
    public class DepartmentsController : Controller
    {
        // GET: Departments
        public ActionResult Index()
        {
			Sage.Accounting.SystemManager.Departments departments = Sage.Accounting.SystemManager.DepartmentsFactory.Factory.CreateNew();

			List<Models.Department> list = new List<Models.Department>();

			foreach (Sage.Accounting.SystemManager.Department department in departments)
			{
				Models.Department item = new Models.Department();
				item.id = department.NLDepartment;
				item.code = department.Code;
				item.name = department.Name;

				list.Add(item);
			}

            return View(list);
        }
    }
}