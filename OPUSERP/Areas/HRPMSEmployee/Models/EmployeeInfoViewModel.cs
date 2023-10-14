using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.Areas.HRPMSPunishment.Models.Lang;
using OPUSERP.Areas.HRPMSReward.Models.Lang;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.ACR;
using OPUSERP.HRPMS.Data.Entity.Employee;
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
    public class EmployeeInfoViewModel
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
        public IEnumerable<Occupation> parentsOccupation { get; set; }
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
		public int? branchIdsbu { get; set; }
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
        public string designationCode { get; set; }
        public string LocationName { get; set; }
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
        //expertise
        public int? expertiseId { get; set; }
        public EmpExpertise expertise { get; set; }

        public int? isManager { get; set; }//0=no; 1=yes;

        public ShiftGroupMaster shiftGroup { get; set; }
        public IEnumerable<SalaryGrade> salaryGrades { get; set; }
        public IEnumerable<SalaryGrade> salarygrades { get; set; }
        public IEnumerable<SalarySlab> salarySlabs { get; set; }
        public IEnumerable<SalarySlab> salaryslabs { get; set; }

        public IEnumerable<EmpExpertise> empExpertises { get; set; }
        public IEnumerable<DisabilityType> disabilityTypes { get; set; }
        public int? disabilityTypeId { get; set; }

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
        public IEnumerable<HrDivision>  hrDivisions { get; set; }
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
		public EmployeeInfoByUsernameSp employeeInfoByUsername { get; set; }
		public AllEmployeeJson allEmployeeJson { get; set; }
       // public IEnumerable<Company> companies { get; set; }
        public string visualEmpCodeName { get; set; }

        public DateTime? lastPromotionDate { get; set; }
        public DateTime? prlDate { get; set; }
        public string refLetterNo { get; set; }
        public DateTime? lastWorkingDate { get; set; }
        public int? lastDesignationId { get; set; }

        public int? JoinDesignationId { get; set; }
        public Designation JoinDesignation { get; set; }
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
        public int? statusbdbl { get; set; }

        public string fileNo { get; set; }
        public string presentPosting { get; set; }
        public DateTime? lastTransferDate { get; set; }
        public string qualification { get; set; }
        public string problem { get; set; }

        public int? role { get; set; }
        public int? eligible { get; set; }
        public IEnumerable<CustomRole> CustomRoles { get; set; }

        public Spouse Spouses { get; set; }
        public Address Addresss { get; set; }

        //previousjob

        public int?[] employeeId { get; set; }
        public int?[] employeeIdJH { get; set; }

        public string[] company { get; set; }
        public string[] position { get; set; }
        public DateTime?[] jobstart { get; set; }
        public DateTime?[] jobend { get; set; }

        public IEnumerable<PrevJobHistory> prevJobHistories { get; set; }
        public IEnumerable<JobResponsibility> jobResponsibilities { get; set; }

        //BDBLjobhistory
        public int?[] postingID { get; set; }
        public int?[] employeeIdBDBL { get; set; }
        public string[] bdblSBU { get; set; }
        public string[] bdblDepartment { get; set; }
        public string[] bdblBranch { get; set; }
        public string[] bdblDivision { get; set; }
        public string[] bdblUnit { get; set; }
        public string[] bdblZone { get; set; }
        public string[] bdblOffice { get; set; }
        public DateTime?[] bdblStartDatejs { get; set; }
        public DateTime?[] bdblEndDatejs { get; set; }
        public string[] bdblremarks { get; set; }
        public string[] bdbltype { get; set; }
        public string[] bdblplaceName { get; set; }
        public string[] bdblplaceNameBn { get; set; }
        public IEnumerable<EmployeePostingPlace> employeePostingPlaces { get; set; }
        public string employeePostingPlaceInfo { get; set; }

		public int? postplaceId { get; set; }
		public int? sbubdbl { get; set; }
		public int? departmentbdbl { get; set; }
		public int? branchbdbl { get; set; }
		public int? unitbdbl { get; set; }
		public int? officebdbl { get; set; }
		public int? divisionbdbl { get; set; }
		public int? locationbdbl { get; set; }
		public DateTime? startbdbl { get; set; }
		public DateTime? endbdbl { get; set; }
		public string remarksbdbl { get; set; }
		public int? typebdbl { get; set; }
		public string placebdbl { get; set; }
		public string placebnbdbl { get; set; }
		public int? responsibilitybdbl { get; set; }
		public int? designationbdbl { get; set; }




	}


    public class EmployeeACRInfoViewModel
	{
		public string yearRange { get; set; }
		public EmployeeInfo employeeInfo { get; set; }
		public Assessment assessment { get; set; }
		public string lastDesignation { get; set; }
		public string lastEducation { get; set; }
		public string joiningDesignation { get; set; }
		public string photoUrl { get; set; }
		public string gradeSlab { get; set; }
		public string jobDuration { get; set; }
		public string jobDurationAsManager { get; set; }
		public string jobDurationAsZonalHead { get; set; }
		public string jobDurationAsOthers { get; set; }
		public string jobUnderSupervisor { get; set; }
		public string diploma1 { get; set; }
		public string diploma2 { get; set; }
		public string profQ { get; set; }
		public string othersQ { get; set; }
		public int? childrenCount { get; set; }
        public SuperVisorViewModel superVisor { get; set; }
        public string postingPlaceMainDuty { get; set; }
    }
	public class EmployeeInfoSpViewModel
	{
		public int? employeeId { get; set; }
		public int? designationCode { get; set; }
		public int? seniorityLevel { get; set; }
		public string employeeCode { get; set; }
		public string nameEnglish { get; set; }
		public string nameBangla { get; set; }
		public string designationName { get; set; }
		public string divisionName { get; set; }
		public DateTime? joiningDate { get; set; }
		public string emailAddress { get; set; }
		public string departmentName { get; set; }
		public string branchName { get; set; }
		public string unitName { get; set; }
		public string zoneName { get; set; }
		public string officeName { get; set; }
		public string photographUrl { get; set; }
		public string signatureUrl { get; set; }
		public int? lastDesignationId { get; set; }
		public int? departmentId { get; set; }
		public int? hrDivisionId { get; set; }
		public int? hrBranchId { get; set; }
		public int? hrUnitId { get; set; }
		public int? zoneId { get; set; }
		public int? officeId { get; set; }
		public int? activityStatus { get; set; }
	}
    public class AuditViewModel
    {
        public IEnumerable<AuditTrailViewModel> auditTrails { get; set; }
        public IEnumerable<AuditTables> auditTables { get; set; }
    }

    public class AuditTrailViewModel
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UserName { get; set; }
        public string Type { get; set; }
    }
	public class AuditTables {
		public string TableName { get; set; }
		public string FieldName { get; set; }
	}

	public class EmployeeInfoByUsernameSp
	{
		public string employeeCode { get; set; }
		public string nameEnglish { get; set; }
		public string designationName { get; set; }
		public string Posting { get; set; }
		public int? employeeId { get; set; }
		public string imageUrl { get; set; }
	}

	public class SuperVisorViewModel
    {
        public string supervisorCode { get; set; }
        public string supervisorName { get; set; }
        public string supervisorDesig { get; set; }
    }
    public class EmployeeInfoSpLoanViewModel
    {
        public int? Id { get; set; }
        public string employeeCode { get; set; }
        public string nameEnglish { get; set; }
        public string placeOfPosting { get; set; }
        public int? lastDesignationId { get; set; }
        public string designationName { get; set; }
    }

	public class SeniorityListVm
	{
		public int? employeeId { get; set; }
		public string employeeCode { get; set; }
		public string nameEnglish { get; set; }
		public int? designationId { get; set; }
		public string designationName { get; set; }
		public string posting { get; set; }
		public string activityStatus { get; set; }
		public string salaryStatus { get; set; }
		public int? seniorityLevel { get; set; }
	}

	public class SeniorViewModel
	{
		public int?[] empIdSeniority { get; set; }
		public int?[] designationIdSeniority { get; set; }
		public string[] empCodeSeniority { get; set; }
		public int?[] seniorityNum { get; set; }

		public IEnumerable<Designation> designations { get; set; }
	}
	public class SeniorityStatus
	{
		public int? status { get; set; }
	}

	public class EmployeeMarkingViewModel
	{
		public IEnumerable<Company> companies { get; set; }
		public MarkingInfoVm markingInfoVm { get; set; }
		public IEnumerable<MarkingEducation> markingEducation { get; set; }
		public IEnumerable<EmployeeAward> awards { get; set; }
	}
	public class MarkingInfoVm
	{
		public string nameEnglish { get; set; }
		public string employeeCode { get; set; }
		public string nameBangla { get; set; }
		public string designationName { get; set; }
		public int? seniorityLevel { get; set; }
		public string presentPosting { get; set; }
		public string salaryScale { get; set; }
		public DateTime? dateOfBirth { get; set; }
		public DateTime? prlDate { get; set; }
		public DateTime? joiningDate { get; set; }
		public DateTime? promotionDateCurrentDesignation { get; set; }
		public DateTime? permanantDateCurrentDesignation { get; set; }
		public string homeDistrict { get; set; }
		public string bankingDiploma1 { get; set; }
		public string bankingDiploma2 { get; set; }
		public string totalDaysOfPromotion { get; set; }
	}
	public class MarkingEducation
	{
		public string nameEnglish { get; set; }
		public string employeeCode { get; set; }
		public string nameBangla { get; set; }
		public string degreeName { get; set; }
		public string institution { get; set; }
		public int? passingYear { get; set; }
	}



    public class DepartBranchZoneHeadViewModel
    {
        public int? HeadId { get; set; }
        public string HeadCode { get; set; }
        public string HeadName { get; set; }
        public string post { get; set; }
        public int? branchId { get; set; }
        public string branchName { get; set; }
    }

    public class BranchZoneDepHeadInfoViewModel
    {
        public int? HeadId { get; set; }
        public string HeadCode { get; set; }
        public string HeadName { get; set; }
        public string post { get; set; }
        public int? branchId { get; set; }
        public int? depId { get; set; }
        public int? locationId { get; set; }
        public string branchName { get; set; }
    }
}
