using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SocialMediaBLL
    {
        SocialMediaDAO dao = new SocialMediaDAO();
        public bool AddSocialMedia(SocialMediaDTO model)
        {
            SocialMedia socialMedia = new SocialMedia();
            socialMedia.Name = model.Name;
            socialMedia.ImagePath = model.ImagePath;
            socialMedia.Link = model.Link;
            socialMedia.AddDate = DateTime.Now;
            socialMedia.LastUpdateDate = DateTime.Now;
            socialMedia.LastUpdateUserID = UserStatic.UserID;
            int ID = dao.AddSocialMedia(socialMedia);
            LogDAO.AddLog(General.ProcessType.SocialMediaAdd, General.TableName.Social, ID);
            return true;
        }

        public List<SocialMediaDTO> GetSocialMedia()
        {
            List<SocialMediaDTO> dtoList = new List<SocialMediaDTO>();
            dtoList = dao.GetSocialMedia();
            return dtoList;
        }

        public SocialMediaDTO GetSocialMediaWithID(int ID)
        {
            SocialMediaDTO dto = dao.GetSocialMediaWithID(ID);
            return dto;
        }

        public string UpdateSocialMedia(SocialMediaDTO model)
        {
            string oldImagePath = dao.UpdateSocialMedia(model);
            LogDAO.AddLog(General.ProcessType.SocialMediaUpdate, General.TableName.Social, model.ID);
            return oldImagePath;
        }

        public string DeleteSocialMedia(int ID)
        {
            string imagePath = dao.DeleteSocialMedia(ID);
            LogDAO.AddLog(General.ProcessType.SocialMediaDelete, General.TableName.Social, ID);
            return imagePath;
        }
    }
}
