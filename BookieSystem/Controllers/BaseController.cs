using BookieDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookieSystem.Controllers
{
    public class BaseController : Controller
    {
        public BookieContext _bookieContext =  new BookieContext();

    }
}
