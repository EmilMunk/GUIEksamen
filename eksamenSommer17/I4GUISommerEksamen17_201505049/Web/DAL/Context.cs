using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.DAL
{
    public class Context :DbContext
    {
        public Context() : base("DefaultConnection")
        {
            
        }

        public DbSet<BackLog> BackLogs { get; set; }
    }
}