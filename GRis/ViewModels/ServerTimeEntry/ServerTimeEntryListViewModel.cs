using System.Collections.Generic;

namespace GRis.ViewModels.ServerTimeEntry
{
    public class ServerTimeEntryListViewModel
    {
        public ServerTimeEntryListViewModel()
        {
            Filters = new ServerTimeEntryFilterViewModel();
            TimeEntries = new List<Gris.Domain.Core.Models.ServerTimeEntry>();
        }

        public ServerTimeEntryFilterViewModel Filters { get; set; }

        public IEnumerable<Gris.Domain.Core.Models.ServerTimeEntry> TimeEntries { get; set; }
    }
}