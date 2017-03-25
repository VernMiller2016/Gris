namespace Gris.Infrastructure.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddIdColumnToProgram : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaySources", "ProgramId", "dbo.Programs");
            DropPrimaryKey("dbo.Programs");
            // remove the previous identity first, then add the new identity column. So, drop it then recreates it.
            //AlterColumn("dbo.Programs", "ProgramId", c => c.Int(nullable: false));
            DropColumn("dbo.Programs", "ProgramId");
            AddColumn("dbo.Programs", "ProgramId", c => c.Int(nullable: false));
            AddColumn("dbo.Programs", "Id", c => c.Int(nullable: false, identity: true));

            AddPrimaryKey("dbo.Programs", "Id");
            AddForeignKey("dbo.PaySources", "ProgramId", "dbo.Programs", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.PaySources", "ProgramId", "dbo.Programs");
            DropPrimaryKey("dbo.Programs");
            // remove the previous identity first, then add the new identity column.
            DropColumn("dbo.Programs", "Id");
            AlterColumn("dbo.Programs", "ProgramId", c => c.Int(nullable: false, identity: true));
            
            AddPrimaryKey("dbo.Programs", "ProgramId");
            AddForeignKey("dbo.PaySources", "ProgramId", "dbo.Programs", "Id");
        }
    }
}
