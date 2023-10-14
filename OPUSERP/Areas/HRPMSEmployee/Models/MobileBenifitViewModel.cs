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
    public class MobileBenifitViewModel
    {
        public string MobileBenifitID { get; set; }
        public string employeeId { get; set; }       
        public string type { get; set; } //Mobile Data, Voice Call, Broadband
        public decimal? amount { get; set; }
        public DateTime? date { get; set; }
        public int? status { get; set; }
        public Photograph photograph { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public MobileBenifit  mobileBenifit { get; set; }
        public IEnumerable<MobileBenifit>  mobileBenifits  { get; set; }
      
    }
}
