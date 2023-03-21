
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FIREBASE.Models;
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
                //objHttpRequestMsg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                objHttpRequestMsg.Headers.TryAddWithoutValidation("Authorization", $"key={this._strApplcationID}");
                objHttpRequestMsg.Headers.TryAddWithoutValidation("Sender", $"id={this._strSenderID}");
                objHttpRequestMsg.Headers.TryAddWithoutValidation("Content-Length", byteArray.Length.ToString());
                objHttpRequestMsg.Content = new StringContent(strDateRequest, Encoding.UTF8, "application/json");
                HttpResponseMessage objHttpResponseMsg = await objHttpClient.SendAsync(objHttpRequestMsg);
                string strResponse = await objHttpResponseMsg.Content.ReadAsStringAsync();
                FirebaseNotificationResult? notificationResult = JsonSerializer.Deserialize<FirebaseNotificationResult>(strResponse);
                if (notificationResult != null && notificationResult.success == 1)
                {
                    return new APIResultBase { IsError = false, Message = "Gửi notification thành công" };
                }

                return new APIResultBase { IsError = true, Message = "Gửi notification thất bại" };
            }
            catch (Exception objEx)
            {
                return new APIResultBase { IsError = true, Message = objEx.ToString() };
            }
        }
    }
}