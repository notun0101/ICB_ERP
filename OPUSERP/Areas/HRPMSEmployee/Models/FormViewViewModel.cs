using Microsoft.AspNetCore.Http;
using OPUSERP.Accounting.Data.Entity.MasterData;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Areas.HRPMSPunishment.Models.Lang;
using OPUSERP.Areas.HRPMSReward.Models.Lang;
using OPUSERP.CRM.Data.Entity.MasterData;
using OPUSERP.Data.Entity.Master;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Disciplinary;
using OPUSERP.HRPMS.Data.Entity.DisciplineInvestigation;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.HrComputerLiteracy;
using OPUSERP.HRPMS.Data.Entity.IELTS;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Organogram;
using OPUSERP.HRPMS.Data.Entity.Suspensions;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.SCM.Data.Entity.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ACRLn = OPUSERP.Areas.HRPMSACR.Models.Lang.ACRLn;
using Address = OPUSERP.Areas.HRPMSEmployee.Models.Lang.Address;
using Offense = OPUSERP.HRPMS.Data.Entity.DisciplineInvestigation.Offense;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class FormViewViewModel
    {
        public string employeeID { get; set; }
        public string employID { get; set; }
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
        public string fatherNameEnglish { get; set; }
        public string fatherNameBangla { get; set; }
        public string nationality { get; set; }
        public string disability { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string gender { get; set; }
        public string birthPlace { get; set; }
        public string maritalStatus { get; set; }
        public int? religion { get; set; }
        public int? employeeType { get; set; }
        public int? activityStatus { get; set; }
        // public int departmentIdp { get; set; }
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
        // public int? designationId { get; set; }
        public string WhatsAppId { get; set; }
        public string skypeId { get; set; }
        public string FacebookId { get; set; }
        public string TwitterId { get; set; }
        public string InstagramId { get; set; }
        public string LinkedInId { get; set; }
        public int? employeeStatusId { get; set; }
        public int? hrProgramId { get; set; }
        public int? hrUnitId { get; set; }
        public int? locationId { get; set; }
        public int? functionInfoId { get; set; }
        public string disablityType { get; set; }
        public int? salaryStatus { get; set; }
        public string salaryStatusComment { get; set; }
        public string BranchName { get; set; }
        public int? DivisionId { get; set; }
        public int? hrBranchId { get; set; }
        public int? hrDivisionId { get; set; }
        public string action { get; set; }
        public EmployeeInfoLn fLang { get; set; }
        public RewardLn fLang2 { get; set; }
        public PunishmentLn fLang3 { get; set; }
        public ACRLn fLang4 { get; set; }


        public int? post { get; set; }//Hidden Field For Designation, Handle.
        public string employeeNameCode { get; set; }

        public EmployeeInfo employeeInfo { get; set; }
        public WagesEmployeeInfo wagesEmployeeInfo { get; set; }
        public IEnumerable<EmployeeInfo> allEmployeeInfos { get; set; }
        public IEnumerable<WagesEmployeeInfo> allWagesEmployeeInfos { get; set; }
        public IEnumerable<Religion> religions { get; set; }
        public IEnumerable<EmployeeType> employeeTypes { get; set; }
        public IEnumerable<OrganoOrganization> organoOrganizations { get; set; }
        public IEnumerable<Designation> designations { get; set; }
        public IEnumerable<District> districts { get; set; }
        public IEnumerable<SpecialBranchUnit> specialBranchUnits { get; set; }
        public IEnumerable<Division> divisions { get; set; }
        public IEnumerable<HrBranch> hrBranches { get; set; }
        public IEnumerable<HrDivision> hrDivisions { get; set; }
        // public IEnumerable<Location> locations { get; set; }

        public IEnumerable<Department> departments { get; set; }
        public IEnumerable<ServiceStatus> serviceStatuses { get; set; }
        public IEnumerable<HrProgram> hrPrograms { get; set; }
        public IEnumerable<HrUnit> hrUnits { get; set; }
        public IEnumerable<Location> locations { get; set; }
        public IEnumerable<IeltsInfo> ieltsInfos { get; set; }
        public IEnumerable<FunctionInfo> functionInfos { get; set; }
        public IEnumerable<AllEmployeeJson> allEmployeeJsons { get; set; }
        public AllEmployeeJson allEmployeeJson { get; set; }

        public string visualEmpCodeName { get; set; }
        //favouriteColor
        public string favouriteColor { get; set; }


        //address
        public int addressEmployeeID { get; set; }
        public int presentAddressID { get; set; }
        public int permanentAddressID { get; set; }
        public string presentRoadNo { get; set; }
        public string permanentRoadNo { get; set; }
        public string presentDivision { get; set; }
        public string presentDistrict { get; set; }
        public string presentUpazila { get; set; }
        public string presentUnion { get; set; }
        public string presentPostOffice { get; set; }
        public string presentPostCode { get; set; }
        public string presentBlockSector { get; set; }
        public string presentHouseVillage { get; set; }
        public string permanentDivision { get; set; }
        public string permanentDistrict { get; set; }
        public string permanentUpazila { get; set; }
        public string permanentUnion { get; set; }
        public string permanentPostOffice { get; set; }
        public string permanentPostCode { get; set; }
        public string permanentBlockSector { get; set; }
        public string permanentHouseVillage { get; set; }
        public string sameAddress { get; set; }
        public HRPMS.Data.Entity.Employee.Address present { get; set; }
        public HRPMS.Data.Entity.Employee.Address permanent { get; set; }

        public HRPMS.Data.Entity.Employee.WagesAddress wagesPresent { get; set; }
        public HRPMS.Data.Entity.Employee.WagesAddress wagesPermanent { get; set; }


        //Spouse
        public int SpouseEmployeeID { get; set; }
        public int spouseID { get; set; }
        public string spouseName { get; set; }
        public string SemailAddress { get; set; }
        public string spouseNameBN { get; set; }
        public DateTime? SdateOfBirth { get; set; }
        public int SoccupationId { get; set; }
        public string Sgender { get; set; }
        public string Sdesignation { get; set; }
        public string Sorganization { get; set; }
        public string Sbin { get; set; }
        public string Snid { get; set; }
        public string SbloodGroup { get; set; }
        public string ShomeDistrict { get; set; }
        public string Srelationship { get; set; }
        public string Snationality { get; set; }
        public string Scontact { get; set; }
        public string ShigherEducation { get; set; }
        public IFormFile spousePhoto { get; set; }
        public Spouse spouse { get; set; }
        public IEnumerable<Spouse> spouses { get; set; }
        public IEnumerable<District> Sdistricts { get; set; }
        public IEnumerable<Occupation> Soccupations { get; set; }

        //children

        public string childrenID { get; set; }
        public int childrenEmployeeID { get; set; }
        public string childName { get; set; }
        public string childNameBN { get; set; }
        public DateTime? CdateOfBirth { get; set; }
        public string Ceducation { get; set; }
        public string Cgender { get; set; }
        public string bin { get; set; }
        public string occupation { get; set; }
        public int CoccupationId { get; set; }
        public string Cdesignation { get; set; }
        public string organization { get; set; }
        public string nid { get; set; }
        public string CbloodGroup { get; set; }
        public string Cnationality { get; set; }
        public string Crelationship { get; set; }
        public string CemailAddressPersonal { get; set; }
        public string CemployeeNameCode { get; set; }
        public int Cdisability { get; set; }
        public string CdisablityType { get; set; }
        public Photograph Cphotograph { get; set; }
        public IEnumerable<Children> children { get; set; }
        public Children child { get; set; }
        public IEnumerable<Occupation> occupations { get; set; }
        public IFormFile childrenPhoto { get; set; }

        //Emergency Contact
        public int EmergencyemployeeID { get; set; }
        public int refID { get; set; }
        public string refName { get; set; }
        public string refRelation { get; set; }
        public string refOrganization { get; set; }
        public string refDesignation { get; set; }
        public string refEmail { get; set; }
        public string refContact { get; set; }
        public string refOccupation { get; set; }
        public IFormFile empPhoto { get; set; }
        public string OfficeAddress { get; set; }
        public string HomeAddress { get; set; }
        public IEnumerable<EmergencyContact> emergencyContacts { get; set; }
        public IEnumerable<WagesEmergencyContact> wagesEmergencyContacts { get; set; }
        public IEnumerable<Relation> relations { get; set; }

        //EmployeeContactInfo

        public int contactId { get; set; }
        public int contactEmployeeID { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public string remarks { get; set; }
        public int isDelete { get; set; }
        public IEnumerable<EmployeeContractInfo> employeeContractInfos { get; set; }

        //nominee

        public int nomineeEmployeeID { get; set; }
        public int? nomineeID { get; set; }
        public string name { get; set; }
        public string relation { get; set; }
        public string contact { get; set; }
        public string address { get; set; }
        public int?[] fundTypeList { get; set; }
        public decimal?[] fundValueList { get; set; }
        public string guardianName { get; set; }
        public string witnessName { get; set; }
        public DateTime? nomineeDate { get; set; }
        public IFormFile nomineePhoto { get; set; }
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
        public Nominee nominee { get; set; }

        //EmployeeInsuranceViewModel

        public int InsuranceemployeeID { get; set; }
        public int? insuranceID { get; set; }
        public string age { get; set; }
        public DateTime? insuranceDate { get; set; }
        public string imageUrl { get; set; }
        public IFormFile imagePath_Challan { get; set; }
        public IEnumerable<EmployeeInsurance> employeeInsurances { get; set; }


        //EducationalQualificationViewModel

        public int EducationEmployeeID { get; set; }
        public int educationId { get; set; }
        public string institution { get; set; }
        public int? resultId { get; set; }
        public string majorGroup { get; set; }
        public string grade { get; set; }
        public int? passingYear { get; set; }
        public int? degreeId { get; set; }
        public int? organizationId { get; set; }
        public int? reldegreesubjectId { get; set; }
        public string levelofeducationName { get; set; }
        public string degreeName { get; set; }
        public string organizationName { get; set; }
        public string organizationType { get; set; }
        public string subjectName { get; set; }
        public string resultName { get; set; }
        public int levelofeducationId { get; set; }
        public int subjectId { get; set; }
        public int degreeSubjectId { get; set; }
        public LevelofEducation levelofeducation { get; set; }
        public IEnumerable<EducationalQualification> educationalQualifications { get; set; }
        public IEnumerable<LevelofEducation> levelofeducationNamelist { get; set; }
        public IEnumerable<Degree> degreeslist { get; set; }
        public IEnumerable<Subject> subjectslist { get; set; }

        //ProfessionalQualificationsViewModel
        public int ProfessionalQemployeeID { get; set; }
        public int? qualificationID { get; set; }
        public int? qualificationHeadId { get; set; }
        public string subject { get; set; }
        public int? result { get; set; }
        public string instituteName { get; set; }
        public string PropassingYear { get; set; }
        public string markGrade { get; set; }
        public string qualificationHeadName { get; set; }
        public IEnumerable<QualificationHead> qualificationHeads { get; set; }
        public IEnumerable<ProfessionalQualifications> professionalQualifications { get; set; }
        public IEnumerable<Result> results { get; set; }



        //OtherQualificationsViewModel
        public int? OtherQualificationemployeeID { get; set; }
        public string OtherQualificationHeadName { get; set; }
        public IEnumerable<OtherQualificationHead> otherQualificationHeads { get; set; }
        public IEnumerable<OtherQualification> otherQualifications { get; set; }


        //TrainingViewModel
        public int TrainingemployeeID { get; set; }
        public int trainingLogID { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string country { get; set; }
        public string category { get; set; }
        public string trainingTitle { get; set; }
        public string institute { get; set; }
        public string sponsoringAgency { get; set; }
        public string trainingCategoryName { get; set; }
        public string trainingInstituteName { get; set; }
        public IEnumerable<Country> countries { get; set; }
        public IEnumerable<TrainingCategory> trainingCategories { get; set; }
        public IEnumerable<TrainingInstitute> trainingInstitutes { get; set; }
        public IEnumerable<TraningLog> traningLogs { get; set; }

        //TransferLogViewModel

        public int TLogemployeeID { get; set; }
        public int transfarID { get; set; }
        public string workStation { get; set; }
        public int? TLgrade { get; set; }
        public int? designationId { get; set; }
        public int? departmentId { get; set; }
        public IEnumerable<SalaryGrade> salaryGrade { get; set; }
        public IEnumerable<TransferLog> transferLogs { get; set; }

        //PromotionLogViewModel
        public int PLogEmployeeID { get; set; }
        public int promotionId { get; set; }
        public int? designationNewId { get; set; }
        public int? designationOldId { get; set; }
        public DateTime date { get; set; }
        public string payScale { get; set; }
        public string nature { get; set; }
        public string basic { get; set; }
        public string rank { get; set; }
        public string remark { get; set; }
        public string goNumber { get; set; }
        public DateTime? goDate { get; set; }
        public IEnumerable<PromotionLog> promotionLogs { get; set; }
        public IEnumerable<SalaryGrade> salaryGrades { get; set; }
        public Designation designationName { get; set; }

        //ACRLogViewModel
        public int? ACRLogID { get; set; }
        public int ACRLogEmployeeId { get; set; }
        public DateTime? startDate { get; set; }
        public string supervisorName { get; set; }
        public string supervisorDesignation { get; set; }
        public DateTime? endDate { get; set; }
        public string signatoryName { get; set; }
        public string signatoryDesignation { get; set; }
        public string year { get; set; }
        public int? score { get; set; }
        public string advanceComment { get; set; }
        public string supervisorCode { get; set; }
        public string signatoryCode { get; set; }
        public string finalStatus { get; set; }
        public DateTime? effectiveDate { get; set; }
        public string fileUrl { get; set; }
        public IEnumerable<AcrInfo> acrInfos { get; set; }

        //DisciplinaryViewModel
        public int disciplinaryId { get; set; }
        public int disciplinaryemployeeId { get; set; }
        public string employeeName { get; set; }
        public int? offenseId { get; set; }
        public string punishmentId { get; set; }
        public DateTime? punishmentDate { get; set; }
        public DateTime? startFrom { get; set; }
        public DateTime? endTo { get; set; }
        public string goNo { get; set; }
        public IFormFile goAttachment { get; set; }
        public IEnumerable<DisciplinaryLog> disciplinaries { get; set; }
        public IEnumerable<Offense> offenses { get; set; }
        public IEnumerable<NaturalPunishment> punishments { get; set; }


        //ApprovalViewModel
        public int approvalMasterId { get; set; }
        public int approvalemployeeId { get; set; }
        public int? employeeInfoId { get; set; }
        public int? approvalTypeId { get; set; }
        public int?[] approverId { get; set; }
        public int?[] canFinalApprover { get; set; }
        public int?[] sortOrder { get; set; }
        public string[] status { get; set; }
        public IEnumerable<ApprovalMaster> approvalMasters { get; set; }
        public IEnumerable<ApprovalType> approvalTypes { get; set; }
        public ApprovarsWithEmp approvarsWithEmp { get; set; }

        //LicenseViewModel
        public int LicenseemployeeID { get; set; }
        public int licenseId { get; set; }
        public string licenseNumber { get; set; }
        public string place { get; set; }
        public DateTime? dateOfIssue { get; set; }
        public DateTime? dateOfExpair { get; set; }
        public string licenseCategory { get; set; }
        public IEnumerable<DrivingLicense> licenses { get; set; }

        //PassportViewModel

        public int PassportemployeeID { get; set; }
        public int passportId { get; set; }
        public string nameInPassport { get; set; }
        public string passPortNumber { get; set; }
        public IEnumerable<PassportDetails> passportDetails { get; set; }

        //TraveInfoViewModel

        public int TraveEmployeeID { get; set; }
        public int travelId { get; set; }
        public string type { get; set; }
        public string purpose { get; set; }
        public string location { get; set; }
        public string file { get; set; }
        public string titleOfFile { get; set; }
        public string titleName { get; set; }
        public string accountCode { get; set; }
        public int? projectId { get; set; }
        public DateTime? leaveStartDate { get; set; }
        public DateTime? leaveEndDate { get; set; }
        public IEnumerable<TraveInfo> traveInfos { get; set; }
        public IEnumerable<TravelPurpose> travelPurposes { get; set; }
        public IEnumerable<Project> projects { get; set; }
       

        //MembershipViewModel

        public int MemberemployeeID { get; set; }
        public int membershipId { get; set; }
        public string nameOrganization { get; set; }
        public string membershipType { get; set; }
        public string membershipNo { get; set; }
        public IEnumerable<EmployeeMembership> employeeMemberships { get; set; }
        public IEnumerable<HRPMS.Data.Entity.Master.Membership> memberships { get; set; }

        //AwardViewModel
        public int AwardemployeeID { get; set; }
        public int awardId { get; set; }
        public int? awardIdName { get; set; }
        public string perpose { get; set; }
        public DateTime? txtAwardDate { get; set; }
        public IFormFile awardPhoto { get; set; }
        public IEnumerable<EmployeeAward> awards { get; set; }
        public IEnumerable<HRPMS.Data.Entity.Master.Award> awardMaster { get; set; }

        //PublicationViewModel

        public int PublicationemployeeID { get; set; }
        public int publicationId { get; set; }
        public string publicationName { get; set; }
        public string publicationType { get; set; }
        public string publicationPlace { get; set; }
        public DateTime? publicationDate { get; set; }
        public IEnumerable<HRPMS.Data.Entity.Employee.Publication> publications { get; set; }

        //LanguageViewModel
        public int LanguageemployeeID { get; set; }
        public int languageId { get; set; }
        public string language { get; set; }
        public string proficiency { get; set; }
        public string reading { get; set; }
        public string writing { get; set; }
        public string speaking { get; set; }
        public IEnumerable<EmployeeLanguage> employeeLanguages { get; set; }
        public IEnumerable<Language> languages { get; set; }

        //OtherActivitiesViewModel

        public int? OtherActemployeeID { get; set; }
        public int? activityType { get; set; }
        public int? activityID { get; set; }
        public int? activityName { get; set; }
        public string description { get; set; }
        public IEnumerable<HRPMSActivityType> hRPMSActivityTypes { get; set; }
        public IEnumerable<ActivityName> activityNames { get; set; }
        public IEnumerable<OtherActivity> otherActivities { get; set; }

        //Bank
        public int bankInfoId { get; set; }
        public int BankemployeeID { get; set; }
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

        //BelongingsViewModel

        public int? BelongemployeeID { get; set; }
        public int? belongingsItemID { get; set; }
        public int? itemID { get; set; }
        public string assetNo { get; set; }
        public string brandName { get; set; }
        public string modelNo { get; set; }
        public DateTime? issueDate { get; set; }
        public DateTime? returnDate { get; set; }
        public int? specificationID { get; set; }
        public int? belongingId { get; set; }
        public IEnumerable<Belongings> belongings { get; set; }
        public IEnumerable<BelongingItem> belongingItems { get; set; }


        //PreviousEmploymentViewModel
        public int? EmploymentemployeeID { get; set; }
        public int? previousEmploymentID { get; set; }
        public string companyName { get; set; }
        public string position { get; set; }
        public string PEdepartment { get; set; }
        public int? PEorganizationType { get; set; }
        public string companyBusiness { get; set; }
        public string companyLocation { get; set; }
        public string responsibilities { get; set; }
        public IEnumerable<HRPMSOrganizationType> hRPMSOrganizationTypes { get; set; }
        public IEnumerable<PriviousEmployment> priviousEmployments { get; set; }
        public IEnumerable<WagesPriviousEmployment> wagesPriviousEmployments { get; set; }


        //FreedomFighterViewModel
        public int? FreedomemployeeID { get; set; }
        public int? FFID { get; set; }
        public string ffNo { get; set; }
        public string owner { get; set; }
        public string relationShip { get; set; }
        public string sectorNo { get; set; }
        public FreedomFighter freedomFighter1 { get; set; }
        public IEnumerable<FreedomFighter> freedomFighters { get; set; }

        //ReferenceViewModel
        public int? ReferenceemployeeID { get; set; }
        public IEnumerable<HRPMS.Data.Entity.Employee.Reference> references { get; set; }
        public IEnumerable<HRPMS.Data.Entity.Employee.WagesReference> wagesReferences { get; set; }

        //OfficeAssignViewModel
        public int? OfficeAssignemployeeID { get; set; }
        public int? officeAssignID { get; set; }
        public string roomNo { get; set; }
        public string floorNo { get; set; }
        public string deskNo { get; set; }
        public IEnumerable<OfficeAssign> officeAssigns { get; set; }

        //EmployeeContractInfoViewModel
        public int? EmpcontractId { get; set; }
        public int contractemployeeID { get; set; }
        public DateTime? EmpContractStartDate { get; set; }
        public DateTime EmpContractEndDate { get; set; }
        public string Empremarks { get; set; }
        public int EmpisDelete { get; set; }
        public IEnumerable<EmployeeContractInfo> employeeContractInfosEmp { get; set; }

        //EmployeeProjectActivityViewModel

        public int? ProjectActivityid { get; set; }
        public int? ProjectActivityemployeeId { get; set; }
        public int? hRDonerId { get; set; }
        public int? hRActivityId { get; set; }
        public int? hRProjectId { get; set; }
        public int? isActive { get; set; }
        public IEnumerable<HRDoner> hRDoners { get; set; }
        public IEnumerable<HRProject> hRProjects { get; set; }
        public IEnumerable<HRActivity> hRActivities { get; set; }
        public IEnumerable<EmployeeProjectActivity> employeeProjectActivities { get; set; }
        public IEnumerable<EmployeeProjectAssign> employeeProjectAssigns { get; set; }


        //HRPMSAttachmentViewModel
        public int AttachmentemployeeId { get; set; }
        public int? atttachmentCategoryId { get; set; }
        public IFormFile AttachmentfileUrl { get; set; }
        public string fileTitle { get; set; }
        public string Attachmentremarks { get; set; }
        public DateTime? attachmentDate { get; set; }
        public string groupName { get; set; }
        public int? atttachmentGroupId { get; set; }
        public string categoryName { get; set; }
        public IEnumerable<AtttachmentGroup> atttachmentGroups { get; set; }
        public IEnumerable<AtttachmentCategory> atttachmentCategory { get; set; }
        public IEnumerable<HRPMSAttachment> hRPMSAttachments { get; set; }


        //EmployeeOtherInfoViewModel

        public int EmployeeOtherid { get; set; }
        public int EmployeeOtheremployeeId { get; set; }
        public int? hRFacilityId { get; set; }
        public string simsId { get; set; }
        public string busArea { get; set; }
        public string EmployeeOthertype { get; set; }

        public IEnumerable<HRFacility> hRFacilities { get; set; }
        public IEnumerable<EmployeeOtherInfo> employeeOtherInfos { get; set; }

        //EmployeeCostCentreViewModel

        public int CostCentreid { get; set; }
        public int CostCentreemployeeId { get; set; }
        public int? costCentreId { get; set; }
        public DateTime? costCentreDate { get; set; }
        public IEnumerable<CostCentre> costCentres { get; set; }
        public IEnumerable<EmployeeCostCentre> employeeCostCentres { get; set; }
        //EmployeeGradeViewModel

        public int Gradeid { get; set; }
        public int GradeemployeeId { get; set; }
        public int? salaryGradeId { get; set; }
        public DateTime? GradeeffectiveDate { get; set; }
        public IEnumerable<SalaryGrade> GradesalaryGrades { get; set; }
        public IEnumerable<EmployeeGrade> employeeGrades { get; set; }

        //ShiftGroupDetailViewModel
        public int shiftMasterId { get; set; }
        public string weekDay { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string holiday { get; set; }
        public string holidayType { get; set; }
        public int shiftGroupMasterId { get; set; }
        public IEnumerable<ShiftGroupDetail> shiftGroupDetailslist { get; set; }
        public IEnumerable<ShiftGroupMaster> shiftGroupMasterslist { get; set; }
        public IEnumerable<ShiftGroupWithDetails> shiftGroupWithDetails { get; set; }

        //IELTSViewModel

        public string ieltsId { get; set; }
        public string examType { get; set; }
        public string centerNo { get; set; }
        public string candidateNo { get; set; }
        public decimal? listeningScore { get; set; }
        public decimal? readingScore { get; set; }
        public decimal? writingScore { get; set; }
        public decimal? speakingScore { get; set; }
        public decimal? overallScore { get; set; }
        public decimal? cefrScore { get; set; }
        public IFormFile ieltsPhoto { get; set; }
        public IEnumerable<IeltsInfo> ielts { get; set; }


        //HrComputerLiteracyViewModel

        public string ComputeremployeeID { get; set; }
        public string LiteracyId { get; set; }
        public ComputerSubject Computersubject { get; set; }
        public string competencyLevel { get; set; } //Beginer, Intermediate, Advance
        public string training { get; set; }
        public string diploma { get; set; }
        public IEnumerable<HrComputerLiteracy> computerLiteracy { get; set; }
        public IEnumerable<HrComputerLiteracy> computerLiteracies { get; set; }
        public IEnumerable<ComputerSubject> ComputerSubjectsList { get; set; }


        //EmployeeHobbyViewModel
        public string EmployeeHobbyID { get; set; }
        public int employeeInfoIdHobby { get; set; }
        public string[] hobbyName { get; set; }
        public IEnumerable<EmployeeHobby> employeeHobbies { get; set; }

        //PhotographViewModel
        public int photographemployeeID { get; set; }
        public int photographID { get; set; }
        public IFormFile banglaSign { get; set; }
        public IFormFile EngSign { get; set; }
        public EmployeeSignature employeeSignature { get; set; }




        // new 


        public string EmpCode { get; set; }
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
        public IEnumerable<Occupation> parentsOccupation { get; set; }
        public int? department { get; set; }
        public int? bloodDonateStatus { get; set; } //0 or 1
        public string designationCode { get; set; }
        public string LocationName { get; set; }
        public string branchCode { get; set; }
        public int? companyId { get; set; }
        public string contructualType { get; set; }
        public int? empTypeId { get; set; }

        public int? shiftGroupId { get; set; }
        public ShiftGroupMaster shiftGroup { get; set; }
        public DateTime? lastPromotionDate { get; set; }
        public DateTime? prlDate { get; set; }
        public string refLetterNo { get; set; }
        public DateTime? lastWorkingDate { get; set; }
        public int? lastDesignationId { get; set; }
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


        // child new



        public string education { get; set; }
        public string presentEducation { get; set; }
        public string relationship { get; set; }
        public int[] childEduId { get; set; }
        public string[] institution1 { get; set; }
        public int[] degreeId1 { get; set; }
        public string[] majorSubject { get; set; }
        public string[] resultType { get; set; } //1st, 2nd, 3rd Class, Grade
        public string[] result1 { get; set; }

        //BankingDiplomaViewModel

        public int employeeId { get; set; }
        public int bankDiplomaId { get; set; }
        public string diplomaPart { get; set; }
        public string diplomaName { get; set; }
        public string passingYear2 { get; set; }
        public string resultType2 { get; set; } //class, grade
        public string result2 { get; set; } //1st class, 2nd class, 3rd class, 5.00
       
        public IEnumerable<BankingDiploma> bankingDiplomas { get; set; }

        //tax 

        public string taxID { get; set; }
        public string taxZone { get; set; }
        public string taxCircle { get; set; }
        public Tax tax { get; set; }
        public IEnumerable<Tax> taxes { get; set; }

        //DuelResidenceViewModel
        public string DuelResidenceID { get; set; }
        public int? duelCountryId { get; set; }
        public string duelPassport { get; set; }
        public DuelResidence duelResidence { get; set; }
        public IEnumerable<DuelResidence> duelResidences { get; set; }
        public IEnumerable<Country> duelCountry { get; set; }

        //FoodLikingViewModel
        public int foodLikingId { get; set; }
        public int? vegiterian { get; set; }
        public int? nonVegiterian { get; set; }
        public FoodLiking foodLiking { get; set; }

        //EmployeeSportsViewModel

        public int EmployeeSportsId { get; set; }
        public string skillType { get; set; } // National, Reasonal Player
        public string skillLevel { get; set; } //Beginer, Intermediate, Advanced
        public IEnumerable<EmployeeSports> employeeSports { get; set; }


        //EmployeeRoleViewModel
        public int emproleid { get; set; }
        public IEnumerable<EmployeeRole> employeeRoles { get; set; }
        public int? roleId { get; set; }
        public IEnumerable<CustomRole> customRoles { get; set; }
        public EmployeeRole employeeRole { get; set; }

        //EmployeeDiseaseViewModel

        public int EmployeeDiseaseId { get; set; }
         public int? medicalDiseaseId { get; set; }
        public int? status1 { get; set; }
       public IEnumerable<MedicalDisease> medicalDiseases { get; set; }
        public IEnumerable<EmployeeDisease> employeeDiseases { get; set; }

        //MobileBenifitViewModel
        public string MobileBenifitID { get; set; }
        public decimal? amount { get; set; }
        public MobileBenifit mobileBenifit { get; set; }
        public IEnumerable<MobileBenifit> mobileBenifits { get; set; }

        //SuspensionViewModel

        public string suspensionID { get; set; }
        public string susDesc { get; set; }
        public string chargeSheetDesc { get; set; }
        public string hearingReport { get; set; }
        public string punishmentType { get; set; }
        public DateTime? effectiveForm { get; set; }
        public IFormFile suspensionFile { get; set; }
        public IFormFile chargeFile { get; set; }
        public IFormFile hearingRepoFile { get; set; }
        public IEnumerable<Suspension> supensionDetails { get; set; }
        public Suspension supensionDetail { get; set; }
        public Suspension suspensionInfo { get; set; }

        //AllegationViewModel
        public int? allegationID { get; set; }
        public string allegationDetail { get; set; }
        public IFormFile allegationUrl { get; set; }
        public string clarification { get; set; }
        public IFormFile clarificationUrl { get; set; }
        public string management { get; set; }
        public IFormFile managementUrl { get; set; }
        public IEnumerable<Allegation> allegations { get; set; }
        public Allegation allegationDetails { get; set; }

        //EmployeeExtraCurricularViewModel

        public string ExtraCurricularId { get; set; }
        public int? extraCurricularTypeId { get; set; }
        public EmployeeExtraCurricular employeeExtraCurricular { get; set; }
        public IEnumerable<ExtraCurricularType> extraCurricularType { get; set; }
        public IEnumerable<EmployeeExtraCurricular> employeeExtraCurriculars { get; set; }
    }



}

