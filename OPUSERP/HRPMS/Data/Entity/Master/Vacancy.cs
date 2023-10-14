using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("Vacancy")]
    public class Vacancy : Base
    {
        public int? designationId { get; set; }
        public Designation designation { get; set; }

        public int? type { get; set; } //3=None(Direct Assign), 2=Recruitment, 3=Promotion

        public int? sanction { get; set; }
        public int? existing { get; set; }
        public int? vacancy { get; set; }

        public int? status { get; set; }
        public string remarks { get; set; }
    }
}
