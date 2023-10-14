using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.RetirementAndTermination
{
    [Table("ResignInformation")]
    public class ResignInformation:Base
    {
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public DateTime? resignDate { get; set; }

        public DateTime? lastWorkingDate { get; set; }
       
        [MaxLength(500)]
        public string resignReason { get; set; }

		public string url { get; set; }

		public int? nextApprover { get; set; } //userid
		public int? status { get; set; }
		public string type { get; set; }
		public string bodyText { get; set; }
	
		public string letterNo { get; set; }
		public string clearanceFile { get; set; }
        public DateTime? clearanceDate { get; set; }
    }
}
