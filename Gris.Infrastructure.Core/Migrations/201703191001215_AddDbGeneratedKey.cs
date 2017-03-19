namespace Gris.Infrastructure.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDbGeneratedKey : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.PaySources");
            DropPrimaryKey("dbo.PlaceOfServices");
            DropPrimaryKey("dbo.Servers");
            DropPrimaryKey("dbo.Services");
            AddColumn("dbo.PaySources", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.PlaceOfServices", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Servers", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Services", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.PaySources", "Id");
            AddPrimaryKey("dbo.PlaceOfServices", "Id");
            AddPrimaryKey("dbo.Servers", "Id");
            AddPrimaryKey("dbo.Services", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Services");
            DropPrimaryKey("dbo.Servers");
            DropPrimaryKey("dbo.PlaceOfServices");
            DropPrimaryKey("dbo.PaySources");
            DropColumn("dbo.Services", "Id");
            DropColumn("dbo.Servers", "Id");
            DropColumn("dbo.PlaceOfServices", "Id");
            DropColumn("dbo.PaySources", "Id");
            AddPrimaryKey("dbo.Services", "ServiceId");
            AddPrimaryKey("dbo.Servers", "ServerId");
            AddPrimaryKey("dbo.PlaceOfServices", "PlaceOfServiceId");
            AddPrimaryKey("dbo.PaySources", "PaySourceId");
        }
    }
}
