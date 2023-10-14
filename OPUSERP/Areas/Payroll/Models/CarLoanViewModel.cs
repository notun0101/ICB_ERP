using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.carLoan;
using OPUSERP.Payroll.Data.Entity.PF;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Models
{
    public class CarLoanViewModel
    {
        public int carloanId { get; set; }
        public int employeeInfoId { get; set; }
        public int salaryPeriodId { get; set; }
        public int salaryHeadId { get; set; }
        public int? salaryGradeId { get; set; }
        public string carLoanNo { get; set; }
        
        public decimal advanceAmount { get; set; }
        public decimal installmentAmount { get; set; }
        public int noOfInstallment { get; set; }       
        public int? isFromSalary { get; set; }
        public DateTime? loanDate { get; set; }
        public string purpose { get; set; }

        public IEnumerable<CarLoan> carLoans { get; set; }
        public IEnumerable<CarLoanSchedule> carLoanSchedules { get; set; }
        public IEnumerable<SalaryPeriod> salaryPeriods { get; set; }
        public IEnumerable<SalaryHead> salaryHeads { get; set; }
        public IEnumerable<SalaryGrade> salaryGradesList { get; set; }
        public string loanType { get; set; }
        public int? approver { get; set; }

        public string visualEmpCodeName { get; set; }

        public IEnumerable<LoansViewModel> loans { get; set; }
    }
    public class CarLoansViewModel
    {
        public CarLoan loan { get; set; }
        public EmployeeInfo employee { get; set; }
    }
}
