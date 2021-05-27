using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Common.DomainModels.Factory;
using Indigox.SSO.Services;

namespace Indigox.SSO.Application.CQRS
{
    public class DeleteRegisteredServiceCommand : Indigox.Web.CQRS.Interface.ICommand
    {
        public int ID { get; set; }

        public void Execute()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<RegisteredService>();
            var service = repository.Get(ID);

            repository.Remove(service);
        }
    }
}
