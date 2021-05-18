using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class GeneralBLL
    {
        GeneralDAO dao = new GeneralDAO();
        AdsDAO adsDAO = new AdsDAO();
        public GeneralDTO GetAllPosts()
        {
            GeneralDTO dto = new GeneralDTO();
            dto.SliderPosts = dao.GetSliderPosts();
            dto.BreakingPosts = dao.GetBreakingPosts();
            dto.PopularPosts = dao.GetPopularPosts();
            dto.MostViewedPosts = dao.GetMostViewedPosts();
            dto.Videos = dao.GetVideos();
            dto.AdsList = adsDAO.GetAds();
            return dto;
        }

        public GeneralDTO GetPostDetailPageItemWithID(int ID)
        {
            GeneralDTO dto = new GeneralDTO();
            dto.BreakingPosts = dao.GetBreakingPosts();
            dto.AdsList = adsDAO.GetAds();
            dto.PostDetail = dao.GetPostDetail(ID);
            return dto;
        }
        CategoryDAO categoryDAO = new CategoryDAO();
        public GeneralDTO GetCategoryPostList(string categoryName)
        {
            GeneralDTO dto = new GeneralDTO();
            dto.BreakingPosts = dao.GetBreakingPosts();
            dto.AdsList = adsDAO.GetAds();
            if(categoryName == "video")
            {
                dto.Videos = dao.GetAllVideos();
                dto.CategoryName = "video";
            }
            else
            {
                List<CategoryDTO> categoryList = categoryDAO.GetCategories();
                int categoryID = 0;
                foreach (var item in categoryList)
                {
                    if (categoryName == SeoLink.GenerateUrl(item.CategoryName))
                    {
                        categoryID = item.ID;
                        dto.CategoryName = item.CategoryName;
                        break;
                    }
                }
                dto.CategoryPostList = dao.GetCategoryPostList(categoryID);

            }
            return dto;
        }
        AddressDAO addressDAO = new AddressDAO();
        public GeneralDTO GetContactPageItems()
        {
            GeneralDTO dto = new GeneralDTO();
            dto.BreakingPosts = dao.GetBreakingPosts();
            dto.Address = addressDAO.GetAddresses().First();
            dto.AdsList = adsDAO.GetAds();
            return dto;
        }

        public GeneralDTO GetSearchPosts(string searchText)
        {
            GeneralDTO dto = new GeneralDTO();
            dto.BreakingPosts = dao.GetBreakingPosts();
            dto.AdsList = adsDAO.GetAds();
            dto.CategoryPostList = dao.GetSearchPosts(searchText);
            dto.SearchText = searchText;
            return dto;
        }
    }
}
