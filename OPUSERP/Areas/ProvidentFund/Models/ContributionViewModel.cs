using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class ContributionViewModel
    {
        public int contributionId { get; set; }
        public int? pfmemberId { get; set; }
        public int? employeeInfoId { get; set; }

        public DateTime? transectionDate { get; set; }

        public DateTime? contributionMonth { get; set; }
        public decimal companyContribution { get; set; }
        public decimal employeeContribution { get; set; }
        public decimal totalEmployeeContribution { get; set; }
        public decimal totalCompanyContribution { get; set; }
        public decimal totalContribution { get; set; }
      
        public string narration { get; set; }

        public IEnumerable<PFContribution> pFContributions { get; set; }
        public IEnumerable<PFMemberInfo> pFMemberInfos { get; set; }
    }
    public class ContributionAmountVM
    {
        public decimal? contributionAmount { get; set; }
    }
}
