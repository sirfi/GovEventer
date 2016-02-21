using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GovEventer.Models
{
    public abstract class BaseRoutableEntity : BaseEntity
    {
        [Display(Name = "Adres Adı")]
        [Index(IsUnique = true)]
        public string Slug { get; set; }
    }
}