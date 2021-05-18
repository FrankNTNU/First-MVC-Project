using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class SocialMediaController : BaseController
    {
        // GET: Admin/SocialMedia
        SocialMediaBLL bll = new SocialMediaBLL();
        public ActionResult AddSocialMedia()
        {
            SocialMediaDTO model = new SocialMediaDTO();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddSocialMedia(SocialMediaDTO model)
        {
            if (model.SocialImage == null)
            {
                ViewBag.ProcessState = General.Messages.ImageMissing;
            }
            else if (ModelState.IsValid)
            {
                HttpPostedFileBase postedFile = model.SocialImage;
                Bitmap socialMedia = new Bitmap(postedFile.InputStream);
                string ext = Path.GetExtension(postedFile.FileName);
                if (ext == ".jpg" || ext == ".png" || ext == ".jpeg" || ext == ".gif")
                {
                    string uniqueNumber = Guid.NewGuid().ToString();
                    string fileName = uniqueNumber + postedFile.FileName;
                    socialMedia.Save(Server.MapPath(@"~\Areas\Admin\Content\SocialMediaImages\" + fileName));
                    model.ImagePath = fileName;
                    if (bll.AddSocialMedia(model))
                    {
                        ViewBag.ProcessState = General.Messages.AddSuccess;
                        model = new SocialMediaDTO();
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.ProcessState = General.Messages.GeneralError;
                    }
                }
                else
                {
                    ViewBag.ProcessState = General.Messages.ExtensionError;
                }
            }
            else
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
            }
            return View(model);
        }
        public ActionResult SocialMediaList()
        {
            List<SocialMediaDTO> dtoList = new List<SocialMediaDTO>();
            dtoList = bll.GetSocialMedia();
            return View(dtoList);
        }
        public ActionResult UpdateSocialMedia(int ID)
        {
            SocialMediaDTO dto = bll.GetSocialMediaWithID(ID);
            return View(dto);
        }
        [HttpPost]
        public ActionResult UpdateSocialMedia(SocialMediaDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
            }
            else
            {
                if (model.SocialImage != null) // There is new added image.
                {
                    HttpPostedFileBase postedFile = model.SocialImage;
                    string ext = Path.GetExtension(postedFile.FileName);
                    if (ext == ".jpg" || ext == ".png" || ext == ".jpeg" || ext == ".gif")
                    {
                        Bitmap socialMedia = new Bitmap(postedFile.InputStream);
                        string uniqueNumber = Guid.NewGuid().ToString();
                        string fileName = uniqueNumber + postedFile.FileName;
                        socialMedia.Save(Server.MapPath(@"~\Areas\Admin\Content\SocialMediaImages\" + fileName));
                        model.ImagePath = fileName;
                    }
                }
                string oldImagePath = bll.UpdateSocialMedia(model);
                if (model.SocialImage != null) // Delete old image.
                {
                    string oldImageFullPath = Server.MapPath(@"~\Areas\Admin\Content\SocialMediaImages\" + oldImagePath);
                    if (System.IO.File.Exists(oldImageFullPath))
                    {
                        System.IO.File.Delete(oldImageFullPath);
                    }
                }
                ViewBag.ProcessState = General.Messages.UpdateSuccess;
            }
            return View(model);
        }
        public JsonResult DeleteSocialMedia(int ID)
        {
            string imagePath = bll.DeleteSocialMedia(ID);
            string ImageFullPath = Server.MapPath(@"~\Areas\Admin\Content\SocialMediaImages\" + imagePath);
            if (System.IO.File.Exists(ImageFullPath))
            {
                System.IO.File.Delete(ImageFullPath);
            }
            return Json("");
        }
    }
}