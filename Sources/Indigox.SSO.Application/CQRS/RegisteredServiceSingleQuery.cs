using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.SSO.Services;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;

namespace Indigox.SSO.Application.CQRS
{
    public class RegisteredServiceSingleQuery : Indigox.Web.CQRS.GenericSingleQuery<RegisteredService>
    {
        public int ID { get; set; }

        public override RegisteredService Single()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<RegisteredService>();
            var condition = new Query();
            var service = repository.Get(ID);
            return service;
        }
    }

}
