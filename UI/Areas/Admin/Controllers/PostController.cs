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
    public class PostController : BaseController
    {
        PostBLL bll = new PostBLL();
        // GET: Admin/Post
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PostList()
        {
            CountDTO countDTO = new CountDTO();
            countDTO = bll.GetAllCounts();
            ViewData["AllCounts"] = countDTO;
            List<PostDTO> postList = new List<PostDTO>();
            postList = bll.GetPosts();
            return View(postList);
        }
        public ActionResult AddPost()
        {
            PostDTO model = new PostDTO();
            model.Categories = CategoryBLL.GetCategoriesForDropdown();
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddPost(PostDTO model)
        {
            if (model.PostImage[0] == null)
            {
                ViewBag.ProcessState = General.Messages.ImageMissing;
            }
            else if (ModelState.IsValid)
            {
                foreach (var item in model.PostImage)
                {
                    string ext = Path.GetExtension(item.FileName);
                    if (ext != ".png" && ext != ".jpg" && ext != ".jpeg")
                    {
                        ViewBag.ProcessState = General.Messages.ExtensionError;
                        model.Categories = CategoryBLL.GetCategoriesForDropdown();
                        return View(model);
                    }
                }
                List<PostImageDTO> imageList = new List<PostImageDTO>();
                foreach (HttpPostedFileBase postedFile in model.PostImage)
                {
                    Bitmap image = new Bitmap(postedFile.InputStream);
                    Bitmap resizedImage = new Bitmap(image, 750, 422);
                    string uniqueNumber = Guid.NewGuid().ToString();
                    string fileName = uniqueNumber + postedFile.FileName;
                    resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/PostImages/" + fileName));
                    PostImageDTO dto = new PostImageDTO();
                    dto.ImagePath = fileName;
                    imageList.Add(dto);
                }
                model.PostImages = imageList;
                if (bll.AddPost(model))
                {
                    ViewBag.ProcessState = General.Messages.AddSuccess;
                    ModelState.Clear();
                    model = new PostDTO();
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
            model.Categories = CategoryBLL.GetCategoriesForDropdown();
            return View(model);
        }
        public ActionResult UpdatePost(int ID)
        {
            PostDTO model = new PostDTO();
            model = bll.GetPostWithID(ID);
            model.Categories = CategoryBLL.GetCategoriesForDropdown();
            model.isUpdated = true;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdatePost(PostDTO model)
        {
            IEnumerable<SelectListItem> selectList = CategoryBLL.GetCategoriesForDropdown();
            if (ModelState.IsValid)
            {
                if (model.PostImage[0] != null)
                {
                    foreach (var item in model.PostImage)
                    {
                        string ext = Path.GetExtension(item.FileName);
                        if (ext != ".png" && ext != ".jpg" && ext != ".jpeg")
                        {
                            ViewBag.ProcessState = General.Messages.ExtensionError;
                            model.Categories = CategoryBLL.GetCategoriesForDropdown();
                            return View(model);
                        }
                    }
                    List<PostImageDTO> imageList = new List<PostImageDTO>();
                    foreach (HttpPostedFileBase postedFile in model.PostImage)
                    {
                        Bitmap image = new Bitmap(postedFile.InputStream);
                        Bitmap resizedImage = new Bitmap(image, 750, 422);
                        string uniqueNumber = Guid.NewGuid().ToString();
                        string fileName = uniqueNumber + postedFile.FileName;
                        resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/PostImages/" + fileName));
                        PostImageDTO dto = new PostImageDTO();
                        dto.ImagePath = fileName;
                        imageList.Add(dto);
                    }
                    model.PostImages = imageList;
                    if (bll.UpdatePost(model))
                    {
                        ViewBag.ProcessState = General.Messages.UpdateSuccess;
                    }
                    else
                    {
                        ViewBag.ProcessState = General.Messages.GeneralError;
                    }
                }
            }
            else
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
            }
            model = bll.GetPostWithID(model.ID);
            model.Categories = selectList;
            model.isUpdated = true;
            return View(model);
        }
        public JsonResult DeletePostImage(int ID)
        {
            string imagePath = bll.DeletePostImage(ID);
            string imageFullPath = Server.MapPath("~/Areas/Admin/Content/PostImages/" + imagePath);
            if (System.IO.File.Exists(imageFullPath))
            {
                System.IO.File.Delete(imageFullPath);
            }
            return Json("");
        }
        public JsonResult DeletePost(int ID)
        {
            List<PostImageDTO> imageList = bll.DeletePost(ID);
            foreach (PostImageDTO item in imageList)
            {
                string imageFullPath = Server.MapPath("~/Areas/Admin/Content/PostImages/" + item.ImagePath);
                if (System.IO.File.Exists(imageFullPath))
                {
                    System.IO.File.Delete(imageFullPath);
                }
            }
            return Json("");
        }
        public JsonResult GetCounts()
        {
            CountDTO dto = new CountDTO();
            dto = bll.GetCounts();
            return Json(dto, JsonRequestBehavior.AllowGet);
        }
    }
}