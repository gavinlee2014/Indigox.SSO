using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.SSO.Logs;

namespace Indigox.SSO.Application.DTO
{
    public class AccessRecordDTO
    {
        public int ID { get; set; }
        public string AccountName { get; set; }
        public string IP { get; set; }
        public DateTime LogTime { get; set; }
        public string ServiceName { get; set; }
        public string ServiceID { get; set; }
        public int ServiceSerialID { get; set; }

        public static AccessRecordDTO ConvertToDTO(AccessRecord item)
        {
            AccessRecordDTO dto = new AccessRecordDTO();
            dto.ID = item.ID;
            dto.AccountName = item.AccountName;
            dto.IP = item.IP;
            dto.LogTime = item.LogTime;
            dto.ServiceName = item.ServiceName;
            dto.ServiceID = item.ServiceID;
            dto.ServiceSerialID = item.ServiceSerialID;

            return dto;
        }
        public static IList<AccessRecordDTO> ConvertToDTOs(IList<AccessRecord> items)
        {
            List<AccessRecordDTO> list = new List<AccessRecordDTO>();
            foreach (AccessRecord item in items)
            {
                list.Add(ConvertToDTO(item));
            }

            return list;
        }
    }
}
