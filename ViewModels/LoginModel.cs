using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GovEventer.ViewModels
{
    public class LoginModel
    {
        [Display(Name = "E-Posta Adresi")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string EMailAddress { get; set; }
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        [Display(Name = "Beni Hatırla")]
        [DefaultValue(false)]
        public bool RememberMe { get; set; }
        [HiddenInput]
        public string ReturnUrl { get; set; }
    }
}