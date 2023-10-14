using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("ApprovalType")]
    public class ApprovalType : Base
    {
        public string approvalTypeName { get; set; }
    }
}
