namespace MyTrap.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProdutoWIndows : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AvailableTraps", "KeyWindows", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AvailableTraps", "KeyWindows");
        }
    }
}
