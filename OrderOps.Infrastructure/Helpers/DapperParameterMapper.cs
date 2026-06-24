using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyBlueWhale.EngineQuery.Abstractions.Models;

namespace OrderOps.Infrastructure.Helpers
{
    public static class DapperParameterMapper
    {
        public static DynamicParameters ToDynamicParameters(this IReadOnlyList<QuerySqlParameter> parameters)
        {
            var dynamicParameters = new DynamicParameters();

            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Name, parameter.Value);
            }

            return dynamicParameters;
        }
    }
}
