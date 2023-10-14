using Microsoft.AspNetCore.Http;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class TransferLogViewModel
    {
        public string employeeID { get; set; }

        public string transfarID { get; set; }

        public string workStation { get; set; }

        public DateTime? fromDate { get; set; }

        public DateTime? toDate { get; set; }
        public DateTime? lastJoinDate { get; set; }

        public int? grade { get; set; }

        
        public string designation { get; set; }
        public int? designationId { get; set; }
        public int? departmentId { get; set; }

        public int? toBranchId { get; set; }
        public int? hrDivisionId { get; set; }

        public int? status { get; set; } //1=apply, 2=joined

        public string employeeNameCode { get; set; }

        public IFormFile transferattach { get; set; }
        public string url { get; set; }

        public IFormFile Clearanceattach { get; set; }
        public string clearenceUrl { get; set; }

        public string reportDateBn { get; set; }
        public string banglaDate { get; set; }
        public string banglaMonth { get; set; }
        public string banglaYear { get; set; }
        public DateTime? reportDateEn { get; set; }
        public string substitutionEmp { get; set; }

        public Photograph photograph { get; set; }
        public DateTime? orderDate { get; set; }
        public string orderNo { get; set; }

        public EmployeeInfo employeeInfo { get; set; }
        public EmployeeInfo employeeInfotransfer { get; set; }
        public EmployeeInfo employeeInfoSig { get; set; }

        public TransferLogLn fLang { get; set; }

        public IEnumerable<SalaryGrade> salaryGrade { get; set; }

        public IEnumerable<TransferLog> transferLogs { get; set; }
        public TransferLog transferLog { get; set; }
        public IEnumerable<Designation> designations { get; set; }
        public IEnumerable<Department> departments { get; set; }
        public IEnumerable<HrBranch> hrBranches { get; set; }
        public IEnumerable<HrDivision> hrDivisions { get; set; }

        public EmployeeInfo joiningSignatory { get; set; }
        public string releaseSignatory { get; set; }



    }
}
