using OPUSERP.Data.Entity.Master;
using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class ForfeitAccountReportViewModel
    {
        public IEnumerable<Company> companies { get; set; }
        public IEnumerable<ForfietAccountsByYear> forfietAccountsByYears { get; set; }
    }
    public class ForfietAccountsByYear
    {
        public int year { get; set; }
        public IEnumerable<ForfeitAccount> forfeitAccounts { get; set; }
    }
}
