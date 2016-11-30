using MyTrap.Repository.Models;
using System.Data.Entity;

namespace MyTrap.Repository
{
    public class EntitiesContext : DbContext
    {
        public DbSet<AvailableTrap> AvailableTraps { get; set; }
        public DbSet<BuyIntent> BuyIntents { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<Trap> Traps { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTrap> UserTraps { get; set; }
        public DbSet<ArmedTrap> ArmedTraps { get; set; }

        public EntitiesContext() : base("MyTrapConnectionString")
        {

        }
    }
}