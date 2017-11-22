using Gris.Application.Core.Contracts.Paging;
using Gris.Application.Core.Contracts.Reports;
using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gris.Application.Core.Services
{
    public class ServerMonthlyReportService : IServerSalaryReportService
    {
        private IServerSalaryReportRepository _serverMonthlyReportRepository;
        private IServerRepository _serverRepository;
        private IUnitOfWork _unitOfWork;

        public ServerMonthlyReportService(IServerSalaryReportRepository serverMonthlyReportRepoitory, IServerRepository serverRepository, IUnitOfWork unitOfWork)
        {
            _serverMonthlyReportRepository = serverMonthlyReportRepoitory;
            _serverRepository = serverRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ServerSalaryReportViewModel> GetServerSalaryMonthlyReport(DateTime selectedDate, PagingInfo pagingInfo = null)
        {
            var result = Enumerable.Empty<ServerSalaryReportEntity>();
            var mappedServerEntities = new List<ServerSalaryReportViewModel>();
            selectedDate = selectedDate.AddMonths(1);
            if (pagingInfo == null)
            {
                result = _serverMonthlyReportRepository.GetServerSalaryDataByMonth(selectedDate);
            }
            else
            {
                result = _serverMonthlyReportRepository.GetServerSalaryDataByMonth(selectedDate);
            }
            if (result.Any())
            {
                var resultsFilteredByGpEmpNumber = result.GroupBy(s => s.ORMSTRID);
                List<ServerSalaryReportEntity> filteredResults = new List<ServerSalaryReportEntity>();
                var allServers = _serverRepository.GetAll();
                foreach (var item in resultsFilteredByGpEmpNumber)
                {
                    if (allServers.FirstOrDefault(s => s.GpEmpNumber == item.Key) != null)
                    {
                        foreach (var itemValue in item)
                        {
                            filteredResults.Add(itemValue);
                        }
                    }
                }
                var filteredResultsGroupedByGpEmpNumber = filteredResults.GroupBy(s => new { s.ORMSTRID });

                foreach (var item in filteredResultsGroupedByGpEmpNumber)
                {
                    var addedSalaryServerEntity = new ServerSalaryReportViewModel
                    {
                        GpEmpNumber = item.Key.ORMSTRID
                    };
                    foreach (var itemValue in item)
                    {
                        if (string.IsNullOrWhiteSpace(addedSalaryServerEntity.ServerName))
                            addedSalaryServerEntity.ServerName = itemValue.ORMSTRNM;
                        if (addedSalaryServerEntity.JRNENTRY == 0)
                            addedSalaryServerEntity.JRNENTRY = itemValue.JRNENTRY;
                        if (string.IsNullOrWhiteSpace(addedSalaryServerEntity.ORGNTSRC))
                            addedSalaryServerEntity.ORGNTSRC = itemValue.ORGNTSRC;
                        if (itemValue.ACTDESCR.ToLower() == Constants.Salaries.ToLower())
                        {
                            addedSalaryServerEntity.SalaryAccount = new SalaryAccount
                            {
                                CreditAmount = itemValue.CRDTAMNT,
                                DebitAmount = itemValue.DEBITAMT
                            };
                        }
                        else if (itemValue.ACTDESCR.ToLower() == Constants.Retirement.ToLower())
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
                    addedSalaryServerEntity.Total = addedSalaryServerEntity.SalaryAccount != null ? (double)addedSalaryServerEntity.SalaryAccount.Value : 0;
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