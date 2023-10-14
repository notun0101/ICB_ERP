using OPUSERP.Data.Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.ProvidentFund.Data.Entity
{
	
	[Table("PFYear", Schema = "PF")]
	public class PFYear : Base
	{
		[MaxLength(100)]
		public string YearName { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string YearStatus { get; set; }
		public DateTime? LockDate { get; set; }
	}
}
