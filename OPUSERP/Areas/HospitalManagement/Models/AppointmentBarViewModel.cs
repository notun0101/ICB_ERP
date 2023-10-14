using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HospitalManagement.Models
{
	public class AppointmentBarViewModel
	{
		public EmployeeContractInfo doctorInfo { get; set; }
		public string date { get; set; }
	}
}
