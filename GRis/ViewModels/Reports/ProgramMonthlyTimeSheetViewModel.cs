using Gris.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GRis.ViewModels.Reports
{
    public class ProgramMonthlyTimeSheetViewModel
    {
        public int SelectedMonthId { get; set; }
        public SelectList Months { get {
                return new SelectList(new List<int>() { 1, 2 ,3,4,5,6,7,8,9,10,11,12});
            } }

        public int SelectedYearId { get; set; }

        public SelectList Years
        {
            get
            {
                return new SelectList(new List<int>() { 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027 });
            }
        }

        public List<ProgramWithServersAndPaySourcesViewModel> ProgramsWithServersAndPaySourcesViewModel { get; set; }
    }
    public class ProgramWithServersAndPaySourcesViewModel
    {
        public string ProgramName { get; set; }

        public int ProgramId { get; set; }

        public List<Gris.Domain.Core.Models.ServerTimeEntry> ServerTimeEntry { get; set; }
    }
}