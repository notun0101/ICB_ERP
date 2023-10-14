using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.Data.Entity.Matrix
{
    [Table("ApproverType")]
    public class ApproverType:Base
    {
        public string approverTypeName { get; set; }
        public int? shortOrder { get; set; }
    }
}
