using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    [NotMapped]
    public class EmployeeWithDesignationVM
    {
        public int? Id { get; set; }

        public string employeeCode { get; set; }

        public string nameBangla { get; set; }

        public string nameEnglish { get; set; }

        public string designationName { get; set; }

        public string fatherNameEnglish { get; set; }

        public string fatherNameBangla { get; set; }

        public string motherNameBangla { get; set; }

        public string motherNameEnglish { get; set; }

        public string joiningDate { get; set; }

        public string promotionType { get; set; }

        public string empType { get; set; }

        public string gradeName { get; set; }

        public DateTime dateOfBirth { get; set; }

        public decimal? Basic { get; set; }

        //public EmployeeInfoLn fLang { get; set; }

        public IEnumerable<EmployeeWithDesignationVM> employeeWithDesignations { get; set; }

        public IEnumerable<HRPMS.Data.Entity.Assignment.Assignment> assignments { get; set; }

        public IEnumerable<HRPMS.Data.Entity.Employee.Promotion> promotions { get; set; }

        public IEnumerable<HRPMS.Data.Entity.PunishmentCharge.Charge> charges { get; set; }

        public IEnumerable<HRPMS.Data.Entity.PunishmentCharge.Punishment> punishments { get; set; }

        public IEnumerable<HRPMS.Data.Entity.RewardInfo.Reward> rewards { get; set; }
        public string visualEmpCodeName { get; set; }

    }

	public class EmployeeInfoWithPostingVm
	{
		public int? employeeId { get; set; }
		public string employeeCode { get; set; }
		public string empName { get; set; }
		public int? currentGradeId { get; set; }
		public int? lastDesignationId { get; set; }
		public DateTime? joiningDate { get; set; }
		public string gender { get; set; }
		public string emailAddress { get; set; }
		public string posting { get; set; }
		public string designation { get; set; }
		public decimal? currentBasic { get; set; }
	}
	public class EmployeeInformationByCodeVm
	{
		public int? employeeId { get; set; }
		public string employeeCode { get; set; }
		public string empName { get; set; }
		public int? lastDesignationId { get; set; }
		public DateTime? joiningDate { get; set; }
		public string gender { get; set; }
		public string emailAddress { get; set; }
		public string mobile { get; set; }
		public string posting { get; set; }
		public string designation { get; set; }
		public string photoUrl { get; set; }
		public decimal? currentBasic { get; set; }
	}
	public class GetAllActiveEmployeesVm
	{
		public int? Id { get; set; }
		public string employeeCode { get; set; }
		public string nameEnglish { get; set; }
		public string designationName { get; set; }
		public string sbu { get; set; }
		public DateTime? joiningDate { get; set; }
		public string gradeName { get; set; }
		public int? currentGradeId { get; set; }
		public decimal? currentBasic { get; set; }
        public string Posting { get; set; }
        public int? serviceStatus { get; set; }
        public int? lastDesignationId { get; set; }
    }

	public class AllUsersOnlineStatusVm
	{
		public string employeeCode { get; set; }
		public string nameEnglish { get; set; }
		public string designationName { get; set; }
		public string designationShortName { get; set; }
		public string Posting { get; set; }
		public string lastLoggedInTime { get; set; }
		public int? totalLoggedIn { get; set; }
		public string OnlineStatus { get; set; }
	}
}
