using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Employee;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class PhotographViewModel
    {
        public int employeeID { get; set; }

        public int photographID { get; set; }

        [Display(Name = "Employee Photo")]
        public IFormFile empPhoto { get; set; }

        public Photograph photograph { get;set; }

        public EmployeeSignature employeeSignature { get;set; }

        public WagesPhotograph wagesPhotograph { get;set; }

        public string employeeNameCode { get; set; }

        public EmployeeInfoLn fLang { get; set; }

        public EmployeeInfo employeeInfo { get; set; }

        public WagesEmployeeInfo wagesEmployeeInfo { get; set; }
        public IFormFile banglaSign { get; set; }  //bangla sign
        public string visualEmpCodeName { get; set; }
        public string queryString { get; set; }

        public int addresse { get; set; }
        public int spousee { get; set; }
        public int childrene { get; set; }
        public int emergencye { get; set; }
        public int nomineee { get; set; }
        public int insurancee { get; set; }
        public int educatione { get; set; }
        public int profqualificatione { get; set; }
        public int otherqualificatione { get; set; }
        public int traininge { get; set; }
        public int servicee { get; set; }
        public int promotione { get; set; }
        public int pmse { get; set; }
        public int diciplinee { get; set; }
        public int supervisore { get; set; }
        public int drivinge { get; set; }
        public int passporte { get; set; }
        public int travele { get; set; }
        public int membere { get; set; }
        public int awarde { get; set; }
        public int publicatione { get; set; }
        public int languagee { get; set; }
        public int otheractivitiese { get; set; }
        public int accountse { get; set; }
        public int belongingse { get; set; }
        public int prevempe { get; set; }
        public int freedome { get; set; }
        public int refe { get; set; }
        public int officeassigne { get; set; }
        public int picturee { get; set; }
        public int signe { get; set; }
        public int contracte { get; set; }
        public int projecte { get; set; }
        public int finantiale { get; set; }
        public int attatchmente { get; set; }
        public int proassigne { get; set; }
        public int otherinfoe { get; set; }
        public int costcentere { get; set; }
        public int empgradee { get; set; }
        public int shiftassigne { get; set; }
        public int leavestatuse { get; set; }
        public int sociale { get; set; }
        public int ieltse { get; set; }
        public int addinfoe { get; set; }
        public int emphobbye { get; set; }
        public int copliteracye { get; set; }
        public int diplomae { get; set; }
        public int taxe { get; set; }
        public int foode { get; set; }
        public int duale { get; set; }
        public int Sport { get; set; }
        public int Rolee { get; set; }
        public int diseases { get; set; }
        public int benifits{ get; set; }
      
        public int suspensions { get; set; }
        public int allegations { get; set; }
        public int employeeExtraCurricular { get; set; }

        public IEnumerable<EmployeePostingPlace> employeePostings { get; set; }

    }
}
