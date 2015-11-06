using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookieDatabase;

namespace BookieSystem.Controllers
{
    public class HomeController : Controller
    {
        private BookieContext _bookieContext;

        public HomeController()
        {
            _bookieContext = new BookieContext();
        }
        public ActionResult Index()
        {
            List<BookieDatabase.Matches> allMatches = _bookieContext.Matches.ToList();
            List<BookieDatabase.Users> allUsers = _bookieContext.Users.ToList();
            List<BookieDatabase.UserResult> allResult = _bookieContext.UserResults.ToList();

            ViewBag.allMatches=allMatches;
            ViewBag.allUsers=allUsers;
            ViewBag.allResult = allResult;
            if (User.Identity.IsAuthenticated)
            {
                var user = _bookieContext.Users.FirstOrDefault(x => x.Username == User.Identity.Name);
                ViewBag.userId = user.Id;
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [Authorize]
        public ActionResult Vote(int id)
        {
            var match = _bookieContext.Matches.FirstOrDefault(x => x.Id == id);
            ViewBag.match = match;
            BookieDatabase.UserResult result = new UserResult();
            result.MatchId = match.Id;
            result.Match = match;
            return View(result);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Vote(BookieDatabase.UserResult result)
        {
            var user = _bookieContext.Users.FirstOrDefault(x => x.Username == User.Identity.Name);
            result.UserId = user.Id;
            var hasResult = _bookieContext.UserResults.FirstOrDefault(ur => ur.UserId == result.UserId && ur.MatchId == result.MatchId);
            if (hasResult != null)
            {
                hasResult.Home = result.Home;
                hasResult.Away = result.Away;
            }
            else
            {
                _bookieContext.UserResults.Add(result);
            }
            _bookieContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [Authorize (Users="test@abv.bg")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize(Users = "test@abv.bg")]
        [HttpPost]
        public ActionResult Contact(BookieDatabase.Matches match)
        {
            _bookieContext.Matches.Add(match);
            _bookieContext.SaveChanges();
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
