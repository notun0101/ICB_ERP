using Microsoft.AspNetCore.Http;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class HandoverTakeoverMasterViewModel
    {
        public int? HTMasterId { get; set; }
        public int? handoverId { get; set; }
        public int? takeoverId { get; set; }
        public DateTime? date { get; set; }

        public int HTDetailsId { get; set; }
        public string taskName { get; set; }
        public string taskDetails { get; set; }

        public int? status { get; set; } //1=ongoing
        public IFormFile file { get; set; }
        public string comments { get; set; }

        public IEnumerable<HandoverTakeoverMaster> hTMasters { get; set; }
        public IEnumerable<HandoverTakeoverDetails> hTDetails { get; set; }
    }
}
