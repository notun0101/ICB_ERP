using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Data.Entity.Auth
{
    public class ForgotPasswordHistory:Base
    {
        public string ipAddress { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string recoverCode { get; set; }
        public string oldPass { get; set; }
        public string newPass { get; set; }
        public DateTime? codeExpire { get; set; }
        public int? changeReqCount { get; set; }
        public int? changeCount { get; set; }
    }
}
