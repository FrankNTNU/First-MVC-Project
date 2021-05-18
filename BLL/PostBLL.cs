using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PostBLL
    {
        PostDAO dao = new PostDAO();
        public bool AddPost(PostDTO model)
        {
            Post post = new Post();
            post.Title = model.Title;
            post.ShortContent = model.ShortContent;
            post.PostContent = model.PostContent;
            post.Slider = model.Slider;
            post.Area1 = model.Area1;
            post.Area2 = model.Area2;
            post.Area3 = model.Area3;
            post.Notification = model.Notification;
            post.CategoryID = model.CategoryID;
            post.SeoLink = SeoLink.GenerateUrl(model.Title);
            post.LanguageName = model.Language;
            post.AddDate = DateTime.Now;
            post.AddUserID = UserStatic.UserID;
            post.LastUpdateDate = DateTime.Now;
            post.LastUpdateUserID = UserStatic.UserID;
            int ID = dao.AddPost(post);
            LogDAO.AddLog(General.ProcessType.PostAdd, General.TableName.Post, ID);
            SavePostImages(model.PostImages, ID);
            AddTags(model.TagText, ID);
            return true;
        }

        public CountDTO GetAllCounts()
        {
            return dao.GetAllCounts();
        }

        public List<CommentDTO> GetAllComments()
        {
            return dao.GetAllComments();
        }

        public void DeleteComment(int ID)
        {
            dao.DeleteComment(ID);
            LogDAO.AddLog(General.ProcessType.CommentDelete, General.TableName.Comment, ID);
        }

        public void ApproveComment(int ID)
        {
            dao.ApproveComment(ID);
            LogDAO.AddLog(General.ProcessType.CommentApprove, General.TableName.Comment, ID);
        }

        public List<CommentDTO> GetComments()
        {
            return dao.GetComments();
        }

        public List<PostDTO> GetPosts()
        {
            return dao.GetPosts();
        }

        private void AddTags(string tagText, int ID)
        {
            string[] tags;
            tags = tagText.Split(',');
            List<PostTag> tagList = new List<PostTag>();
            foreach (string item in tags)
            {
                PostTag tag = new PostTag();
                tag.PostID = ID;
                tag.TagContent += item;
                tag.AddDate = DateTime.Now;
                tag.LastUpdateDate = DateTime.Now;
                tag.LastUpdateUserID = UserStatic.UserID;
                tagList.Add(tag);
            }
            foreach (var item in tagList)
            {
                int tagID = dao.AddTag(item);
                LogDAO.AddLog(General.ProcessType.TagAdd, General.TableName.Tag, tagID);
            }

        }

        public bool AddComment(GeneralDTO model)
        {
            Comment comment = new Comment();
            comment.PostID = model.PostID;
            comment.NameSurname = model.Name;
            comment.Email = model.Email;
            comment.CommentContent = model.Message;
            comment.AddDate = DateTime.Now;
            dao.AddComment(comment);
            return true;
        }

        void SavePostImages(List<PostImageDTO> list, int PostID)
        {
            List<PostImage> imageList = new List<PostImage>();
            foreach (PostImageDTO item in list)
            {
                PostImage image = new PostImage();
                image.PostID = PostID;
                image.ImagePath = item.ImagePath;
                image.AddDate = DateTime.Now;
                image.LastUpdateDate = DateTime.Now;
                image.LastUpdateUserID = UserStatic.UserID;
                imageList.Add(image);
            }
            foreach (PostImage item in imageList)
            {
                int imageID = dao.AddImage(item);
                LogDAO.AddLog(General.ProcessType.ImageAdd, General.TableName.Image, imageID);
            }
        }

        public PostDTO GetPostWithID(int ID)
        {
            PostDTO dto = new PostDTO();
            dto = dao.GetPostWithID(ID);
            dto.PostImages = dao.GetPostImagesWithPostID(ID);
            List<PostTag> tagList = dao.GetPostTagsWithPostID(ID);
            string tagValue = "";
            foreach (PostTag item in tagList)
            {
                tagValue += item.TagContent;
                tagValue += ",";
            }
            dto.TagText = tagValue;
            return dto;
        }

        public bool UpdatePost(PostDTO model)
        {
            model.SeoLink = SeoLink.GenerateUrl(model.Title);
            dao.UpdatePost(model);
            LogDAO.AddLog(General.ProcessType.PostUpdate, General.TableName.Post, model.ID);
            if (model.PostImages != null)
            {
                SavePostImages(model.PostImages, model.ID);
            }
            dao.DeleteTags(model.ID);
            AddTags(model.TagText, model.ID);
            return true;
        }

        public string DeletePostImage(int ID)
        {
            string imagePath = dao.DeletePostImage(ID);
            LogDAO.AddLog(General.ProcessType.ImageDelete, General.TableName.Image, ID);
            return imagePath;
        }

        public List<PostImageDTO> DeletePost(int ID)
        {
            List<PostImageDTO> imageList = dao.DeletePost(ID);
            LogDAO.AddLog(General.ProcessType.PostDelete, General.TableName.Post, ID);
            return imageList;
        }

        public CountDTO GetCounts()
        {
            CountDTO dto = new CountDTO();
            dto.MessageCount = dao.GetMessageCount();
            dto.CommentCount = dao.GetCommentCount();
            return dto;
        }
    }
}
