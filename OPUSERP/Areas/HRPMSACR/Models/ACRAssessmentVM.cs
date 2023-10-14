using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.ACR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSACR.Models
{
    public class ACRAssessmentVM
    {
        public IEnumerable<AssessmentViewModel> lstRecom { get; set; }
        public IEnumerable<AssessmentViewModel> lstSign { get; set; }
        public IEnumerable<AssessmentInfoViewModel> lstFinal { get; set; }
        public IEnumerable<EmpPostingsSpViewModel> empPostings { get; set; }
        //2019
        public IEnumerable<AssessmentViewModel> lstRecom19 { get; set; }

        //public EmployeeUserViewModel ACRUsers { get; set; }
        public ApplicationUser ACRUsers { get; set; }
        public AssessmentViewModel AssessmentBasic { get; set; }
        public string acrYear { get; set; }
		public IEnumerable<AcrForAdminViewModel> assessments { get; set; }
		public IEnumerable<GetAllActiveEmployeesVm> AllEmployees { get; set; }
        public IEnumerable<Assessment>AddAssessments { get; set; }
	}
	public class ACRAssaignVm
	{
		public int?[] assId { get; set; }
		public string[] recommendator { get; set; }
		public string[] signatory { get; set; }
		public string[] signatory2 { get; set; }
	}
}
