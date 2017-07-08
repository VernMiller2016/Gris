using Gris.Application.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gris.Application.Core.Contracts.Paging;
using Gris.Application.Core.Contracts.Reports;
using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.Interfaces;
using AutoMapper;

namespace Gris.Application.Core.Services
{
    public class ServerMonthlyReportService : IServerSalaryReportService
    {
        

        private IServerSalaryReportRepository _serverMonthlyReportRepository;
        private IServerRepository _serverRepository;
        private IUnitOfWork _unitOfWork;

        public ServerMonthlyReportService(IServerSalaryReportRepository serverMonthlyReportRepoitory,IServerRepository serverRepository, IUnitOfWork unitOfWork)
        {
            _serverMonthlyReportRepository = serverMonthlyReportRepoitory;
            _serverRepository = serverRepository;
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<ServerSalaryReportViewModel> GetServerSalaryMonthlyReport(DateTime selectedDate, PagingInfo pagingInfo = null)
        {
            var result = Enumerable.Empty<ServerSalaryReportEntity>();
            var mappedServerEntities = new List<ServerSalaryReportViewModel>();
            if (pagingInfo == null)
            {
                //result = _serverMonthlyReportRepository.
                //            Get(t => t.TRXDATE.Year == selectedDate.Year && t.TRXDATE.Month == selectedDate.Month
                //            , (list => list.OrderBy(st => st.ORMSTRNM)));
                result = _serverMonthlyReportRepository.GetServerSalaryDataByMonth(selectedDate);
            }
            else
            {
                //int total = 0;
                //result = _serverMonthlyReportRepository.
                //            FilterWithPaging(t => t.TRXDATE.Year == selectedDate.Year && t.TRXDATE.Month == selectedDate.Month
                //            , (list => list.OrderBy(st => st.ORMSTRNM))
                //            , out total, pagingInfo.PageIndex, AppSettings.PageSize);
                //pagingInfo.Total = total;
                result = _serverMonthlyReportRepository.GetServerSalaryDataByMonth(selectedDate);
            }
            if(result.Any())
            {

                var resultsFilteredByGpEmpNumber = result.GroupBy(s => s.ORMSTRID);
                List<ServerSalaryReportEntity> filteredResults = new List<ServerSalaryReportEntity>();
                var allServers = _serverRepository.GetAll();
                foreach (var item in resultsFilteredByGpEmpNumber)
                {
                    if(allServers.FirstOrDefault(s => s.GpEmpNumber == item.Key) != null)
                    {
                        foreach (var itemValue in item)
                        {
                            filteredResults.Add(itemValue);
                        }
                    }
                }
                var filteredResultsGroupedByGpEmpNumber = filteredResults.GroupBy(s =>new { s.ORMSTRID,s.ORMSTRNM,s.JRNENTRY,s.ORGNTSRC });

                foreach (var item in filteredResultsGroupedByGpEmpNumber)
                {
                    var addedSalaryServerEntity = new ServerSalaryReportViewModel
                    {
                        // ACTNUMST = itemValue.ACTNUMST,
                        GpEmpNumber = item.Key.ORMSTRID,
                        ServerName = item.Key.ORMSTRNM,
                        JRNENTRY = item.Key.JRNENTRY,
                        ORGNTSRC = item.Key.ORGNTSRC,
                        //TRXDATE = itemValue.TRXDATE
                    };
                    foreach (var itemValue in item)
                    {
                       
                        if(itemValue.ACTDESCR.ToLower() == Constants.Salaries.ToLower())
                        {
                            addedSalaryServerEntity.SalaryAccount = new SalaryAccount
                            {
                                CreditAmount = itemValue.CRDTAMNT,
                                DebitAmount = itemValue.DEBITAMT
                            };
                        }
                        else if(itemValue.ACTDESCR.ToLower() == Constants.Retirement.ToLower())
                        {
                            addedSalaryServerEntity.RetirementAccount = new RetirementAccount
                            {
                                CreditAmount = itemValue.CRDTAMNT,
                                DebitAmount = itemValue.DEBITAMT
                            };
                        }
                        else if (itemValue.ACTDESCR.ToLower() == Constants.SocialSecurity.ToLower())
                        {
                            addedSalaryServerEntity.SocialSecurityAccount = new SocialSecurityAccount
                            {
                                CreditAmount = itemValue.CRDTAMNT,
                                DebitAmount = itemValue.DEBITAMT
                            };
                        }
                        else if (itemValue.ACTDESCR.ToLower() == Constants.IndustrialInsurance.ToLower())
                        {
                            addedSalaryServerEntity.IndustrialInsuranceAccount = new IndustrialInsuranceAccount
                            {
                                CreditAmount = itemValue.CRDTAMNT,
                                DebitAmount = itemValue.DEBITAMT
                            };
                        }
                        else if (itemValue.ACTDESCR.ToLower() == Constants.MentalHealth.ToLower())
                        {
                            addedSalaryServerEntity.MedicalAndLifeInsuranceAccount = new MedicalAndLifeInsuranceAccount
                            {
                                CreditAmount = itemValue.CRDTAMNT,
                                DebitAmount = itemValue.DEBITAMT
                            };
                        }
                        
                    }
                    addedSalaryServerEntity.Total = addedSalaryServerEntity.SalaryAccount != null ? (double)addedSalaryServerEntity.SalaryAccount.Value:0;
                    addedSalaryServerEntity.Total += addedSalaryServerEntity.TempHelpAccount != null ? (double)addedSalaryServerEntity.SalaryAccount.Value : 0;
                    addedSalaryServerEntity.Total += addedSalaryServerEntity.OverTimeAccount != null ? (double)addedSalaryServerEntity.OverTimeAccount.Value : 0;
                    addedSalaryServerEntity.Total += addedSalaryServerEntity.RetirementAccount != null ? (double)addedSalaryServerEntity.RetirementAccount.Value : 0;
                    addedSalaryServerEntity.Total += addedSalaryServerEntity.SocialSecurityAccount != null ? (double)addedSalaryServerEntity.SocialSecurityAccount.Value : 0;
                    addedSalaryServerEntity.Total += addedSalaryServerEntity.MedicalAndLifeInsuranceAccount != null ? (double)addedSalaryServerEntity.MedicalAndLifeInsuranceAccount.Value : 0;
                    addedSalaryServerEntity.Total += addedSalaryServerEntity.IndustrialInsuranceAccount != null ? (double)addedSalaryServerEntity.IndustrialInsuranceAccount.Value : 0;

                    mappedServerEntities.Add(addedSalaryServerEntity);

                }
               
            }
            return mappedServerEntities;
        }
    }
}
