using System;

namespace Indigox.SSO.Logs
{
    public class LogRecord
    {
        private int id;
        private string accountName;
        private string ip;
        private DateTime logTime;
        private ActionType action;

        public LogRecord() { }

        public LogRecord(string accountName, string ip, ActionType action)
        {
            this.accountName = accountName;
            this.ip = ip;
            this.action = action;
            this.logTime = DateTime.Now;
        }

        public int ID 
        { 
            get { return this.id; }
        }

        public string AccountName 
        { 
            get { return this.accountName; } 
        }

        public string IP 
        { 
            get { return this.ip; } 
        }

        public DateTime LogTime 
        { 
            get { return this.logTime; } 
        }

        public ActionType Action 
        { 
            get { return this.action; } 
        }
    }

    public enum ActionType
    {
        Login,Logout
    }
}
