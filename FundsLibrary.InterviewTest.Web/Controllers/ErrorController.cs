using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FundsLibrary.InterviewTest.Web.Controllers
{
    public class ErrorController : Controller
    {

        public ActionResult EntityNotFound(string entityName)
        {
            Response.StatusCode = 404;
            return View((object)entityName);
        }
    }
}