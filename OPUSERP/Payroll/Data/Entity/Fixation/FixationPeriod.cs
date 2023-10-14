using OPUSERP.Data.Entity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.Payroll.Data.Entity.Fixation
{
    [Table("FixationPeriod")]
    public class FixationPeriod : Base
    {
        public string PeriodName { get; set; }
        public int FixationYear { get; set; }
        public int FixationTypeId { get; set; }
        public int ContextId { get; set; }
        public int LockLevel { get; set; }
        public string LockedBy { get; set; }
        public DateTime LockDate { get; set; }

        
    }
}
