using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Data.Entity
{
	[Table("PFVoucherType")]
	public class PFVoucherType:Base
	{
		public string nameEn { get; set; }
		public int? type { get; set; } //1=credit, 2=debit
		public int? status { get; set; }
	}
}
