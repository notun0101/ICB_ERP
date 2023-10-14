using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Recruitment
{
    [Table("ApplicantAddress")]
    public class ApplicantAddress:Base
    {
        public int? countryId { get; set; }

        public Country country { get; set; }

        public int? divisionId { get; set; }

        public Division division { get; set; }

        public int? districtId { get; set; }

        public District district { get; set; }

        public int? thanaId { get; set; }

        public Thana thana { get; set; }

        public string union { get; set; }

        public string postOffice { get; set; }

        public string postCode { get; set; }

        public string blockSector { get; set; }

        public string houseVillage { get; set; }

        public string roadNumber { get; set; }
        
        [Required]
        public string type { get; set; } //Type: Permamnent or Present

        public int applicationFormId { get; set; }
        public ApplicationForm applicationForm { get; set; }
    }
}
