using Microsoft.AspNetCore.Http;
using OPUSERP.CRM.Data.Entity.MasterData;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class AtmboothViewModel
    {
        public int atmId { get; set; }
        public int? branchId { get; set; }
        public HrBranch branch { get; set; }

        public int? subBranchId { get; set; }
        public HrSubBranch subBranch { get; set; }

        public string location { get; set; }
        public int? noOfMachine { get; set; }

        public string contactPerson { get; set; }
        public string contactNo { get; set; }

        public int? status { get; set; }
        public IEnumerable<HrAtmBooth> hrAtmBooths { get; set; }

        public IEnumerable<HrBranch> hrBranches { get; set; }
        public IEnumerable<HrSubBranch> hrSubBranches { get; set; }

    }
}
