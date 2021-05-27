using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.Utilities;

namespace Indigox.SSO.Services
{
    public class ServiceManager
    {
        public RegisteredService GetDefaultRegisteredService()
        {
            var repos = RepositoryFactory.Instance.CreateRepository<RegisteredService>();
            RegisteredService service = repos.First(
                Query.NewQuery.FindByCondition(
                    Specification.Equal( "IsDefaultService", true )
                )
            );
            return service;
        }

        public RegisteredService GetRegisteredService( string serviceID )
        {
            ArgumentAssert.NotEmpty( serviceID, "serviceID" );

            var repos = RepositoryFactory.Instance.CreateRepository<RegisteredService>();
            RegisteredService service = repos.First(
                Query.NewQuery.FindByCondition(
                    Specification.Equal( "ServiceID", serviceID )
                )
            );

            if ( service == null )
            {
                throw new ApplicationException( "找不到服务 " + serviceID + "。" );
            }

            return service;
        }

        public IList<RegisteredService> GetWindowsAuthenticationServices()
        {
            var repos = RepositoryFactory.Instance.CreateRepository<RegisteredService>();
            IList<RegisteredService> list = repos.Find(
                Query.NewQuery.FindByCondition(
                    Specification.And(
                        Specification.Equal( "IsEnabled", true ),
                        Specification.Equal( "IsWindowsAuthentication", true )
                    )
                )
            );
            return list;
        }
    }
}