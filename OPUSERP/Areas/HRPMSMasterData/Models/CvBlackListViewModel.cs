using Microsoft.AspNetCore.Http;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class CvBlackListViewModel
    {
        public int CvBlackListId { get; set; }
        public string sscRoll { get; set; }
        public string sscReg { get; set; }
        public DateTime? date { get; set; }
        public IFormFile attatchment { get; set; }
        public string reason { get; set; }
        public IEnumerable<CvBlackList>  cvBlackLists { get; set; }
        public CvBlackList cvBlackList { get; set; }

    }
}
