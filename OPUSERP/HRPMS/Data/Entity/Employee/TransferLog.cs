using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
    [Table("TransferLog")]
    public class TransferLog : Base
    {
        //Foreign Reliation
        public int employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public string workStation { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? from { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? to { get; set; }

        public string designation { get; set; }
        
        public int? designatioId { get; set; }
        public Designation designatio { get; set; }

        public int? departmentId { get; set; }
        public Department department { get; set; }

        public int? salaryGradeId { get; set; }
        public SalaryGrade salaryGrade { get; set; }


        public int? toBranchId { get; set; }
        public HrBranch toBranch { get; set; }

        public int? zoneId { get; set; }
        public Location zone { get; set; }

        public int? officeId { get; set; }
        public FunctionInfo office { get; set; }

        public int? divisionId { get; set; }
        public HrDivision division { get; set; }

        public int? hrUnitId { get; set; }
        public HrUnit hrUnit { get; set; }

        public int? status { get; set; } //1=apply, 2=joined
        public string substitutionEmp { get; set; }
        public string clearenceUrl { get; set; } //joining letter
        public string url { get; set; } //release letter

        public string reportDateBn { get; set; }
        public DateTime? reportDateEn { get; set; }
        public DateTime? orderDate { get; set; }
        public string orderNo { get; set; }
        public string releaseSignatory { get; set; }
        public string signatoryCode { get; set; }
    }
}
