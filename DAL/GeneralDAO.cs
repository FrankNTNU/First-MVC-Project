using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GeneralDAO : PostContext
    {
        public List<PostDTO> GetSliderPosts()
        {
            List<PostDTO> dtoList = new List<PostDTO>();
            var list = (from p in db.Posts.Where(x => x.Slider == true && x.isDeleted == false)
                        select new
                        {
                            postID = p.ID,
                            title = p.Title,
                            categoryName = p.Category.CateogryName,
                            seoLink = p.SeoLink,
                            viewCount = p.ViewCount,
                            addDate = p.AddDate
                        }).OrderByDescending(x => x.addDate).Take(8).ToList();
            foreach (var item in list)
            {
                PostDTO dto = new PostDTO();
                dto.ID = item.postID;
                dto.Title = item.title;
                dto.CategoryName = item.categoryName;
                dto.ViewCount = item.viewCount;
                dto.SeoLink = item.seoLink;
                PostImage image = db.PostImages.First(x => x.isDeleted == false && x.PostID == item.postID); // Show only the first image in the slider.
                dto.ImagePath = image.ImagePath;
                dto.CommentCount = db.Comments.Where(x => x.isDeleted == false && x.PostID == item.postID && x.isApproved == true).Count();
                dto.AddDate = item.addDate;
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public List<VideoDTO> GetAllVideos()
        {
            List<Video> videos = db.Videos.Where(x => x.isDeleted == false).OrderByDescending(x => x.AddDate).ToList();
            List<VideoDTO> dtoList = new List<VideoDTO>();
            foreach (Video item in videos)
            {
                VideoDTO dto = new VideoDTO();
                dto.ID = item.ID;
                dto.Title = item.Title;
                dto.VideoPath = item.VideoPath;
                dto.OriginalVideoPath = item.OriginalVideoPath;
                dto.AddDate = item.AddDate;
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public List<PostDTO> GetCategoryPostList(int categoryID)
        {

            List<PostDTO> dtoList = new List<PostDTO>();
            var list = (from p in db.Posts.Where(x => x.CategoryID == categoryID && x.isDeleted == false)
                        select new
                        {
                            postID = p.ID,
                            title = p.Title,
                            categoryName = p.Category.CateogryName,
                            seoLink = p.SeoLink,
                            viewCount = p.ViewCount,
                            addDate = p.AddDate
                        }).OrderByDescending(x => x.addDate).ToList();
            foreach (var item in list)
            {
                PostDTO dto = new PostDTO();
                dto.ID = item.postID;
                dto.Title = item.title;
                dto.CategoryName = item.categoryName;
                dto.ViewCount = item.viewCount;
                dto.SeoLink = item.seoLink;
                PostImage image = db.PostImages.First(x => x.isDeleted == false && x.PostID == item.postID);
                dto.ImagePath = image.ImagePath;
                dto.CommentCount = db.Comments.Where(x => x.isDeleted == false && x.PostID == item.postID && x.isApproved == true).Count();
                dto.AddDate = item.addDate;
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public List<PostDTO> GetSearchPosts(string searchText)
        {
            List<PostDTO> dtoList = new List<PostDTO>();
            var list = (from p in db.Posts.Where(x => x.isDeleted == false && (x.Title.Contains(searchText) || x.PostContent.Contains(searchText)))
                        select new
                        {
                            postID = p.ID,
                            title = p.Title,
                            categoryName = p.Category.CateogryName,
                            seoLink = p.SeoLink,
                            viewCount = p.ViewCount,
                            addDate = p.AddDate
                        }).OrderByDescending(x => x.addDate).ToList();
            foreach (var item in list)
            {
                PostDTO dto = new PostDTO();
                dto.ID = item.postID;
                dto.Title = item.title;
                dto.CategoryName = item.categoryName;
                dto.ViewCount = item.viewCount;
                dto.SeoLink = item.seoLink;
                PostImage image = db.PostImages.First(x => x.isDeleted == false && x.PostID == item.postID);
                dto.ImagePath = image.ImagePath;
                dto.CommentCount = db.Comments.Where(x => x.isDeleted == false && x.PostID == item.postID && x.isApproved == true).Count();
                dto.AddDate = item.addDate;
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public PostDTO GetPostDetail(int ID)
        {
            Post post = db.Posts.First(x => x.ID == ID);
            post.ViewCount++;
            db.SaveChanges();
            PostDTO dto = new PostDTO();
            dto.ID = post.ID;
            dto.Title = post.Title;
            dto.ShortContent = post.ShortContent;
            dto.PostContent = post.PostContent;
            dto.Language = post.LanguageName;
            dto.SeoLink = post.SeoLink;
            dto.CategoryID = post.CategoryID;
            dto.CategoryName = post.Category.CateogryName;
            List<PostImage> images = db.PostImages.Where(x => x.PostID == post.ID && x.isDeleted == false).ToList();
            List<PostImageDTO> imageDTOList = new List<PostImageDTO>();
            foreach (PostImage item in images)
            {
                PostImageDTO postImageDTO = new PostImageDTO();
                postImageDTO.ID = item.ID;
                postImageDTO.ImagePath = item.ImagePath;
                imageDTOList.Add(postImageDTO);
            }
            dto.PostImages = imageDTOList;
            dto.CommentCount = db.Comments.Where(x => x.isDeleted == false && x.PostID == ID && x.isApproved == true).Count();
            List<Comment> comments = db.Comments.Where(x => x.isDeleted == false && x.PostID == ID && x.isApproved == true).ToList();
            List<CommentDTO> commentDTOList = new List<CommentDTO>();
            foreach (Comment item in comments)
            {
                CommentDTO commentDTO = new CommentDTO();
                commentDTO.ID = item.ID;
                commentDTO.CommentContent = item.CommentContent;
                commentDTO.Name = item.NameSurname;
                commentDTO.Email = item.Email;
                commentDTO.AddDate = item.AddDate;
                commentDTOList.Add(commentDTO);
            }
            dto.CommentList = commentDTOList;
            List<PostTag> tags = db.PostTags.Where(x => x.PostID == ID && x.isDeleted == false).ToList();
            List<TagDTO> tagDTOlist = new List<TagDTO>();
            foreach (PostTag item in tags)
            {
                TagDTO tagDTO = new TagDTO();
                tagDTO.ID = item.ID;
                tagDTO.TagContent = item.TagContent;
                tagDTOlist.Add(tagDTO);
            }
            dto.TagList = tagDTOlist;
            return dto;
        }

        public List<VideoDTO> GetVideos()
        {
            List<Video> videos = db.Videos.Where(x => x.isDeleted == false).OrderByDescending(x => x.AddDate).Take(3).ToList();
            List<VideoDTO> dtoList = new List<VideoDTO>();
            foreach (Video item in videos)
            {
                VideoDTO dto = new VideoDTO();
                dto.ID = item.ID;
                dto.Title = item.Title;
                dto.VideoPath = item.VideoPath;
                dto.OriginalVideoPath = item.OriginalVideoPath;
                dto.AddDate = item.AddDate;
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public List<PostDTO> GetMostViewedPosts()
        {
            List<PostDTO> dtoList = new List<PostDTO>();
            var list = (from p in db.Posts.Where(x => x.isDeleted == false)
                        select new
                        {
                            postID = p.ID,
                            title = p.Title,
                            categoryName = p.Category.CateogryName,
                            seoLink = p.SeoLink,
                            viewCount = p.ViewCount,
                            addDate = p.AddDate
                        }).OrderByDescending(x => x.viewCount).Take(5).ToList();
            foreach (var item in list)
            {
                PostDTO dto = new PostDTO();
                dto.ID = item.postID;
                dto.Title = item.title;
                dto.CategoryName = item.categoryName;
                dto.ViewCount = item.viewCount;
                dto.SeoLink = item.seoLink;
                PostImage image = db.PostImages.First(x => x.isDeleted == false && x.PostID == item.postID); // Show only the first image in the slider.
                dto.ImagePath = image.ImagePath;
                dto.CommentCount = db.Comments.Where(x => x.isDeleted == false && x.PostID == item.postID && x.isApproved == true).Count();
                dto.AddDate = item.addDate;
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public List<PostDTO> GetPopularPosts()
        {
            List<PostDTO> dtoList = new List<PostDTO>();
            var list = (from p in db.Posts.Where(x => x.Area2 == true && x.isDeleted == false)
                        select new
                        {
                            postID = p.ID,
                            title = p.Title,
                            categoryName = p.Category.CateogryName,
                            seoLink = p.SeoLink,
                            viewCount = p.ViewCount,
                            addDate = p.AddDate
                        }).OrderByDescending(x => x.addDate).Take(5).ToList();
            foreach (var item in list)
            {
                PostDTO dto = new PostDTO();
                dto.ID = item.postID;
                dto.Title = item.title;
                dto.CategoryName = item.categoryName;
                dto.ViewCount = item.viewCount;
                dto.SeoLink = item.seoLink;
                PostImage image = db.PostImages.First(x => x.isDeleted == false && x.PostID == item.postID);
                dto.ImagePath = image.ImagePath;
                dto.CommentCount = db.Comments.Where(x => x.isDeleted == false && x.PostID == item.postID && x.isApproved == true).Count();
                dto.AddDate = item.addDate;
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public List<PostDTO> GetBreakingPosts()
        {
            List<PostDTO> dtoList = new List<PostDTO>();
            var list = (from p in db.Posts.Where(x => x.Slider == false && x.isDeleted == false)
                        select new
                        {
                            postID = p.ID,
                            title = p.Title,
                            categoryName = p.Category.CateogryName,
                            seoLink = p.SeoLink,
                            viewCount = p.ViewCount,
                            addDate = p.AddDate
                        }).OrderByDescending(x => x.addDate).Take(5).ToList();
            foreach (var item in list)
            {
                PostDTO dto = new PostDTO();
                dto.ID = item.postID;
                dto.Title = item.title;
                dto.CategoryName = item.categoryName;
                dto.ViewCount = item.viewCount;
                dto.SeoLink = item.seoLink;
                PostImage image = db.PostImages.First(x => x.isDeleted == false && x.PostID == item.postID);
                dto.ImagePath = image.ImagePath;
                dto.CommentCount = db.Comments.Where(x => x.isDeleted == false && x.PostID == item.postID && x.isApproved == true).Count();
                dto.AddDate = item.addDate;
                dtoList.Add(dto);
            }
            return dtoList;
        }
    }
}
