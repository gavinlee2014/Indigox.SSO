using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.SSO.Services;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;

namespace Indigox.SSO.Application.CQRS
{
    public class RegisteredServiceListQuery : Indigox.Web.CQRS.GenericListQuery<RegisteredService>
    {
        public override IList<RegisteredService> List()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<RegisteredService>();
            var condition = new Query();
            condition.StartFrom(FirstResult).LimitTo(FetchSize);
            var list = repository.Find(condition);
            return list;
        }

        public override int Size()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<RegisteredService>();
            var condition = new Query();
            return repository.GetTotalCount(condition);
        }
    }
}
