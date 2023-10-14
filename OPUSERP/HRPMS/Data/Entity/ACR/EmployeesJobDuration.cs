using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
    [Table("EmployeesJobDuration", Schema = "ACR")]
    public class EmployeesJobDuration:Base
    {
        [MaxLength(10)]
        public string empCode { get; set; }
        [MaxLength(4)]
        public string yearName { get; set; }
        [MaxLength(150)]
        public string JobDuration { get; set; }
        [MaxLength(150)]
        public string JobDurationAsBranchManger { get; set; }
        [MaxLength(150)]
        public string JobDurationAsZonalHead { get; set; }
        [MaxLength(150)]
        public string JobDurationAsOthers { get; set; }
    }
}
