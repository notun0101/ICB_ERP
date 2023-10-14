using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Net;
using System;
using OPUSERP.Data;
using OPUSERP.Data.Entity.MasterData;
using System.Threading.Tasks;

namespace OPUSERP.Helpers
{
    public class Messaging
    {
        private readonly ERPDbContext context;

        public Messaging(ERPDbContext context)
        {
            this.context = context;
        }

        public static SMSResponseLog SendSMSMessage(string mobile, string message)
        {
            try
            {
                string url = String.Format("https://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/OTPMessage.php?masking=BDBL%20Alert&userName=BDBL&password=82a27fd0d2e74b0aab0379fc6ba723f1&MsgType=TEXT&receiver={0}&message={1}", mobile, message);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                var log = new SMSResponseLog
                {
                    body = message,
                    mobileNo = mobile,
                    type = "SMS",
                    responseString = responseString
                };



                return log;

                //return new SMSResponseLog();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public static SMSResponseLog SendOTPMessage(string mobile, string message)
        {
            try
            {
                string url = String.Format("https://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/OTPMessage.php?masking=BDBL%20OTP&userName=BDBL&password=82a27fd0d2e74b0aab0379fc6ba723f1&MsgType=TEXT&receiver={0}&message={1}", mobile, message);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                var log = new SMSResponseLog
                {
                    body = message,
                    mobileNo = mobile,
                    type = "SMS",
                    responseString = responseString
                };



                return log;

                //return new SMSResponseLog();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
