using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GovEventer.Models
{
    public class ContentCategory : BaseRoutableEntity
    {
        [Display(Name = "Ad")]
        public string Name { get; set; }
    }
}