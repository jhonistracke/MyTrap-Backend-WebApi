using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTrap.Repository.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(250), Required]
        public string Name { get; set; }

        [StringLength(250), Required]
        public string Email { get; set; }

        [Required]
        public int RegisterType { get; set; }

        [StringLength(250)]
        public string RegisterProfileId { get; set; }

        [StringLength(250)]
        public string AppRegistration { get; set; }

        [Required]
        public int PlatformId { get; set; }

        [StringLength(10), Required]
        public string Language { get; set; }

        [StringLength(50), Required]
        public string TimeZone { get; set; }

        public int Points { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public Image ProfilePicture { get; set; }
        public List<UserTrap> Traps { get; set; }
    }
}