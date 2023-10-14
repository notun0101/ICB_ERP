using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.TrainingNew
{
    [Table("TrainingHonorarium")]
    public class TrainingHonorarium:Base
    {
        public int? trainingId { get; set; }
        public TrainingInfoNew training { get; set; }

        public int? trainerId { get; set; }
        public ResourcePerson trainer { get; set; }


        public int? CoOrdinatorId { get; set; }
        public EmployeeInfo CoOrdinator { get; set; }
        public decimal? coOrdinatorPayment { get; set; }

        public decimal? receivedMoney { get; set; }
        public decimal? taxPercentage { get; set; }
        public decimal? taxAmount { get; set; }

        public int? session { get; set; }
        public DateTime? effectiveDate { get; set; }

        public int? status { get; set; }
        public int? isPaid { get; set; }
    }
}
