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
    public class VideoPBController : Controller
    {
        
        // GET: VideoPB
        public ActionResult Index()
        {
            return View();
        }

       

        [HttpPost]
        public ActionResult UploadVideo(HttpPostedFileBase fileupload)
        {
            if (fileupload != null)
            {
                string fileName = Path.GetFileName(fileupload.FileName);
                int fileSize = fileupload.ContentLength;
                string filePath = "~/VideoDirectory/" + fileName;
                int Size = fileSize / 1000;



                VideoFile Video = new VideoFile { videoID=0, videopath=filePath,videosize=Size,vidtitle=fileName,uploader=(int)Session["UID"] };
             

                int ret = Models.crud.addVideoToDB(Video);

                switch (ret)
                {
                    case 1:
                        fileupload.SaveAs(Server.MapPath("~/VideoDirectory/" + fileName));
                        return Content("<script>alert('Video Upload Successfull!');" + "window.location = 'Index';</script>");

                    case -1:
                        return Content("<script > alert('Video couldn't be uploaded!\nTry Again.'); window.location = 'Index';</script >");

                    case -404:
                        return Content("<script > alert('Empty Video File!\nTry Again.'); window.location = 'Index';</script>");
                    default:
                        break;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ShowVideo()
        {
            List<VideoFile> videolist;

            videolist = Models.crud.RetrieveVid();
            return View(videolist);
        }

        public ActionResult PlayVideo()
        {
            return View();
        }


    }


}