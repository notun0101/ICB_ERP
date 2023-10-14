using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Auth.Models
{
	public class UserLogsViewModel
	{
		public string empCode { get; set; }
		public string empName { get; set; }
		public string designation { get; set; }
		public DateTime? dateOfBirth { get; set; }
		public DateTime? joiningDate { get; set; }
		public string emailOffice { get; set; }
		public string emailPersonal { get; set; }
		public string phoneNumberOffice { get; set; }
		public string phoneNumberPersonal { get; set; }
		public string gender { get; set; }
		public string bloodGroup { get; set; }
		public string nid { get; set; }
		public string homeDistrict { get; set; }
		public string disability { get; set; }
		public string maritalStatus { get; set; }
		public string religion { get; set; }
		public string salaryAccount { get; set; }
		public string passportNo { get; set; }
		public string passportIssue { get; set; }
		public string passportExpire { get; set; }
		public string fatherName { get; set; }
		public string fatherOccupation { get; set; }
		public string fatherMobile { get; set; }
		public string motherName { get; set; }
		public string motherOccupation { get; set; }
		public string motherMobile { get; set; }

		public IEnumerable<Spouse> employeeSpouses { get; set; }
		public IEnumerable<Children> employeeChildren { get; set; }

		public string posting { get; set; }
		public string ipAddress1 { get; set; }
		public string ipAddress2 { get; set; }
		public string MAC { get; set; }
		public string PCName { get; set; }
		public string Latitude { get; set; }
		public string Longitude { get; set; }
		public string Address { get; set; }
		public DateTime? LastLogin { get; set; }
		public string Key { get; set; }
	}
}
