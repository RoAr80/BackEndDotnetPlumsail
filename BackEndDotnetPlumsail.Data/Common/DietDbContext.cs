using System;
using System.Collections.Generic;
using System.Text;
using BackEndDotnetPlumsail.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndDotnetPlumsail.Data.Common
{
    public class DietDbContext : DbContext
    {
        public DietDbContext(DbContextOptions<DietDbContext> options) : base(options)
        {

        }

        public DbSet<Ration> Rations { get; set; }
        public DbSet<AppVersion> AppVersions { get; set; }

    }
}
