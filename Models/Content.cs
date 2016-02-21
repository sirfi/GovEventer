using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace GovEventer.Models
{
    public class Content : BaseRoutableEntity
    {
        [Display(Name = "Başlık")]
        public string Title { get; set; }
        [Display(Name = "Açıklama")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "İçerik")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Body { get; set; }
        [Display(Name = "Kategori")]
        [ForeignKey("CategoryId")]
        public ContentCategory Category { get; set; }
        [Display(Name = "Kategori")]
        public int? CategoryId { get; set; }
    }
}