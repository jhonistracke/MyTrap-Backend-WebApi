namespace MyTrap.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArmedTraps : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArmedTraps",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        NameKey = c.String(nullable: false, maxLength: 250),
                        Date = c.DateTime(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Disarmed = c.Boolean(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArmedTraps", "User_Id", "dbo.Users");
            DropIndex("dbo.ArmedTraps", new[] { "User_Id" });
            DropTable("dbo.ArmedTraps");
        }
    }
}
