namespace Gris.Infrastructure.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateVendorKeyColumnName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaySources", "VendorId", c => c.Int(nullable: false));
            AddColumn("dbo.PlaceOfServices", "VendorId", c => c.Int(nullable: false));
            AddColumn("dbo.Servers", "VendorId", c => c.Int(nullable: false));
            AddColumn("dbo.Services", "VendorId", c => c.Int(nullable: false));
            DropColumn("dbo.PaySources", "PaySourceId");
            DropColumn("dbo.PlaceOfServices", "PlaceOfServiceId");
            DropColumn("dbo.Servers", "ServerId");
            DropColumn("dbo.Services", "ServiceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "ServiceId", c => c.Int(nullable: false));
            AddColumn("dbo.Servers", "ServerId", c => c.Int(nullable: false));
            AddColumn("dbo.PlaceOfServices", "PlaceOfServiceId", c => c.Int(nullable: false));
            AddColumn("dbo.PaySources", "PaySourceId", c => c.Int(nullable: false));
            DropColumn("dbo.Services", "VendorId");
            DropColumn("dbo.Servers", "VendorId");
            DropColumn("dbo.PlaceOfServices", "VendorId");
            DropColumn("dbo.PaySources", "VendorId");
        }
    }
}
