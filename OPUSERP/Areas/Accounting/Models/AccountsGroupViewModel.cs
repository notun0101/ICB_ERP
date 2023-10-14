using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Accounting.Models
{
    public class AccountsGroupViewModel
    {
        public int? accountGroupId { get; set; }
        public string accountGroupName { get; set; }
        public int? natureId { get; set; }
        public string accType { get; set; }
        public string groupCode { get; set; }
        public string Nature { get; set; }
        public int? parentGroupId { get; set; }
        public int? id { get; set; }
    }
}
