using Microsoft.AspNetCore.Http;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class LisenceViewModel
    {
        public int LisenceId { get; set; }
        public string lisenceNo { get; set; }
        public DateTime? issueDate { get; set; }
        public DateTime? expDate { get; set; }
        public IFormFile attatchment { get; set; }
        public int? lisenceTypeId { get; set; }
        public string typeName { get; set; }
        public IEnumerable<LisenceType>  lisenceTypes { get; set; }
        public IEnumerable<Lisence>  lisences { get; set; }
        public Lisence lisence { get; set; }

    }
}
