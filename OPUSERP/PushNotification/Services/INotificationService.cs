﻿using OPUSERP.PushNotification.Models;
using CorePush.Google;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static OPUSERP.PushNotification.Models.GoogleNotification;
using static OPUSERP.PushNotification.Services.NotificationService;

namespace OPUSERP.PushNotification.Services
{
    public interface INotificationService
    {
        Task<ResponseModel> SendNotification(NotificationModel notificationModel);
        void Send(FirebaseNotificationModel firebaseModel);
    }

    public class NotificationService : INotificationService
    {
        private readonly FcmNotificationSetting _fcmNotificationSetting;
        

        public NotificationService(IOptions<FcmNotificationSetting> settings)
        {
            _fcmNotificationSetting = settings.Value;
        }

        public async Task<ResponseModel> SendNotification(NotificationModel notificationModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (notificationModel.IsAndroiodDevice)
                {
                    /* FCM Sender (Android Device) */
                    FcmSettings settings = new FcmSettings()
                    {
                        SenderId = _fcmNotificationSetting.SenderId,
                        ServerKey = _fcmNotificationSetting.ServerKey
                    };
                    HttpClient httpClient = new HttpClient();
                    //httpClient.PostAsync("https://fcm.googleapis.com/fcm/send", Encoding.UTF8);
                    string authorizationKey = string.Format("keyy={0}", settings.ServerKey);
                    string deviceToken = notificationModel.DeviceId;

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept
                            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    DataPayload dataPayload = new DataPayload();
                    dataPayload.Title = notificationModel.Title;
                    dataPayload.Body = notificationModel.Body;

                    GoogleNotification notification = new GoogleNotification();
                    notification.Data = dataPayload;
                    notification.Notification = dataPayload;

                    var fcm = new FcmSender(settings, httpClient);
                    var fcmSendResponse = await fcm.SendAsync(deviceToken, notification);

                    if (fcmSendResponse.IsSuccess())
                    {
                        response.IsSuccess = true;
                        response.Message = "Notification sent successfully";
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = fcmSendResponse.Results[0].Error;
                        return response;
                    }
                }
                else
                {
                    /* Code here for APN Sender (iOS Device) */
                    //var apn = new ApnSender(apnSettings, httpClient);
                    //await apn.SendAsync(notification, deviceToken);
                }
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something went wrong";
                return response;
            }
        }

        public class FirebaseNotificationModel
        {
            [JsonProperty(PropertyName = "to")]
            public string To { get; set; }

            [JsonProperty(PropertyName = "notification")]
            public NotificationModel Notification { get; set; }
        }

        public async void Send(FirebaseNotificationModel firebaseModel)
        {
            HttpRequestMessage httpRequest = null;
            HttpClient httpClient = null;
            FcmSettings settings = new FcmSettings()
            {
                SenderId = _fcmNotificationSetting.SenderId,
                ServerKey = _fcmNotificationSetting.ServerKey
            };
            var authorizationKey = string.Format("key={0}", settings.ServerKey);
            var jsonBody = JsonConvert.SerializeObject(firebaseModel);

            try
            {
                httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send");

                httpRequest.Headers.TryAddWithoutValidation("Authorization", authorizationKey);
                httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                httpClient = new HttpClient();
                using (await httpClient.SendAsync(httpRequest))
                {
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                httpRequest.Dispose();
                httpClient.Dispose();
            }
        }

    }
}
