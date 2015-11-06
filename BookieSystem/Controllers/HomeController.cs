using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookieDatabase;
using System.Text;

namespace BookieSystem.Controllers
{
    public class HomeController : BaseController
    {

        [AllowAnonymous]
        public ActionResult Dummy(string returnUrl)
        {
            var activeMaches = _bookieContext.Matches.Where(x => x.Close == false).ToList();
            DateTime now = DateTime.Now;
            for (int i = 0; i < activeMaches.Count; i++)
            {
                if (activeMaches[i].TimeToBet < now)
                {
                    activeMaches[i].Close = true;
                    for (int j = 0; j <  activeMaches[i].Results.Count; j++)
                    {
                        activeMaches[i].Results[i].ComfirmResult = true;
                    }
                    _bookieContext.SaveChanges();
                }
                else
                {
                }
                ViewBag.usersResultCount +="До:"+ now +" за: "+ activeMaches[i].Home + "-" + activeMaches[i].Away + ". Дали резултати са:" + activeMaches[i].Results.Count + "\n";
            }
            return View();
        }

        public ActionResult Index()
        {
            List<BookieDatabase.Matches> allMatches = _bookieContext.Matches.Where(x=>x.Close == true).ToList();
            List<BookieDatabase.Users> allUsers = _bookieContext.Users.ToList();
            List<BookieDatabase.UserResult> allResult = _bookieContext.UserResults.ToList();
            bool isAdmin = false;
            if (User.Identity.Name == "boyan.zhelyazkov@scalefocus.com" || 
                User.Identity.Name =="kristiyan.nikolov@scalefocus.com" ||
                User.Identity.Name=="stefan.uzunov@scalefocus.com")
            {
                isAdmin = true;
            }
            allUsers=allUsers.OrderBy(x=>x.Username).ToList();
            ViewBag.allMatches=allMatches;
            
            ViewBag.allResult = allResult;
            ViewBag.isAdmin = isAdmin;
            ViewBag.userId = 0;
            if (User.Identity.IsAuthenticated)
            {
                var user = _bookieContext.Users.FirstOrDefault(x => x.Username == User.Identity.Name);
                ViewBag.userId = user.Id;
                allUsers.Remove(user);
                allUsers.Insert(0, user);
                ViewBag.emails = user.EmailCanceled;
            }
            ViewBag.allUsers=allUsers;

            var activeMaches = _bookieContext.Matches.Where(x => x.Close == false).ToList();
            ViewBag.activeMaches = activeMaches;
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
            var user = _bookieContext.Users.FirstOrDefault(x=>x.Username == User.Identity.Name);
            ViewBag.match = match;

            BookieDatabase.UserResult result = _bookieContext.UserResults.FirstOrDefault(x => x.MatchId == id && x.UserId == user.Id);

            if (result == null)
            {
                result = new UserResult();
                result.MatchId = match.Id;
                result.Match = match;
            }
            return View(result);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Vote(BookieDatabase.UserResult result)
        {
            var user = _bookieContext.Users.FirstOrDefault(x => x.Username == User.Identity.Name);
            ViewBag.emails = user.EmailCanceled;
            result.UserId = user.Id;
            var hasResult = _bookieContext.UserResults.FirstOrDefault(ur => ur.UserId == result.UserId && ur.MatchId == result.MatchId);
            var match = _bookieContext.Matches.FirstOrDefault(ur =>  ur.Id == result.MatchId);
            if (hasResult != null && !hasResult.ComfirmResult && !match.Close)
            {
                hasResult.Home = result.Home;
                hasResult.Away = result.Away;
                hasResult.ComfirmResult = result.ComfirmResult;
            }
            else if (!match.Close)
            {
                _bookieContext.UserResults.Add(result);
            }
            _bookieContext.SaveChanges();

            if (result.ComfirmResult)
            {
                string emailBody = "You successfully confirm your result! <br/> " + match.Home + "-" + match.Away + " <br/>" + result.Home + " - " + result.Away + " <br/> Good luck :)  <br/> Another result:  <br/>";
                StringBuilder sb = new StringBuilder();
                sb.Append("<table>");
                sb.Append("<tr>");
                sb.Append("<td>");
                sb.Append("</td>");
                foreach (var userResult in match.Results)
	                {
                        if (userResult != null && userResult.Home != null)
                        {
                            string username = userResult.User.Username.Split('@')[0];
                            sb.Append("<td>");
                            sb.Append(username);
                            sb.Append("</td>");
                        }
	                }
                sb.Append("<tr>");

                sb.Append("<tr>");
                sb.Append("<td>");
                sb.Append(match.Home + "-" + match.Away);
                sb.Append("</td>");
                foreach (var userResult in match.Results)
                {
                    if (userResult != null && userResult.Home != null)
                    {
                        sb.Append("<td>");
                        sb.Append(userResult.Home +":"+ userResult.Away);
                        if (userResult.ComfirmResult)
                        {
                            sb.Append("+");
                        }
                        sb.Append("<td>");
                    }
                }
                sb.Append("<tr>");
                sb.Append("</table>");
                //emailBody += sb.ToString();
                EMails.Email.SendGroupEmail(User.Identity.Name, "You successfully confirm your result! ",emailBody);
                var confirmUser = User.Identity.Name.Split('@')[0];
                EMails.Email.SendGroupEmail("kristiyan.nikolov@scalefocus.com", confirmUser + " successfully confirm your result! ", emailBody);
                EMails.Email.SendGroupEmail("boyan.zhelyazkov@scalefocus.com", confirmUser + " successfully confirm your result! ", emailBody);
                /*
                var allUsers = _bookieContext.Users.ToList();
                string newSystem = "This is brand new scalefocus booking system. <br/> Here you can vote for next match. <br/> This is the link http://bookie.boyanzhelyazkov.com/Home/Vote/7 ";
                foreach (var item in allUsers)
	{
		 
	}
                 */
                            }
            return RedirectToAction("ActiveMatches", "Home");
        }

         [Authorize(Users = "boyan.zhelyazkov@scalefocus.com, kristiyan.nikolov@scalefocus.com, stefan.uzunov@scalefocus.com")]
        public ActionResult GivenMoney(int? id, int userId, int matchId)
        {
            int idInt=0;
      
            if (id.HasValue)
            {
                idInt = id.Value;
                var hasResult = _bookieContext.UserResults.FirstOrDefault(ur => ur.Id == idInt);
                hasResult.GivenMoney = true;
                hasResult.GivenMoneyByBookie = false;
                _bookieContext.SaveChanges();
            }
            else
            {
                var result = new UserResult();
                result.Home = null;
                result.Away = null;
                result.GivenMoney = true;
                result.GivenMoneyByBookie = false;
                result.UserId = userId;
                result.MatchId = matchId;
                _bookieContext.UserResults.Add(result);
                _bookieContext.SaveChanges();
            }
            _bookieContext.SaveChanges();
            return RedirectToAction("ActiveMatches", "home");
        }

         [Authorize(Users = "boyan.zhelyazkov@scalefocus.com, kristiyan.nikolov@scalefocus.com, stefan.uzunov@scalefocus.com")]
         public ActionResult GivenMoneyBookie(int? id, int userId, int matchId)
         {
             int idInt = 0;

             if (id.HasValue)
             {
                 idInt = id.Value;
                 var hasResult = _bookieContext.UserResults.FirstOrDefault(ur => ur.Id == idInt);
                 hasResult.GivenMoney = false;
                 hasResult.GivenMoneyByBookie = true;
             }
             else
             {
                 var result = new UserResult();
                 result.Home = null;
                 result.Away = null;
                 result.GivenMoney = false;
                 result.GivenMoneyByBookie = true;
                 result.UserId = userId;
                 result.MatchId = matchId;
                 _bookieContext.UserResults.Add(result);
             }
             _bookieContext.SaveChanges();
             return RedirectToAction("ActiveMatches", "home");
         }

         [Authorize(Users = "boyan.zhelyazkov@scalefocus.com, kristiyan.nikolov@scalefocus.com, stefan.uzunov@scalefocus.com")]
         public ActionResult NotPaid(int? id, int userId, int matchId)
         {
             int idInt = 0;

             if (id.HasValue)
             {
                 idInt = id.Value;
                 var hasResult = _bookieContext.UserResults.FirstOrDefault(ur => ur.Id == idInt);
                 hasResult.GivenMoney = false;
                 hasResult.GivenMoneyByBookie = false;
             }
             else
             {
                 var result = new UserResult();
                 result.Home = null;
                 result.Away = null;
                 result.GivenMoney = false;
                 result.GivenMoneyByBookie = false;
                 _bookieContext.UserResults.Add(result);
             }
             _bookieContext.SaveChanges();
             return RedirectToAction("ActiveMatches", "home");
         }
        /* [Authorize(Users = "boyan.zhelyazkov@scalefocus.com, kristiyan.nikolov@scalefocus.com, stefan.uzunov@scalefocus.com")]
        [HttpPost]
        public ActionResult GivenMoney(BookieDatabase.UserResult result)
        {
            var user = _bookieContext.Users.FirstOrDefault(x => x.Username == User.Identity.Name);
            
            var hasResult = _bookieContext.UserResults.FirstOrDefault(ur => ur.Id == result.Id);
            if (hasResult != null)
            {
                hasResult.GivenMoney = result.GivenMoney;
            }
            else
            {
                result.Home = null;
                result.Away = null;
                _bookieContext.UserResults.Add(result);
            }
            _bookieContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }*/


        [Authorize(Users = "boyan.zhelyazkov@scalefocus.com, kristiyan.nikolov@scalefocus.com, stefan.uzunov@scalefocus.com")]
         public ActionResult ResultMatch(int id)
        {
            var match = _bookieContext.Matches.FirstOrDefault(x => x.Id == id);
            return View(match);
        }

         [Authorize(Users = "boyan.zhelyazkov@scalefocus.com, kristiyan.nikolov@scalefocus.com, stefan.uzunov@scalefocus.com")]
        [HttpPost]
        public ActionResult resultMatch(BookieDatabase.Matches result)
        {
            var match = _bookieContext.Matches.FirstOrDefault(x => x.Id == result.Id);
            match.HomeResult = result.HomeResult;
            match.AwayResult = result.AwayResult;
            _bookieContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

         [Authorize(Users = "boyan.zhelyazkov@scalefocus.com, kristiyan.nikolov@scalefocus.com, stefan.uzunov@scalefocus.com")]
         public ActionResult Close(int id)
         {
             var match = _bookieContext.Matches.FirstOrDefault(x => x.Id == id);
             match.Close = !match.Close;
             _bookieContext.SaveChanges();
             return RedirectToAction("Index", "Home");
         }

        [Authorize (Users="test@abv.bg")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize(Users = "boyan.zhelyazkov@scalefocus.com, kristiyan.nikolov@scalefocus.com, stefan.uzunov@scalefocus.com")]
        [HttpPost]
        public ActionResult Contact(BookieDatabase.Matches match)
        {
            _bookieContext.Matches.Add(match);
            _bookieContext.SaveChanges();
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult Bet(int Id)
        {
            var match = _bookieContext.Matches.FirstOrDefault(x => x.Id == Id);
            var users = _bookieContext.Users.ToList();
            return View(match);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Bet(BookieDatabase.Matches match)
        {
            _bookieContext.Matches.Add(match);
            _bookieContext.SaveChanges();
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [Authorize]
        public ActionResult ActiveMatches()
        {
            var matches = _bookieContext.Matches.Where(x => x.Close == false).ToList();

            ViewBag.matches = matches;
            var users = _bookieContext.Users.ToList();
            var me = users.FirstOrDefault(x => x.Username == User.Identity.Name);
            users.Remove(me);
            users = users.OrderBy(x => x.Username).ToList();
            users.Insert(0, me);
            ViewBag.users = users;

            bool isAdmin = false;
            if (User.Identity.Name == "boyan.zhelyazkov@scalefocus.com" ||
                User.Identity.Name == "kristiyan.nikolov@scalefocus.com" ||
                User.Identity.Name == "stefan.uzunov@scalefocus.com")
            {
                isAdmin = true;
            }

            ViewBag.isAdmin = isAdmin;

            return View();
        }

        [Authorize]
        public ActionResult PaymentMatches(int id=1)
        {
            if (User.Identity.Name == "boyan.zhelyazkov@scalefocus.com" ||
                User.Identity.Name == "kristiyan.nikolov@scalefocus.com" ||
                User.Identity.Name == "stefan.uzunov@scalefocus.com")
            {

                var matches = _bookieContext.Matches.OrderByDescending(x => x.Id).Take(id).ToList();

                ViewBag.matches = matches;
                var users = _bookieContext.Users.ToList();
                var me = users.FirstOrDefault(x => x.Username == User.Identity.Name);
                users.Remove(me);
                users = users.OrderBy(x => x.Username).ToList();
                users.Insert(0, me);
                ViewBag.users = users;

                bool isAdmin = false;
                if (User.Identity.Name == "boyan.zhelyazkov@scalefocus.com" ||
                    User.Identity.Name == "kristiyan.nikolov@scalefocus.com" ||
                    User.Identity.Name == "stefan.uzunov@scalefocus.com")
                {
                    isAdmin = true;
                }

                ViewBag.isAdmin = isAdmin;

                return View();
            }
               return RedirectToAction("Index","Home");
            
        }

    }
}
