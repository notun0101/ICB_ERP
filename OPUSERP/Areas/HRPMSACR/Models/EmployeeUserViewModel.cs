using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSACR.Models
{
	public class EmployeeUserViewModel
	{
		public int? EmpId { get; set; }
		public string EmpCode { get; set; }
		public string EmpName { get; set; }
		public string EmpNameBn { get; set; }
		public string FatherName { get; set; }
		public string MotherName { get; set; }
		public string SpouseName { get; set; }
		public string DistrictName { get; set; }
		public string WorkStation { get; set; }
		public string BranchCode { get; set; }
		public string BranchName { get; set; }
		public string PresentAddress { get; set; }
		public string PermanentAddress { get; set; }
		public string JoiningDesignation { get; set; }
		public string DesignationName { get; set; }
		public string BirthDate { get; set; }
		public string dateOfConfirmation { get; set; }
		public string PrlDate { get; set; }
		public string NID { get; set; }
		public string SignUrl { get; set; }
		public string ImageUrl { get; set; }
		public string DesiCode { get; set; }
		public string currentGrade { get; set; }
		public decimal? currentBasic { get; set; }
		public decimal? totalPaidByBank { get; set; }
		public string DesiCat { get; set; }
		public string MaritalStatus { get; set; }
		public int? UserTypeId { get; set; }
		public string BD1 { get; set; }
		public string BD2 { get; set; }
		public string ActionDate { get; set; }
		public string RewardsList { get; set; }
		public string RefNo { get; set; }
        public string PunishmentLetterNo { get; set; }
        public string PunishmentDate { get; set; }
        public string PunishmentList { get; set; }
		public EmployeeSignature signature { get; set; }
		public string JoiningDate { get; set; }
        public EmployeeInfo employee { get; set; }
        public int childrenCount { get; set; }
        public string lastPromotionDate { get; set; }

        public IEnumerable<AssessmentViewModel> assessmentList { get; set; }
	}

	public class EmployeeAcrViewModel
	{
		public int? EmpId { get; set; }
		public string EmpCode { get; set; }
		public string EmpName { get; set; }
		public string EmpNameBn { get; set; }
		public string FatherName { get; set; }
		public string MotherName { get; set; }
		public string SpouseName { get; set; }
		public string homeDistrict { get; set; }
		public string WorkStation { get; set; }
		public string BranchCode { get; set; }
		public string BranchName { get; set; }
		public string PresentAddress { get; set; }
		public string PermanentAddress { get; set; }
		public string JoiningDesignation { get; set; }
		public string DesignationName { get; set; }
		public DateTime? BirthDate { get; set; }
		public DateTime? PrlDate { get; set; }
		public DateTime? dateOfConfirmation { get; set; }
		public DateTime? lastPromotionDate { get; set; }
		public string NID { get; set; }
		public string SignUrl { get; set; }
		public string ImageUrl { get; set; }
		public string DesiCode { get; set; }
		public string currentGrade { get; set; }
		public decimal? currentBasic { get; set; }
		public decimal? totalPaidByBank { get; set; }
		//public string DesiCat { get; set; }
		public string MaritalStatus { get; set; }
		//public int? UserTypeId { get; set; }
		public string BD1 { get; set; }
		public string BD2 { get; set; }
		//public string ActionDate { get; set; }
		//public string RewardsList { get; set; }
		public string RefNo { get; set; }
		public string PunishmentLetterNo { get; set; }
		public DateTime? PunishmentDate { get; set; }
		public string PunishmentList { get; set; }
		public DateTime? JoiningDate { get; set; }
		public int childrenCount { get; set; }
	}
    public class UserIsHRAdminVm
    {
        public int isHRAdmin { get; set; }
    }
}
