using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTrap.Repository.Models
{
    public class UserTrap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Trap Trap { get; set; }
        public int Amount { get; set; }
    }
}