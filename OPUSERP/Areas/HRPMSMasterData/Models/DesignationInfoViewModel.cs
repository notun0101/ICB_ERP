using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class DesignationInfoViewModel
    {
        //[Display(Name = "Employee Type")]
        //public int? empType { get; set; }

        public int designationId { get; set; }

        [Required]
        [Display(Name = "Designation Code")]
        public string designationCode { get; set; }

        [Required]
        [Display(Name = "Designation Name")]
        public string designationName { get; set; }

        public string designationNameBn { get; set; }

        public int? customRoleId { get; set; }
        public int? summaryRole { get; set; } //1=DGM - Above, 2=1st Class - Above, 3=2nd Class

        //[Required]
        [Display(Name = "Short Name")]
        public string shortName { get; set; }

        public DesignationLn fLang { get; set; }

        public IEnumerable<Designation> designations { get; set; }
        public IEnumerable<CustomRole> customRoles { get; set; }

    }
}
