using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class BranchTypeViewModel
    {
        public int BranchTypeId { get; set; }
        public string name { get; set; }
        public int? status { get; set; }
        public IEnumerable<HrBranchType> hrBranchTypes { get; set; }

    }
}
