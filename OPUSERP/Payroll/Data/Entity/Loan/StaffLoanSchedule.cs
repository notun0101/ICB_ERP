using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.Loan
{
    [Table("StaffLoanSchedule", Schema = "Payroll")]
    public class StaffLoanSchedule:Base
    {
        public int? staffLoanId { get; set; }
        public StaffLoan staffLoan { get; set; }

        public int salaryPeriodId { get; set; }

        public decimal installmentAmount { get; set; }
        public decimal restAmount { get; set; }
        public int noOfInstallment { get; set; }

        [DefaultValue(0)]
        public int? isComplete { get; set; }
    }
}
