namespace Ecom.API.Helper
{
    public class Pagination<T> where T : class
    {
        public Pagination(int pageNumber, int pageSize, int totalCount, IEnumerable<T> data)
        {
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
            TotalCount = totalCount;
            Data = data;
        }

        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Data { get; set; }

    }
}
