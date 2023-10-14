using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.CRM.Data.Entity.MasterData
{
    [Table("Industry")]
    public class Industry : Base
    {
        [Required]
        public string industryName { get; set; }
    }
}
