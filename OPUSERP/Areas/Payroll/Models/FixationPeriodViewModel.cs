using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OPUSERP.Payroll.Data.Entity.Fixation;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;

namespace OPUSERP.Areas.Payroll.Models
{
    public class FixationPeriodViewModel
    {
        public int Id { get; set; }
        public string PeriodName { get; set; }
        public int FixationYear { get; set; }
        public int FixationTypeId { get; set; }
        public int ContextId { get; set; }
        public int LockLevel { get; set; }
        public string LockedBy { get; set; }
        public DateTime LockDate { get; set; } =DateTime.Now;
       
        public string fixationYearString { get; set; }
        public string FixationType { get; set; }
        public string Context { get; set; }
        public string LockLevelName { get; set; }
        public IEnumerable<FixationPeriod> fixationPeriods { get; set; }
    }
}
