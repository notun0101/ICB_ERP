using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
    [Table("AcrPromotion")]
    public class AcrPromotion : Base
    {
        public int? assessmentId { get; set; }
        public Assessment assessment { get; set; }
        [MaxLength(10)]
        public string EmpCode { get; set; }
        [MaxLength(8)]
        public string PromotionDate { get; set; }
        [MaxLength(4)]
        public string DesignationCode { get; set; }
    }
}
