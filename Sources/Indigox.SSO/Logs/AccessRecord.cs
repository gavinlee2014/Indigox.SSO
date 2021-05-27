using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.SSO.Interface;

namespace Indigox.SSO.Logs
{
    public class AccessRecord
    {
        private int id;
        private string accountName;
        private string ip;
        private DateTime logTime;
        private string serviceName;
        private string serviceID;
        private int serviceSerialID;

        public AccessRecord() { }

        public AccessRecord(string accountName, string ip, IRegisteredService service)
        {
            this.accountName = accountName;
            this.ip = ip;
            this.logTime = DateTime.Now;
            this.serviceName = service.Name;
            this.serviceID = service.ServiceID;
            this.serviceSerialID = service.ID;
        }

        public int ID
        {
            get { return id; }
        }

        public string AccountName
        {
            get { return accountName; }
        }

        public string IP
        {
            get { return ip; }
        }

        public DateTime LogTime
        {
            get { return logTime; }
        }

        public string ServiceName
        {
            get { return serviceName; }
        }

        public string ServiceID
        {
            get { return serviceID; }
        }

        public int ServiceSerialID
        {
            get { return serviceSerialID; }
        }
    }
}
