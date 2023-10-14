using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("HrAtmBooth")]
    public class HrAtmBooth:Base
    {
        public int? branchId { get; set; }
        public HrBranch branch { get; set; }

        public int? subBranchId { get; set; }
        public HrSubBranch subBranch { get; set; }

        public string location { get; set; }
        public int? noOfMachine { get; set; }

        public string contactPerson { get; set; }
        public string contactNo { get; set; }

        public int? status { get; set; }
    }
}
