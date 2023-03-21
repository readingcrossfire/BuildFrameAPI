namespace ML.WMS
{
    public class WMSLog
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Event { get; set; } = null;
        public string UserName { get; set; } = null;
        public int StoreID { get; set; } = -1;
        public string Module { get; set; } = null;
    }
}