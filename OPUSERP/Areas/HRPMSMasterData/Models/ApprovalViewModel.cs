using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class ApprovalViewModel
    {
        public int approvalMasterId { get; set; }
        public int? employeeInfoId { get; set; }
        public int? approvalTypeId { get; set; }
		public EmployeeInfo employeeInfo { get; set; }
        public int employeeID { get; set; }

        public int?[] approverId { get; set; }
		public int?[] canFinalApprover { get; set; }
        public int?[] sortOrder { get; set; }
        public string[] status { get; set; }
        public string queryString { get; set; }

        public IEnumerable<ApprovalMaster> approvalMasters { get; set; }
        public IEnumerable<ApprovalType> approvalTypes { get; set; }

        public string visualEmpCodeName { get; set; }
		public ApprovarsWithEmp approvarsWithEmp { get; set; }
        public Photograph photograph { get; set; }
    }
	public class ApprovarsWithEmp
	{
		public int EmpId { get; set; }
		public int masterId { get; set; }
		public string EmpCode { get; set; }
		public string EmpName { get; set; }
		public IEnumerable<ApprovarsViewModel> approvarsViewModels { get; set; }
	}
	public class ApprovarsViewModel
	{
		public int userId { get; set; }
		public string name { get; set; }
		public int sortOrder { get; set; }
		public int isFinalApprovar { get; set; }
    }    
}
