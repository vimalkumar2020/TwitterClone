using BusinessTier;
using EFMVC.BusinessTier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(PersonModel model)
        {
            PersonModel person = BL.Login(model);
            if (person != null)
            {
                UserContext.Person = person;
                TempData["Person"] = person;
                return RedirectToAction("TwitterStream", "TwitterStream", new TwitterStreamModel() { });
            }
            return View("Login");
        }

        public ActionResult Profiles()
        {
            return View(UserContext.Person);
        }

        [HttpPost]
        public ActionResult Profiles(PersonModel model)
        {
            BL.Profile(model);
            return View();
        }

        public ActionResult Logout()
        {
            UserContext.Person = null;
            return View("Login");
        }

        public ActionResult DeleteAccount()
        {
            BL.DeletePerson(UserContext.Person.UserId);
            UserContext.Person = null;
            return View("Login");
        }
    }
}