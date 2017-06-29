namespace Gris.Infrastructure.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePaysourceWithMultiProgram : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaySources", "ProgramId", "dbo.Programs");
            DropIndex("dbo.PaySources", new[] { "ProgramId" });
            CreateTable(
                "dbo.ProgramPaySources",
                c => new
                    {
                        Program_Id = c.Int(nullable: false),
                        PaySource_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Program_Id, t.PaySource_Id })
                .ForeignKey("dbo.Programs", t => t.Program_Id, cascadeDelete: true)
                .ForeignKey("dbo.PaySources", t => t.PaySource_Id, cascadeDelete: true)
                .Index(t => t.Program_Id)
                .Index(t => t.PaySource_Id);
            
            AddColumn("dbo.ServerTimeEntries", "ProgramId", c => c.Int());
            CreateIndex("dbo.ServerTimeEntries", "ProgramId");
            AddForeignKey("dbo.ServerTimeEntries", "ProgramId", "dbo.Programs", "Id");
            DropColumn("dbo.PaySources", "ProgramId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaySources", "ProgramId", c => c.Int());
            DropForeignKey("dbo.ServerTimeEntries", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ProgramPaySources", "PaySource_Id", "dbo.PaySources");
            DropForeignKey("dbo.ProgramPaySources", "Program_Id", "dbo.Programs");
            DropIndex("dbo.ProgramPaySources", new[] { "PaySource_Id" });
            DropIndex("dbo.ProgramPaySources", new[] { "Program_Id" });
            DropIndex("dbo.ServerTimeEntries", new[] { "ProgramId" });
            DropColumn("dbo.ServerTimeEntries", "ProgramId");
            DropTable("dbo.ProgramPaySources");
            CreateIndex("dbo.PaySources", "ProgramId");
            AddForeignKey("dbo.PaySources", "ProgramId", "dbo.Programs", "Id");
        }
    }
}
