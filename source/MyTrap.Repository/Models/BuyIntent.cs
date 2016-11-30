using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTrap.Repository.Models
{
    public class BuyIntent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime DateIntent { get; set; }
        public DateTime? DateResult { get; set; }

        public User User { get; set; }
        public AvailableTrap AvailableTrap { get; set; }

        [StringLength(250), Required]
        public string StoreKey { get; set; }

        public bool Realized { get; set; }
        public bool Processed { get; set; }
    }
}