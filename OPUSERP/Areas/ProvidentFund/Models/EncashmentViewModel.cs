using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class EncashmentViewModel
    {
        public int encashId { get; set; }
        public DateTime encashDate { get; set; }
        public int encashAmount { get; set; }
        public int adjustAmount { get; set; }
        public string encashAccount { get; set; }
        public string adjustAccount { get; set; }

        public IEnumerable<EncashmentViewModel> encashmentViewModels { get; set; }
    }
}
