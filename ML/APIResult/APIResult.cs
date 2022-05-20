using Newtonsoft.Json;

namespace ML;

public class APIResult
{
    public bool IsError { get; set; }
    public int StatusID { get; set; }
    public string Message { get; set; }
    public string MessageDetail { get; set; }
    public object ResultObject { get; set; }

    public APIResult()
    {
    }

    public APIResult(ResultMessage objResultMessage)
    {
        this.IsError = objResultMessage.IsError;
        this.StatusID = objResultMessage.ErrorType;
        this.Message = objResultMessage.Message;
        this.MessageDetail = objResultMessage.MessageDetail;
    }

    public APIResult(int intStatusID, string strMessage, object objResultObject)
    {
        this.StatusID = intStatusID;
        this.Message = strMessage;
        this.ResultObject = objResultObject;
    }

    public APIResult(bool bolIsError, int intStatusID, string strMessage, string strMessageDetail)
    {
        this.IsError = bolIsError;
        this.StatusID = intStatusID;
        this.Message = strMessage;
        this.MessageDetail = strMessageDetail;
    }

    public APIResult(bool bolIsError, ResultMessage.ErrorTypes enErrorTypes, string strMessage, string strMessageDetail)
    {
        this.IsError = bolIsError;
        this.StatusID = (int)enErrorTypes;
        this.Message = strMessage;
        this.MessageDetail = strMessageDetail;
    }

    public string ToJsonString()
    {
        return JsonConvert.SerializeObject(this);
    }
}