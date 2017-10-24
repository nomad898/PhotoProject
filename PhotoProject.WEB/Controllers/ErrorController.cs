using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoProject.WEB.Controllers
{
    public class ErrorController : Controller
    {
        [Authorize]
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}