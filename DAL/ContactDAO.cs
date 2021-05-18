﻿using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ContactDAO : PostContext
    {
        public void Add(Contact contact)
        {
            db.Contacts.Add(contact);
            db.SaveChanges();
        }

        public List<ContactDTO> GetUnreadMessages()
        {
            List<ContactDTO> dtoList = new List<ContactDTO>();
            List<Contact> contacts = db.Contacts.Where(x => x.isDeleted == false && x.isRead == false).OrderByDescending(x => x.AddDate) .ToList();
            foreach (var item in contacts)
            {
                ContactDTO dto = new ContactDTO();
                dto.ID = item.ID;
                dto.Subject = item.Subject;
                dto.Name = item.NameSurname;
                dto.Message = item.Message;
                dto.Email = item.Email;
                dto.AddDate = item.AddDate;
                dto.isRead = item.isRead;
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public List<ContactDTO> GetMessages()
        {
            List<ContactDTO> dtoList = new List<ContactDTO>();
            List<Contact> contacts = db.Contacts.Where(x => x.isDeleted == false).OrderByDescending(x => x.AddDate).ToList();
            foreach (var item in contacts)
            {
                ContactDTO dto = new ContactDTO();
                dto.ID = item.ID;
                dto.Subject = item.Subject;
                dto.Name = item.NameSurname;
                dto.Message = item.Message;
                dto.Email = item.Email;
                dto.AddDate = item.AddDate;
                dto.isRead = item.isRead;
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public void DeleteMessage(int ID)
        {
            Contact contact = db.Contacts.First(x => x.ID == ID);
            contact.isDeleted = true;
            contact.DeletedDate = DateTime.Now;
            contact.LastUpdateDate = DateTime.Now;
            contact.LastUpdateUserID = UserStatic.UserID;
            db.SaveChanges();
        }

        public void ReadMessage(int ID)
        {
            Contact contact = db.Contacts.First(x => x.ID == ID);
            contact.isRead = true;
            contact.ReadUserID = UserStatic.UserID;
            contact.LastUpdateDate = DateTime.Now;
            contact.LastUpdateUserID = UserStatic.UserID;
            db.SaveChanges();
        }
    }
}