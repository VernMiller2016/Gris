namespace Gris.Infrastructure.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddServerAvailableHoursTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServerAvailableHours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServerId = c.Int(nullable: false),
                        DateRange = c.DateTime(nullable: false),
                        AvailableHours = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Servers", t => t.ServerId, cascadeDelete: true)
                .Index(t => t.ServerId)
                .Index(t => t.DateRange, name: "DateRangeIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServerAvailableHours", "ServerId", "dbo.Servers");
            DropIndex("dbo.ServerAvailableHours", "DateRangeIndex");
            DropIndex("dbo.ServerAvailableHours", new[] { "ServerId" });
            DropTable("dbo.ServerAvailableHours");
        }
    }
}
