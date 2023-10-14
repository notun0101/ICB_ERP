using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Data.Entity
{
	[Table("PfFinalSettlement", Schema = "PF")]
	public class PfFinalSettlement : Base
	{
		public int? pfMemberId { get; set; }
		public PFMemberInfo pfMember { get; set; }

		public string pfMemberCode { get; set; }
		public decimal? pfTotalOwn { get; set; }
		public decimal? pfTotalBank { get; set; }
		public decimal? pfTotalInterest { get; set; }
		public decimal? pfTotalLoan { get; set; }
		public decimal? pfTotal { get; set; }
	}
}
