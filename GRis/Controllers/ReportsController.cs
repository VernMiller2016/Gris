using Gris.Application.Core.Interfaces;
using GRis.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GRis.Controllers
{
    public class ReportsController : Controller
    {
        IProgramService _programService;
        IServerTimeEntryService _serverTimeEntryService;
        IPaySourceService _paySourceService;
        public ReportsController(IProgramService programService, IServerTimeEntryService serverTimeEntryService,IPaySourceService paySourceService)
        {
            _programService = programService;
            _serverTimeEntryService = serverTimeEntryService;
            _paySourceService = paySourceService;
        }

        // GET: Reports
        public ActionResult Index()
        {
            var model = new ProgramMonthlyTimeSheetViewModel
            {   
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Search(ProgramMonthlyTimeSheetViewModel model)
        {
            if (model == null)
            {
                return View("Index");
            }
            else
            {
                DateTime selectedMonthAndYear = new DateTime(model.SelectedYearId, model.SelectedMonthId, 1);
                var serverTimeEntries = _serverTimeEntryService.GetServerTimeEntriesByMonthAndYear(selectedMonthAndYear);
                if (model.ProgramsWithServersAndPaySourcesViewModel == null)
                {
                    model.ProgramsWithServersAndPaySourcesViewModel = new List<ProgramWithServersAndPaySourcesViewModel>();
                }
                if(model.ProgramsWithServersAndPaySourcesViewModel == null)
                {
                    model.ProgramsWithServersAndPaySourcesViewModel = new List<ProgramWithServersAndPaySourcesViewModel>();
                }
                foreach (var serverTime in serverTimeEntries.GroupBy(key => key.PaySource.ProgramId))
                {
                    var program = _programService.GetById(serverTime.Key.Value);
                    var programWithServersAndPaySource = new ProgramWithServersAndPaySourcesViewModel
                    {
                        ProgramId = serverTime.Key.Value,
                        ProgramName = program.Name,
                    };
                    programWithServersAndPaySource.ServerTimeEntry = new List<Gris.Domain.Core.Models.ServerTimeEntry>();
                    foreach (var item in serverTime)
                    {
                        programWithServersAndPaySource.ServerTimeEntry.Add(item);
                    }
                    model.ProgramsWithServersAndPaySourcesViewModel.Add(programWithServersAndPaySource);

                }
                return View("Index",model);
            }
        }
    }
}