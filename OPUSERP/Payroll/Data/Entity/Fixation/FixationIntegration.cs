using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.Fixation
{
	[Table("FixationIntegration")]
	public class FixationIntegration:Base
	{
		public int? employeeId { get; set; }
		public EmployeeInfo employee { get; set; }

		public string empCode { get; set; }
		public DateTime? joiningDate { get; set; }
		public DateTime? dateOfBirth { get; set; }
		public string grade { get; set; }
		public string currentGrade { get; set; }
		public DateTime? lastIncrementDate { get; set; }
		public string postingPlace { get; set; }
		public decimal? initialBasic { get; set; }
		public decimal? increment { get; set; }
		public decimal? startingBasic { get; set; }
		public decimal? newBasicPoint { get; set; }
		public decimal? basicPay { get; set; }
		public DateTime? lastPromotionDate { get; set; }
		public DateTime? lastPromotionDatePrev { get; set; }
		public DateTime? punishmentDate { get; set; }
		public string branchName { get; set; }
        public string designation { get; set; }
        public int? lastDesignationId { get; set; }
        public Designation lastDesignation { get; set; }
        public string fixation { get; set; }    
		public string brachCode { get; set; }
		public string divisionCode { get; set; }
		public string promotionScale { get; set; }
		public decimal? amount { get; set; }
		public int? fixationTypeId { get; set; }
        public int? fixaTypeId { get; set; }
        public FixaType fixaType { get; set; }


        public string fixationType { get; set; }

		public int? fixationGradeId { get; set; }
		public SalaryGrade fixationGrade { get; set; }

		public DateTime? effectiveDate { get; set; }
		public string refNo { get; set; }
		public DateTime? nextIncrementDate { get; set; }
		public int? slNo { get; set; }
		public int? fileNo { get; set; }
		public DateTime? letterDate { get; set; }
		public string note { get; set; }
		public int? batchId { get; set; }
		public int? isApproved { get; set; }
		public string reportHeader { get; set; }
		public string particular { get; set; }

		public int? employeeStatusId { get; set; }

		public int? status { get; set; } //10=locked
		public string remarks { get; set; }
		public int? salaryStructureCreated { get; set; } = 0;

        public string newSignatory { get; set; }
        public string newSignatoryName { get; set; }
        public string newSignatoryDesig { get; set; }
        public string newSignatoryPhone { get; set; }
        public int? categoryId { get; set; }
    }
}
