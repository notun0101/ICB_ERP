using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class MemberTotalContributionViewModel
    {
        public int? pfmemberId { get; set; }
        public PFMemberInfo pfmember { get; set; }

        public DateTime? transectionDate { get; set; }

        public DateTime? contributionMonth { get; set; }
        public decimal? companyContribution { get; set; }
        public decimal? employeeContribution { get; set; }
    }
}
