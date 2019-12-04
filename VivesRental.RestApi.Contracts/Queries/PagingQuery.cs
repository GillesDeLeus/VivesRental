namespace VivesRental.RestApi.Contracts.Queries
{
    public class PagingQuery
    {
        public PagingQuery()
        {
            PageNumber = 1;
            PageSize = 100;
        }

        public PagingQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize > 100 ? 100 : pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
