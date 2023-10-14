using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class EducationalQualificationViewModel
    {
        public int employeeId { get; set; }
        public int educationId { get; set; }        
        public string institution { get; set; }
        public int? resultId { get; set; }
        public string majorGroup { get; set; }        
        public string grade { get; set; }                
        public int? passingYear { get; set; }        
        public int? degreeId { get; set; }        
        public int? organizationId { get; set; }
        public int? reldegreesubjectId { get; set; }

        public string employeeNameCode { get; set; }
        public string levelofeducationName { get; set; }
        public string degreeName { get; set; }
        public string organizationName { get; set; }
        //[Required]
        public string organizationType { get; set; }
        public string subjectName { get; set; }
        public string resultName { get; set; }
        public int levelofeducationId { get; set; }
        public int subjectId { get; set; }
        public int degreeSubjectId { get; set; }

        public LevelofEducation levelofeducation { get; set; }
		public string duration { get; set; }

		public Photograph photograph { get; set; }
        public EmployeeInfo employeeInfo { get; set; }

        public EducationalQualificationLn fLang { get; set; }
        public IEnumerable<EducationalQualification> educationalQualifications { get; set; }
        public IEnumerable<LevelofEducation> levelofeducationNamelist { get; set; }
        public IEnumerable<Degree> degreeslist { get; set; }
        public IEnumerable<Subject> subjectslist { get; set; }
    }
}
