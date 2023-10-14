using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System;

namespace OPUSERP.Payroll.Data.Entity.Salary
{
    public class BonusPeriodLock : Base
    {
        public int SalaryYearId { get; set; }
        public SalaryYear SalaryYear { get; set; }
                   
        public int? BonusTypeId { get; set; }
        public BonusType BonusType { get; set; }

        public int PeriodId  { get; set; }

        [MaxLength(100)]
        public string PeriodName { get; set; }
        
        public int? LockLabel { get; set; }
        [MaxLength(100)]
        public string LockBy { get; set; }
        public DateTime? LockDate { get; set; }
                
        public int? CreateBy { get; set; } //userid
        public DateTime? CreateAt { get; set; }
		public int? UpdateBy { get; set; } //userid
		public DateTime? UpdateAt { get; set; }


	}
}
