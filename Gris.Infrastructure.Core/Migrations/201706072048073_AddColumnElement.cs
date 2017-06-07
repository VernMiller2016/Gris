namespace Gris.Infrastructure.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnElement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servers", "Element", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Servers", "Element");
        }
    }
}
