using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class ExpertiseViewModel
    {
        public int expertiseid { get; set; }
        public string nameEn { get; set; }
        public string nameBn { get; set; }
        public string remarks { get; set; }

        public IEnumerable<EmpExpertise> empExpertises { get; set; }

    }
}
