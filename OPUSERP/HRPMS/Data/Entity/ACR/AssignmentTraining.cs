using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
    [Table("AssignmentTraining", Schema = "ACR")]
    public class AssignmentTraining:Base
    {
        public int? assessmentId { get; set; }
        public Assessment assessment { get; set; }
        public string EmpCode { get; set; }
        public int? Criteria { get; set; }
        public int? EntryNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string BranchCode { get; set; }
        

        public int? sbuId { get; set; }
        public SpecialBranchUnit sbu { get; set; }//BDBL & its subsidiary

        public int? branchId { get; set; }
        public HrBranch branch { get; set; }//BDBL Branch

        public int? divisionId { get; set; }
        public HrDivision division { get; set; }

        public int? departmentId { get; set; }
        public Department department { get; set; }

        public int? designationId { get; set; }
        public Designation designation { get; set; }

        public int? jobResponsibilityId { get; set; }
        public JobResponsibility jobResponsibility { get; set; }

        public string Remarks { get; set; }

        //OPTIONAL FIELD
        //public string CircleName { get; set; }
        //public string ZoneName { get; set; }
        //public string BranchName { get; set; }
        //public string DivisionName { get; set; }
        //public string DesignationName { get; set; }
        //public string PerDistrict { get; set; }

        //public string EmpName { get; set; }
        //public string BirthDate { get; set; }
        //public string JoiningDate { get; set; }
        //public int? Year { get; set; }
        //public int? Month { get; set; }
        //public int? Day { get; set; }
        //public string ResponsibilityName { get; set; }
    }
}
