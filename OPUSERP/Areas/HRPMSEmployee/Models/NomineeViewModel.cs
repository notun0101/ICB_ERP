using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class NomineeViewModel
    {
        public int? employeeID { get; set; }
        public int? nomineeID { get; set; }
        public string name { get; set; }
        public string relation { get; set; }
        public string contact { get; set; }
        public string address { get; set; }
        public int?[] fundTypeList { get; set; }
        public decimal?[] fundValueList { get; set; }

        public string employeeNameCode { get; set; }
        public string guardianName { get; set; }
        public string witnessName { get; set; }
        public DateTime? nomineeDate { get; set; }

        public Photograph photograph { get; set; }
        public IFormFile nomineePhoto { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public string Designation { get; set; }
        public string Organization { get; set; }
        public string NID { get; set; }
        public string BRN { get; set; }
        public string witnessPhone { get; set; }

        public string relationName { get; set; }
        public string occupationName { get; set; }
        public IEnumerable<NomineeFund> nomineeFunds { get; set; }
        public IEnumerable<NomineeDetail> nomineeDetails { get; set; }
        public IEnumerable<Nominee> nominees { get; set; }
        public IEnumerable<Occupation> occupation { get; set; }
        public IEnumerable<Relation> relations { get; set; }
        public Nominee nominee { get; set; }
    }
}
