using OPUSERP.Data.Entity.Master;
using System.Collections.Generic;

namespace OPUSERP.Areas.Accounting.Models
{
    public class TrialBalanceViewModel
    {
        public int? accountId { get; set; }
        public string accountCode { get; set; }
        public string accountName { get; set; }
        public decimal? openingBalance { get; set; }
        public decimal? debit { get; set; }
        public decimal? credit { get; set; }
        public decimal? closing { get; set; }
        public Company Company { get; set; }
        public string action { get; set; }
        public string Balance { get; set; }
        public int? groupId { get; set; }
        public string groupCode { get; set; }
        public string groupName { get; set; }
    }

    public class GroupWiseTrialBalanceViewModel
    {
        public int? groupId { get; set; }
        public string groupCode { get; set; }
        public string groupName { get; set; }
        public decimal? debit { get; set; }
        public decimal? credit { get; set; }
        public string Balance { get; set; }

    }
}
