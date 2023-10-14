using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class EmployeeExtraCurricularViewModel
    {
        public string ExtraCurricularId { get; set; }
        public string employeeId { get; set; }
        public int? extraCurricularTypeId { get; set; }       
        public string skillLevel { get; set; } // Primary, Mid-Level, Expert
        public string skillType { get; set; } //National, Regional
        public string description { get; set; }
        public Photograph photograph { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public EmployeeExtraCurricular employeeExtraCurricular { get; set; }
        //public ExtraCurricularType extraCurricularType { get; set; }
        public IEnumerable<ExtraCurricularType> extraCurricularType { get; set; }
        //public IEnumerable<ExtraCurricularType> extraCurricularTypes { get; set; }
        public IEnumerable<EmployeeExtraCurricular> employeeExtraCurriculars { get; set; }
    }
}
