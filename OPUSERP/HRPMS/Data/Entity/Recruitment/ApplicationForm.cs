using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using OPUSERP.HRPMS.Data.Entity.Master;
using System.ComponentModel.DataAnnotations.Schema;
using OPUSERP.Data.Entity;
using OPUSERP.Payroll.Data.Entity.Salary;

namespace OPUSERP.HRPMS.Data.Entity.Recruitment
{
    [Table("ApplicationForm")]
    public class ApplicationForm : Base
    {
        public string applicationNo { get; set; }
        public string nameEN { get; set; }
        public string nameBN { get; set; }
        [MaxLength(100)]
        public string nidNO { get; set; }
        [MaxLength(100)]
        public string binNO { get; set; }
        public DateTime birthDate { get; set; }
        [MaxLength(100)]
        public string birtPlace { get; set; }
        public string payRef { get; set; }
        public string payDoc { get; set; } //file
        public string fNmaeEN { get; set; }
        public string fNmaeBN { get; set; }
        public string mNmaeBN { get; set; }
        public string mNmaeEN { get; set; }
        public string sNameEN { get; set; }
        public string sNameBN { get; set; }
        [MaxLength(50)]
        public string mobile { get; set; }
        [MaxLength(250)]
        public string email { get; set; }
        [MaxLength(50)]
        public string nationality { get; set; }

        public int? religionId { get; set; }
        public Religion religion { get; set; }

        public string occupation { get; set; }
        public string gender { get; set; }

        public string type { get; set; }

        public string applicantPhoto { get; set; }
        public string applicantSignature { get; set; }

        public DateTime? JoiningLetter { get; set; }
        public string attachmentUrl { get; set; }

        public int? status { get; set; } //applied = 1; accepted = 2; rejected = 3; returned = 4; selected = 5; sortlisted = 6; printed = 7; Joined = 8;

        public int? writtenMarks { get; set; }
        public int? vivaResult { get; set; }
        public int? totalMarks { get; set; }
        public string remarks { get; set; }

		public int? newDesignationId { get; set; }
		public Designation newDesignation { get; set; }

		public int? newBranchId { get; set; }
		public HrBranch newBranch { get; set; }

		public DateTime? joiningDate { get; set; }

		public string addressBangla { get; set; }
		public string postBangla { get; set; }
		public string mailingAddress { get; set; }

		public string termsAndConditions { get; set; }

		public int? salaryGradeId { get; set; }
		public SalaryGrade salaryGrade { get; set; }

        public string reportDateBn { get; set; }
        public DateTime? reportDateEn { get; set; }
    }
}
