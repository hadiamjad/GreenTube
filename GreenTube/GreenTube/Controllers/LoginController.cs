using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreenTube.Controllers
{
    public class LoginController : Controller
    {
        int result;

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        // function that performs login authentication
        public ActionResult Authenticate(string userId, string _password)
        {
            result = Models.crud.Login(userId,_password);

            if(result==-1)
            {
                ViewBag.Message = "Could not connect to database.";
                return View();
            }
            else if (result==0)
            {
                ViewBag.Message = "Invalid Credentials.";
                return View();
            }

            return RedirectToAction("Index","Home");
        }

        public ActionResult AddUser(string userName, string _password, string userId, DateTime _date)
        {
            result = Models.crud.addUser(userName, _password, userId, _date);

            if (result == -1)
            {
                ViewBag.Message = "Could not connect to database.";
                return View();
            }
            else if (result == 0)
            {
                ViewBag.Message = "Invalid Credentials.";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}