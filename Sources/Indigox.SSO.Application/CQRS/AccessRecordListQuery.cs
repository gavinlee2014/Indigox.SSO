using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.SSO.Application.DTO;
using Indigox.Common.DomainModels.Factory;
using Indigox.SSO.Logs;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.DomainModels.Interface.Specifications;

namespace Indigox.SSO.Application.CQRS
{
    public class AccessRecordListQuery : Indigox.Web.CQRS.GenericListQuery<AccessRecordDTO>
    {
        public string AccountName { get; set; }
        public string LogTimeBegin { get; set; }
        public string LogTimeEnd { get; set; }
        public string IP { get; set; }
        public string ServiceName { get; set; }
        public string ServiceID { get; set; }

        public override IList<AccessRecordDTO> List()
        {
            Query query = InitQuery();
            query.StartFrom(FirstResult).LimitTo(FetchSize).OrderByDesc("LogTime");
            IList<AccessRecord> list = RepositoryFactory.Instance.CreateRepository<AccessRecord>().Find(query);
            return AccessRecordDTO.ConvertToDTOs(list);
        }

        public override int Size()
        {
            Query query = InitQuery();
            return RepositoryFactory.Instance.CreateRepository<AccessRecord>().GetTotalCount(query);
        }

        public Query InitQuery()
        {
            Query query = new Query();
            List<ISpecification> conditions = new List<ISpecification>();

            if(!string.IsNullOrEmpty(AccountName)){
                conditions.Add(Specification.Like("AccountName", "%" + AccountName + "%"));
            }
            DateTime start,end;
            if (!string.IsNullOrEmpty(LogTimeBegin) && DateTime.TryParse(LogTimeBegin, out start))
            {
                conditions.Add(Specification.GreaterOrEqual("LogTime", start));
            }
            if (!string.IsNullOrEmpty(LogTimeEnd) && DateTime.TryParse(LogTimeEnd, out end))
            {
                conditions.Add(Specification.LessOrEqual("LogTime", end));
            }
            if (!string.IsNullOrEmpty(IP))
            {
                conditions.Add(Specification.Like("IP", "%"+IP+"%"));
            }
            if (!string.IsNullOrEmpty(ServiceName))
            {
                conditions.Add(Specification.Like("ServiceName", "%" + ServiceName + "%"));
            }
            if (!string.IsNullOrEmpty(ServiceID))
            {
                conditions.Add(Specification.Like("ServiceID", "%" + ServiceID + "%"));
            }
            query.Specifications = Specification.And(conditions.ToArray());
            return query;
        }
    }
}
