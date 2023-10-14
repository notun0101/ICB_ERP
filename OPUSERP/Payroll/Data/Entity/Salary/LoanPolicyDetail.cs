using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.Salary
{
    public class LoanPolicyDetail:Base
    {
        public int? loanPolicyId { get; set; }
        public LoanPolicyNew loanPolicy { get; set; }

        public int? designationId { get; set; }
        public Designation designation { get; set; }
    }
}
