namespace Gris.Infrastructure.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnGpEmpNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servers", "GpEmpNumber", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Servers", "GpEmpNumber");
        }
    }
}
