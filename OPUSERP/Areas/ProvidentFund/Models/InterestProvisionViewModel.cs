﻿using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.ProvidentFund.Data.Entity;
using System;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class InterestProvisionViewModel
    {
        public int Id { get; set; }
        public int? pfmemberId { get; set; }
        public PFMemberInfo pfmember { get; set; }
        public int? loanManagementId { get; set; }
        public PFLoanManagement loanManagement { get; set; }

        //Asad Added
        public int? EmployeeInfoId { get; set; }
        public EmployeeInfo EmployeeInfo { get; set; }
        //Asad Added
        public int? SalaryPeriodId { get; set; }
        public SalaryPeriod SalaryPeriod { get; set; }

        public decimal? InterestAmount { get; set; }

        public DateTime? TransactionDate { get; set; }
        public decimal? TransactionBehaviour { get; set; }
        public string note { get; set; }
        public int? branchId { get; set; }
        public int? isJournal { get; set; } // 0 = Data Upload ,1 = AutoVoucher Create
    }
}
