using BusinessTier;
using EFMVC.BusinessTier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Controllers
{
    public class SignupController : Controller
    {
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(PersonModel model)
        {
            BL.Signup(model);
            return View();
        }
    }
}