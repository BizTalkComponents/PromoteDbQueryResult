using BizTalkComponents.Utilities.DbQueryUtility;
using BizTalkComponents.Utilities.DbQueryUtility.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizTalkComponents.PipelineComponents.PromoteDbQueryResult.PromoteDbQueryResult
{
    public class DbQueryHelper
    {
        private IDbQueryRepository _dbQueryRepository = null;
        private DbQueryUtilityService _utility = null;

        public DbQueryHelper(IDbQueryRepository dbQueryRepository)
        {
            if(dbQueryRepository == null)
            {
                throw new ArgumentNullException("dbQueryRepository");
            }

            _dbQueryRepository = dbQueryRepository;
            _utility = new DbQueryUtilityService(_dbQueryRepository);
        }

        public string Query(string query, string configKey)
        {
            var d = _utility.Query(query, configKey);

            return d.SelectSingleNode("/Result/*[1]").Value;
        }
    }
}
