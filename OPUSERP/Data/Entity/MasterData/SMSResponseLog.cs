namespace OPUSERP.Data.Entity.MasterData
{
    public class SMSResponseLog:Base
    {
        public string body { get; set; }
        public string mobileNo { get; set; }
        public string type { get; set; } = "SMS";
        public string responseString { get; set; }
    }
}
