namespace ML.APIResult
{
    public class MessageResult<T>: MessageResultBase
    {
        public T? ResultObject { get; set; }
    }
}