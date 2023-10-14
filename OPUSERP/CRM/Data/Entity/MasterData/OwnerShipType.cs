using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.CRM.Data.Entity.MasterData
{
    [Table("OwnerShipType")]
    public class OwnerShipType : Base
    {
        [Required]
        public string ownerShipTypeName { get; set; }
    }
}
