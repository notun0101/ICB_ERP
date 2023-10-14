using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("TravelPurpose")]
    public class TravelPurpose:Base
    {
        [Required]
        public string purposeName { get; set; }
        public string purposeNameBn { get; set; }

        public string purposeShortName { get; set; }
    }
}
