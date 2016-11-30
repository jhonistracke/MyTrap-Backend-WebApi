using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTrap.Repository.Models
{
    public class AvailableTrap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int Amount { get; set; }

        [StringLength(250), Required]
        public string NameKey { get; set; }

        [StringLength(250), Required]
        public string KeyGoogle { get; set; }

        [StringLength(250), Required]
        public string KeyApple { get; set; }

        [StringLength(250)]
        public string KeyWindows { get; set; }

        public double Value { get; set; }
        public bool Active { get; set; }
    }
}