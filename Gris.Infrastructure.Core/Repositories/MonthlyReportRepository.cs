using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gris.Infrastructure.Core.DAL;
using System.Data.Entity;

namespace Gris.Infrastructure.Core.Repositories
{
    public class ServerSalaryReportRepository : EFRepository<ServerSalaryReportEntity>, IServerSalaryReportRepository
    {
        public ServerSalaryReportRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<ServerSalaryReportEntity> GetServerSalaryDataByMonth(DateTime date)
        {
            var endDate = date.AddDays(DateTime.DaysInMonth(date.Year, date.Month) - 1); //DateTime.DaysInMonth(date.Year, date.Month)
            //var query = (from server in this._dbContext.Servers
            //             join report in this._dbSet
            //             on server.GpEmpNumber equals report.ORMSTRID
            //             //where report.TRXDATE.Year == date.Year && report.TRXDATE.Month == date.Month
            //             //where DbFunctions.TruncateTime(report.TRXDATE) >= DbFunctions.TruncateTime(date)
            //             //&& DbFunctions.TruncateTime(report.TRXDATE) <= DbFunctions.TruncateTime(endDate)
            //             select report);
            
            return _dbContext.Database.SqlQuery<ServerSalaryReportEntity>("GetServerSalaryReportData {0},{1}", date, endDate).ToList();
        }
    }
}
