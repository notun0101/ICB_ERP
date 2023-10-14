using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Data.Entity
{
    [Table("PFDistributionMaster")]
    public class PFDistributionMaster : Base
    {
        public string processBy { get; set; }
        public string refNo { get; set; }
        public int? status { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public int? ledgerId { get; set; }
        public Ledger Ledger { get; set; }
    }
}
