namespace Gris.Infrastructure.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddElementTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Elements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisplayName = c.String(nullable: false),
                        VendorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Servers", "ElementId", c => c.Int());
            CreateIndex("dbo.Servers", "ElementId");
            AddForeignKey("dbo.Servers", "ElementId", "dbo.Elements", "Id");
            DropColumn("dbo.Servers", "Element");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Servers", "Element", c => c.Int(nullable: false));
            DropForeignKey("dbo.Servers", "ElementId", "dbo.Elements");
            DropIndex("dbo.Servers", new[] { "ElementId" });
            DropColumn("dbo.Servers", "ElementId");
            DropTable("dbo.Elements");
        }
    }
}
