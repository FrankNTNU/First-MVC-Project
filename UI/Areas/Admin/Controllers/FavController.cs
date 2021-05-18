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
    public class FavController : BaseController
    {
        FavBLL bll = new FavBLL();
        // GET: Admin/Fav
        public ActionResult UpdateFav()
        {
            FavDTO dto = new FavDTO();
            dto = bll.GetFav();
            return View(dto);
        }
        [HttpPost]
        public ActionResult UpdateFav(FavDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
            }
            else
            {
                if (model.FavImage != null)
                {
                    HttpPostedFileBase postedFileFav = model.FavImage;
                    string ext = Path.GetExtension(postedFileFav.FileName);
                    if (ext == ".ico" || ext == ".png" || ext == ".jpg" || ext == "jpg")
                    {
                        Bitmap favImage = new Bitmap(postedFileFav.InputStream);
                        Bitmap resizedFavImage = new Bitmap(favImage, 100, 100);
                        string uniqueNumber = Guid.NewGuid().ToString();
                        string favName = uniqueNumber + postedFileFav.FileName;
                        resizedFavImage.Save(Server.MapPath("~/Areas/Admin/Content/FavImages/" + favName));
                        model.Fav = favName;
                    }
                    else
                    {
                        ViewBag.ProcessState = General.Messages.ExtensionError;
                    }
                }
                if (model.LogoImage != null)
                {
                    HttpPostedFileBase postedFileLogo = model.LogoImage;
                    string ext = Path.GetExtension(postedFileLogo.FileName);
                    if (ext == ".ico" || ext == ".png" || ext == ".jpg" || ext == "jpg")
                    {
                        Bitmap logoImage = new Bitmap(postedFileLogo.InputStream);
                        Bitmap resizedLogoImage = new Bitmap(logoImage, 500, 500);
                        string uniqueNumber = Guid.NewGuid().ToString();
                        string logoName = uniqueNumber + postedFileLogo.FileName;
                        resizedLogoImage.Save(Server.MapPath("~/Areas/Admin/Content/FavImages/" + logoName));
                        model.Logo = logoName;
                    }
                    else
                    {
                        ViewBag.ProcessState = General.Messages.ExtensionError;
                    }
                }
                FavDTO returnDTO = new FavDTO();
                returnDTO = bll.UpdateFav(model);
                if (model.FavImage != null)
                {
                    string favImageFullPath = Server.MapPath("~/Areas/Admin/Content/FavImages/" + returnDTO.Fav);
                    if (System.IO.File.Exists(favImageFullPath))
                    {
                        System.IO.File.Delete(favImageFullPath);
                    }
                }
                if (model.LogoImage != null)
                {
                    string logoImageFullPath = Server.MapPath("~/Areas/Admin/Content/FavImages/" + returnDTO.Logo);
                    if (System.IO.File.Exists(logoImageFullPath))
                    {
                        System.IO.File.Delete(logoImageFullPath);
                    }
                }
                ViewBag.ProcessState = General.Messages.UpdateSuccess;
            }
            return View(model);
        }
    }
}