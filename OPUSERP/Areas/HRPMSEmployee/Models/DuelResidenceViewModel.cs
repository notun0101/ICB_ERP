using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Employee;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.HRPMS.Data.Entity.Master;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class DuelResidenceViewModel
    {
        public string DuelResidenceID { get; set; }
        public string employeeID { get; set; }
        public int? duelCountryId { get; set; }
        public string duelPassport { get; set; }
        public string employeeNameCode { get; set; }
        public Photograph photograph { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public DuelResidence duelResidence { get; set; }
        public IEnumerable<DuelResidence> duelResidences  { get; set; }
        public IEnumerable<Country> duelCountry { get; set; }
        
    }
}
