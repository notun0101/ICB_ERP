using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
    [Table("BankingDiploma", Schema = "HR")]
    public class BankingDiploma : Base
    {
        public int employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public string diplomaPart { get; set; } //Part 1, Part 2
        //public string diplomaName { get; set; }
        public string passingYear { get; set; }
        //public string resultType { get; set; } //class, grade
        public string result { get; set; }

		public string session { get; set; }
	}
}
