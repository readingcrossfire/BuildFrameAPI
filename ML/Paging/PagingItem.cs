namespace ML.Paging
{
    public class PagingItem
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int PageTotal { get; set; }
    }
}