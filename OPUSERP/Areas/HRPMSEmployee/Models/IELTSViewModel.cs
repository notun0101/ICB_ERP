using Microsoft.AspNetCore.Http;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.IELTS;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class IELTSViewModel
    {
        //public int employeeId { get; set; }
        public string ieltsId { get; set; }
        public string employeeID { get; set; }
        public Photograph photograph { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public string examType { get; set; } 
        public string centerNo { get; set; }
        public DateTime? date { get; set; }
        public string candidateNo { get; set; }

        public decimal? listeningScore { get; set; }
        public decimal? readingScore { get; set; }
        public decimal? writingScore { get; set; }
        public decimal? speakingScore { get; set; }
        public decimal? overallScore { get; set; }
        public decimal? cefrScore { get; set; }

        //public string attachment { get; set; }
        public IFormFile ieltsPhoto { get; set; }

        public int? status { get; set; }
     
        public IEnumerable<IeltsInfo> ieltsInfos { get; set; }
        public IEnumerable<IeltsInfo> ielts { get; set; }
        public IEnumerable<EmployeeType> employeeTypes { get; set; }
        public string employeeNameCode { get; set; }
        public string visualEmpCodeName { get; set; }
    }
}
