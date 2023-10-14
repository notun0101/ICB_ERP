using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
    [Table("MobileBenifit")]
    public class MobileBenifit:Base
    {
		public int employeeId { get; set; }
		public EmployeeInfo employee { get; set; }

		public string type { get; set; } //Mobile Data, Voice Call, Broadband
        public decimal? amount { get; set; }
        public DateTime? date { get; set; }

        public int? status { get; set; }
    }
}
