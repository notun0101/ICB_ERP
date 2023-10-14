using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Disciplinary
{
    [Table("Offense")]
    public class Offense:Base
    {
        public string offense { get; set; }

        public string description { get; set; }
    }
}
