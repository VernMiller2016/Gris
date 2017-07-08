namespace Gris.Infrastructure.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SalaryReportView : DbMigration
    {
        public override void Up()
        {
            Sql(@"EXEC ('/****** Script for SelectTopNRows command from SSMS  ******/


              CREATE PROCEDURE GetServerSalaryReportData
	            @StartDate DATETIME,
	            @EndDate DATETIME
            AS
            BEGIN
	        -- SET NOCOUNT ON added to prevent extra result sets from
	        -- interfering with SELECT statements.
	        SET NOCOUNT ON;

            SELECT
            TRXDATE,
            JRNENTRY,
            ACTNUMST,
            ACTDESCR,
            ORMSTRID,
            ORMSTRNM,
            CRDTAMNT,
            DEBITAMT,
            ORGNTSRC
            FROM GCTEST.dbo.slbAccountTrx join GCTEST.dbo.GL00105 
             ON slbAccountTrx.actindx = GL00105.actindx

            where GL00105.ACTNUMST BETWEEN '108.150.00.0000.560001100' 
              AND '108.150.00.0000.560002599'
              AND slbAccountTrx.TRXDATE BETWEEN @StartDate AND @EndDate
            ORDER BY ACTNUMST, JRNENTRY, ORMSTRNM
            
            END
            GO
            
            ')");
        }

        public override void Down()
        {
            Sql(@"  DROP PROCEDURE GetServerSalaryReportData;");
        }
    }
}
