using BackEndDotnetPlumsail.Data.Common;
using BackEndDotnetPlumsail.Data.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndDotnetPlumsail.Data.Repositories
{
    public class ElasticsearchService : IElasticSearchService
    {
        private readonly IConfiguration _configuration;
        private readonly IElasticClient _client;
        private readonly DietDbContext _context;
        private const string indexName = "all005";

        public ElasticsearchService(IConfiguration configuration, DietDbContext context)
        {
            _configuration = configuration;
            _client = CreateInstance();
            _context = context;
        }
        private ElasticClient CreateInstance()
        {
            string host = _configuration.GetSection("ElasticsearchServer:Host").Value;
            string port = _configuration.GetSection("ElasticsearchServer:Port").Value;
            string username = _configuration.GetSection("ElasticsearchServer:Username").Value;
            string password = _configuration.GetSection("ElasticsearchServer:Password").Value;
            var settings = new ConnectionSettings(new Uri(host + ":" + port));
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                settings.BasicAuthentication(username, password);

            return new ElasticClient(settings);
        }
        public async Task CheckIndex()
        {
            var anyy = await _client.Indices.ExistsAsync(indexName);
            if (anyy.Exists)
                return;
           
            var response = await _client.Indices.CreateAsync(indexName, ci => ci.Index(indexName).ObjectMapping().Settings(s => s.NumberOfShards(3).NumberOfReplicas(1)));

            return;
        }

        public async Task<List<dynamic>> SearchRations(string query)
        {
            await CheckIndex();
            ISearchResponse<dynamic> response;
            if (string.IsNullOrEmpty(query))
            {
                response = await _client.SearchAsync<dynamic>(s => s.Index(indexName).Size(10000).Query(q => q.MatchAll()));
            }
            else
            {
                response = await _client.SearchAsync<dynamic>(s => s.Index(indexName).Size(10000).Query(q => q.Match(m => m.Field("Search").Query(query))));
            }
            var pocoListWithIds = response.Hits.Select(h =>
            {
                return new
                {
                    id = h.Id,
                    Source = h.Source,
                };
            }).ToList<dynamic>();
            
            return pocoListWithIds;
        }

        public async Task InsertRation(dynamic ration)
        {
            await CheckIndex();
            string docJson = JsonConvert.SerializeObject(ration);
            var check = JsonConvert.DeserializeObject<ExpandoObject>(docJson);

            var response = _client.Index(check, q => q.Index(indexName));
            if (response.ApiCall?.HttpStatusCode == 409)
            {
                await _client.UpdateAsync<object>((object)ration.Id, a => a.Index(indexName).Doc(ration));
            }
        }

        public async Task<dynamic> GetRation(string id)
        {
            await CheckIndex();
            var response = await _client.GetAsync<dynamic>(id, q => q.Index(indexName));
            return response.Source;
        }        

        public async Task<byte[]> GetAppVersionByRationId(string id)
        {
            await CheckIndex();
            
            dynamic ration = await GetRation(id);
            string docJson = JsonConvert.SerializeObject(ration);
            var check = JsonConvert.DeserializeObject<ExpandoObject>(docJson);
            dynamic eod = check;
            var appVersionId = (int)eod.appVersionId;
            var res = await _context.AppVersions.Where(r => r.Id == appVersionId).FirstOrDefaultAsync();
            return res.File;
        }

    }
}
