using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSACR.Models
{
	public class AcrUserViewModel
    {
		public int? EmpId { get; set; }
		public string EmpCode { get; set; }
		public string EmpName { get; set; }
		public string BranchCode { get; set; }
		public string BranchName { get; set; }
		public string DesignationName { get; set; }
		public string DesiCode { get; set; }
		public int? UserTypeId { get; set; }
		public string SignPath { get; set; }

    }
}
