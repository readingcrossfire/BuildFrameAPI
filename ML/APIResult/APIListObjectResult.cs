namespace ML.APIResult
{
    public class APIListObjectResult<T> : APIResult
    {
        public List<T>? ListObject { get; set; }
    }
}