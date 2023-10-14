using Microsoft.AspNetCore.Http;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class BondLetterViewModel
    {


        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }
        public int bondId { get; set; }

        public DateTime? date { get; set; }
        public IFormFile imageUrl { get; set; }
        //public string fileUrl { get; set; }
        public string type { get; set; }

        public string letterNo { get; set; }

        public int? status { get; set; }
        public string remarks { get; set; }
        public BondLetter bondLetter { get; set; }
        public IEnumerable<BondLetter> bondLetters { get; set; }
        //public IEnumerable<BondLetter> BondLetterEmployeeInfos { get; set; }

    }
}
