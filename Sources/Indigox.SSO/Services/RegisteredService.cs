using System;
using Indigox.SSO.Interface;

namespace Indigox.SSO.Services
{
    public class RegisteredService : IRegisteredService, IEquatable<IRegisteredService>
    {
        private int id;
        private string serviceID;
        private string name;
        private string secretKey;
        private string accessLoginUrl;
        private string loginUrl;
        private string loginOutUrl;
        private DateTime createTime;
        private bool isEnabled;
        private bool isWindowsAuthentication;
        private bool isDefaultService;
        private bool isAllowedToProxy;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string ServiceID
        {
            get { return serviceID; }
            set { serviceID = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string SecretKey
        {
            get { return secretKey; }
            set { secretKey = value; }
        }

        public string AccessLoginUrl
        {
            get { return accessLoginUrl; }
            set { accessLoginUrl = value; }
        }

        public string LoginUrl
        {
            get { return loginUrl; }
            set { loginUrl = value; }
        }

        public string LoginOutUrl
        {
            get { return loginOutUrl; }
            set { loginOutUrl = value; }
        }

        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public bool IsWindowsAuthentication
        {
            get { return isWindowsAuthentication; }
            set { isWindowsAuthentication = value; }
        }

        public bool IsDefaultService
        {
            get { return isDefaultService; }
            set { isDefaultService = value; }
        }

        public bool IsAllowedToProxy
        {
            get { return isAllowedToProxy; }
            set { isAllowedToProxy = value; }
        }

        public bool Equals(IRegisteredService other)
        {
            return this.serviceID == other.ServiceID;
        }
    }
}