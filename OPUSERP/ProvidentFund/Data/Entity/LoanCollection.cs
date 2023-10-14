using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.CRM.Data.Entity.MasterData;
using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Data.Entity
{
    [Table("LoanCollection")]
    public class LoanCollection : Base
    {

        public int? pfmemberId { get; set; }
        public PFMemberInfo pfmember { get; set; }
        public int? loanManagementId { get; set; }
        public PFLoanManagement loanManagement { get; set; }

        public int? SalaryPeriodId { get; set; }
        public SalaryPeriod SalaryPeriod { get; set; }
        //Asad Added
        public decimal? collectionAmount { get; set; }
        public DateTime? collectionDate { get; set; }
        public int? branchId { get; set; }
        public int? isJournal { get; set; } // 0 = Data Upload ,1 = AutoVoucher Create

        public int? duration { get; set; }
        public string particular { get; set; }
        public decimal? debit { get; set; }
        public decimal? credit { get; set; }
        public decimal? principal { get; set; }
        public decimal? interestCharge { get; set; }
        public decimal? interestPer { get; set; }
        public decimal? interest { get; set; }
        public int isProcessed { get; set; } = 0;

        public int? IsPayrollEmi { get; set; }
        public int? EmployeeInfoId { get; set; }
        public EmployeeInfo EmployeeInfo { get; set; }
        public string note { get; set; }

    }
}
