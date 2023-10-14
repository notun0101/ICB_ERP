using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
 
    public class NaturalDisasterViewModel
    {
        public int naturaldisasterId { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public IEnumerable<NaturalDigester> naturalDigesters { get; set; }
    }
}
