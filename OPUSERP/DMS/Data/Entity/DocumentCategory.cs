using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.DMS.Data.Entity
{
    [Table("DocumentCategory")]
    public class DocumentCategory : Base
    {
        [MaxLength(300)]
        public string categoryName { get; set; }
    }
}
