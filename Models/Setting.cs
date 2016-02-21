using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GovEventer.Models
{
    public class Setting
    {
        [Key]
        [Display(Name = "Kimlik")]
        public int OID { get; set; }
        [Display(Name = "Anahtar")]
        [Index(IsUnique = true)]
        public SettingKey Key { get; set; }
        [Display(Name = "Değer")]
        public string Value { get; set; }
    }

    public enum SettingKey
    {
        [Display(Name = "Site Adı")]
        SiteName,
        [Display(Name = "Site Açıklama")]
        SiteDescription,
        [Display(Name = "Site Telefon No")]
        SiteTelephoneNumber,
        [Display(Name = "Site E-Posta Adresi")]
        SiteEMail
    }
}