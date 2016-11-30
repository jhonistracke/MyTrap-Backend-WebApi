namespace MyTrap.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AvailableTraps",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        NameKey = c.String(nullable: false, maxLength: 250),
                        KeyGoogle = c.String(nullable: false, maxLength: 250),
                        KeyApple = c.String(nullable: false, maxLength: 250),
                        Value = c.Double(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BuyIntents",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        DateIntent = c.DateTime(nullable: false),
                        DateResult = c.DateTime(),
                        StoreKey = c.String(nullable: false, maxLength: 250),
                        Realized = c.Boolean(nullable: false),
                        Processed = c.Boolean(nullable: false),
                        AvailableTrap_Id = c.Guid(),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AvailableTraps", t => t.AvailableTrap_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.AvailableTrap_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Email = c.String(nullable: false, maxLength: 250),
                        RegisterType = c.Int(nullable: false),
                        RegisterProfileId = c.String(maxLength: 250),
                        AppRegistration = c.String(maxLength: 250),
                        PlatformId = c.Int(nullable: false),
                        Language = c.String(nullable: false, maxLength: 10),
                        TimeZone = c.String(nullable: false, maxLength: 50),
                        Points = c.Int(nullable: false),
                        Token = c.String(maxLength: 250),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                        LastPositionDate = c.DateTime(),
                        ProfilePicture_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Images", t => t.ProfilePicture_Id)
                .Index(t => t.ProfilePicture_Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Url = c.String(nullable: false, maxLength: 250),
                        OriginUrl = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserTraps",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Trap_Id = c.Guid(),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Traps", t => t.Trap_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Trap_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Traps",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NameKey = c.String(nullable: false, maxLength: 250),
                        NameResource = c.String(nullable: false, maxLength: 250),
                        Points = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Parameters",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Key = c.String(nullable: false, maxLength: 250),
                        Value = c.String(nullable: false, maxLength: 250),
                        Description = c.String(nullable: false, maxLength: 250),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BuyIntents", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserTraps", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserTraps", "Trap_Id", "dbo.Traps");
            DropForeignKey("dbo.Users", "ProfilePicture_Id", "dbo.Images");
            DropForeignKey("dbo.BuyIntents", "AvailableTrap_Id", "dbo.AvailableTraps");
            DropIndex("dbo.UserTraps", new[] { "User_Id" });
            DropIndex("dbo.UserTraps", new[] { "Trap_Id" });
            DropIndex("dbo.Users", new[] { "ProfilePicture_Id" });
            DropIndex("dbo.BuyIntents", new[] { "User_Id" });
            DropIndex("dbo.BuyIntents", new[] { "AvailableTrap_Id" });
            DropTable("dbo.Parameters");
            DropTable("dbo.Traps");
            DropTable("dbo.UserTraps");
            DropTable("dbo.Images");
            DropTable("dbo.Users");
            DropTable("dbo.BuyIntents");
            DropTable("dbo.AvailableTraps");
        }
    }
}
