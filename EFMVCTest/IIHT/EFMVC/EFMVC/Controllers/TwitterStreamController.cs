using BusinessTier;
using EFMVC.BusinessTier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Controllers
{
    public class TwitterStreamController : Controller
    {
        public ActionResult TwitterStream()
        {
            var person = (PersonModel)TempData["Person"];
            var tweet = (int?)TempData["Tweet"];
            var users = (List<PersonModel>)TempData["UserList"];
            TwitterStreamModel model = new TwitterStreamModel();
            if (tweet != null)
            {
                model.CurrentTweet = BL.GetTweetById(tweet.Value);
            }
            if (users != null)
            {
                model.PersonList = users;
            }
            if (person != null)
            {
                model.Person = (PersonModel)TempData["Person"];
            }
            else if (UserContext.Person != null)
            {
                model.Person = UserContext.Person;
            }
            model.TweetList = BL.GetTweet(model.Person);
            model.PersonList= BL.GetPerson(UserContext.Person.UserId, model.SearchText);
            return View(model);
        }

        [HttpPost]
        public ActionResult TwitterStream(TwitterStreamModel model)
        {
            if (!string.IsNullOrEmpty(model.CurrentTweet.Message))
            {
                model.CurrentTweet.UserId = model.Person.UserId;
                BL.AddTweet(model.CurrentTweet);
            }
            else  
            {
                TempData["UserList"] = BL.GetPerson(model.Person.UserId, model.SearchText);

            }
            return RedirectToAction("TwitterStream");
        }

        public ActionResult DeleteTweet(int id)
        {
            BL.DeleteTweet(id);
            return RedirectToAction("TwitterStream");
        }

        public ActionResult EditTweet(int id)
        {
            TempData["Tweet"] = id;
            return RedirectToAction("TwitterStream");
        }

        public ActionResult SearchUser(string text)
        {
            TempData["userSearch"] = text;
            return RedirectToAction("TwitterStream");
        }
        public ActionResult FollowUser(string id)
        {
            BL.FollowPerson(id, UserContext.Person.UserId);
            return RedirectToAction("TwitterStream");
        }
    }
}