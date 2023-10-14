using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class PostingViewModel
    {
        public int? employeeId { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public int? postingID { get; set; }

        public string placeName { get; set; }
        public string placeNameBn { get; set; }

        public int? departmentId { get; set; }
        public int? hrBranchId { get; set; }
        public int? branchId { get; set; }
        public int? hrDivisionId { get; set; }
        public int? hrUnitId { get; set; }
        public int? locationId { get; set; }
        public int? officeId { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }

        public int? status { get; set; }
        public int? type { get; set; } //parmanent = 1, temporary = 0

        public string remarks { get; set; }


        public string employeeNameCode { get; set; }

        public Photograph photograph { get; set; }
        public IEnumerable<EmployeePostingPlace> employeePostingPlaces { get; set; }
        public EmployeePostingPlace employeePosting { get; set; }
        public IEnumerable<SpecialBranchUnit> specialBranchUnits { get; set; }
        public IEnumerable<HrBranch> hrBranches { get; set; }
        public IEnumerable<HrDivision> hrDivisions { get; set; }
        public IEnumerable<HrUnit> hrUnits { get; set; }
        public IEnumerable<Location> locations { get; set; }
        public IEnumerable<Department> departments { get; set; }
        public IEnumerable<FunctionInfo> functionInfos { get; set; }
        

    }
}
