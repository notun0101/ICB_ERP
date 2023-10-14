using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;

namespace OPUSERP.Areas.Payroll.Models
{
    public class BonusProcessViewModel
    {
        public int? employeeInfoId { get; set; }
        public int? hrBranchId { get; set; }
        public int? zoneId { get; set; }
        public int salaryPeriodId { get; set; }
        public int salaryHeadId { get; set; }
        public DateTime? lastDayofPeriod { get; set; }
        public string userName { get; set; }
        public string bonusFor { get; set; }
        public string empCode { get; set; }

        public IEnumerable<SalaryPeriod> salaryPeriods { get; set; }
        public IEnumerable<SalaryHead> salaryHeads { get; set; }
        public IEnumerable<HrBranch> hrBranches { get; set; }
        public IEnumerable<Location> zones { get; set; }
    }
}
