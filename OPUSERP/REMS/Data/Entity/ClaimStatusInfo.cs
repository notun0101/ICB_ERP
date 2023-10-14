using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.REMS.Data.Entity
{
    [Table("ClaimStatusInfo")]
    public class ClaimStatusInfo:Base
    {
        public string statusName { get; set; }
    }
}
