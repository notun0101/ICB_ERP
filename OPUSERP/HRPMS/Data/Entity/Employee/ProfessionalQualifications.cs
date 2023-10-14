using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
    [Table("ProfessionalQualifications")]
    public class ProfessionalQualifications:Base
    {
        public int employeeID { get; set; }
        public EmployeeInfo employee { get; set; }

        public int? qualificationHeadId { get; set; }
        public QualificationHead qualificationHead { get; set; }
		public string certificationName { get; set; }

		public string subject { get; set; }

        public int? resultId { get; set; }
        public Result result { get; set; }

        public string instituteName { get; set; }

        public string passingYear { get; set; }

        public string markGrade { get; set; }
    }
}
