using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("ShiftGroupMaster")]
    public class ShiftGroupMaster : Base
    {
        [Required]
        public string shiftName { get; set; }
        public string shiftNameBn { get; set; }
    }
}
