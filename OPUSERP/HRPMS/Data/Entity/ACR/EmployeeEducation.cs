using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
    [Table("EmployeeEducation")]
    public class EmployeeEducation:Base
    {
        public int? assessmentId { get; set; }
        public Assessment assessment { get; set; }
        [MaxLength(10)]
        public string EmpCode { get; set; }
        [MaxLength(250)]
        public string LOECat { get; set; }
        [MaxLength(250)]
        public string UniversityName { get; set; }
        [MaxLength(150)]
        public string LavelOfEducation { get; set; }
        [MaxLength(150)]
        public string MajorGroup { get; set; }
        [MaxLength(4)]
        public string PassingYear { get; set; }
        [MaxLength(150)]
        public string ResultClass { get; set; }
        public int? SerialNo { get; set; }
    }
}
