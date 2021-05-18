using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class AddressBLL
    {
        AddressDAO dao = new AddressDAO();
        public bool AddAddress(AddressDTO model)
        {
            Address address = new Address();
            address.Address1 = model.AddressContent;
            address.Email = model.Email;
            address.Phone = model.Phone;
            address.Phone2 = model.Phone2;
            address.Fax = model.Fax;
            address.MapPathLarge = model.LargeMapPath;
            address.MapPathSmall = model.SmallMapPath;
            address.AddDate = DateTime.Now;
            address.LastUpdateDate = DateTime.Now;
            address.LastUpdateUserID = UserStatic.UserID;
            int ID = dao.AddAddress(address);
            LogDAO.AddLog(General.ProcessType.AddressAdd, General.TableName.Address, ID);
            return true;
        }

        public List<AddressDTO> GetAddresses()
        {
            return dao.GetAddresses();
        }

        public bool UpdateAddress(AddressDTO model)
        {
            dao.UpdateAddress(model);
            LogDAO.AddLog(General.ProcessType.AddressUpdate, General.TableName.Address, model.ID);
            return true;

        }

        public void DeleteAddress(int ID)
        {
            dao.DeleteAddress(ID);
            LogDAO.AddLog(General.ProcessType.AddressDelete, General.TableName.Address, ID);
        }
    }
}
