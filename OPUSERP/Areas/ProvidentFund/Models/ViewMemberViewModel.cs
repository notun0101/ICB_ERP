using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class ViewMemberViewModel
    {
        public int? memberId { get; set; }
        public string memberName { get; set; }
        public string employeeId { get; set; }
        public string designation { get; set; }
        public string department { get; set; }
        public string memberType { get; set; }
        public string nid { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public DateTime? birthDate { get; set; }
        public DateTime? joiningDate { get; set; }
        public string note { get; set; }
    }
}
