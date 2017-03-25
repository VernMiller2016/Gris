namespace Gris.Infrastructure.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddServerTimeEntryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServerTimeEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServerId = c.Int(nullable: false),
                        PaySourceId = c.Int(nullable: false),
                        Duration = c.Time(nullable: false, precision: 7),
                        BeginDate = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaySources", t => t.PaySourceId, cascadeDelete: true)
                .ForeignKey("dbo.Servers", t => t.ServerId, cascadeDelete: true)
                .Index(t => t.ServerId)
                .Index(t => t.PaySourceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServerTimeEntries", "ServerId", "dbo.Servers");
            DropForeignKey("dbo.ServerTimeEntries", "PaySourceId", "dbo.PaySources");
            DropIndex("dbo.ServerTimeEntries", new[] { "PaySourceId" });
            DropIndex("dbo.ServerTimeEntries", new[] { "ServerId" });
            DropTable("dbo.ServerTimeEntries");
        }
    }
}
