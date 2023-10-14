using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.CRM.Data.Entity.MasterData
{
    [Table("CRMActivityStatus")]
    public class ActivityStatus : Base
    {       
        [Required]
        public string status { get; set; }
    }
}
