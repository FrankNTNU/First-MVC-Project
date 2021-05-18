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
    public class UserController : BaseController
    {
        UserBLL bll = new UserBLL();
        // GET: Admin/User
        public ActionResult UserList()
        {
            List<UserDTO> model = new List<UserDTO>();
            model = bll.GetUsers();
            return View(model);
        }
        public ActionResult AddUser()
        {
            UserDTO model = new UserDTO();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddUser(UserDTO model)
        {
            if (model.UserImage == null)
            {
                ViewBag.ProcessState = General.Messages.ImageMissing;
            }
            else if (ModelState.IsValid)
            {
                HttpPostedFileBase postedFile = model.UserImage;
                string ext = Path.GetExtension(postedFile.FileName);
                if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                {
                    Bitmap userImage = new Bitmap(postedFile.InputStream);
                    Bitmap resizedImage = new Bitmap(userImage, 128, 128);
                    string uniqueNumber = Guid.NewGuid().ToString();
                    string fileName = uniqueNumber + postedFile.FileName;
                    resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/UserImage/" + fileName));
                    model.ImagePath = fileName;
                    bll.AddUser(model);
                    ViewBag.ProcessState = General.Messages.AddSuccess;
                    ModelState.Clear();
                    model = new UserDTO();
                }
                else
                {
                    ViewBag.ProcessState = General.Messages.ExtensionError;
                }
            }
            else
            {
                ViewBag.ProcessState = General.Messages.GeneralError;
            }
            return View(model);
        }
        public ActionResult UpdateUser(int ID)
        {
            UserDTO dto = new UserDTO();
            dto = bll.GetUserWithID(ID);
            return View(dto);
        }
        [HttpPost]
        public ActionResult UpdateUser(UserDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
            }
            else
            {
                if (model.UserImage != null) // Image has been changed.
                {
                    HttpPostedFileBase postedFile = model.UserImage;
                    string ext = Path.GetExtension(postedFile.FileName);
                    if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                    {
                        Bitmap userImage = new Bitmap(postedFile.InputStream);
                        Bitmap resizedImage = new Bitmap(userImage, 128, 128);
                        string uniqueNumber = Guid.NewGuid().ToString();
                        string fileName = uniqueNumber + postedFile.FileName;
                        resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/UserImage/" + fileName));
                        model.ImagePath = fileName;
                    }
                    string oldImagePath = bll.UpdateUser(model);
                    if (model.UserImage != null)
                    {
                        string oldImageFullPath = "~/Areas/Admin/Content/UserImage/" + oldImagePath;
                        if (System.IO.File.Exists(Server.MapPath(oldImageFullPath)))
                        {
                            System.IO.File.Delete(Server.MapPath(oldImageFullPath));
                        }
                        ViewBag.ProcessState = General.Messages.UpdateSuccess;
                    }
                }
            }
            return View(model);
        }
        public JsonResult DeleteUser(int ID)
        {
            string imagePath = bll.DeleteUser(ID);
            string ImageFullPath = Server.MapPath(@"~\Areas\Admin\Content\UserImage\" + imagePath);
            if (System.IO.File.Exists(ImageFullPath))
            {
                System.IO.File.Delete(ImageFullPath);
            }
            return Json("");
        }
    }
}