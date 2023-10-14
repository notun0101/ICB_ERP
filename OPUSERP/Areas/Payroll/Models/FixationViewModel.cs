using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Fixation;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Models
{
    public class FixationViewModel
    {
        public IEnumerable<Company> companies { get; set; }
        public FixationIntegration fixationIntegration { get; set; }
        public IEnumerable<FixationIntegration> fixationIntegrations { get; set; }
        public IEnumerable<SalarySlab> AllSlab { get; set; }
        public string newbasicAmount { get; set; }
        public IEnumerable<ReFixationReportVm> reFixationReportVms { get; set; }
        public IEnumerable<EmployeePostingPlace> employeePostingPlaces { get; set; }
        public FixationReportViewModel fixationDetails { get; set; }
        public IEnumerable<EmpPostingPlaceVm> empPostingPlace { get; set; }
        public IEnumerable<YearlyFixationVm> yearlyFixations { get; set; }
        public BranchManager branchManager { get; set; }
		public IEnumerable<FixationSummaryViewModel> fixationSummaryViewModels { get; set; }

        public IEnumerable<FixationAllViewModel> fixationAllViewModels { get; set; }
    }
	public class FixationSummaryViewModel
	{
		public string designation { get; set; }
		public string fixationType { get; set; }
		public int? designationCode { get; set; }
		public int? year { get; set; }
		public int? totalEmp { get; set; }
	}
    public class BranchManager
    {
        public string code { get; set; }
        public string name { get; set; }
        public string designation { get; set; }
        public string designationBn { get; set; }
    }
    public class AllSlab
    {
        public string slabName { get; set; }
    }
    public class ReFixationReportVm
    {
        public string particular { get; set; }
        public string amount { get; set; }
    }

    public class FixationReportViewModel
    {
        public string empCode { get; set; }
        public DateTime? letterDate { get; set; }
        public string refNo { get; set; }
        public string gender { get; set; }
        public string nameBangla { get; set; }
        public string designationNameBN { get; set; }
        public int? lastDesignationId { get; set; }
        public int? JoinDesignationId { get; set; }
        public int? categoryId { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? effectiveDate { get; set; }
        public DateTime? lastPromotionDate { get; set; }
        public DateTime? lastPromotionDatePrev { get; set; }
        public DateTime? joiningDate { get; set; }
        public decimal? amount { get; set; }
        public decimal? newSlabAmount { get; set; }
        public string AllSlab { get; set; }
        public string newSignatory { get; set; }
        public string newSignatoryName { get; set; }
        public string newSignatoryPhone { get; set; }
        public string newSignatoryDesig { get; set; }
    }

    public class EmpPostingPlaceVm
    {
        public int? hrBranchId { get; set; }
        public string branchNameBn { get; set; }
        public string branchAddress { get; set; }

        public int? departmentId { get; set; }
        public string deptNameBn { get; set; }

        public int? hrDivisionId { get; set; }
        public string divNameBn { get; set; }

        public int? hrUnitId { get; set; }
        public string unitNameBn { get; set; }

        public int? zoneId { get; set; }
        public string zoneNameBn { get; set; }

        public int? officeId { get; set; }
        public string officeNameBn { get; set; }
        public string officeAddress { get; set; }
        public string managerDesignation { get; set; }
    }

    public class YearlyFixationVm
    {
        public string empCode { get; set; }
        public string nameEnglish { get; set; }
        public string nameBangla { get; set; }
        public string designation { get; set; }
        public int? designationCode { get; set; }
        public string posting { get; set; }
        public string payScale { get; set; }
        public decimal? substantiveAmount { get; set; }
        public DateTime? effectiveDate { get; set; }
        public decimal? nextSlab { get; set; }
        public string punishment { get; set; }
        public string recommendation { get; set; }
        public string signature { get; set; }
        public string seniorityLevel { get; set; }
        public int? Status { get; set; }
    }
	public class IncrementViewModel
	{
		public int Id { get; set; }
		public int? employeeId { get; set; }
		public string empCode { get; set; }
		public string empName { get; set; }
		public string reason { get; set; }
		public string orderNo { get; set; }
		public DateTime? expireDate { get; set; }

		public IEnumerable<IncrementHeld> IncrementHelds { get; set; }
	}

    public class FixationAllViewModel
    {
        public FixationIntegration fixation { get; set; }
        public IEnumerable<SalarySlab> salarySlabs { get; set; }
        public string branchName { get; set; }
    }


    public class FixationPreviewViewModel
    {
        public string empCode { get; set; }
        public DateTime? letterDate { get; set; }
        public string refNo { get; set; }
        public string fileNo { get; set; }
        public string gender { get; set; }
        public string nameBangla { get; set; }
        public string designationNameBN { get; set; }
        public int? lastDesignationId { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? effectiveDate { get; set; }
        public DateTime? lastPromotionDate { get; set; }
        public DateTime? lastPromotionDatePrev { get; set; }
        public DateTime? joiningDate { get; set; }
        public decimal? amount { get; set; }
        public decimal? newSlabAmount { get; set; }
        public string AllSlab { get; set; }
        public string newSignatory { get; set; }
        public string newSignatoryName { get; set; }
        public string newSignatoryPhone { get; set; }
        public string newSignatoryDesig { get; set; }
        public int? categoryId { get; set; }

        //public FixationReportViewModel fixationDetails { get; set; }
        public FixationReportPreviewViewModel fixationDetails { get; set; }
        public IEnumerable<EmpPostingPlaceVm> empPostingPlace { get; set; }
    }

    public class FixationReportPreviewViewModel
    {
        public string empCode { get; set; }
        //public DateTime? letterDate { get; set; }
        //public string refNo { get; set; }
        public string gender { get; set; }
        public string nameBangla { get; set; }
        public string designationNameBN { get; set; }
        public int? lastDesignationId { get; set; }
        public int? JoinDesignationId { get; set; }
      
        public DateTime? lastPromotionDate { get; set; }
        public DateTime? joiningDate { get; set; }
        public decimal? amount { get; set; }
        public decimal? newSlabAmount { get; set; }
        public string AllSlab { get; set; }
        
    }


}
