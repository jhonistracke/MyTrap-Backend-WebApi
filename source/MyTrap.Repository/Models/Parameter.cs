using System;
using System.ComponentModel.DataAnnotations;

namespace MyTrap.Repository.Models
{
    public class Parameter
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(250), Required]
        public string Key { get; set; }

        [StringLength(250), Required]
        public string Value { get; set; }

        [StringLength(250), Required]
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}