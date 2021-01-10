using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductManagement.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error/NotFount
        public ActionResult NotFound()
        {
            return View();
        }
    }
}