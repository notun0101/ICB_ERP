using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("NomineeFund")]
    public class NomineeFund:Base
    {
        public string name { get; set; }
    }
}
