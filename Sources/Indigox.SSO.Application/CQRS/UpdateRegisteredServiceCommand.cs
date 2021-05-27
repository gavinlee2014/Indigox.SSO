using Indigox.SSO.Services;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;

namespace Indigox.SSO.Application.CQRS
{
    public class UpdateRegisteredServiceCommand : Indigox.Web.CQRS.Interface.ICommand
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string SecretKey { get; set; }
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
            var condition = new Query();
            condition.Specifications = Specification.Equal("ID", ID);
            var service = repository.First(condition);
            service.Name = Name;
            service.SecretKey = SecretKey;
            service.AccessLoginUrl = AccessLoginUrl;
            service.LoginUrl = LoginUrl;
            service.LoginOutUrl = LoginOutUrl;
            service.IsEnabled = IsEnabled;
            service.IsDefaultService = IsDefaultService;
            service.IsAllowedToProxy = IsAllowedToProxy;
            service.IsWindowsAuthentication = IsWindowsAuthentication;

            repository.Update(service);
        }
    }
}
