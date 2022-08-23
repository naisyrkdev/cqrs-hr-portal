namespace Application.Common.Wrappers
{
    public class PaginationWrapper<T>
    {
        public int TotalRecords { get; set; }

        public T Data { get; set; }
    }
}
