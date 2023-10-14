using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
	[Table("ResignationLetter")]
	public class ResignationLetter:Base
	{
		public int employeeId { get; set; }
		public EmployeeInfo employee { get; set; }

		public DateTime? submissionDate { get; set; }
		public DateTime? lastworkingDate { get; set; }
		public string reason { get; set; }
		public string totalWorkingDays { get; set; }
		public string fileUrl { get; set; }
	}
}
