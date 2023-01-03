namespace ML.APIResult
{
    public class APIResult<T>: APIResultBase
    {
        public T ResultObject { get; set; }
    }
}