using OPUSERP.Data.Entity;
using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.Payroll.Data.Entity.Salary
{
    [Table("SalaryPeriod")]
    public class SalaryPeriod : Base
    {
        public int salaryYearId { get; set; }
        public SalaryYear salaryYear { get; set; }

        public int taxYearId { get; set; }
        public TaxYear taxYear  { get; set; }

		public int? PFYearId { get; set; }
		public PFYear PFYear { get; set; }
        
		public int salaryTypeId { get; set; }
        public SalaryType salaryType { get; set; }

        public int? bonusTypeId { get; set; }
        public BonusType bonusType { get; set; }

        public int? organizationsBranchId { get; set; }
        [MaxLength(100)]
        public string periodName { get; set; }
        [MaxLength(10)]
        public string monthName { get; set; }

        public int? lockLabel { get; set; }
        [MaxLength(100)]
        public string lockBy { get; set; }
        public DateTime? lockDate { get; set; }

        public decimal? daysWorking { get; set; }
        
        public string mailText { get; set; }
        [MaxLength(250)]
        public string mailSub { get; set; }

		public int? madeBy { get; set; } //userid
		public DateTime? madeDate { get; set; }

		public int? checkedBy { get; set; } //userid
		public DateTime? checkedDate { get; set; }

		public int? approvedBy { get; set; } //userid
		public DateTime? approvedDate { get; set; }

		public int? disbursedBy { get; set; } //userid
		public DateTime? disbursedDate { get; set; }

        public int? isPfProcessed { get; set; } //1=processed
        //Asad added on 21.06.2023
        public int? AffectedSalaryPeriodId { get; set; }
    }
}
