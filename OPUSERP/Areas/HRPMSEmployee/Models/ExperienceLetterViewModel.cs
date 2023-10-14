using Microsoft.AspNetCore.Http;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class ExperienceLetterViewModel
    {
        public int ExpLetterId { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public string Designation { get; set; }
        public string Depeartment { get; set; }
        public DateTime? Date { get; set; }
        public IFormFile ExprienceLetterPhoto { get; set; }

        public IEnumerable<ExperianceLetter> ExprienceLetterEmployeeInfos { get; set; }
    }
}
