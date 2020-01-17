using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using GreenTube.Models;


namespace GreenTube.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            List<VideoFile> mostrecent = Models.crud.mostrecentvids();
            List<VideoFile> mostwatched = Models.crud.mostwatchedvids();
            List<VideoFile> mostliked = Models.crud.mostlikedvids();

            List<List<VideoFile>> videos = new List<List<VideoFile>>();
            videos.Add(mostrecent);
            videos.Add(mostwatched);
            videos.Add(mostliked);
           
            
            return View(videos);
        }

        [HttpGet]
        public ActionResult PageDecider(int id = 0)
        {
            id = (int)Session["UID"];
            int BoolValue = Models.crud.PageDecider(id);

            if(BoolValue==0)
                return RedirectToAction("UserPage", "Home");
            else
                return RedirectToAction("CreatorView", "Home");
        }


        [HttpGet]
        public ActionResult UserPage(int id=0)
        {
          id = (int)Session["UID"];
            UserData currUser = Models.crud.UserInformation(id);

            return View(currUser);
        }


        [HttpGet]
        public ActionResult DeleteUser(int id=0)
        {
            id = (int)Session["UID"];
            Models.crud.DeleteId(id);

            return RedirectToAction("Index", "Login");
        }


        [HttpGet]
        public ActionResult SignOut(int id = 0)
        {
            id = (int)Session["UID"];
            Models.crud.SignOut(id);

            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public ActionResult CreatorView(int id = 0)
        {
           id = (int)Session["UID"];
            Creator creatorstats = Models.crud.CreatorView(id);
            return View(creatorstats);
        }

        [HttpGet]
        public ActionResult becomeCreator(int id = 9)
        {
            id = (int)Session["UID"];
            Models.crud.becomeCreator(id);
            return RedirectToAction("CreatorView", "Home");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}