using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LayoutBLL
    {
        CategoryDAO categoryDAO = new CategoryDAO();
        SocialMediaDAO socialMediaDAO = new SocialMediaDAO();
        FavDAO favDAO = new FavDAO();
        MetaDAO metaDAO = new MetaDAO();
        AddressDAO addressDAO = new AddressDAO();
        PostDAO postDAO = new PostDAO();
        public HomeLayoutDTO GetLayoutData()
        {
            HomeLayoutDTO dto = new HomeLayoutDTO();
            dto.Categories = categoryDAO.GetCategories();
            List<SocialMediaDTO> socialMediaList = new List<SocialMediaDTO>();
            socialMediaList = socialMediaDAO.GetSocialMedia();
            dto.Facebook = socialMediaList.First(x => x.Link.Contains("facebook"));
            dto.Twitter = socialMediaList.First(x => x.Link.Contains("twitter"));
            dto.Instagram = socialMediaList.First(x => x.Link.Contains("instagram"));
            dto.YouTube = socialMediaList.First(x => x.Link.Contains("youtube"));
            dto.Linkedin = socialMediaList.First(x => x.Link.Contains("linkedin"));
            dto.FavDTO = favDAO.GetFav();
            dto.MetaList = metaDAO.GetMetaDate();
            List<AddressDTO> addressList = addressDAO.GetAddresses();
            dto.Address = addressList.First();
            dto.HotNews = postDAO.GetHotNews();
            return dto;
        }
    }
}
