namespace ML.APIResult
{
    public class APIObjectResult<T> : APIResult where T : class
    {
        public T Object { get; set; }
    }
}