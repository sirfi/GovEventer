using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GovEventer.Models
{
    public class User
    {
        [Key]
        [Display(Name = "Kimlik")]
        public int Id { get; set; }
        [Index(IsUnique = true)]
        [Display(Name = "E-Posta Adresi")]
        [DataType(DataType.EmailAddress)]
        public string EMailAddress { get; set; }
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string Password { get; set; }
        [Display(Name = "Şifre Özütü")]
        public string PasswordHash { get; set; }
        [Required]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; }
        [Display(Name = "Yönetici Mi")]
        public bool IsAdmin { get; set; }
        [Display(Name = "Oluşturulma Zamanı")]
        public DateTime? CreateDateTime { get; set; }
        [Display(Name = "Son Değişiklik Zamanı")]
        public DateTime? LastModifyDateTime { get; set; }
    }
}