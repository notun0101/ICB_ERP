using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class BankingDiplomaViewModel
    {
        public int employeeId { get; set; }
        public int bankDiplomaId { get; set; }
        public string employeeID { get; set; }
        public EmployeeInfo employee { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public string employeeNameCode { get; set; }

        public string diplomaPart { get; set; }
        public string diplomaName { get; set; }
        public string passingYear { get; set; }
        public string resultType { get; set; } //class, grade
        public string result { get; set; } //1st class, 2nd class, 3rd class, 5.00
        public string session { get; set; }

        public Photograph photograph { get; set; }
        public IEnumerable<BankingDiploma> bankingDiplomas { get; set; }
    }
}
