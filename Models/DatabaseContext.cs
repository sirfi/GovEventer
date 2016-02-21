using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GovEventer.Properties;

namespace GovEventer.Models
{
    public class DatabaseContext : System.Data.Entity.DbContext
    {
        public DatabaseContext()
            : base(Properties.Settings.Default.DefaultConnectionString)
        {

        }

        public IDbSet<ContentCategory> ContentCategories { get; set; }
        public IDbSet<Content> Contents { get; set; }
        public IDbSet<EventCategory> EventCategories { get; set; }
        public IDbSet<Event> Events { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<Setting> Settings { get; set; }
    }
}