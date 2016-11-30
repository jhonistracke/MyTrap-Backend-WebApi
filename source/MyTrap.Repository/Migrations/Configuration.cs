namespace MyTrap.Repository.Migrations
{
    using Models;
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<MyTrap.Repository.EntitiesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EntitiesContext context)
        {
            context.Parameters.AddOrUpdate(
                p => p.Key,
                new Parameter { Id = Guid.NewGuid(), CreateDate = DateTime.UtcNow, Key = "AMOUNT_TRAP_NEW_REGISTER", Description = "Amount of traps that the new user will win", Value = "5" },
                new Parameter { Id = Guid.NewGuid(), CreateDate = DateTime.UtcNow, Key = "ID_TRAP_NEW_REGISTER", Description = "Id of trap that a new user will win", Value = "7662efa1-33d0-4bf7-9e2b-77e3a8002933" }
            );

            context.Traps.AddOrUpdate(
                p => p.NameKey,
                new Trap { Id = new Guid("7662efa1-33d0-4bf7-9e2b-77e3a8002933"), Active = true, CreateDate = DateTime.UtcNow, NameResource = "name_bear_trap", NameKey = "BEAR_TRAP", Points = 5 },
                new Trap { Id = new Guid("2dbac008-49ab-45a4-ba0a-c5509ddd8b05"), Active = true, CreateDate = DateTime.UtcNow, NameResource = "name_mine_trap", NameKey = "MINE_TRAP", Points = 10 },
                new Trap { Id = new Guid("dc7449d5-5db4-42be-8401-c604776fd26b"), Active = true, CreateDate = DateTime.UtcNow, NameResource = "name_pit_trap", NameKey = "PIT_TRAP", Points = 8 },
                new Trap { Id = new Guid("edbbff8f-7863-4cc1-bd83-f42b54539687"), Active = true, CreateDate = DateTime.UtcNow, NameResource = "name_dogs_trap", NameKey = "DOGS_TRAP", Points = 13 }
            );

            context.AvailableTraps.AddOrUpdate(
                p => p.NameKey,
                new AvailableTrap { Id = Guid.NewGuid(), Active = true, Amount = 10, KeyGoogle = "com.mytrap.product_10_beartrap", NameKey = "BEAR_TRAP", Value = 0, KeyApple = "1", KeyWindows = "com.mytrap.product_10_beartrap" },
                new AvailableTrap { Id = Guid.NewGuid(), Active = true, Amount = 10, KeyGoogle = "com.mytrap.product_10_minetrap", NameKey = "MINE_TRAP", Value = 0, KeyApple = "1", KeyWindows = "com.mytrap.product_10_minetrap" },
                new AvailableTrap { Id = Guid.NewGuid(), Active = true, Amount = 10, KeyGoogle = "com.mytrap.product_10_pittrap", NameKey = "PIT_TRAP", Value = 0, KeyApple = "1", KeyWindows = "com.mytrap.product_10_pittrap" },
                new AvailableTrap { Id = Guid.NewGuid(), Active = true, Amount = 10, KeyGoogle = "com.mytrap.product_10_dogtrap", NameKey = "DOGS_TRAP", Value = 0, KeyApple = "1", KeyWindows = "com.mytrap.product_10_dogtrap" }
            );
        }
    }
}