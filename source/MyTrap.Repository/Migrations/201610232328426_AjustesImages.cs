namespace MyTrap.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjustesImages : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Images", "Url", c => c.String(nullable: false));
            AlterColumn("dbo.Images", "OriginUrl", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Images", "OriginUrl", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Images", "Url", c => c.String(nullable: false, maxLength: 250));
        }
    }
}
