using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lib360.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
        {
            if (Session["UserEmail"] == null && Session["UserName"] == null && Session["UserID"] == null && Session["UserRol"] == null)
            {
                return RedirectToAction("SignIn", "Usuario");
            }

            return View();
		}

		
	}
}