using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.CRM.Data.Entity.MasterData;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class EmployeeDiseaseViewModel
    {
        public int EmployeeDiseaseId { get; set; }
        public int employeeID { get; set; }
        public int? medicalDiseaseId { get; set; }
        public int? status { get; set; }
        public int? hospitalised { get; set; } //1=yes, 0=no
        public int? underTreatment { get; set; } //1=yes, 0=no
        public int? vaccinated { get; set; } //1=yes, 0=no
        public string observation { get; set; }
        public Photograph photograph { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public IEnumerable<MedicalDisease>  medicalDiseases { get; set; }
        public IEnumerable<EmployeeDisease>  employeeDiseases { get; set; }
    }
}
