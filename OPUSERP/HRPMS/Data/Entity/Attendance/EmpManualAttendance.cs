using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Attendance
{
	[Table("EmpManualAttendance", Schema = "HR")]
	public class EmpManualAttendance : Base
	{
		[MaxLength(50)]
		public string punchCardNo { get; set; }
		[MaxLength(20)]
		public string workDate { get; set; }
		[MaxLength(20)]
		public string startTime { get; set; }
		[MaxLength(20)]
		public string endTime { get; set; }
		public int? summaryId { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal? workingTime { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal? latetime { get; set; }
		public string remarks { get; set; }
		[MaxLength(200)]
		public string attenUpdateBy { get; set; } //rejectedBy
		[MaxLength(200)]
		public string attenUpdateAppBy { get; set; } //approvedBy

        public string notes { get; set; } //reason for latearrival 

		public int? approveStatus { get; set; } //0=Pending, 1=Approve, 2=Reject

        public string location { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
		public string reasonforrejecting { get; set; }
        public int? ApproverId { get; set; }

        public int? AttendanceTypeId { get; set; }
        public AttendanceType AttendanceType { get; set; }
        
    }
}
