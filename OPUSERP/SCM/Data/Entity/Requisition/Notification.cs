using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Data.Entity.Requisition
{
	public class Notification:Base
	{
		public String senderId { get; set; }
		public ApplicationUser sender { get; set; }

		public String receiverId { get; set; }
		public ApplicationUser receiver { get; set; }

		public DateTime? date { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public string type { get; set; }
        public int? typeMasterId { get; set; }
        public int isRead { get; set; }
		public string url { get; set; }
		public int status { get; set; }
		public string statusName { get; set; }
		public string remarks { get; set; }
		//isDelete = RegisterId
	}
}
