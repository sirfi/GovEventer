using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GovEventer.Models
{
    public abstract class BaseEntity
    {
        [Key]
        [Display(Name = "Kimlik")]
        public int Id { get; set; }
        [Display(Name = "Oluşturan Kullanıcı")]
        public int? CreatorUserId { get; set; }
        [Display(Name = "Oluşturulma Zamanı")]
        public DateTime? CreateDateTime { get; set; }
        [Display(Name = "Son Değiştiren Kullanıcı")]
        public int? LastModifierByUserId { get; set; }
        [Display(Name = "Son Değiştirilme Zamanı")]
        public DateTime? LastModifyDateTime { get; set; }
        [ForeignKey("CreatorUserId")]
        [Display(Name = "Oluşturan Kullanıcı")]
        public virtual User CreatorUser { get; set; }
        [ForeignKey("LastModifierByUserId")]
        [Display(Name = "Son Değiştiren Kullanıcı")]
        public virtual User LastModifierByUser { get; set; }
        [Display(Name = "Aktif Mi")]
        [DefaultValue(false)]
        public bool IsActive { get; set; }
    }
}