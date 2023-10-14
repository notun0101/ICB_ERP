using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Models
{
	public class CBSProcessViewModel
	{
		public int? periodId { get; set; }
		public string sbu { get; set; }
		public int? hrBranchId { get; set; }
		public int? zoneId { get; set; }
		public string empCode { get; set; }
		public string Type { get; set; }

		public IEnumerable<SalaryPeriod> salaryPeriods { get; set; }
		public IEnumerable<HrBranch> hrBranches { get; set; }
		public IEnumerable<Location> zones { get; set; }
		public IEnumerable<SpecialBranchUnit> sbus { get; set; }
		public IEnumerable<ProcessedVouchersVm> processedVouchersVms { get; set; }
		public IEnumerable<ProcessedSalaryVm> processedSalaryVms { get; set; }

		public IEnumerable<CBSSalarySheetViewModel> cBSSalarySheets { get; set; }
	}

	public class ProcessedVouchersVm
	{
		public DateTime? createdAt { get; set; }
		public DateTime? updatedAt { get; set; }
		public string branchName { get; set; }
		public string heading { get; set; }
		public decimal? amount { get; set; }
		public string accountCode { get; set; }
		public string Status { get; set; }
		public string uniqueId { get; set; }
		public string refCode { get; set; }
		public string successStatus { get; set; }
		public string message { get; set; }
	}


	public class ProcessedSalaryVm
	{
		public DateTime? createdAt { get; set; }
		public DateTime? updatedAt { get; set; }
		public string branchName { get; set; }
		public string branchCode { get; set; }
		public string Status { get; set; }
		public string type { get; set; }
		public string successStatus { get; set; }
		public string message { get; set; }
		public string refCode { get; set; }
	}
	public class CBSVoucherPostingResponseVm
	{
		public int? status { get; set; }
		public string branchCode { get; set; }
		public string branchName { get; set; }
		public string trxcode { get; set; }
		public string message { get; set; }
	}
	public class CBSProcessSp
	{
		public int? status { get; set; }
	}
	public class UpdateSalaryStructureExpire
	{
		public int? status { get; set; }
	}
	public class CBSProcessLogViewModel
	{
        public int? branchId { get; set; }
        public string branchName { get; set; }
        public string branchType { get; set; }
        public int? periodId { get; set; }
        public int? totalBranch { get; set; }
        public DateTime? voucherPostingDate { get; set; }
    }

	public class CBSPostingLogReport
	{
		public SalaryPeriod salaryPeriod { get; set; }
		public IEnumerable<Company> companies { get; set; }
		public IEnumerable<CBSPostingLogViewModel> cBSPostingLogViewModels { get; set; }
	}
	public class CBSPostingLogViewModel
	{
		public string branchCode { get; set; }
		public string branchName { get; set; }
		public string type { get; set; }
		public string key { get; set; }
		public DateTime? postingDate { get; set; }
		public string successStatus { get; set; }
		public string refCode { get; set; }
		public string message { get; set; }
		public int? periodId { get; set; }
	}
}
