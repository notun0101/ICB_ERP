using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.SCMPurchaseProcess.Models
{
    public class CSDetailsReportViewModel
    {
        public int? supplierId { get; set; }
        public int? ReqDetailID { get; set; }
        public string organizationName { get; set; }
        public string ItemSpac { get; set; }
        public string DeliveryDate { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Total { get; set; }
    }
}
