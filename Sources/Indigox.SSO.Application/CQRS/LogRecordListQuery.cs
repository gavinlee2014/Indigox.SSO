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
    public class LogRecordListQuery : Indigox.Web.CQRS.GenericListQuery<LogRecordDTO>
    {
        public string AccountName { get; set; }
        public string LogTimeBegin { get; set; }
        public string LogTimeEnd { get; set; }
        public string IP { get; set; }

        public override IList<LogRecordDTO> List()
        {
            Query query = InitQuery();
            query.StartFrom(FirstResult).LimitTo(FetchSize).OrderByDesc("LogTime");
            IList<LogRecord> list = RepositoryFactory.Instance.CreateRepository<LogRecord>().Find(query);
            return LogRecordDTO.ConvertToDTOs(list);
        }

        public override int Size()
        {
            Query query = InitQuery();
            return RepositoryFactory.Instance.CreateRepository<LogRecord>().GetTotalCount(query);
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
            query.Specifications = Specification.And(conditions.ToArray());
            return query;
        }
    }
}
