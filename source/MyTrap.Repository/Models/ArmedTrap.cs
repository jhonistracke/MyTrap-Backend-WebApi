using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTrap.Repository.Models
{
    public class ArmedTrap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(250), Required]
        public string NameKey { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public bool Disarmed { get; set; }
    }
}