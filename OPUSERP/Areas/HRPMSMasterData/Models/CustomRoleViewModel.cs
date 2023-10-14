using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class CustomRoleViewModel
    {
        public int CustomRoleId { get; set; }
        public string roleName { get; set; }
        public string remarks { get; set; }

        public IEnumerable<CustomRole> customRoles { get; set; } 
    }
}
