using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class DepartmentViewModel
    {
        public string departmentId { get; set; }
        public string deptCode { get; set; }
        public string deptName { get; set; }
        public string deptNameBn { get; set; }
        public string shortName { get; set; }
        public DateTime? startDate { get; set; }

        public DepartmentInfoLn fLang { get; set; }
        public int? hrdivisionId { get; set; }
        public int? hrBranchId { get; set; }
        public int? status { get; set; }

        public IEnumerable<Department> departments { get; set; }
        public IEnumerable<HrDivision> hrDivisions { get; set; }
        public IEnumerable<HrBranch> hrBranch { get; set; }


    }
}
