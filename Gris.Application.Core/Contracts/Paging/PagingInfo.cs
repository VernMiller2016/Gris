namespace Gris.Application.Core.Contracts.Paging
{
    public class PagingInfo
    {
        public int PageIndex { get { return PageNumber - 1; } }

        public int PageNumber { get; set; }

        public int Total { get; set; }

        public string SearchOption { get; set; }

        public string SearchValue { get; set; }
    }
}