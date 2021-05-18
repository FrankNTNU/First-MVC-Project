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
    public class AdsController : BaseController
    {
        AdsBLL bll = new AdsBLL();
        // GET: Admin/Ads
        public ActionResult AdsList()
        {
            List<AdsDTO> AdsList = new List<AdsDTO>();
            AdsList = bll.GetAds();
            return View(AdsList);
        }
        public ActionResult AddAds()
        {
            AdsDTO dto = new AdsDTO();
            return View(dto);
        }
        [HttpPost] 
        public ActionResult AddAds(AdsDTO model)
        {
            if (model.AdsImage == null)
            {
                ViewBag.ProcessState = General.Messages.ImageMissing;
            }
            else if (ModelState.IsValid)
            {
                HttpPostedFileBase postedFile = model.AdsImage;
                string ext = Path.GetExtension(postedFile.FileName);
                if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                {
                    Bitmap userImage = new Bitmap(postedFile.InputStream);
                    Bitmap resizedImage = new Bitmap(userImage);
                    string uniqueNumber = Guid.NewGuid().ToString();
                    string fileName = uniqueNumber + postedFile.FileName;
                    resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/AdsImages/" + fileName));
                    model.ImagePath = fileName;
                    bll.AddAds(model);
                    ViewBag.ProcessState = General.Messages.AddSuccess;
                    ModelState.Clear();
                    model = new AdsDTO();
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
        public ActionResult UpdateAds(int ID)
        {
            AdsDTO dto = bll.GetAdsWithID(ID);
            return View(dto);
        }
        [HttpPost]
        public ActionResult UpdateAds(AdsDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
            }
            else
            {
                if (model.AdsImage != null)
                {
                    HttpPostedFileBase postedFile = model.AdsImage;
                    string ext = Path.GetExtension(postedFile.FileName);
                    if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                    {
                        Bitmap userImage = new Bitmap(postedFile.InputStream);
                        Bitmap resizedImage = new Bitmap(userImage);
                        string uniqueNumber = Guid.NewGuid().ToString();
                        string fileName = uniqueNumber + postedFile.FileName;
                        resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/AdsImages/" + fileName));
                        model.ImagePath = fileName;
                    }
                }
                string oldImagePath = bll.UpdateAds(model);
                if (model.AdsImage != null) // There is new image.
                {
                    string oldImageFullPath = Server.MapPath("~/Areas/Admin/Content/AdsImages/" + oldImagePath);
                    if (System.IO.File.Exists(oldImageFullPath))
                    {
                        System.IO.File.Delete(oldImageFullPath);
                    }
                }
                ViewBag.ProcessState = General.Messages.AddSuccess;
            }
            return View(model);
        }
        public JsonResult DeleteAds(int ID)
        {
            string imagePath = bll.DeleteAds(ID);
            string imageFullPath = Server.MapPath("~/Areas/Admin/Content/AdsImages/" + imagePath);
            if (System.IO.File.Exists(imageFullPath))
            {
                System.IO.File.Delete(imageFullPath);
            }
            return Json("");
        }
    }
}