using OPUSERP.CRM.Data.Entity.MasterData;
using OPUSERP.Data.Entity;
using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
	[Table("EmployeeAccounts", Schema = "HR")]
	public class EmployeeAccounts : Base
	{
		public int? employeeInfoId { get; set; }
		public EmployeeInfo employeeInfo { get; set; }

		public int? pfMemberInfoId { get; set; }
		public PFMemberInfo pfMemberInfo { get; set; }

		public string accountType { get; set; } //MCA,MCAR,HBL-B13,HBL-A13,Com,PF

		public int? bankId { get; set; }
		public Bank bank { get; set; }

		public string accountNumber { get; set; }

		public int? status { get; set; }
		public string remarks { get; set; }
	}
}
