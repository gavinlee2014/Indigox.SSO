using Indigox.Common.DomainModels.Factory;
using Indigox.SSO.Interface;
using Indigox.SSO.Logs;
using Indigox.Common.Logging;

namespace Indigox.SSO.Services
{
    public class Loggin
    {
        public static void RecordLogin(string accountName, string ip, ActionType action)
        {
            Log.Debug("添加登录/登出日志: accountName: " + accountName + ", ip: " + ip + ", action: " + action);
            RepositoryFactory.Instance.CreateRepository<LogRecord>().Add(new LogRecord(accountName, ip, action));
            Log.Debug("日志记录完毕");
        }

        public static void RecordAccess(string accountName, string ip, IRegisteredService service)
        {
            RepositoryFactory.Instance.CreateRepository<AccessRecord>().Add(new AccessRecord(accountName, ip, service));
        }
    }
}
