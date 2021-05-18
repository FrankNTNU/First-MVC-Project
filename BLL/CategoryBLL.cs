using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace BLL
{
    public class CategoryBLL
    {
        CategoryDAO dao = new CategoryDAO();
        public bool AddCategory(CategoryDTO model)
        {
            Category category = new Category();
            category.CateogryName = model.CategoryName;
            category.AddDate = DateTime.Now;
            category.LastUpdateDate = DateTime.Now;
            category.LastUpdateUserID = UserStatic.UserID;
            int ID = dao.AddCategory(category);
            LogDAO.AddLog(General.ProcessType.CategoryAdd, General.TableName.Category, ID);
            return true;
        }

        public static IEnumerable<SelectListItem> GetCategoriesForDropdown()
        {
            return CategoryDAO.GetCategoriesForDropdown();
        }

        public List<CategoryDTO> GetCategories()
        {
            return dao.GetCategories();
        }

        public CategoryDTO GetCategoryWithID(int ID)
        {
            return dao.GetCategoryWithID(ID);
        }

        public bool UpdateCategory(CategoryDTO model)
        {
            dao.UpdateCategory(model);
            LogDAO.AddLog(General.ProcessType.CategoryUpdate, General.TableName.Category, model.ID);
            return true;
        }
        PostBLL postBLL = new PostBLL();
        public List<PostImageDTO> DeleteCategory(int ID)
        {
            List<Post> postList = dao.DeleteCategory(ID);
            LogDAO.AddLog(General.ProcessType.CategoryDelete, General.TableName.Category, ID);
            List<PostImageDTO> postImageList = new List<PostImageDTO>();
            foreach (Post post in postList)
            {
                List<PostImageDTO> imageList = postBLL.DeletePost(post.ID);
                LogDAO.AddLog(General.ProcessType.PostDelete, General.TableName.Post, post.ID);
                foreach (PostImageDTO image in imageList)
                {
                    postImageList.Add(image);
                }
            }
            return postImageList;
        }
    }
}
