using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class HrCommunicationViewModel
    {
        public int CommunicationId { get; set; }
        public int? employeeId { get; set; }
        public string refNo { get; set; }
        public string email { get; set; }
        public DateTime? date { get; set; }

        public int? status { get; set; }
        public string remarks { get; set; }
        public IEnumerable<HrCommunication> hrCommunications { get; set; }
        

    }
}
