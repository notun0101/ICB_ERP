using Microsoft.AspNetCore.Http;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Recruitment;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace OPUSERP.Areas.HRPMSRecruitment.Models
{
    public class ApplicationFormViewModel
    {
        public int ApplicationId { get; set; }
        public string nameEN { get; set; }
        public string nameBN { get; set; }
        public string nidNO { get; set; }
        public string binNO { get; set; }
        public DateTime birthDate { get; set; }
        public string birtPlace { get; set; }
        public string payRef { get; set; }
        public IFormFile payDoc { get; set; }
        public string fNmaeEN { get; set; }
        public string fNmaeBN { get; set; }
        public string mNmaeBN { get; set; }
        public string mNmaeEN { get; set; }
        public string sNameEN { get; set; }
        public string sNameBN { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string nationality { get; set; }
        public int? religionId { get; set; }
        public string occupation { get; set; }
        public string gender { get; set; }
        public string type { get; set; }
        public string applicantsNo { get; set; }
        public IFormFile applicantPhoto { get; set; }
        public IFormFile applicantSignature { get; set; }


        public DateTime? JoiningLetter { get; set; }
        public IFormFile JoiningAttachmentUrl { get; set; }

        public string reportDateBn { get; set; }
        public string banglaDate { get; set; }
        public string banglaMonth { get; set; }
        public string banglaYear { get; set; }
        public DateTime? reportDateEn { get; set; }
        public DateTime? joiningDate { get; set; }

        //new
        public int? status { get; set; } //applied = 1; accepted = 2; rejected = 3; returned = 4; selected = 5

        public int? writtenMarks { get; set; }
        public int? vivaResult { get; set; }
        public int? totalMarks { get; set; }
        public string remarks { get; set; }
        //Adress

        public int PresentAdressId { get; set; }
        public int PermanentAdressId { get; set; }
        public int? countryId { get; set; }
        public int? divisionId { get; set; }
        public int? districtId { get; set; }
        public int? thanaId { get; set; }
        public string union { get; set; }
        public string postOffice { get; set; }
        public string postCode { get; set; }
        public string blockSector { get; set; }
        public string houseVillage { get; set; }
        public string roadNumber { get; set; }
        public int? applicationFormId { get; set; }
        public int? applicationFormIdp { get; set; }
        public int? countryIdp { get; set; }
        public int? divisionIdp { get; set; }
        public int? districtIdp { get; set; }
        public int? thanaIdp { get; set; }
        public string unionp { get; set; }
        public string postOfficep { get; set; }
        public string postCodep { get; set; }
        public string blockSectorp { get; set; }
        public string houseVillagep { get; set; }
        public string roadNumberp { get; set; }
      
        

        //ApplicationEdu

        public int?[] EduId { get; set; }
        public string[] degree { get; set; }
        public string[] institution { get; set; }
        public string[] boardUniversity { get; set; }
        public string[] rollNo { get; set; }
        public string[] country { get; set; }
        public string[] yearOfCertification { get; set; }
        public string[] mainSubject { get; set; }
        public string[] gpaDivision { get; set; }
        public string[] type1 { get; set; }
        
        //ApplicationExp
        public int?[] ExpId { get; set; }
        public string[] officeName { get; set; }
        public string[] position { get; set; }
        public DateTime?[] from { get; set; }
        public DateTime?[] to { get; set; }
     

        //ApplicationQuota
        public int? QuotaIdr { get; set; } //1=yes, 0=no
        public int? isFredomFighter { get; set; } //1=yes, 0=no
        public string relation { get; set; }
        public IFormFile document { get; set; } //file
        public int? isDisable { get; set; } //1=yes, 0=no
        public int? isTrible { get; set; } //1=yes, 0=no
        public int? isAnser { get; set; } //1=yes, 0=no
        public int? isOther { get; set; } //1=yes, 0=no
        public string otherQuotaName { get; set; }
        public string otherQuotaDoc { get; set; } //file
        public int applicationFormId4 { get; set; }

        public IEnumerable<ApplicationForm> applicationForms { get; set; }
        public IEnumerable<ApplicantAddress> ApplicantAddress { get; set; }
        public IEnumerable<ApplicationEdu> ApplicationEdu { get; set; }
        public IEnumerable<ApplicationExp> ApplicationExp { get; set; }
        public IEnumerable<ApplicationQuota> ApplicationQuota { get; set; }
        public IEnumerable<Religion> Religion { get; set; }
        public IEnumerable<Country>  Countries { get; set; }
        public IEnumerable<Division> Divisions { get; set; }
        public IEnumerable<District> Districts { get; set; }
        public IEnumerable<Thana> Thanas { get; set; }
        public IEnumerable<SalaryGrade> salaryGrades { get; set; }


        public ApplicantAddress present { get; set; }
        public ApplicantAddress permanent { get; set; }
        public ApplicationEdu  applicationEdu { get; set; }
        public ApplicationExp  applicationExps { get; set; }
        public ApplicationQuota  applicationQuota { get; set; }
        public ApplicationForm applicationForm { get; set; }

        //AcceptedCandidate
        public string applicationID { get; set; }
        public string ApplicationNo { get; set; }
        public string ApplicantName { get; set; }
        public int? WrittenMarks { get; set; }
        public int? VivaMarks { get; set; }
        public int? TotalMarks { get; set; }
        public string ApplicantRemarks { get; set; }
        public int Id { get; set; }

        public int?[] Check { get; set; }



		public string addressBangla { get; set; }
		public string postBangla { get; set; }
		public string mailingAddress { get; set; }

		public int? salaryGradeId { get; set; }
		public string termsAndConditions { get; set; }

        public IEnumerable<HRPMSAttachment> hRPMSAttachments { get; set; }
    }
}
