namespace MyTrap.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserCache : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "Token");
            DropColumn("dbo.Users", "LastPositionDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "LastPositionDate", c => c.DateTime());
            AddColumn("dbo.Users", "Token", c => c.String(maxLength: 250));
        }
    }
}
