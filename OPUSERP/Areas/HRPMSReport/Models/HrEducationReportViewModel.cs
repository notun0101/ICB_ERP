using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSReport.Models
{
    public class HrEducationReportViewModel
    {
        public Int64? rowSlNo { get; set; }
        public string employeeCode { get; set; }
        public string nameEnglish { get; set; }
        public string designation { get; set; }
        public string deptName { get; set; }
        public string branchUnitName { get; set; }
        public string gender { get; set; }
        public string subjectName { get; set; }
        public string resultMaxValue { get; set; }
        public string organizationName { get; set; }
        public string levelofeducationName { get; set; }

        public string qualification { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public DateTime? joiningDate { get; set; }
        public DateTime? lastPromotionDate { get; set; }
        public DateTime? lastTransferDate { get; set; }
        public DateTime? DateOfRetirement { get; set; }
        public string presentPosting { get; set; }
        public string homeDistrict { get; set; }
    }
}
