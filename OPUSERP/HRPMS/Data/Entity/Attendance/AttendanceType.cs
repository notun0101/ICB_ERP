
using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Attendance
{
    [Table("AttendanceType")]
    public class AttendanceType: Base
    {
        public string Name { get; set; }
    }
}
