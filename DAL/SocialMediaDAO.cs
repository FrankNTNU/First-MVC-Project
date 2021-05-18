using DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SocialMediaDAO : PostContext
    {
        public int AddSocialMedia(SocialMedia socialMedia)
        {
            try
            {
                db.SocialMedias.Add(socialMedia);
                db.SaveChanges();
                return socialMedia.ID;
            }
            catch (DbEntityValidationException ex)
            {
                throw ex;
            }
        }

        public List<SocialMediaDTO> GetSocialMedia()
        {
            List<SocialMedia> list = db.SocialMedias.Where(x => x.isDeleted == false).ToList();
            List<SocialMediaDTO> dtoList = new List<SocialMediaDTO>();
            foreach (var item in list)
            {
                SocialMediaDTO dto = new SocialMediaDTO();
                dto.Name = item.Name;
                dto.ID = item.ID;
                dto.ImagePath = item.ImagePath;
                dto.Link = item.Link;
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public SocialMediaDTO GetSocialMediaWithID(int ID)
        {
            SocialMedia socialMedia = db.SocialMedias.First(x => x.ID == ID);
            SocialMediaDTO dto = new SocialMediaDTO();
            dto.ID = ID;
            dto.Name = socialMedia.Name;
            dto.ImagePath = socialMedia.ImagePath;
            dto.Link = socialMedia.Link;
            return dto;
        }

        public string UpdateSocialMedia(SocialMediaDTO model)
        {
            try
            {
                SocialMedia socialMedia = db.SocialMedias.First(x => x.ID == model.ID);
                string oldImagePath = socialMedia.ImagePath;
                socialMedia.Name = model.Name;
                socialMedia.Link = model.Link;
                socialMedia.LastUpdateUserID = UserStatic.UserID;
                socialMedia.LastUpdateDate = DateTime.Now;
                if (model.ImagePath != null)
                {
                    socialMedia.ImagePath = model.ImagePath;
                }
                db.SaveChanges();
                return oldImagePath;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string DeleteSocialMedia(int ID)
        {
            try
            {
                SocialMedia socialMedia = db.SocialMedias.First(x => x.ID == ID);
                string imagePath = socialMedia.ImagePath;
                socialMedia.isDeleted = true;
                socialMedia.DeletedDate = DateTime.Now;
                socialMedia.LastUpdateDate = DateTime.Now;
                socialMedia.LastUpdateUserID = UserStatic.UserID;
                db.SaveChanges();
                return imagePath;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
