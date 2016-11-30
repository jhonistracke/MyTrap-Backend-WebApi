using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTrap.Repository.Models
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(int.MaxValue), Required]
        public string Url { get; set; }

        [StringLength(int.MaxValue), Required]
        public string OriginUrl { get; set; }
    }
}