using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Arrear;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Models
{
    public class ArrearViewModel
    {
        public int ArrearinfoId { get; set; }

        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public decimal? currentBasic { get; set; }
        public decimal? oldBasic { get; set; }

        public int? periodId { get; set; }
        public SalaryPeriod period { get; set; }

        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }

        public decimal? otherAddition { get; set; }
        public decimal? otherDeduction { get; set; }
        public int? salarySlab { get; set; }
        public IEnumerable<EmployeeArrearInfo> employeeArrearList { get; set; }
        public EmployeeArrearInfo employeeArrear { get; set; }
        public IEnumerable<SalaryPeriod> salaryPeriods { get; set; }


        public List<EpmsalaryStrustureList> salaryStrustureList { get; set; }
        public IEnumerable<EmployeesSalaryStructure> employeesSalaryStructuresList { get; set; }

    }
}
