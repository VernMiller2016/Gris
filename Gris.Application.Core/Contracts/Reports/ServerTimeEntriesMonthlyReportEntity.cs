using System;

namespace Gris.Application.Core.Contracts.Reports
{
    public class ServerTimeEntriesMonthlyReportEntity
    {
        public string ServerName { get; set; }

        public int ServerVendorId { get; set; }

        public DateTime BeginDate { get; set; }

        public TimeSpan Duration { get; set; }

        public int PaysourceVendorId { get; set; }

        public string PaysourceDescription { get; set; }

        public string ProgramName { get; set; }

        public int ProgramId { get; set; }
    }
}