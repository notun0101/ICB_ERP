using OPUSERP.Data.Entity;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Data.Entity.Master

{
    [Table("StaffLoanSetting", Schema = "Payroll")]

    public class StaffLoanSetting : Base
    {
        public int? SalaryHeadId { get; set; }
        public SalaryHead SalaryHead { get; set; }

        public decimal? InstallmentAmount { get; set; }
    }
}
