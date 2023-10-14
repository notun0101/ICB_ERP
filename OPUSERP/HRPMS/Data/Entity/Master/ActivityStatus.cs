using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("ActivityStatus")]
    public class ActivityStatus:Base
    {
        [Required]
        public string statusName { get; set; }
        public string statusNameBn { get; set; }

        public string shortName { get; set; }
    }
}
