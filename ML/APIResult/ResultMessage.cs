namespace ML;

public class ResultMessage
{
    public enum ErrorTypes
    {
        No_Error,
        LoadInfo,
        Insert,
        Update,
        Delete,
        SearchData,
        GetData,
        ClientNotRegistered,
        InvalidPassowrd,
        AuthenticationHeaderNotExist,
        InvalidIV,
        TokenNotExist,
        TokenInvalid,
        TokenExpired,
        CheckData,
        AccessDataError,
        Others,
        UserDefine,
        UserMustReLogin,
        FileAccess,
        FileNotExists,
        NoNewAppVersion,
        PermissionDenied,
        InvalidDeviceID
    }

    public bool IsError { get; set; } = false;
    public int ErrorType { get; set; }
    public string Message { get; set; }
    public string MessageDetail { get; set; }

    public ResultMessage()
    {
        this.IsError = false;
        this.ErrorType = 0;
        this.Message = "OK";
    }

    public ResultMessage(bool bolIsError, ErrorTypes enErrorTypes, string strMessage, string strMessageDetail)
    {
        this.IsError = bolIsError;
        this.ErrorType = (int)enErrorTypes;
        this.Message = strMessage;
        this.MessageDetail = strMessageDetail;
    }

    public ResultMessage(bool bolIsError, int intErrorType, string strMessage)
    {
        this.IsError = bolIsError;
        this.ErrorType = intErrorType;
        this.Message = strMessage;
        this.MessageDetail = "";
    }

    public ResultMessage(bool bolIsError, int intErrorType, string strMessage, string strMessageDetail)
    {
        this.IsError = bolIsError;
        this.ErrorType = intErrorType;
        this.Message = strMessage;
        this.MessageDetail = strMessageDetail;
    }

    public ResultMessage(bool bolIsError, ErrorTypes enErrorTypes, string strMessage)
    {
        this.IsError = bolIsError;
        this.ErrorType = (int)enErrorTypes;
        this.Message = strMessage;
        this.MessageDetail = "";
    }

    public ResultMessage(APIResult objAPIResult)
    { 
        this.IsError = objAPIResult.IsError;
        this.ErrorType = objAPIResult.StatusID;
        this.Message = objAPIResult.Message;
        this.MessageDetail = objAPIResult.MessageDetail;
    }
}