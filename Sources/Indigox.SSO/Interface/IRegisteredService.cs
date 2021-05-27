using System;

namespace Indigox.SSO.Interface
{
    public interface IRegisteredService
    {
        int ID { get; set; }
        string ServiceID { get; set; }
        string Name { get; set; }
        string SecretKey { get; set; }
        string LoginUrl { get; set; }
        string LoginOutUrl { get; set; }
        DateTime CreateTime { get; set; }
        bool IsEnabled { get; set; }
        string AccessLoginUrl { get; set; }
        bool IsWindowsAuthentication { get; set; }
        bool IsDefaultService { get; set; }
        bool IsAllowedToProxy { get; set; }
    }
}