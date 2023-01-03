using System.Net;
using System.Text;
using System.Text.Json;
using FIREBASE.SendNotification.Interface;
using Microsoft.AspNetCore.Mvc;
using ML.APIResult;
using SHARED.LoadConfig;

namespace FIREBASE.SendNotification
{
    public class SendNotification : ISendNotification
    {
        private string _strApplcationID = "";
        private string _strSenderID = "";

        private void Init()
        {
            _strApplcationID = LoadConfig.Instance.GetValue<string>("Firebase:ApplicationID");
            _strSenderID = LoadConfig.Instance.GetValue<string>("Firebase:SenderID");
        }

        [NonAction]
        public async Task<APIResultBase> Send(string strDevicesId, string strTitle, string strContent)
        {
            try
            {
                Init();

                HttpClient objHttpClient = new HttpClient();
                object objDataRequest = new
                {
                    to = strDevicesId,
                    notification = new
                    {
                        body = strContent,
                        title = strTitle,                        
                        icon = "myicon"
                    }
                };

                string strDateRequest = JsonSerializer.Serialize(objDataRequest);
                Byte[] byteArray = Encoding.UTF8.GetBytes(strDateRequest);

                HttpRequestMessage objHttpRequestMsg = new HttpRequestMessage(HttpMethod.Post, new Uri("https://fcm.googleapis.com/fcm/send"));
                objHttpRequestMsg.Headers.Add("Content-Type", "application/json");
                objHttpRequestMsg.Headers.Add("Authorization", $"key={this._strApplcationID}");
                objHttpRequestMsg.Headers.Add("Sender", $"id={this._strSenderID}");
                objHttpRequestMsg.Headers.Add("Content-Length", byteArray.Length.ToString());

                HttpResponseMessage objHttpResponseMsg = await objHttpClient.SendAsync(objHttpRequestMsg);
                string strResponse = await objHttpResponseMsg.Content.ReadAsStringAsync();
                return null;
            }
            catch (Exception objEx)
            {
                return await Task.FromResult(new APIResultBase { IsError = true, Message = objEx.ToString() });
            }
        }
    }
}