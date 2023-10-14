using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.Accounting.Data.Entity.Voucher
{
    [Table("TransectionMode")]
    public class TransectionMode : Base
    {
        [Column(TypeName = "nvarchar(150)")]
        public string modeName { get; set; }

        public int? shortOrder { get; set; }
    }
}
