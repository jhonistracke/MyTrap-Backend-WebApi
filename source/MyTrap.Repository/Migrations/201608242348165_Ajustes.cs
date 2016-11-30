namespace MyTrap.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ajustes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserTraps", "Amount", c => c.Int(nullable: false));
            AddColumn("dbo.AvailableTraps", "Amount", c => c.Int(nullable: false));
            DropColumn("dbo.UserTraps", "Quantity");
            DropColumn("dbo.AvailableTraps", "Quantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AvailableTraps", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.UserTraps", "Quantity", c => c.Int(nullable: false));
            DropColumn("dbo.AvailableTraps", "Amount");
            DropColumn("dbo.UserTraps", "Amount");
        }
    }
}
