using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class VideoController : BaseController
    {
        VideoBLL bll = new VideoBLL();
        // GET: Admin/Video
        public ActionResult VideoList()
        {
            List<VideoDTO> dtoList = new List<VideoDTO>();
            dtoList = bll.GetVideos();
            return View(dtoList); 
        }
        public ActionResult AddVideo()
        {
            VideoDTO dto = new VideoDTO();
            return View(dto);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddVideo(VideoDTO model)
        {
            // VideoPath: < iframe width = "560" height = "315" src = "https://www.youtube.com/embed/DUg2SWWK18I" title = "YouTube video player" frameborder = "0" allow = "accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen ></ iframe >
            // OriginalVideoPath: https://www.youtube.com/watch?v=DUg2SWWK18I
            if (ModelState.IsValid)
            {
                string path = model.OriginalVideoPath.Substring(32); // DUg2SWWK18I
                string mergeLink = "https://www.youtube.com/embed/";
                mergeLink += path;
                model.VideoPath = String.Format(@"<iframe width = ""350"" height = ""200"" src = ""{0}"" frameborder = ""0"" allowfullscreen ></iframe>", mergeLink);
                if (bll.AddVideo(model))
                {
                    ViewBag.ProcessState = General.Messages.AddSuccess;
                    ModelState.Clear();
                    model = new VideoDTO();
                }
                else
                {
                    ViewBag.ProcessState = General.Messages.GeneralError;
                }
            }
            else
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
            }
            return View(model);
        }
        public ActionResult UpdateVideo(int ID)
        {
            VideoDTO dto = bll.GetVideoWithID(ID);
            return View(dto);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateVideo(VideoDTO model)
        {
            if (ModelState.IsValid)
            {
                string path = model.OriginalVideoPath.Substring(32);
                string mergeLink = "https://www.youtube.com/embed/";
                mergeLink += path;
                model.VideoPath = String.Format(@"< iframe width = ""300"" height = ""200"" src = ""{0}"" frameborder = ""0"" allowfullscreen ></ iframe >", mergeLink);
                if (bll.UpdateVideo(model))
                {   
                    ViewBag.ProcessState = General.Messages.UpdateSuccess;
                }
                else
                {
                    ViewBag.ProcessState = General.Messages.GeneralError;
                }
            }
            else
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
            }
            return View(model);
        }
        public JsonResult DeleteVideo(int ID)
        {
            bll.DeleteVideo(ID);
            return Json("");
        }
    }
}