using System;
using System.ComponentModel.DataAnnotations;

namespace MyTrap.Repository.Models
{
    public class Trap
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(250), Required]
        public string NameKey { get; set; }

        [StringLength(250), Required]
        public string NameResource { get; set; }

        public int Points { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}