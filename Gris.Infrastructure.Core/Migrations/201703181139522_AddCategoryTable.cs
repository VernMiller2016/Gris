namespace Gris.Infrastructure.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCategoryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Servers", "CategoryId", c => c.Int());
            CreateIndex("dbo.Servers", "CategoryId");
            AddForeignKey("dbo.Servers", "CategoryId", "dbo.Categories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Servers", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Servers", new[] { "CategoryId" });
            DropColumn("dbo.Servers", "CategoryId");
            DropTable("dbo.Categories");
        }
    }
}
