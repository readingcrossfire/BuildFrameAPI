namespace ML.APIResult
{
    public class APIResult<T> where T : class
    {
        public bool IsError { get; set; }
        public string Message { get; set; }
        public T ResultObject { get; set; }
    }
}