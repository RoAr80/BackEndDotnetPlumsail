using BackEndDotnetPlumsail.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BackEndDotnetPlumsail.Data.Repositories
{
    public interface IRationRepository
    {
        Task<AppVersion> GetCurrentAppVersion();
        Task<byte[]> GetCurrentAppVersionFile();
        Task<byte[]> GetAppVersion(int id);
        Task<byte[]> GetAppVersionByRationId(int id);
        Task<int> AddRation(Ration ration);
        Task<List<Ration>> SearchRations(string query);
        Task<Ration> GetRation(int ration);
    }
}
