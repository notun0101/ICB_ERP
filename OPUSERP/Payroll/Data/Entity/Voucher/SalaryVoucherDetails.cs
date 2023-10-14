using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.Voucher
{
	[Table("SalaryVoucherDetails")]
	public class SalaryVoucherDetails:Base
	{
		public string particular { get; set; }
		public string accountCode { get; set; }
		public string type { get; set; } //Debit, Credit
		public decimal? amount { get; set; }

		public int? voucherMasterId { get; set; }
		public SalaryVoucherMaster voucherMaster { get; set; }
	}
}
