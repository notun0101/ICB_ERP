using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
    [Table("EmployeeDisease", Schema = "HR")]
    public class EmployeeDisease:Base
    {
        public int employeeInfoId { get; set; }
        public EmployeeInfo employeeInfo { get; set; }

        public int? medicalDiseaseId { get; set; }
        public MedicalDisease medicalDisease { get; set; }

        public int? isActive { get; set; }
        public int? status { get; set; }
        public DateTime? date { get; set; }

		public int? hospitalised { get; set; } //1=yes, 0=no
		public int? underTreatment { get; set; } //1=yes, 0=no
		public int? vaccinated { get; set; } //1=yes, 0=no
		public string observation { get; set; }
	}
}
