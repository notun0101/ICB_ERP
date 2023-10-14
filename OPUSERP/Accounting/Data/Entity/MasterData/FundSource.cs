using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.Accounting.Data.Entity.MasterData
{
    [Table("FundSource")]
    public class FundSource : Base
    {
        public int? companyId { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string sourceName { get; set; }
    }
}
