using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GovEventer.Models;

namespace GovEventer.Core
{
    public static class DatabaseFactory
    {
        private static DatabaseContext _instance;

        public static DatabaseContext Instance
        {
            get
            {
                {
                    return _instance ?? (_instance = new DatabaseContext());
                }
            }
        }
    }
}