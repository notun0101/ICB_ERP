using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Data.Entity
{
	[Table("PFVoucherDetails", Schema = "PF")]
	public class PFVoucherDetails:Base
	{
		public string pfMemberCode { get; set; }

		public int? pfMemberId { get; set; }
		public PFMemberInfo pFMember { get; set; }

		public string employeeCode { get; set; }

		public int? employeeId { get; set; }
		public EmployeeInfo employee { get; set; }

		public string nameEnglish { get; set; }
		public DateTime? transactionDate { get; set; }
		public decimal? credit { get; set; }
		public decimal? debit { get; set; }

		public int? pfVoucherTypeId { get; set; }
		public PFVoucherType pfVoucherType { get; set; }
	}
}
