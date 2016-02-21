using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using GovEventer.Models;

namespace GovEventer.Core
{
    public static class SettingFactory
    {
        public static Dictionary<SettingKey, string> Dictionary
        {
            get { return DatabaseFactory.Instance.Settings.ToDictionary(x => x.Key, x => x.Value); }
        }

        public static string Get(this SettingKey key)
        {
            return
                DatabaseFactory.Instance.Settings
                    .Where(x => x.Key == key)
                    .Select(x => x.Value)
                    .FirstOrDefault();
        }

        public static void Set(this SettingKey key, string value)
        {
            var setting = DatabaseFactory.Instance.Settings.FirstOrDefault(x => x.Key == key);
            if (setting == null)
            {
                setting = new Setting() { Key = key, Value = value };
            }
            else
            {
                setting.Value = value;
            }
            DatabaseFactory.Instance.Settings.AddOrUpdate(setting);
            DatabaseFactory.Instance.SaveChanges();
        }
    }
}