using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.Data.Entity.Matrix
{
    [Table("StatusInfo")]
    public class StatusInfo:Base
    {
        public string StatusName { get; set; }
        public int? sortOrder { get; set; }
    }
}
