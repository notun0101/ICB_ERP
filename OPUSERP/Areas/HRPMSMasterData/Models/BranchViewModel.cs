using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class BranchViewModel
    {
        public string branchId { get; set; }
        public string branchCode { get; set; }
        public string branchName { get; set; }
        public string branchNameBn { get; set; }

        public string address { get; set; }

		public int? zoneId { get; set; }
        public int? officeId { get; set; }
        public int? status { get; set; }
      

        public string branchTypeId { get; set; }
        public HrBranchType branchType { get; set; }
        public IEnumerable<HrBranchType> hrBranchTypes { get; set; }

        public IEnumerable<HrBranch> hrBranches { get; set; }
        public IEnumerable<Location> zones { get; set; }
        public IEnumerable<FunctionInfo> offices { get; set; }
        public int? branchPlace { get; set; }
        public IEnumerable<BranchWithManagerViewModel> branchesWithManager { get; set; }
    }

    public class BranchWithManagerViewModel
    {
        public int? Id { get; set; }
        public string branchCode { get; set; }
        public string branchName { get; set; }
        public string branchNameBn { get; set; }
        public string address { get; set; }
        public string branchType { get; set; }
        public string branchPlace { get; set; }
        public int? branchPlaceId { get; set; }
        public string zoneName { get; set; }
        public string officeName { get; set; }
        public string branchManager { get; set; }
        public int? branchTypeId { get; set; }
        public int? locationId { get; set; }
        public int? officeId { get; set; }
        public int? isDelete { get; set; }
    }
}
