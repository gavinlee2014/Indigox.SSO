using Indigox.SSO.Services;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;
using System;
using Indigox.Common.Utilities;

namespace Indigox.SSO.Application.CQRS
{
    public class CreateRegisteredServiceCommand : Indigox.Web.CQRS.Interface.ICommand
    {
        public string Name { get; set; }
        public string AccessLoginUrl { get; set; }
        public string LoginUrl { get; set; }
        public string LoginOutUrl { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDefaultService { get; set; }
        public bool IsAllowedToProxy { get; set; }
        public bool IsWindowsAuthentication { get; set; }

        public void Execute()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<RegisteredService>();
            RegisteredService service = new RegisteredService();
            service.ServiceID = GenerateServiceID();
            service.Name = Name;
            service.SecretKey = DESCrypt.GenerateKey();
            service.AccessLoginUrl = AccessLoginUrl;
            service.LoginUrl = LoginUrl;
            service.LoginOutUrl = LoginOutUrl;
            service.IsEnabled = IsEnabled;
            service.IsDefaultService = IsDefaultService;
            service.IsAllowedToProxy = IsAllowedToProxy;
            service.CreateTime = DateTime.Now;
            service.IsWindowsAuthentication = IsWindowsAuthentication;

            repository.Add(service);
        }

        private string GenerateServiceID()
        {
            string guid= Guid.NewGuid().ToString();
            int lastIndex=guid.LastIndexOf('-');
            return guid.Substring(lastIndex+1,guid.Length-lastIndex-1);
        }

    }
}
