namespace Gris.Infrastructure.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameToProgram : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Programs", "Name", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Programs", "ProgramId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Programs", "ProgramId", c => c.Int(nullable: false));
            DropColumn("dbo.Programs", "Name");
        }
    }
}
