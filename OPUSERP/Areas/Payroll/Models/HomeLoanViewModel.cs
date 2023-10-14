using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.homeLoan;
using OPUSERP.Payroll.Data.Entity.PF;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Models
{
    public class HomeLoanViewModel
    {
        public int homeloanId { get; set; }
        public int employeeInfoId { get; set; }
        public int salaryPeriodId { get; set; }
        public int salaryHeadId { get; set; }
        public int? salaryGradeId { get; set; }
        public string homeLoanNo { get; set; }
        
        public decimal advanceAmount { get; set; }
        public decimal installmentAmount { get; set; }
        public int noOfInstallment { get; set; }       
        public int? isFromSalary { get; set; }
        public DateTime? loanDate { get; set; }
        public string purpose { get; set; }
        public string loanType { get; set; }
        public int? approver { get; set; }

        public IEnumerable<HomeLoan> homeLoans { get; set; }
        public IEnumerable<HomeLoanSchedule> homeLoanSchedules { get; set; }
        public IEnumerable<SalaryPeriod> salaryPeriods { get; set; }
        public IEnumerable<SalaryHead> salaryHeads { get; set; }
        public IEnumerable<SalaryGrade> salaryGradesList { get; set; }

        public string visualEmpCodeName { get; set; }
        public IEnumerable<LoansViewModel> loans { get; set; }
    }

    public class HomeLoansViewModel
    {
        public HomeLoan loan { get; set; }
        public EmployeeInfo employee { get; set; }
    }
}
