using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GovEventer.Models
{
    public class Event : BaseRoutableEntity
    {
        [Display(Name = "Ad")]
        public string Name { get; set; }
        [Display(Name = "Kısa Açıklama")]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }
        [Display(Name = "Açıklama")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }
        [Display(Name = "Tarih")]
        public DateTime EventDate { get; set; }
        [Display(Name = "Yer")]
        public string Location { get; set; }
        [Display(Name = "Yer Koordinatları")]
        public string LocationCoordinates { get; set; }
        [Display(Name = "Kategori")]
        [ForeignKey("CategoryId")]
        public EventCategory Category { get; set; }
        [Display(Name = "Kategori")]
        public int? CategoryId { get; set; }
    }
}