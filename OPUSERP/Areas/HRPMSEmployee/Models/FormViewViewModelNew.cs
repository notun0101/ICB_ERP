using Microsoft.AspNetCore.Http;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Areas.HRPMSPunishment.Models.Lang;
using OPUSERP.Areas.HRPMSReward.Models.Lang;
using OPUSERP.Areas.HRPMSTrainingNew.Models;
using OPUSERP.CRM.Data.Entity.MasterData;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.ACR;
using OPUSERP.HRPMS.Data.Entity.DisciplineInvestigation;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.HrComputerLiteracy;
using OPUSERP.HRPMS.Data.Entity.IELTS;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Organogram;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ACRLn = OPUSERP.Areas.HRPMSACR.Models.Lang.ACRLn;
using Address = OPUSERP.HRPMS.Data.Entity.Employee.Address;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class FormViewViewModelNew
    {
        public string employeeID { get; set; }
        public string EmpCode { get; set; }
        public string employeeCode { get; set; }
        public string nationalID { get; set; }
        public Photograph photograph { get; set; }
        public string birthIdentificationNo { get; set; }
        public string govtID { get; set; }
        public string gpfNomineeName { get; set; }
        public string gpfAcNo { get; set; }
        public string nameEnglish { get; set; }
        public string nameBangla { get; set; }
        public string motherNameEnglish { get; set; }
        public string motherNameBangla { get; set; }
		public int? statusbdbl { get; set; }
		public int? locationbdbl { get; set; }
		public int? designationbdbl { get; set; }
		public string motherNationality { get; set; }
        public string motherNID { get; set; }
        public string motherPassportNo { get; set; }
        public string motherPassportIssueDate { get; set; }
        public string motherPassportExDate { get; set; }
        public int? motherOccupationId { get; set; }
        public Occupation motherOccupation { get; set; }
        public string motherEmail { get; set; }
        public string motherMobile { get; set; }
        public string motherAddress { get; set; }
        public string fatherNameEnglish { get; set; }
        public string fatherNameBangla { get; set; }
        public string fatherNationality { get; set; }
        public string fatherNID { get; set; }
        public string fatherPassportNo { get; set; }
        public string fatherPassportIssueDate { get; set; }
        public string fatherPassportExDate { get; set; }
        public int? fatherOccupationId { get; set; }
        public Occupation fatherOccupation { get; set; }
        public string fatherEmail { get; set; }
        public string fatherMobile { get; set; }
        public string fatherAddress { get; set; }
        public string nationality { get; set; }
        public string disability { get; set; }
        public string Cdisability { get; set; }
        public DateTime? postingDate { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string gender { get; set; }
        public string birthPlace { get; set; }
        public string maritalStatus { get; set; }
        public int? religion { get; set; }
        public int? employeeType { get; set; }
        public int? activityStatus { get; set; }
        public int? department { get; set; }
        public string tin { get; set; }
        public string batch { get; set; }
        public int? sbu { get; set; }
        public int? pns { get; set; }
        public string bloodGroup { get; set; }
        public string freedomFighter { get; set; }
        public string freedomFighterNo { get; set; }
        public string telephoneOffice { get; set; }
        public string telephoneResidence { get; set; }
        public string pabx { get; set; }
        public string emailAddress { get; set; }
        public string mobileNumberOffice { get; set; }
        public string mobileNumberPersonal { get; set; }
        public string dateOfLPR { get; set; }
        public string emailAddressPersonal { get; set; }
        public DateTime? joiningDatePresentWorkstation { get; set; }
        public DateTime? joiningDateGovtService { get; set; }
        public DateTime? dateofregularity { get; set; }
        public DateTime? dateOfPermanent { get; set; }
        public DateTime? DateOfRetirement { get; set; }
        public string homeDistrict { get; set; }
        public string natureOfRequitment { get; set; }
        public string specialSkill { get; set; }
        public string seniorityNumber { get; set; }
        public string joiningDesignation { get; set; }
        public string designation { get; set; }
        public int? height { get; set; }
        public int? weight { get; set; }
        public string WhatsAppId { get; set; }
        public string skypeId { get; set; }
        public string FacebookId { get; set; }
        public string TwitterId { get; set; }
        public string InstagramId { get; set; }
        public string LinkedInId { get; set; }
        public int? bloodDonateStatus { get; set; } //0 or 1
        public IEnumerable<Country> countries { get; set; }
        public int? employeeStatusId { get; set; }
        public int? hrProgramId { get; set; }
        public int? hrUnitId { get; set; }
        public int? locationId { get; set; }
        public int? functionInfoId { get; set; }
        public string disablityType { get; set; }
        public string CdisablityType { get; set; }
        public int? salaryStatus { get; set; }
        public string salaryStatusComment { get; set; }
        public string BranchName { get; set; }
        public int? DivisionId { get; set; }
        public int? hrBranchId { get; set; }
        public int? hrDivisionId { get; set; }
        public string HrBranchTypeId { get; set; }
        public string action { get; set; }
        public EmployeeInfoLn fLang { get; set; }
        public RewardLn fLang2 { get; set; }
        public PunishmentLn fLang3 { get; set; }
        public ACRLn fLang4 { get; set; }
        public string occupationName { get; set; }
        public string designationName { get; set; }

        public int? JoinDesignationId { get; set; }
        public Designation JoinDesignation { get; set; }

        public string designationCode { get; set; }
        public string LocationName { get; set; }
		public string accountType { get; set; }
		public string branchCode { get; set; }
        public int? companyId { get; set; }
        public int? post { get; set; }//Hidden Field For Designation, Handle.
        public string employeeNameCode { get; set; }
        public string contructualType { get; set; }
        public int? empTypeId { get; set; }
        public int? shiftGroupId { get; set; }
        public int? isDelete { get; set; }
        public string joiningGradeName { get; set; }
        public string identificationSign { get; set; }
        public int? joiningGradeId { get; set; }
        public SalaryGrade joiningGrade { get; set; }
        public decimal? joiningBasic { get; set; }
        public string sbAccount { get; set; }
        public string PassportNo { get; set; }
        public string PassportIssueDate { get; set; }
        public string PassportExDate { get; set; }
        public string SupervisorCode { get; set; }
        public string SupervisorName { get; set; }

        public int? currentGradeId { get; set; }
        public SalaryGrade currentGrade { get; set; }
        public decimal? currentBasic { get; set; }

        public IEnumerable<DisabilityType> disabilityTypes { get; set; }
        public int? disabilityTypeId { get; set; }

        //posting
        public int? postplaceId { get; set; }
        public int? sbubdbl { get; set; }
        public int? departmentbdbl { get; set; }
        public int? branchbdbl { get; set; }
        public int? unitbdbl { get; set; }
        public int? officebdbl { get; set; }
        public int? divisionbdbl { get; set; }
        public DateTime? startbdbl { get; set; }
        public DateTime? endbdbl { get; set; }
        public string remarksbdbl { get; set; }
        public int? typebdbl { get; set; }
        public string placebdbl { get; set; }
        public string placebnbdbl { get; set; }
        public int? responsibilitybdbl { get; set; }


        //expertise
        public int? expertiseId { get; set; }
        public EmpExpertise expertise { get; set; }

        public int? isManager { get; set; }//0=no; 1=yes;

      

        public AllEmployeeJson allEmployeeJson { get; set; }
        // public IEnumerable<Company> companies { get; set; }
        public string visualEmpCodeName { get; set; }

        public DateTime? lastPromotionDate { get; set; }
        public DateTime? prlDate { get; set; }
        public string refLetterNo { get; set; }
        public DateTime? lastWorkingDate { get; set; }
        public int? lastDesignationId { get; set; }
        public int? ApplicantId { get; set; }
        public decimal? lastSalary { get; set; }
        public string prevOrganisationResp { get; set; } //textarea
        public string significantAchivement { get; set; } //textarea
        public string areaOfProficiency { get; set; } //textarea
        public string joiningLocation { get; set; } //dropdown
        public string placeOfPosting { get; set; } //dropdown
        public DateTime? probationStart { get; set; }
        public DateTime? probationEnd { get; set; }
        public DateTime? bondDateStart { get; set; }
        public DateTime? bondDateEnd { get; set; }

        public string fileNo { get; set; }
        public string presentPosting { get; set; }
        public DateTime? lastTransferDate { get; set; }
        public string qualification { get; set; }
        public string problem { get; set; }

        public int? role { get; set; }
        public int? eligible { get; set; }

    

        //previousjob

        public int?[] employeeId { get; set; }
        public int?[] employeeIdJH { get; set; }

        public string[] company { get; set; }
        public string[] position { get; set; }
        public int?[] jobtype { get; set; }
        public DateTime?[] jobstart { get; set; }
        public DateTime?[] jobend { get; set; }

		public string namepv { get; set; }
		public string positionpv { get; set; }
		public DateTime? startdatepv { get; set; }
		public DateTime? enddatepv { get; set; }
		public int? typepv { get; set; }
		public int? idpv { get; set; }
		public int? empidpv { get; set; }

		public IEnumerable<JobResponsibility> jobResponsibilities { get; set; }
        //BDBLjobhistory
        public int?[] postingID { get; set; }
        public int?[] employeeIdBDBL { get; set; }
        public string[] bdblSBU { get; set; }
        public string[] bdblDepartment { get; set; }
        public string[] bdblBranch { get; set; }
        public string[] bdblDivision { get; set; }
        public string[] bdblUnit { get; set; }
        public string[] bdblOffice { get; set; }
        public DateTime?[] bdblStartDatejs { get; set; }
        public DateTime?[] bdblEndDatejs { get; set; }
        public string[] bdblremarks { get; set; }
        public string[] bdbltype { get; set; }
        public string[] bdblplaceName { get; set; }
        public string[] bdblplaceNameBn { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public WagesEmployeeInfo wagesEmployeeInfo { get; set; }
        public Address Addresss { get; set; }

        //Approval
        public int approvalMasterId { get; set; }
        public int? employeeInfoId { get; set; }
        public int? approvalTypeId { get; set; }
        public int?[] approverId { get; set; }
        public int?[] canFinalApprover { get; set; }
        public int?[] sortOrder { get; set; }
        public string[] status { get; set; }
        public string queryString { get; set; }
        public ApprovarsWithEmp approvarsWithEmp { get; set; }
        public IEnumerable<ApprovalMaster> approvalMasters { get; set; }
        public IEnumerable<ApprovalType> approvalTypes { get; set; }
        public IEnumerable<IndTrainingReportSPViewModel> indTrainingReportSPs { get; set; }
        public class ApprovarsWithEmp
        {
            public int EmpId { get; set; }
            public int masterId { get; set; }
            public string EmpCode { get; set; }
            public string EmpName { get; set; }
            public IEnumerable<ApprovarsViewModel> approvarsViewModels { get; set; }
        }
        public class ApprovarsViewModel
        {
            public int userId { get; set; }
            public string name { get; set; }
            public int sortOrder { get; set; }
            public int isFinalApprovar { get; set; }
        }
        //Address
        public int presentAddressID { get; set; }
        public int permanentAddressID { get; set; }

        public string presentRoadNo { get; set; }

        public string permanentRoadNo { get; set; }

        //[Required]
        [Display(Name = "Present Division")]
        public string presentDivision { get; set; }

        //[Required]
        [Display(Name = "Present District")]
        public string presentDistrict { get; set; }

        //[Required]
        [Display(Name = "Present Upazila")]
        public string presentUpazila { get; set; }

        [Display(Name = "Present Union")]
        public string presentUnion { get; set; }


        [Display(Name = "Present Post Office")]
        public string presentPostOffice { get; set; }

        [StringLength(50, ErrorMessage = "The {0} Must be at least {2} and at most {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Present Post Code")]
        public string presentPostCode { get; set; }

        [Display(Name = "Present Block/Sector")]
        public string presentBlockSector { get; set; }

        [Display(Name = "Present House/Village")]
        public string presentHouseVillage { get; set; }

        //[Required]
        [Display(Name = "Permanent Division")]
        public string permanentDivision { get; set; }

        //[Required]
        [Display(Name = "Permanent District")]
        public string permanentDistrict { get; set; }

        //[Required]
        [Display(Name = "Permanent Upazila")]
        public string permanentUpazila { get; set; }

        [Display(Name = "Permanent Union")]
        public string permanentUnion { get; set; }

        [Display(Name = "Permanent Post Office")]
        public string permanentPostOffice { get; set; }

        [StringLength(50, ErrorMessage = "The {0} Must be at least {2} and at most {1} characters long.", MinimumLength = 3)]
        public string permanentPostCode { get; set; }


        [Display(Name = "Permanent Block/Sector")]
        public string permanentBlockSector { get; set; }


        [Display(Name = "Permanent House/Village")]
        public string permanentHouseVillage { get; set; }

        public string sameAddress { get; set; }


        public string prCountry { get; set; }
        public string prNo { get; set; }
        public int? countryId { get; set; }
        public IEnumerable<Country> Country { get; set; }


        public HRPMS.Data.Entity.Employee.Address present { get; set; }
        public HRPMS.Data.Entity.Employee.Address permanent { get; set; }

        public HRPMS.Data.Entity.Employee.WagesAddress wagesPresent { get; set; }
        public HRPMS.Data.Entity.Employee.WagesAddress wagesPermanent { get; set; }

        //spouse
        public string spouseID { get; set; }
        public string spouseName { get; set; }

        public string emailAddresssp { get; set; }

        public string spouseNameBN { get; set; }



        public DateTime? dateOfBirthsp { get; set; }
        public DateTime? dateOfMarriage { get; set; }

        public int? occupationId { get; set; }

        public string gendersp { get; set; }

        public string maritalstatusSpouse { get; set; }

        public string designationsp { get; set; }

        public string organization { get; set; }

        public string bin { get; set; }

        public string nid { get; set; }

        public string bloodGroupsp { get; set; }

        public string homeDistrictsp { get; set; }
        public string relationship { get; set; }
        public string nationalitysp { get; set; }


        [Display(Name = "Mobile Number")]
        public string contact { get; set; }

        public string higherEducation { get; set; }

        public IFormFile spousePhoto { get; set; }
        public IFormFile MarriagePhoto { get; set; }


        public Spouse spouse { get; set; }
        //Children
        public string childrenID { get; set; }
        public string childName { get; set; }
        public string childNameBN { get; set; }
        public DateTime? dateOfBirthcd { get; set; }
        public string education { get; set; }
        public string gendercd { get; set; }
        public string contactcd { get; set; }
        public string bincd { get; set; }
        public int? occupationcd { get; set; }
        public string designationcd { get; set; }
        public string organizationcd { get; set; }
        // public string nid { get; set; }
        public string bloodGroupcd { get; set; }
        public string presentEducation { get; set; }
        public string nationalitycd { get; set; }
        public string relationshipcd { get; set; }
        public string emailAddressPersonalcd { get; set; }
        public int disabilitycd { get; set; }
        public string disablityTypecd { get; set; }
        public string nidcd { get; set; }
        public int? childNo { get; set; }
        public string maritalstatuscd { get; set; }

        public int[] childEduId { get; set; }
        public string[] institution { get; set; }

        public int[] degreeId { get; set; }
        public int[] levelOfEducationId { get; set; }

        public string[] majorSubject { get; set; }
        public string[] passingYear { get; set; }

        public string[] resultType { get; set; } //1st, 2nd, 3rd Class, Grade
        public string[] result { get; set; }
        public IFormFile childrenPhoto { get; set; }
        public IEnumerable<Children> children { get; set; }
        public Children child { get; set; }
        public IEnumerable<Occupation> choccupations { get; set; }
        public IEnumerable<Degree> degrees { get; set; }
        public IEnumerable<LevelofEducation> levelofEducations { get; set; }
        public LevelofEducation levelOfEducation { get; set; }

        //Emergency Contact

        public int? refID { get; set; }
        public string refName { get; set; }
        public string refRelation { get; set; }
        public string refOrganization { get; set; }
        public string refDesignation { get; set; }
        public string refEmail { get; set; }
        public string refContact { get; set; }
        public string EMOccupation { get; set; }

        [Display(Name = "Employee Photo")]
        public IFormFile empPhoto { get; set; }

        public string OfficeAddress { get; set; }
        public string HomeAddress { get; set; }
        public EmergencyContact emergencyContact { get; set; }

        //Nominee

        public int? nomineeID { get; set; }
        public string nameNominee { get; set; }
        public string relation { get; set; }
        public string contactnominee { get; set; }
        public string addressNomi { get; set; }
        public int?[] fundTypeList { get; set; }
        public decimal?[] fundValueList { get; set; }

        public string guardianName { get; set; }
        public string witnessName { get; set; }
        public DateTime? nomineeDate { get; set; }

        public IFormFile nomineePhoto { get; set; }
        public string EmailNominee { get; set; }
        public string OccupationNominee { get; set; }
        public string DesignationNomi { get; set; }
        public string OrganizationNomi { get; set; }
        public string NID { get; set; }
        public string BRN { get; set; }
        public string witnessPhone { get; set; }

        public string relationName { get; set; }
        //public string occupationName { get; set; }
        public IEnumerable<NomineeFund> nomineeFunds { get; set; }
        public IEnumerable<NomineeDetail> nomineeDetails { get; set; }
        public IEnumerable<Nominee> nominees { get; set; }
        public IEnumerable<Result> results { get; set; }
        public IEnumerable<ProfessionalQualifications> professionalQualifications { get; set; }
        public IEnumerable<QualificationHead> qualificationHeads { get; set; }
        public Nominee nominee { get; set; }

        //insurance
        public int? insuranceID { get; set; }

        public string nameins { get; set; }
        public string relationins { get; set; }
        public string NIDins { get; set; }
        public string age { get; set; }
        public string genderins { get; set; }
        public DateTime? dateOfBirthins { get; set; }
        public DateTime? insuranceDate { get; set; }
        public string imageUrl { get; set; }

        //public IFormFile fileUrl { get; set; }
        public IFormFile insurancephoto { get; set; }

        //Education Table

        public int educationId { get; set; }
        public string institutionedu { get; set; }
        public int? resultId { get; set; }
        public string majorGroup { get; set; }
        public string grade { get; set; }
        public int? passingYearedu { get; set; }
        public int? degreeIdedu { get; set; }
        public int? organizationId { get; set; }
        public int? reldegreesubjectId { get; set; }

        public string levelofeducationName { get; set; }
        public string degreeName { get; set; }
        public string organizationName { get; set; }
        //[Required]
        public string organizationType { get; set; }
        public string subjectName { get; set; }
        public string resultName { get; set; }
        public int levelofeducationId { get; set; }
        public int subjectId { get; set; }
        public string subject { get; set; }
        public int degreeSubjectId { get; set; }

        public LevelofEducation levelofeducation { get; set; }
        public string duration { get; set; }

        public IEnumerable<EducationalQualification> educationalQualifications { get; set; }
        public IEnumerable<LevelofEducation> levelofeducationNamelist { get; set; }
        public IEnumerable<Degree> degreeslist { get; set; }
        public IEnumerable<Subject> subjectslist { get; set; }

        //Training History
        public int trainingLogID { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string country { get; set; }
        public int? category { get; set; }
        public string trainingTitle { get; set; }
        public int? institute { get; set; }
		public string instituteName { get; set; }
		public string sponsoringAgency { get; set; }
        public string remarks { get; set; }
        public string remarktraining { get; set; }
        public string trainingCategoryName { get; set; }
        public string trainingInstituteName { get; set; }
        public IEnumerable<TrainingCategory> trainingCategories { get; set; }
        public IEnumerable<TrainingInstitute> trainingInstitutes { get; set; }
        public IEnumerable<TraningLog> traningLogs { get; set; }

        //Promotion History
        public string promotionId { get; set; }

        public string designationpm { get; set; }

        public int? designationNewId { get; set; }

        public int? designationOldId { get; set; }

        [Display(Name = "Promotion Date")]
        public DateTime date { get; set; }

        public string payScale { get; set; }

        public string nature { get; set; }

        public string basic { get; set; }

        public string rank { get; set; }

        public string remark { get; set; }

        public string goNumber { get; set; }

        public DateTime? goDate { get; set; }

        public IEnumerable<PromotionLog> promotionLogs { get; set; }
        public PromotionLog promotions { get; set; }
        public Designation design { get; set; }

        //Disciplinary Action
        public int disciplinaryId { get; set; }

        public string employeeName { get; set; }

        public int? offenseId { get; set; }
		public string offenceName { get; set; }
		public string naturalPunishmentName { get; set; }


		public string punishmentId { get; set; }

        public DateTime? punishmentDate { get; set; }

        public DateTime? startFrom { get; set; }

        public DateTime? endTo { get; set; }

        public string goNo { get; set; }

        //[Display(Name = "GO Attachment")]
        public IFormFile goAttachments { get; set; }
        //public string goAttachmentExist { get; set; }
        public string remarksdis { get; set; }
        public IEnumerable<DisciplinaryLog> disciplinaries { get; set; }
        public IEnumerable<Offense> offenses { get; set; }
        public IEnumerable<NaturalPunishment> punishments { get; set; }

        //License
        public string licenseId { get; set; }
        [Display(Name = "License Number")]
        public string licenseNumber { get; set; }
        [Display(Name = "Place of Issue")]
        public string place { get; set; }
        [Display(Name = "Date Of Issue")]
        public DateTime? dateOfIssue { get; set; }
        [Display(Name = "Date Of Expiry")]
        public DateTime? dateOfExpair { get; set; }
        public string licenseCategory { get; set; }
        public IFormFile licenseUrl { get; set; }
        public IEnumerable<DrivingLicense> licenses { get; set; }

        //Passport
        public string passportId { get; set; }
        public string nameInPassport { get; set; }
        [Display(Name = "Passport Number")]
        public string passPortNumber { get; set; }

        [Display(Name = "Place of Issue")]
        public string placepp { get; set; }

        [Display(Name = "Date Of Issue")]
        public DateTime? dateOfIssuepp { get; set; }

        [Display(Name = "Date Of Expair")]
        public DateTime? dateOfExpairpp { get; set; }
        public IFormFile passportPhoto { get; set; }

        public IEnumerable<PassportDetails> passportDetails { get; set; }

        //Travel
        public int travelId { get; set; }

        [Display(Name = "Tour Type")]
        public string type { get; set; }

        [Display(Name = "Tour purpose")]
        public string purpose { get; set; }

        [Display(Name = "Tour location")]
        public string location { get; set; }


        [Display(Name = "Sponsoring Agency")]
        public string sponsoringAgencytr { get; set; }


        [Display(Name = "Start Date")]
        public DateTime? startDate { get; set; }


        [Display(Name = "End Date")]
        public DateTime? endDate { get; set; }

        [Display(Name = "Go Number")]
        public string goNumbertr { get; set; }

        [Display(Name = "Go Date")]
        public DateTime? goDatetr { get; set; }

      

        [Display(Name = "Title Of File")]
        public string titleOfFile { get; set; }
        public IFormFile goAttachmenttr { get; set; }
        public string goAttachmentExisttr { get; set; }
        [Display(Name = "Remarks")]
        public string remarkstr { get; set; }

        public string titleName { get; set; }
        public string accountCode { get; set; }
        public DateTime? leaveStartDate { get; set; }
        public DateTime? leaveEndDate { get; set; }

        public IEnumerable<TraveInfo> traveInfos { get; set; }

        public IEnumerable<TravelPurpose> travelPurposes { get; set; }

        //Certification
        public int? qualificationID { get; set; }
        public int? qualificationHeadId { get; set; }
        public string qualificaitonsubject { get; set; }
        public int? qualificationResultId { get; set; }
        public string qinstituteName { get; set; }
        public string qpassingYear { get; set; }
        public string qmarkGrade { get; set; }

        //Membership

        public int employeeMembershipID { get; set; }
        public string membershipNo { get; set; }
        public string remarksmm { get; set; }
        public int? membershipId { get; set; }
        public MembershipOrganization membership { get; set; }
        public string nameOrganization { get; set; }

        public IEnumerable<EmployeeMembership> employeeMemberships { get; set; }
        public IEnumerable<HRPMS.Data.Entity.Master.Membership> memberships { get; set; }
        public IEnumerable<MembershipOrganization> membershipOrganization { get; set; }
        public string membershipType { get; set; }

        //Award

        public string awardId { get; set; }
        public int? awardd { get; set; }
        public IEnumerable<OPUSERP.HRPMS.Data.Entity.Master.Award> awardlist { get; set; }

        [Display(Name = "Perpose")]
        public string perpose { get; set; }

        [Display(Name = "Date")]
        public DateTime? txtAwardDate { get; set; }
        public IFormFile awardPhoto { get; set; }
        public IEnumerable<EmployeeAward> awards { get; set; }

        //Language
        public string languageId { get; set; }
        [Display(Name = "Language")]
        public string language { get; set; }
        [Display(Name = "Proficiency")]
        public string proficiency { get; set; }
        public string reading { get; set; }
        public string writing { get; set; }
        public string speaking { get; set; }
        public string listening { get; set; }
        public IEnumerable<EmployeeLanguage> employeeLanguages { get; set; }
        public IEnumerable<Language> languages { get; set; }

        //Freedom fighter
        public int? FFID { get; set; }
        public string ffNo { get; set; }
        public string owner { get; set; }
        public string relationShip { get; set; }
        public string sectorNo { get; set; }
        public string remarkff { get; set; }

        //BankACC
        public int bankInfoId { get; set; }
        public int? fnCodeId { get; set; }
        public int? bankId { get; set; }
        public string fnCode { get; set; }
        public string bankName { get; set; }
        public string branchName { get; set; }
        public string accountNumber { get; set; }
        public int? walletTypeId { get; set; }
        public string walletNumber { get; set; }

        public string ibus { get; set; }
        public IEnumerable<BankInfo> bankInfos { get; set; }
        public IEnumerable<WagesBankInfo> wagesBankInfos { get; set; }
        public IEnumerable<FinanceCode> financeCodes { get; set; }
        public IEnumerable<Bank> banks { get; set; }
        public IEnumerable<WalletType> walletTypes { get; set; }

        //Reference
        public int? refIDR { get; set; }
        public string refNameR { get; set; }
        public string refOrganizationR { get; set; }
        public string refDesignationR { get; set; }
        public string refEmailR { get; set; }
        public string refContactR { get; set; }

        //profilephoto
        public int photographID { get; set; }
        //signature
        public string signatureid { get; set; }
        public EmployeeSignature employeeSignature { get; set; }
        public IFormFile EnglishPhoto { get; set; }
        public IFormFile BanglaPhoto { get; set; }
        public WagesPhotograph wagesPhotograph { get; set; }

        //public IFormFile banglaSign { get; set; }  //bangla sign
        //public IFormFile englishSign { get; set; }  //english sign


        public IEnumerable<HRPMS.Data.Entity.Employee.Reference> references { get; set; }
        public IEnumerable<HRPMS.Data.Entity.Employee.WagesReference> wagesReferences { get; set; }

        public FreedomFighter freedomFighterff { get; set; }
        public IEnumerable<FreedomFighter> freedomFighters { get; set; }
        //Attachment
        public int attachmentId { get; set; }

        public int? atttachmentCategoryId { get; set; }
        public IFormFile fileUrlAttachment { get; set; }

        public string fileTitle { get; set; }

        public string remarksat { get; set; }
        public DateTime? attachmentDate { get; set; }

        public string groupName { get; set; }
        public int? atttachmentGroupId { get; set; }
        public string categoryName { get; set; }
        public IEnumerable<AtttachmentGroup> atttachmentGroups { get; set; }
        public IEnumerable<AtttachmentCategory> atttachmentCategory { get; set; }
        public IEnumerable<HRPMSAttachment> hRPMSAttachments { get; set; }

        //ielts
        public string ieltsId { get; set; }
        public string examType { get; set; }
        public string centerNo { get; set; }
        public DateTime? dateI { get; set; }
        public string candidateNo { get; set; }

        public decimal? listeningScore { get; set; }
        public decimal? readingScore { get; set; }
        public decimal? writingScore { get; set; }
        public decimal? speakingScore { get; set; }
        public decimal? overallScore { get; set; }
        public decimal? cefrScore { get; set; }

        //public string attachment { get; set; }
        public IFormFile ieltsPhoto { get; set; }

        public int? statusI { get; set; }

        public IEnumerable<IeltsInfo> ielts { get; set; }
        public IEnumerable<EmployeeInsurance> employeeInsurances { get; set; }

        //hobby
        public string EmployeeHobbyID { get; set; }
        public string[] hobbyName { get; set; }
        public int? isActive { get; set; }
        public int? statushobby { get; set; }
        public DateTime? dateh { get; set; }
        public IEnumerable<EmployeeHobby> employeeHobbies { get; set; }

        //Literacy
        public string LiteracyId { get; set; }
        public string subjectlit { get; set; }
        public string competencyLevel { get; set; } //Beginer, Intermediate, Advance
        public string traininglt { get; set; }
        public string diplomalt { get; set; }

        public string remarkslt { get; set; }

        public IEnumerable<HrComputerLiteracy> computerLiteracy { get; set; }
        public IEnumerable<HrComputerLiteracy> computerLiteracies { get; set; }

        public IEnumerable<ComputerSubject> ComputerSubjectsList { get; set; }

        //Banking Diploma
        public int bankDiplomaId { get; set; }
        public string diplomaPart { get; set; }
        public string diplomaName { get; set; }
        public string passingYeardp { get; set; }
        public string resultTypedp { get; set; } //class, grade
        public string resultdp { get; set; } //1st class, 2nd class, 3rd class, 5.00
        public string session { get; set; }
        //mobile
        public string MobileBenifitID { get; set; }
        public string typemb { get; set; } //Mobile Data, Voice Call, Broadband
        public decimal? amount { get; set; }
        public DateTime? datemb { get; set; }
        public int? statusmb { get; set; }
        public MobileBenifit mobileBenifit { get; set; }
        public IEnumerable<MobileBenifit> mobileBenifits { get; set; }

        //Tax
        public string taxID { get; set; }
        public string taxZone { get; set; }
        public string taxCircle { get; set; }
        public string certificationName { get; set; }
		public string etinTax { get; set; }
		public Tax tax { get; set; }
        public IEnumerable<Tax> taxes { get; set; }

        //Extra Curricular
        public string ExtraCurricularId { get; set; }
        public int? extraCurricularTypeId { get; set; }
        public string skillLevel { get; set; } // Primary, Mid-Level, Expert
        public string skillType { get; set; } //National, Regional
        public string description { get; set; }
        public EmployeeExtraCurricular employeeExtraCurricular { get; set; }
        //public ExtraCurricularType extraCurricularType { get; set; }
        public IEnumerable<ExtraCurricularType> extraCurricularType { get; set; }
        //public IEnumerable<ExtraCurricularType> extraCurricularTypes { get; set; }
        public IEnumerable<EmployeeExtraCurricular> employeeExtraCurriculars { get; set; }


        public IEnumerable<BankingDiploma> bankingDiplomas { get; set; }
        public IEnumerable<EmergencyContact> emergencyContacts { get; set; }
        public IEnumerable<WagesEmergencyContact> wagesEmergencyContacts { get; set; }
        public IEnumerable<Relation> relations { get; set; }

        public IEnumerable<Spouse> spouses { get; set; }
        public IEnumerable<District> districts { get; set; }
        public IEnumerable<Organization> organizations { get; set; }
        public IEnumerable<Occupation> spOccupation { get; set; }
        public IEnumerable<EmployeePostingPlace> employeePostingPlaces { get; set; }
        public IEnumerable<PrevJobHistory> prevJobHistories { get; set; }
        public IEnumerable<CustomRole> CustomRoles { get; set; }
        public ShiftGroupMaster shiftGroup { get; set; }
        public IEnumerable<SalaryGrade> salaryGrades { get; set; }
        public IEnumerable<SalaryGrade> salarygrades { get; set; }
        public IEnumerable<SalarySlab> salarySlabs { get; set; }
        public IEnumerable<SalarySlab> salaryslabs { get; set; }
        public IEnumerable<EmpExpertise> empExpertises { get; set; }
      
        public IEnumerable<EmployeeInfo> allEmployeeInfos { get; set; }
        public IEnumerable<WagesEmployeeInfo> allWagesEmployeeInfos { get; set; }
        public IEnumerable<Religion> religions { get; set; }
        public IEnumerable<EmployeeType> employeeTypes { get; set; }
        public IEnumerable<OrganoOrganization> organoOrganizations { get; set; }
        public IEnumerable<Designation> designations { get; set; }
        public IEnumerable<Thana> thanas { get; set; }
        public IEnumerable<SpecialBranchUnit> specialBranchUnits { get; set; }
        public IEnumerable<Division> divisions { get; set; }
        public IEnumerable<HrBranch> hrBranches { get; set; }
        public IEnumerable<HrDivision> hrDivisions { get; set; }
        public IEnumerable<Department> departments { get; set; }
        public IEnumerable<ServiceStatus> serviceStatuses { get; set; }
        public IEnumerable<HrProgram> hrPrograms { get; set; }
        public IEnumerable<HrUnit> hrUnits { get; set; }
        public IEnumerable<Location> locations { get; set; }
        public IEnumerable<IeltsInfo> ieltsInfos { get; set; }
        public IEnumerable<FunctionInfo> functionInfos { get; set; }
        public IEnumerable<AllEmployeeJson> allEmployeeJsons { get; set; }
        public IEnumerable<AllEmployeeJsonNew> allEmployeeJsonNew { get; set; }
        public IEnumerable<EmployeePostingPlace> EmployeePostings { get; set; }
        public IEnumerable<Occupation> parentsOccupation { get; set; }
        public IEnumerable<EmployeeAccounts> employeeAccounts { get; set; }






    }


}