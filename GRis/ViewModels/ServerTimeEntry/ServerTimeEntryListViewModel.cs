using System.Linq;
using X.PagedList;

namespace GRis.ViewModels.ServerTimeEntry
{
    public class ServerTimeEntryListViewModel
    {
        public ServerTimeEntryListViewModel()
        {
            Filters = new ServerTimeEntryFilterViewModel();
            TimeEntries = Enumerable.Empty<ServerTimeEntryDetailsViewModel>() as IPagedList<ServerTimeEntryDetailsViewModel>; //new IEnumerable<Gris.Domain.Core.Models.ServerTimeEntry>();
        }

        public ServerTimeEntryFilterViewModel Filters { get; set; }

        public IPagedList<ServerTimeEntryDetailsViewModel> TimeEntries { get; set; }
    }
}