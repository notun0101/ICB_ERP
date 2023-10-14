using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class VacancyViewModel
    {
        public string employeeID { get; set; }
        public int vacancyId { get; set; }

        public int? designationId { get; set; }
        public Designation designation { get; set; }

        public int? type { get; set; } //3=None(Direct Assign), 2=Recruitment, 3=Promotion

        public int? sanction { get; set; }
        public int? existing { get; set; }
        public int? vacancy { get; set; }

        public int? status { get; set; }
        public string remarks { get; set; }

        public IEnumerable<Designation> designations { get; set; }
        public IEnumerable<Vacancy> vacancies { get; set; }
        public Photograph photograph { get; set; }
        public EmployeeInfo employeeInfo { get; set; }

    }
}
