using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gris.Application.Core.Contracts.Reports
{
    public class ServerSalaryReportViewModel
    {
        public DateTime TRXDATE { get; set; }

        public int JRNENTRY { get; set; }

        public string ACTNUMST { get; set; }

        // public string AccountDescription { get; set; }

        public string GpEmpNumber { get; set; }

        public string ServerName { get; set; }

        //public decimal CreditAmount { get; set; }

        //public decimal DebitAmount { get; set; }

        //public decimal AccountValue
        //{
        //    get
        //    {
        //        return DebitAmount != 0 ? DebitAmount : CreditAmount;
        //    }
        //}

        public string ORGNTSRC { get; set; }

        //
        public SalaryAccount SalaryAccount { get; set; }
        public TempHelpAccount TempHelpAccount { get; set; }

        public OverTimeAccount OverTimeAccount { get; set; }

        public RetirementAccount RetirementAccount { get; set; }

        public SocialSecurityAccount SocialSecurityAccount { get; set; }

        public MedicalAndLifeInsuranceAccount MedicalAndLifeInsuranceAccount { get; set; }

        public IndustrialInsuranceAccount IndustrialInsuranceAccount { get; set; }

        public double Total { get; set; }
        //
    }

    public class SalaryAccount
    {

        public decimal Value
        {
            get
            {
                return DebitAmount != 0 ? DebitAmount : CreditAmount;
            }
        }

        public decimal CreditAmount { get; set; }

        public decimal DebitAmount { get; set; }
    }

    public class TempHelpAccount
    {

        public decimal Value
        {
            get
            {
                return DebitAmount != 0 ? DebitAmount : CreditAmount;
            }
        }

        public decimal CreditAmount { get; set; }

        public decimal DebitAmount { get; set; }
    }


    public class OverTimeAccount
    {

        public decimal Value
        {
            get
            {
                return DebitAmount != 0 ? DebitAmount : CreditAmount;
            }
        }

            public decimal CreditAmount { get; set; }

        public decimal DebitAmount { get; set; }
    }

    public class RetirementAccount
    {

        public decimal Value
        {
            get
            {
                return DebitAmount != 0 ? DebitAmount : CreditAmount;
            }
        }
            public decimal CreditAmount
        {
            get; set;
        }

            public decimal DebitAmount { get; set; }
    }

    public class SocialSecurityAccount
    {

        public decimal Value
        {
            get
            {
                return DebitAmount != 0 ? DebitAmount : CreditAmount;
            }
        }

        public decimal CreditAmount { get; set; }

        public decimal DebitAmount { get; set; }
    }

    public class MedicalAndLifeInsuranceAccount
    {

        public decimal Value
        {
            get
            {
                return DebitAmount != 0 ? DebitAmount : CreditAmount;
            }
        }

        public decimal CreditAmount { get; set; }

        public decimal DebitAmount { get; set; }
    }

    public class IndustrialInsuranceAccount
    {

        public decimal Value
        {
            get
            {
                return DebitAmount != 0 ? DebitAmount : CreditAmount;
            }
        }

        public decimal CreditAmount { get; set; }

        public decimal DebitAmount { get; set; }
    }
}
