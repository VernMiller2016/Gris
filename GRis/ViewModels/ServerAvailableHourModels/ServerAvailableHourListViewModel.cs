using GRis.ViewModels.Common;
using System.Collections.Generic;
using System.Linq;

namespace GRis.ViewModels.ServerAvailableHourModels
{
    public class ServerAvailableHourListViewModel
    {
        public ServerAvailableHourListViewModel()
        {
            Data = Enumerable.Empty<ServerAvailableHourDetailsViewModel>();
            UploadViewModel = new ServerAvailableHourUploadViewModel();
        }

        public ServerAvailableHourFilterViewModel Filters { get; set; }

        public IEnumerable<ServerAvailableHourDetailsViewModel> Data { get; set; }

        public ServerAvailableHourUploadViewModel UploadViewModel { get; set; }
    }
}