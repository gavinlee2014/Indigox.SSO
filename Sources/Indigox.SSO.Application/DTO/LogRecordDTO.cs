using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.SSO.Logs;

namespace Indigox.SSO.Application.DTO
{
    public class LogRecordDTO
    {
        public int ID {get;set;}
        public string AccountName { get; set; }
        public string IP { get; set; }
        public DateTime LogTime { get; set; }
        public int Action { get; set; }

        public static LogRecordDTO ConvertToDTO(LogRecord item)
        {
            LogRecordDTO dto = new LogRecordDTO();
            dto.ID = item.ID;
            dto.AccountName = item.AccountName;
            dto.IP = item.IP;
            dto.LogTime = item.LogTime;
            dto.Action = (int)item.Action;

            return dto;
        }
        public static IList<LogRecordDTO> ConvertToDTOs(IList<LogRecord> items)
        {
            List<LogRecordDTO> list = new List<LogRecordDTO>();
            foreach (LogRecord item in items)
            {
                list.Add(ConvertToDTO(item));
            }

            return list;
        }
    }
}
