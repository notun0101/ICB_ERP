using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class HrDivisionViewModel
    {
        public int divId { get; set; }
        public string divCode { get; set; }

        [Required]
        public string divName { get; set; }
        public string divNameBn { get; set; }

        public string shortName { get; set; }
        public DateTime? startDate { get; set; }
        public IEnumerable<HrDivision> hrDivisions { get; set; }
        public int? functionId { get; set; }  //officeId
        public int? hrBranchId { get; set; }
        public IEnumerable<FunctionInfo> functionInfos { get; set; }
        public IEnumerable<HrBranch> hrBranch { get; set; }
		public int? status { get; set; }

	}
}
