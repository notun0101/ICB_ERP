using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Models
{
    public class EmployeesSalaryStructureViewModel
    {
        public int? editId { get; set; }       
        public int employeeInfoId { get; set; }
        public int? salaryGradeId { get; set; }
        public int? salarySlab { get; set; }
        //public int salaryYearId { get; set; }
        public decimal? amount { get; set; }
        public string isActive { get; set; }
        public DateTime? effectiveDate { get; set; }
		public int salaryHeadId { get; set; }
		public int editsalarystructureDedId { get; set; }
        public string pfType { get; set; }
        public int? pfPercentage { get; set; }
		public string deductionType { get; set; }
		public decimal? amntPerLakh { get; set; }

		public IEnumerable<EmployeesSalaryStructure> employeesSalaryStructuresList { get; set; }
        public EmployeesSalaryStructure employeesSalaryStructure { get; set; }
        public IEnumerable<SalaryGrade> salaryGradesList { get; set; }
        public SalaryPeriod salaryPeriod { get; set; }
        public List<EpmsalaryStrustureList> salaryStrustureList { get; set; }
		public List<EmpWithSalaryStructure> empWithSalaryStructures { get; set; }
		public IEnumerable<SalaryHead> salaryHeads { get; set; }
        public IEnumerable<SalaryPeriod> salaryPeriods { get; set; }
        public GetLoansByEmpCodeAndTypeViewModel staffLoanHBA { get; set; }
    }
	public class EmpWithSalaryStructure
	{
        public Department department { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
		public IEnumerable<EmployeesSalaryStructure> employeesSalaryStructures { get; set; }
	}

	public class GetLoansByEmpCodeAndTypeViewModel
	{
		public decimal? principalDue { get; set; }
		public decimal? DeductionAmount { get; set; }
		public string DeductionType { get; set; }
		public decimal? DePercentagePerLakh { get; set; }
	}


	public class EmployeesSalaryStructureModel
    {
        public int employeeInfoId { get; set; }
        public string empName { get; set; }

        public int salarySlabId { get; set; }
        public SalarySlab salarySlab { get; set; }

        public int salaryHeadId { get; set; }
        public SalaryHead salaryHead { get; set; }

        public decimal amount { get; set; }
        public string isActive { get; set; }
        public DateTime? effectiveDate { get; set; }
        public DateTime? expiryDate { get; set; }
    }

    public class EpmsalaryStrustureList
    {
        public int? headId { get; set; }
        public string headName { get; set; }
        public string status { get; set; }
        public decimal? amount { get; set; }
		public DateTime? ex { get; set; } = null;
    }
    public class HouseRentCalculate
    {
        public decimal? houseRent { get; set; }
    }
    public class pfSubscriptionVm
    {
        public decimal? pfAmount { get; set; }
    }
	public class PartialSalaryViewModel
	{
		public int? employeeID { get; set; }
		public int? partialId { get; set; }
		public string employeeName { get; set; }
		public int? salaryPeriodId { get; set; }
		public DateTime? fromDate { get; set; }
		public DateTime? toDate { get; set; }
		public int? type { get; set; }

		public int? type1 { get; set; }
		public int? salaryPeriod1 { get; set; }


		public PartialSalaryLog partialSalaryLog { get; set; }
		public IEnumerable<PartialSalaryLog> partialSalaryLogs { get; set; }
		public IEnumerable<SalaryPeriod> salaryPeriods { get; set; }
		public IEnumerable<PartialSalaryLogViewModel> distinctPartialSalaryLogs { get; set; }
		public IEnumerable<PRLEmployeeViewModel> pRLEmployeeViewModels { get; set; }
	}

    public class PRLEmployeeViewModel
    {
        public string employeeCode { get; set; }
        public string nameEnglish { get; set; }
        public string designationName { get; set; }
        public string Posting { get; set; }
    }

	public class CarMaintenanceViewModel
	{
		public int? Id { get; set; }
		public int? employeeInfoId { get; set; }

		public int? salaryPeriodId { get; set; }

		public DateTime? effectiveDate { get; set; }
		public decimal? amount { get; set; }

		public IEnumerable<SalaryPeriod> salaryPeriods { get; set; }
		public IEnumerable<CarMaintenance> carMaintenances { get; set; }
	}

	public class MobileAllowanceViewModel
	{
		public int? Id { get; set; }
		public int? employeeInfoId { get; set; }

		public int? salaryPeriodId { get; set; }

		public DateTime? effectiveDate { get; set; }
		public decimal? amount { get; set; }

		public IEnumerable<SalaryPeriod> salaryPeriods { get; set; }
		public IEnumerable<MobileAllowanceStructure> mobileAllowances { get; set; }
	}

	public class PartialResultViewModel
	{
		public int? partialSaved { get; set; }
	}

	public class PartialSalaryLogViewModel
	{
		public string employeeCode { get; set; }
		public string nameEnglish { get; set; }
		public int? employeeId { get; set; }
		public string periodName { get; set; }
		public int? periodId { get; set; }
		public DateTime? fromDate { get; set; }
		public DateTime? toDate { get; set; }
		public string Type { get; set; }
		public int? totalDays { get; set; }
	}
	public class StaffLoanInfoViewModel
	{
		public int? loanId { get; set; }
		public string loanNo { get; set; }
		public DateTime? disburseDate { get; set; }
		public DateTime? lastInstallmentDate { get; set; }
		public decimal? totalDisbursement { get; set; }
		public decimal? Principal { get; set; }
		public decimal? Charge { get; set; }
		public decimal? Interest { get; set; }
		public decimal? Total { get; set; }
	}

	public class ArrearAmountVm
    {
        public decimal? PrevBasic { get; set; }
        public decimal? ArrearAmount { get; set; }
    }
	public class EmployeeInfoCarMaintanceVm
	{
		public int? employeeId { get; set; }
		public string employeeCode { get; set; }
		public string empName { get; set; }
		public string posting { get; set; }
		public string designation { get; set; }
		public decimal? currentBasic { get; set; }
	}
	public class HrBranchSalary
	{
		public int? Id { get; set; }
		public string branchName { get; set; }
	}
	public class ParticularNamesVm
	{
        public string particular { get; set; }
    }

    public class StaffLoanInfoVm
    {
        public string EmployeeName { get; set; }
        public int? loanId { get; set; }
        public string loanNo { get; set; }
        public DateTime? disburseDate { get; set; }
        public DateTime? firstInstallmentDate { get; set; }
        public decimal? totalDisbursement { get; set; }
        public string loanType { get; set; }
          
        public decimal? Principal { get; set; }
        public decimal? Charge { get; set; }
        public decimal? Interest { get; set; }
        public decimal? Total { get; set; }
        public IEnumerable<StaffLoanInfoVm> staffLoanInfo { get; set; }
    }
}
