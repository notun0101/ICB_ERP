using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class ClientServeLostViewModel
    {
        public int ClientServeLostId { get; set; }
        public string year { get; set; }
        public int? clientServe { get; set; }
        public int? clientLost { get; set; }

        public string businessTarget { get; set; }
        public string businessGrowth { get; set; }

        public string profit { get; set; }
        public string dividend { get; set; }

        public IEnumerable<ClientServeLost> clientServeLosts { get; set; } 
    }
}
