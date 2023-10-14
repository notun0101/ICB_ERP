using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Disciplinary
{
    [Table("Disciplinary")]
    public class Disciplinary:Base
    {
        public int employeeId { get; set; }
    }
}
