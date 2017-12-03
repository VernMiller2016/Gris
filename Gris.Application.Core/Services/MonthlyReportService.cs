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
        private IServerTimeEntryService _serverTimeEntry;
        private IServerRepository _serverRepository;
        private IUnitOfWork _unitOfWork;

        public ServerMonthlyReportService(IServerSalaryReportRepository serverMonthlyReportRepoitory,IServerTimeEntryService serverTimeEntry, IServerRepository serverRepository, IUnitOfWork unitOfWork)
        {
            _serverMonthlyReportRepository = serverMonthlyReportRepoitory;
            _serverTimeEntry = serverTimeEntry;
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
                            if (addedSalaryServerEntity.SalaryAccount == null)
                            {
                                addedSalaryServerEntity.SalaryAccount = new SalaryAccount
                                {
                                    CreditAmount = itemValue.CRDTAMNT,
                                    DebitAmount = itemValue.DEBITAMT
                                };
                            }
                            else
                            {
                                addedSalaryServerEntity.SalaryAccount.CreditAmount += itemValue.CRDTAMNT;
                                addedSalaryServerEntity.SalaryAccount.DebitAmount += itemValue.DEBITAMT;
                            }
                        }
                        else if (itemValue.ACTDESCR.ToLower() == Constants.Retirement.ToLower())
                        {
                            if (addedSalaryServerEntity.RetirementAccount == null)
                            {
                                addedSalaryServerEntity.RetirementAccount = new RetirementAccount
                                {
                                    CreditAmount = itemValue.CRDTAMNT,
                                    DebitAmount = itemValue.DEBITAMT
                                };
                            }
                            else
                            {
                                addedSalaryServerEntity.RetirementAccount.CreditAmount += itemValue.CRDTAMNT;
                                addedSalaryServerEntity.RetirementAccount.DebitAmount += itemValue.DEBITAMT;
                            }
                        }
                        else if (itemValue.ACTDESCR.ToLower() == Constants.SocialSecurity.ToLower())
                        {
                            if (addedSalaryServerEntity.SocialSecurityAccount == null)
                            {
                                addedSalaryServerEntity.SocialSecurityAccount = new SocialSecurityAccount
                                {
                                    CreditAmount = itemValue.CRDTAMNT,
                                    DebitAmount = itemValue.DEBITAMT
                                };
                            }
                            else
                            {
                                addedSalaryServerEntity.SocialSecurityAccount.CreditAmount += itemValue.CRDTAMNT;
                                addedSalaryServerEntity.SocialSecurityAccount.DebitAmount += itemValue.DEBITAMT;
                            }
                        }
                        else if (itemValue.ACTDESCR.ToLower() == Constants.IndustrialInsurance.ToLower())
                        {
                            if (addedSalaryServerEntity.IndustrialInsuranceAccount == null)
                            {
                                addedSalaryServerEntity.IndustrialInsuranceAccount = new IndustrialInsuranceAccount
                                {
                                    CreditAmount = itemValue.CRDTAMNT,
                                    DebitAmount = itemValue.DEBITAMT
                                };
                            }
                            else
                            {
                                addedSalaryServerEntity.IndustrialInsuranceAccount.CreditAmount += itemValue.CRDTAMNT;
                                addedSalaryServerEntity.IndustrialInsuranceAccount.DebitAmount += itemValue.DEBITAMT;
                            }
                        }
                        else if (itemValue.ACTDESCR.ToLower() == Constants.MentalHealth.ToLower())
                        {
                            if (addedSalaryServerEntity.MedicalAndLifeInsuranceAccount == null)
                            {
                                addedSalaryServerEntity.MedicalAndLifeInsuranceAccount = new MedicalAndLifeInsuranceAccount
                                {
                                    CreditAmount = itemValue.CRDTAMNT,
                                    DebitAmount = itemValue.DEBITAMT
                                };
                            }
                            else
                            {
                                addedSalaryServerEntity.MedicalAndLifeInsuranceAccount.CreditAmount += itemValue.CRDTAMNT;
                                addedSalaryServerEntity.MedicalAndLifeInsuranceAccount.DebitAmount += itemValue.DEBITAMT;
                            }
                        }
                        else if (itemValue.ACTDESCR.ToLower() == Constants.OVERTIME.ToLower())
                        {
                            if (addedSalaryServerEntity.OverTimeAccount == null)
                            {
                                addedSalaryServerEntity.OverTimeAccount = new OverTimeAccount
                                {
                                    CreditAmount = itemValue.CRDTAMNT,
                                    DebitAmount = itemValue.DEBITAMT
                                };
                            }
                            else
                            {
                                addedSalaryServerEntity.OverTimeAccount.CreditAmount += itemValue.CRDTAMNT;
                                addedSalaryServerEntity.OverTimeAccount.DebitAmount += itemValue.DEBITAMT;
                            }
                        }
                        else if(itemValue.ACTDESCR.ToLower() == Constants.MedicalAndLifeInsurance.ToLower())
                        {
                            if(addedSalaryServerEntity.MedicalAndLifeInsuranceAccount == null)
                            {
                                addedSalaryServerEntity.MedicalAndLifeInsuranceAccount = new MedicalAndLifeInsuranceAccount
                                {
                                    CreditAmount = itemValue.CRDTAMNT,
                                    DebitAmount = itemValue.DEBITAMT
                                };
                            }
                            else
                            {
                                addedSalaryServerEntity.MedicalAndLifeInsuranceAccount.CreditAmount += itemValue.CRDTAMNT;
                                addedSalaryServerEntity.MedicalAndLifeInsuranceAccount.DebitAmount += itemValue.DEBITAMT;
                            }
                        }

                        else if (itemValue.ACTDESCR.ToLower() == Constants.TempHelp.ToLower())
                        {
                            if (addedSalaryServerEntity.TempHelpAccount == null)
                            {
                                addedSalaryServerEntity.TempHelpAccount = new TempHelpAccount
                                {
                                    CreditAmount = itemValue.CRDTAMNT,
                                    DebitAmount = itemValue.DEBITAMT
                                };
                            }
                            else
                            {
                                addedSalaryServerEntity.TempHelpAccount.CreditAmount += itemValue.CRDTAMNT;
                                addedSalaryServerEntity.TempHelpAccount.DebitAmount += itemValue.DEBITAMT;
                            }
                        }
                    }
                    addedSalaryServerEntity.Total = addedSalaryServerEntity.SalaryAccount != null ? (double)addedSalaryServerEntity.SalaryAccount.Value : 0;
                    addedSalaryServerEntity.Total += addedSalaryServerEntity.TempHelpAccount != null ? (double)addedSalaryServerEntity.TempHelpAccount.Value : 0;
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

        public IEnumerable<ServerSalaryReportViewModel> GetServerSalaryMonthlyPercentageReport(DateTime selectedDate, PagingInfo pagingInfo = null)
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
                var serverTimeEntries = _serverTimeEntry.GetServerTimeEntriesMonthlyReport(selectedDate);
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
                if (allServers != null)
                {
                    foreach (var server in serverTimeEntries)
                    {
                        var localServer = allServers.FirstOrDefault(s => s.Id == server.ServerId);
                        if (localServer != null && localServer.GpEmpNumber!=null)
                        {
                            server.ServerId = int.Parse(localServer.GpEmpNumber);
                        }
                        else
                        {

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
                        var serverTimeEntryList = serverTimeEntries.Where(t => t.ServerId.ToString() == itemValue.ORMSTRID);
                        if (serverTimeEntryList != null)
                        {
                            if (addedSalaryServerEntity.Programs == null || !addedSalaryServerEntity.Programs.Any())
                            {
                                addedSalaryServerEntity.Programs = new List<ServerTimeEntriesMonthlyReportEntity>();
                            }
                            foreach (var serverTimeEntry in serverTimeEntryList)
                            {
                                if (!addedSalaryServerEntity.Programs.Any(p => p.ProgramId == serverTimeEntry.ProgramId && p.ServerId == serverTimeEntry.ServerId))
                                {
                                    addedSalaryServerEntity.Programs.Add(new ServerTimeEntriesMonthlyReportEntity
                                    {
                                        ProgramId = serverTimeEntry.ProgramId,
                                        ProgramName = serverTimeEntry.ProgramName,
                                        ServerId = serverTimeEntry.ServerId,
                                        Duration = serverTimeEntry.Duration
                                    });
                                }
                            }
                           
                        }
                        if (string.IsNullOrWhiteSpace(addedSalaryServerEntity.ServerName))
                            addedSalaryServerEntity.ServerName = itemValue.ORMSTRNM;
                        if (addedSalaryServerEntity.JRNENTRY == 0)
                            addedSalaryServerEntity.JRNENTRY = itemValue.JRNENTRY;
                        if (string.IsNullOrWhiteSpace(addedSalaryServerEntity.ORGNTSRC))
                            addedSalaryServerEntity.ORGNTSRC = itemValue.ORGNTSRC;
                        if (itemValue.ACTDESCR.ToLower() == Constants.Salaries.ToLower())
                        {
                            if (addedSalaryServerEntity.SalaryAccount == null)
                            {
                                addedSalaryServerEntity.SalaryAccount = new SalaryAccount
                                {
                                    CreditAmount = itemValue.CRDTAMNT,
                                    DebitAmount = itemValue.DEBITAMT
                                };
                            }
                            else
                            {
                                addedSalaryServerEntity.SalaryAccount.CreditAmount += itemValue.CRDTAMNT;
                                addedSalaryServerEntity.SalaryAccount.DebitAmount += itemValue.DEBITAMT;
                            }
                        }
                        else if (itemValue.ACTDESCR.ToLower() == Constants.Retirement.ToLower())
                        {
                            if (addedSalaryServerEntity.RetirementAccount == null)
                            {
                                addedSalaryServerEntity.RetirementAccount = new RetirementAccount
                                {
                                    CreditAmount = itemValue.CRDTAMNT,
                                    DebitAmount = itemValue.DEBITAMT
                                };
                            }
                            else
                            {
                                addedSalaryServerEntity.RetirementAccount.CreditAmount += itemValue.CRDTAMNT;
                                addedSalaryServerEntity.RetirementAccount.DebitAmount += itemValue.DEBITAMT;
                            }
                        }
                        else if (itemValue.ACTDESCR.ToLower() == Constants.SocialSecurity.ToLower())
                        {
                            if (addedSalaryServerEntity.SocialSecurityAccount == null)
                            {
                                addedSalaryServerEntity.SocialSecurityAccount = new SocialSecurityAccount
                                {
                                    CreditAmount = itemValue.CRDTAMNT,
                                    DebitAmount = itemValue.DEBITAMT
                                };
                            }
                            else
                            {
                                addedSalaryServerEntity.SocialSecurityAccount.CreditAmount += itemValue.CRDTAMNT;
                                addedSalaryServerEntity.SocialSecurityAccount.DebitAmount += itemValue.DEBITAMT;
                            }
                        }
                        else if (itemValue.ACTDESCR.ToLower() == Constants.IndustrialInsurance.ToLower())
                        {
                            if (addedSalaryServerEntity.IndustrialInsuranceAccount == null)
                            {
                                addedSalaryServerEntity.IndustrialInsuranceAccount = new IndustrialInsuranceAccount
                                {
                                    CreditAmount = itemValue.CRDTAMNT,
                                    DebitAmount = itemValue.DEBITAMT
                                };
                            }
                            else
                            {
                                addedSalaryServerEntity.IndustrialInsuranceAccount.CreditAmount += itemValue.CRDTAMNT;
                                addedSalaryServerEntity.IndustrialInsuranceAccount.DebitAmount += itemValue.DEBITAMT;
                            }
                        }
                        else if (itemValue.ACTDESCR.ToLower() == Constants.MentalHealth.ToLower())
                        {
                            if (addedSalaryServerEntity.MedicalAndLifeInsuranceAccount == null)
                            {
                                addedSalaryServerEntity.MedicalAndLifeInsuranceAccount = new MedicalAndLifeInsuranceAccount
                                {
                                    CreditAmount = itemValue.CRDTAMNT,
                                    DebitAmount = itemValue.DEBITAMT
                                };
                            }
                            else
                            {
                                addedSalaryServerEntity.MedicalAndLifeInsuranceAccount.CreditAmount += itemValue.CRDTAMNT;
                                addedSalaryServerEntity.MedicalAndLifeInsuranceAccount.DebitAmount += itemValue.DEBITAMT;
                            }
                        }
                        else if (itemValue.ACTDESCR.ToLower() == Constants.OVERTIME.ToLower())
                        {
                            if (addedSalaryServerEntity.OverTimeAccount == null)
                            {
                                addedSalaryServerEntity.OverTimeAccount = new OverTimeAccount
                                {
                                    CreditAmount = itemValue.CRDTAMNT,
                                    DebitAmount = itemValue.DEBITAMT
                                };
                            }
                            else
                            {
                                addedSalaryServerEntity.OverTimeAccount.CreditAmount += itemValue.CRDTAMNT;
                                addedSalaryServerEntity.OverTimeAccount.DebitAmount += itemValue.DEBITAMT;
                            }
                        }
                        else if (itemValue.ACTDESCR.ToLower() == Constants.MedicalAndLifeInsurance.ToLower())
                        {
                            if (addedSalaryServerEntity.MedicalAndLifeInsuranceAccount == null)
                            {
                                addedSalaryServerEntity.MedicalAndLifeInsuranceAccount = new MedicalAndLifeInsuranceAccount
                                {
                                    CreditAmount = itemValue.CRDTAMNT,
                                    DebitAmount = itemValue.DEBITAMT
                                };
                            }
                            else
                            {
                                addedSalaryServerEntity.MedicalAndLifeInsuranceAccount.CreditAmount += itemValue.CRDTAMNT;
                                addedSalaryServerEntity.MedicalAndLifeInsuranceAccount.DebitAmount += itemValue.DEBITAMT;
                            }
                        }
                        else if (itemValue.ACTDESCR.ToLower() == Constants.TempHelp.ToLower())
                        {
                            if (addedSalaryServerEntity.TempHelpAccount == null)
                            {
                                addedSalaryServerEntity.TempHelpAccount = new TempHelpAccount
                                {
                                    CreditAmount = itemValue.CRDTAMNT,
                                    DebitAmount = itemValue.DEBITAMT
                                };
                            }
                            else
                            {
                                addedSalaryServerEntity.TempHelpAccount.CreditAmount += itemValue.CRDTAMNT;
                                addedSalaryServerEntity.TempHelpAccount.DebitAmount += itemValue.DEBITAMT;
                            }
                        }
                    }
                    addedSalaryServerEntity.Total = addedSalaryServerEntity.SalaryAccount != null ? (double)addedSalaryServerEntity.SalaryAccount.Value : 0;
                    addedSalaryServerEntity.Total += addedSalaryServerEntity.TempHelpAccount != null ? (double)addedSalaryServerEntity.TempHelpAccount.Value : 0;
                    addedSalaryServerEntity.Total += addedSalaryServerEntity.OverTimeAccount != null ? (double)addedSalaryServerEntity.OverTimeAccount.Value : 0;
                    addedSalaryServerEntity.Total += addedSalaryServerEntity.RetirementAccount != null ? (double)addedSalaryServerEntity.RetirementAccount.Value : 0;
                    addedSalaryServerEntity.Total += addedSalaryServerEntity.SocialSecurityAccount != null ? (double)addedSalaryServerEntity.SocialSecurityAccount.Value : 0;
                    addedSalaryServerEntity.Total += addedSalaryServerEntity.MedicalAndLifeInsuranceAccount != null ? (double)addedSalaryServerEntity.MedicalAndLifeInsuranceAccount.Value : 0;
                    addedSalaryServerEntity.Total += addedSalaryServerEntity.IndustrialInsuranceAccount != null ? (double)addedSalaryServerEntity.IndustrialInsuranceAccount.Value : 0;

                    #region Percentage
                    if(addedSalaryServerEntity.SalaryAccount != null)
                    {
                        if(addedSalaryServerEntity.SalaryAccount.DebitAmount != 0)
                        {
                            addedSalaryServerEntity.SalaryAccount.ValueInPercentage = (addedSalaryServerEntity.SalaryAccount.DebitAmount /
                                                                                (decimal)addedSalaryServerEntity.Total) * 100;
                        }
                        else
                        {
                            addedSalaryServerEntity.SalaryAccount.ValueInPercentage = (addedSalaryServerEntity.SalaryAccount.CreditAmount /
                                                                               (decimal)addedSalaryServerEntity.Total) * 100;
                        }
                    }
                    if (addedSalaryServerEntity.SocialSecurityAccount != null)
                    {
                        if (addedSalaryServerEntity.SocialSecurityAccount.DebitAmount != 0)
                        {
                            addedSalaryServerEntity.SocialSecurityAccount.ValueInPercentage = (addedSalaryServerEntity.SocialSecurityAccount.DebitAmount /
                                                                                (decimal)addedSalaryServerEntity.Total) * 100;
                        }
                        else
                        {
                            addedSalaryServerEntity.SocialSecurityAccount.ValueInPercentage = (addedSalaryServerEntity.SocialSecurityAccount.CreditAmount /
                                                                               (decimal)addedSalaryServerEntity.Total) * 100;
                        }
                    }
                    if (addedSalaryServerEntity.RetirementAccount != null)
                    {
                        if (addedSalaryServerEntity.RetirementAccount.DebitAmount != 0)
                        {
                            addedSalaryServerEntity.RetirementAccount.ValueInPercentage = (addedSalaryServerEntity.RetirementAccount.DebitAmount /
                                                                                (decimal)addedSalaryServerEntity.Total) * 100;
                        }
                        else
                        {
                            addedSalaryServerEntity.RetirementAccount.ValueInPercentage = (addedSalaryServerEntity.RetirementAccount.CreditAmount /
                                                                               (decimal)addedSalaryServerEntity.Total) * 100;
                        }
                    }
                    if (addedSalaryServerEntity.IndustrialInsuranceAccount != null)
                    {
                        if (addedSalaryServerEntity.IndustrialInsuranceAccount.DebitAmount != 0)
                        {
                            addedSalaryServerEntity.IndustrialInsuranceAccount.ValueInPercentage = (addedSalaryServerEntity.IndustrialInsuranceAccount.DebitAmount /
                                                                                (decimal)addedSalaryServerEntity.Total) * 100;
                        }
                        else
                        {
                            addedSalaryServerEntity.IndustrialInsuranceAccount.ValueInPercentage = (addedSalaryServerEntity.IndustrialInsuranceAccount.CreditAmount /
                                                                               (decimal)addedSalaryServerEntity.Total) * 100;
                        }
                    }
                    if (addedSalaryServerEntity.MedicalAndLifeInsuranceAccount != null)
                    {
                        if (addedSalaryServerEntity.MedicalAndLifeInsuranceAccount.DebitAmount != 0)
                        {
                            addedSalaryServerEntity.MedicalAndLifeInsuranceAccount.ValueInPercentage = (addedSalaryServerEntity.MedicalAndLifeInsuranceAccount.DebitAmount /
                                                                                (decimal)addedSalaryServerEntity.Total) * 100;
                        }
                        else
                        {
                            addedSalaryServerEntity.MedicalAndLifeInsuranceAccount.ValueInPercentage = (addedSalaryServerEntity.MedicalAndLifeInsuranceAccount.CreditAmount /
                                                                               (decimal)addedSalaryServerEntity.Total) * 100;
                        }
                    }
                    if (addedSalaryServerEntity.OverTimeAccount != null)
                    {
                        if (addedSalaryServerEntity.OverTimeAccount.DebitAmount != 0)
                        {
                            addedSalaryServerEntity.OverTimeAccount.ValueInPercentage = (addedSalaryServerEntity.OverTimeAccount.DebitAmount /
                                                                                (decimal)addedSalaryServerEntity.Total) * 100;
                        }
                        else
                        {
                            addedSalaryServerEntity.OverTimeAccount.ValueInPercentage = (addedSalaryServerEntity.OverTimeAccount.CreditAmount /
                                                                               (decimal)addedSalaryServerEntity.Total) * 100;
                        }
                    }
                    if (addedSalaryServerEntity.TempHelpAccount != null)
                    {
                        if (addedSalaryServerEntity.TempHelpAccount.DebitAmount != 0)
                        {
                            addedSalaryServerEntity.TempHelpAccount.ValueInPercentage = (addedSalaryServerEntity.TempHelpAccount.DebitAmount /
                                                                                (decimal)addedSalaryServerEntity.Total) * 100;
                        }
                        else
                        {
                            addedSalaryServerEntity.TempHelpAccount.ValueInPercentage = (addedSalaryServerEntity.TempHelpAccount.CreditAmount /
                                                                               (decimal)addedSalaryServerEntity.Total) * 100;
                        }
                    }
                    #endregion
                    addedSalaryServerEntity.TotalInPercentage = 100;
                    mappedServerEntities.Add(addedSalaryServerEntity);
                }
            }
            return mappedServerEntities;
        }
    }
}