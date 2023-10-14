using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.TrainingNew
{
    [Table("ResourcePerson")]
    public class ResourcePerson : Base
    {
        public string name { get; set; }

        public string designation { get; set; }

        public string workPlace { get; set; }

        public string specialization { get; set; }
		public string type { get; set; } //In House/External

		public string contactNumber { get; set; }

        public string performance { get; set; }

        public string email { get; set; }

        public string address  { get; set; }

        public string remarks { get; set; }

        public string url { get; set; }

		public int? employeeInfoId { get; set; }
		public EmployeeInfo employeeInfo { get; set; }

		//Other For Future
		public string orgType { get; set; }

	}
}
