using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Arrear;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data
{
    [Table("EmployeesArrearStructure", Schema = "Payroll")]
    public class EmployeesArrearStructure:Base
    {
        public int? arrearMasterId { get; set; }
        public EmployeeArrearInfo arrearMaster { get; set; }

        public int employeeInfoId { get; set; }
        public EmployeeInfo employeeInfo { get; set; }

        public int salarySlabId { get; set; }
        public SalarySlab salarySlab { get; set; }

        public int salaryHeadId { get; set; }
        public SalaryHead salaryHead { get; set; }

        public decimal amount { get; set; }
        [MaxLength(10)]
        public string isActive { get; set; }

        public DateTime? effectiveDate { get; set; }
    }
}
