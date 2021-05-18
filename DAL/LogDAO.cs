﻿using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL
{
    public class LogDAO : PostContext
    {
        public static void AddLog(int ProcessType, string TableName, int ProcessID)
        {
            Log_Table log = new Log_Table();
            log.UserID = UserStatic.UserID;
            log.ProcessType = ProcessType;
            log.ProcessID = ProcessID;
            log.ProcessCategoryType = TableName;
            log.ProcessDate = DateTime.Now;
            log.IPAddress = HttpContext.Current.Request.UserHostAddress;
            db.Log_Table.Add(log);
            db.SaveChanges();
        }

        public List<LogDTO> GetLogs()
        {
            List<LogDTO> dtoList = new List<LogDTO>();
            List<LogDTO> list = (from l in db.Log_Table
                        select new LogDTO
                        {
                            ID = l.ID,
                            UserName = l.T_User.NameSurname,
                            TableName = l.ProcessCategoryType,
                            TableID = l.ProcessID,
                            ProcessName = l.ProcessType1.ProcessName,
                            ProcessDate = l.ProcessDate,
                            IPAddress = l.IPAddress
                        }).OrderByDescending(x => x.ProcessDate) .ToList();
            foreach (var item in list)
            {
                LogDTO dto = new LogDTO();
                dto.ID = item.ID;
                dto.UserName = item.UserName;
                dto.TableName = item.TableName;
                dto.TableID = item.TableID;
                dto.ProcessName = item.ProcessName;
                dto.ProcessDate = item.ProcessDate;
                dto.IPAddress = item.IPAddress;
                dtoList.Add(dto);
            }
            return dtoList;
        }
    }
}
