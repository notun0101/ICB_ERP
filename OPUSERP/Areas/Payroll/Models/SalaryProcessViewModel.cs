using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.PF;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Models
{
    public class SalaryProcessViewModel
    {
        public int? employeeInfoId { get; set; }
        public int? salaryPeriodId { get; set; }

        public int?[] SalaryContributionID { get; set; }

        public string[] month { get; set; }
        public string[] year { get; set; }
        public string[] employeeCode { get; set; }
        public int?[] employeeId { get; set; }
        public decimal?[] own { get; set; }
        public decimal?[] company { get; set; }
		public int? lockLabel { get; set; }
        public int? hrBranchId { get; set; }
        public int? locationId { get; set; }


        public IEnumerable<SalaryPeriod> salaryPeriods { get; set; }
		public IEnumerable<HrBranch> hrBranches { get; set; }
		public IEnumerable<Location> locations { get; set; }
        public IEnumerable<PFSalaryContribution> pFSalaryContributions { get; set; }
        public IEnumerable<EmployeesSalaryStructure> empsalarystruc { get; set; }
        public IEnumerable<SalaryStatusLog> salaryStatusLogs { get; set; }
        public IEnumerable<SalaryLockWithStatusVm> allLockSalary { get; set; }
        public IEnumerable<CheckLoansVm> checkLoans { get; set; }
        public ResolveAllVm resolveAllVms { get; set; }
    }

	public class CheckLoansVm
	{
		public int? employeeId { get; set; }
		public string empCode { get; set; }
		public string employeeName { get; set; }
		public string loanType { get; set; }
		public decimal? loanDue { get; set; }
		public decimal? Structure { get; set; }
	}

    public class ResolveAllVm
    {
        public int status { get; set; }
    }

    public class PartialHouseRentVm
    {
        public int? employeeId { get; set; }
        public string employeeCode { get; set; }
        public string employeeName { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int? totalDays { get; set; }
        public decimal? houseRent { get; set; }
        public string branchName { get; set; }
        public string zoneName { get; set; }
        public string periodName { get; set; }
    }
}
