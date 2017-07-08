using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gris.Domain.Core.Models
{
   public class ServerSalaryReportEntity
    {

        [Key]
        public string ORMSTRID { get; set; }
        public DateTime TRXDATE { get; set; }

        public int JRNENTRY { get; set; }

        public string ACTNUMST { get; set; }

        public string ACTDESCR { get; set; }

        public string ORMSTRNM { get; set; }

        public decimal CRDTAMNT { get; set; }

        public decimal DEBITAMT { get; set; }

        public string ORGNTSRC { get; set; }

    }
}
