using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSACR.Models
{
    public class UserProfileViewModel
    {
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string DesignationName { get; set; }
        public string DivisionName { get; set; }
        public string UserName { get; set; }
        public string userTypeName { get; set; }
        public string signaturePath { get; set; }
        public string picturePath { get; set; }
        public string JobDuration { get; set; }
        public string JobDurationAsBranchManger { get; set; }
        public string JobDurationAsZonalHead { get; set; }
        public string JobDurationAsOthers { get; set; }
        //public string JobDurationDataCard { get; set; }
        public string JobDurationAsBranchMangerDataCard { get; set; }
        public string JobDurationAsZonalHeadDataCard { get; set; }
        public string JobDurationAsOthersDataCard { get; set; }
        public int assignmentNo { get; set; }
    }
}
