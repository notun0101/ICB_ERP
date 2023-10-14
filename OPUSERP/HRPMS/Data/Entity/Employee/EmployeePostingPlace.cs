using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.ACR;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
    [Table("EmployeePostingPlace", Schema = "HR")]
    public class EmployeePostingPlace:Base
    {
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public int? departmentId { get; set; }
        public Department department { get; set; }

        public int? branchId { get; set; }
        public SpecialBranchUnit branch { get; set; }

        public int? hrBranchId { get; set; }
        public HrBranch hrBranch { get; set; }

        public int? hrDivisionId { get; set; }
        public HrDivision hrDivision { get; set; }

        public int? hrUnitId { get; set; }
        public HrUnit hrUnit { get; set; }

        public int? locationId { get; set; }
        public Location location { get; set; }
        public int? officeId { get; set; }
        public FunctionInfo office { get; set; }

        public string placeName { get; set; }
        public string placeNameBn { get; set; }

		public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }

        public int? status { get; set; } 
        public int? type { get; set; } //parmanent = 1, temporary = 0
        public string remarks { get; set; }

        public int? sortOrder { get; set; }

        public int? responsibilityId { get; set; }
        public JobResponsibility responsibility { get; set; }

        public int? designationId { get; set; }
        public Designation designation { get; set; }
    }
}
