using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class SubBranchViewModel
    {
        public string subbranchId { get; set; }
        public string subbranchCode { get; set; }
        public string subbranchName { get; set; }
        public string subaddress { get; set; }

        public int? subbranchTypeId { get; set; }
        public HrBranchType subbranchType { get; set; }

        public int? hrBranchId { get; set; }
   

        public int? status { get; set; }
        public int? isActive { get; set; }
        public string remarks { get; set; }
        
        public IEnumerable<HrBranchType> hrBranchTypes { get; set; }

        public IEnumerable<HrSubBranch> hrSubBranches { get; set; }
        public IEnumerable<HrBranch>  hrBranches { get; set; }



    }
}
