using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Models
{
    public class EmailingViewModel
    {
        public int? employeeInfoId { get; set; }
        public int? All { get; set; }
        public int? salaryPeriodId { get; set; }
        public int? hrBranchid { get; set; }
        public int? designationId { get; set; }
        public DateTime? payDate { get; set; }

        public string mailSub { get; set; }
        public string mailText { get; set; }

        public IEnumerable<SalaryPeriod> salaryPeriods { get; set; }
        public IEnumerable<HrBranch> hrBranches { get; set; }
        public IEnumerable<Designation> designations { get; set; }
        public IEnumerable<EmailStatusViewModel> emailStatusViewModels { get; set; }
    }

    public class EmailStatusViewModel
    {
        public int? Id { get; set; }
        public int? lastDesignationId { get; set; }
        public string employeeCode { get; set; }
        public string nameEnglish { get; set; }
        public string designationName { get; set; }
        public string Desig { get; set; }
        public string emailAddress { get; set; }
        public string status { get; set; }
        public string url { get; set; }
    }
}
