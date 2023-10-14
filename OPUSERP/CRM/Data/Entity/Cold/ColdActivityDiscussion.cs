using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.CRM.Data.Entity.Cold
{
    [Table("ColdActivityDiscussion")]
    public class ColdActivityDiscussion : Base
    {
        public int coldActivityMastersId { get; set; }
        public ColdActivityMasters coldActivityMasters { get; set; }

    
        public string discussion { get; set; }
    }
}
