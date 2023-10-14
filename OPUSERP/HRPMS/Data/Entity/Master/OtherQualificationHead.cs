using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("OtherQualificationHead")]
    public class OtherQualificationHead:Base
    {
        public string name { get; set; }
    }
}
