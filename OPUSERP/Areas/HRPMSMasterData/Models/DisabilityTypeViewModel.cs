using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class DisabilityTypeViewModel
    {
        public int? DisabilityTypeId { get; set; }

        
        public string DisabilityTypeName { get; set; }
        public string remarks { get; set; }


        public IEnumerable<DisabilityType> disabilityTypes { get; set; }
       
    }
}
