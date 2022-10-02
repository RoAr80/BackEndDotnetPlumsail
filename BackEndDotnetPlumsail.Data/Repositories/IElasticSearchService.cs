using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace BackEndDotnetPlumsail.Data.Repositories
{
    public interface IElasticSearchService
    {
        Task CheckIndex();
        Task<List<object>> SearchRations(string query);
        Task InsertRation(dynamic ration);
        Task<dynamic> GetRation(string id);
        Task<byte[]> GetAppVersionByRationId(string id);
    }
}
