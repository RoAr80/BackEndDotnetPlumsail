using BackEndDotnetPlumsail.Data.Common;
using BackEndDotnetPlumsail.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndDotnetPlumsail.Data.Repositories
{
    public class RationRepository : IRationRepository
    {
        private readonly DietDbContext _context;
        public RationRepository(DietDbContext context)
        {
            _context = context;
        }

        public async Task<AppVersion> GetCurrentAppVersion()
        {

            string currDir = Directory.GetCurrentDirectory();
            string folder = Path.GetDirectoryName(currDir) + @"\BackEndDotnetPlumsail\wwwroot\";
            string filter = "*.html";
            string[] files = Directory.GetFiles(folder, filter);
            string fileNameWithoutExt = default;
            if(files.Length >= 1)
            {
                fileNameWithoutExt = Path.GetFileNameWithoutExtension(files[0]);
                string fileName = Path.GetFileName(files[0]);
                var appVersion = await _context.AppVersions.Where(a => a.Hash == fileNameWithoutExt).FirstOrDefaultAsync();
                var appReturnVersion = new AppVersion();
                if (appVersion == default)
                {
                    var fileBytes = File.ReadAllBytes(folder + fileName);
                    var app = new AppVersion()
                    {
                        Hash = fileNameWithoutExt,
                        File = fileBytes,
                    };
                    await _context.AppVersions.AddAsync(app);
                    await _context.SaveChangesAsync();
                    appReturnVersion.Hash = app.Hash;
                    appReturnVersion.Id = app.Id;
                    appReturnVersion.File = app.File;
                }
                else
                {
                    appReturnVersion.Hash = appVersion.Hash;
                    appReturnVersion.Id = appVersion.Id;
                    appReturnVersion.File = appVersion.File;
                }
                return appReturnVersion;
            }
            return new AppVersion();
        }

        public async Task<byte[]> GetCurrentAppVersionFile()
        {
            var res = await GetCurrentAppVersion();
            
            return res.File;
        }

        public async Task<byte[]> GetAppVersion(int id)
        {
            var res = await _context.AppVersions.Where(app => app.Id == id).FirstOrDefaultAsync();
            return res.File;
        }
        public async Task<byte[]> GetAppVersionByRationId(int id)
        {
            var res = await _context.Rations.Include(o => o.AppVersion).Where(r => r.Id == id).FirstOrDefaultAsync();
            if(res == null)
            {
                return null;
            }
            return res.AppVersion.File;
        }


        public async Task<int> AddRation(Ration ration)
        {
            //var res = await _context.AppVersions.Where(app => app.Hash == ration.Jash).FirstOrDefaultAsync();
            await _context.Rations.AddAsync(ration);
            await _context.SaveChangesAsync();
            return ration.Id;
        }

        public async Task<List<Ration>> SearchRations(string query)
        {
            var res = new List<Ration>();
            if (string.IsNullOrEmpty(query))
            {
                res = await _context.Rations.ToListAsync();
            }
            else
            {
                res = await _context.Rations.Where(ration => ration.Keywords.Contains(query)).ToListAsync();
            }
            return res;
        }

        public async Task<Ration> GetRation(int id)
        {
            var res = await _context.Rations.Where(ration => ration.Id == id).FirstOrDefaultAsync();
            return res;
        }
    }
}
