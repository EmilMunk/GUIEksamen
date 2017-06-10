using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebAPI.Models;

namespace WebAPI.DAL
{
    public class KalorieIndholdDbContext : DbContext
    {
        public KalorieIndholdDbContext() : base("DefaultConnection")
        {
            
        }

        public DbSet<KalorieIndhold> Kalorier { get; set; }
        public DbSet<KalorieIndtag> Indtag { get; set; }

        public DbSet<Indtag> IndtastIndtag { get; set; }

    }
}