using OPUSERP.Areas.HRPMSDisciplineInvestigation.Models;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System.Collections.Generic;
using System;

namespace OPUSERP.Areas.HRPMSReport.Models
{
    public class AllHrReportViewModel
    {
        public IEnumerable<Company> companies { get; set; }
        public IEnumerable<Vacancy> vacancies { get; set; }
        public IEnumerable<HrReportViewModel> hrReportViewModels { get; set; }
        public IEnumerable<HrEducationReportViewModel> hrEducationReportViewModels { get; set; }
        public IEnumerable<HrTrainingReportViewModel> hrTrainingReportViewModels { get; set; }
        public IEnumerable<HrBelongingReportViewModel> hrBelongingReportViewModels { get; set; }
        public IEnumerable<HrSummaryReportViewModel> hrSummaryReportViewModels { get; set; }
        public IEnumerable<HrSummaryReportViewModel> hrSummaryReportViewModels6 { get; set; }
        public IEnumerable<HrSummaryReportViewModel> hrSummaryReportViewModels12 { get; set; }

        public IEnumerable<GManViewModel> genderManpowerViewModels { get; set; }
        public IEnumerable<GManViewModel> genderManpowerViewModels6 { get; set; }
        public IEnumerable<GManViewModel> genderManpowerViewModels12 { get; set; }
        public IEnumerable<SignatureReportViewModel> signatureReports { get; set; }

        public IEnumerable<EducationManpowerViewModel> educationManpowerViewModels { get; set; }
        public IEnumerable<EducationManpowerViewModel> educationManpowerViewModels1 { get; set; }
        public IEnumerable<EducationManpowerViewModel> educationManpowerViewModels2 { get; set; }
        public IEnumerable<EducationManpowerViewModel> educationManpowerViewModels1y { get; set; }

        public IEnumerable<SpecialBranchUnit> specialBranchUnits { get; set; }
        public IEnumerable<Subject> subjects { get; set; }
        public IEnumerable<Organization> organizations { get; set; }
        public IEnumerable<LevelofEducation> levelofEducations { get; set; }
        public IEnumerable<CourseTitle> courseTitles { get; set; }
        public IEnumerable<BelongingItem> belongingItems { get; set; }
        public IEnumerable<Religion> religions { get; set; }
        public IEnumerable<Division> divisions { get; set; }
        public IEnumerable<District> districts { get; set; }
        public IEnumerable<Location> branches { get; set; }
        public IEnumerable<Location> zoneList { get; set; }
        public IEnumerable<CustomRole> customRoles { get; set; }
        public IEnumerable<Designation> designations { get; set; }
        public IEnumerable<HrBranch> hrBranches { get; set; }
        public IEnumerable<HrDivision> hrDivisions { get; set; }
        public IEnumerable<Department> departments { get; set; }
        public IEnumerable<FunctionInfo> functionInfos { get; set; }


        public IEnumerable<EmployeeReligion> employeeReligions { get; set; }
        public IEnumerable<EmployeeDivision> employeeDivisions { get; set; }
        public IEnumerable<EmployeeDistrict> employeeDistricts { get; set; }
        public IEnumerable<EmployeeBranch> employeeBranches { get; set; }

        public EmployeeReligion employeeReligion { get; set; }

        public IEnumerable<EmployeeGender> employeeGenders { get; set; }
        public EmployeeGender employeeGender { get; set; }

        public EmployeeDivision employeeDivision { get; set; }
        public EmployeeDistrict employeeDistrict { get; set; }
        public EmployeeBranch employeeBranch { get; set; }

        public IEnumerable<EmployeeInfo> employeeInfos { get; set; }
        public IEnumerable<EducationalQualification> educationalQualifications { get; set; }
        public IEnumerable<EmpSuspensionViewModel> employeeInfos1 { get; set; }
        public IEnumerable<EmpDiplomaViewModel> employeeDiploma { get; set; }
        public IEnumerable<EmpDiplomaVm> employeeDiplomas { get; set; }
        public IEnumerable<AwardWiseEmployee> awardWiseEmployees { get; set; }

        public IEnumerable<DesignationSeniorityViewModel> seniorityWiseDesig { get; set; }
        public IEnumerable<EmployeeWithYearsTransfer> EmployeeTransferYears { get; set; }
        public IEnumerable<EmployeeWithPostingPlacesViewModel> employeeWithPostingPlaces { get; set; }
        public IEnumerable<AllDivisionEmpViewModel> allDivisionEmpViewModels { get; set; }
        public IEnumerable<EmployeeWithOfficeViewModel> employeeWithOfficeViewModels { get; set; }
        public IEnumerable<RoleWithEmployees> roleWithEmployees { get; set; }
        public IEnumerable<OfficeBranchPosition> officeBranchPositions { get; set; }
        //public IEnumerable<HrManpowerByAge> hrManpowerByAges { get; set; }
        public RoleWiseAgeViewModel roleWiseAge { get; set; }
        public VacancyViewModel vacancyViewModel { get; set; }
        public IEnumerable<VacancyViewModel> vacancyViewModels { get; set; }
        public IEnumerable<SeniorityListViewModelNew> seniorityListViewModelNews { get; set; }
        public IEnumerable<BranchManagerViewModel> branchManagerViewModels { get; set; }

    }

    public class HrReportApiViewModel
    {
        public IEnumerable<Company> companies { get; set; }
        public IEnumerable<HrEducationReportViewModel> hrEducationReportViewModels { get; set; }
        public IEnumerable<HrReportViewModel> hrReportViewModels { get; set; }
        public IEnumerable<EmployeeInfo> employeeInfos { get; set; }
        public EmployeeGender employeeGender { get; set; }
        public IEnumerable<EmployeeWithPostingPlacesViewModel> employeeWithPostingPlaces { get; set; }
        public IEnumerable<HrSummaryReportViewModel> hrSummaryReportViewModels { get; set; }
        public IEnumerable<HrSummaryReportViewModel> hrSummaryReportViewModels6 { get; set; }
        public IEnumerable<HrSummaryReportViewModel> hrSummaryReportViewModels12 { get; set; }
        public IEnumerable<AllEmployeeJsonNew> allEmployee { get; set; }
    }

    public class SignatureReportViewModel
    {
        public int? Id { get; set; }
        public string employeeCode { get; set; }
        public string nameEnglish { get; set; }
        public string designationName { get; set; }
        public string telephoneOffice { get; set; }
        public string emailAddress { get; set; }
        public string posting { get; set; }
        public string englishSign { get; set; }
        public string banglaSign { get; set; }
    }
    public class EmployeeInfoApiViewModel
    {
        public int? Id { get; set; }
        public string employeeCode { get; set; }
        public string nameEnglish { get; set; }
        public string nameBangla { get; set; }
        public string phonePersonal { get; set; }
        public string phoneOffice { get; set; }
        public string designation { get; set; }
        public string emailpersonal { get; set; }
        public string emailOffice { get; set; }    
        public string branchName { get; set; }
        public string joiningDate { get; set; }
        public string placeOfPosting { get; set; }
        public string imageUrl { get; set; }
        public string englishSign { get; set; }
        public string banglaSign { get; set; }
		public string bloodGroup { get; set; }
		
	}
    public class RoleWiseAgeViewModel
    {
        public int? entryMale { get; set; }
        public int? entryFemale { get; set; }

        public int? midMale { get; set; }
        public int? midFemale { get; set; }

        public int? seniorMale { get; set; }
        public int? seniorFemale { get; set; }

        public int? less30Male { get; set; }
        public int? less30Female { get; set; }

        public int? thirtyTofiftyMale { get; set; }
        public int? thirtyTofiftyFemale { get; set; }

        public int? upto50Male { get; set; }
        public int? upto50Female { get; set; }
    }

    public class GenderManpowerViewModel
    {
        public int male { get; set; }
        public int maleRegular { get; set; }
        public int maleContructual { get; set; }

        public int female { get; set; }
        public int femaleRegular { get; set; }
        public int femaleContructual { get; set; }
    }
    public class GManViewModel
    {
        public string title { get; set; }
        public int count { get; set; }
    }

    public class EducationManpowerViewModel
    {
        public string title { get; set; }
        public int count { get; set; }
        public IEnumerable<EmpExpertise> empExpertises { get; set; }
        public IEnumerable<EmployeeInfo> employeeInfos { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
    }

    //public class HrManpowerByAge
    //{
    //    public int rowSlNo { get; set; }
    //    //public string deptName { get; set; }
    //    public int? totalNumber { get; set; }
    //    public decimal? Mix { get; set; }

    //}
    public class RoleWithEmployees
    {
        public CustomRole customRole { get; set; }
        public IEnumerable<EmployeeInfo> employees { get; set; }
    }
    public class OfficeBranch
    {
        public IEnumerable<FunctionInfo> office { get; set; }
        public IEnumerable<HrBranch> hrBranches { get; set; }
    }
    public class OfficeBranchPosition
    {
        public string officeName { get; set; }
        public int Officers { get; set; } = 0;
        public int Staffs { get; set; } = 0;
        public int Total { get; set; } = 0;
    }
    public class EmployeeWithOfficeViewModel
    {
        public EmployeeInfo employee { get; set; }
        public string designation { get; set; }
        public string age { get; set; }
    }
    public class EmployeeWithPostingPlacesViewModel
    {
        public EmployeeInfo employee { get; set; }
        public string designation { get; set; }
        public int? count { get; set; }
        public IEnumerable<EmployeePostingPlaceViewModel> postings { get; set; }
        public IEnumerable<EmployeeInfo> DegWiseEmpList { get; set; }
        
        public string age { get; set; }
        public string degName { get; set; }
        public int? deg { get; set; }
        public string duration { get; set; }
        public string branch { get; set; }
    }
     public class AllDivisionEmpViewModel
    {
        public HrBranch  hrBranch { get; set; }
        public HrDivision hrDivision { get; set; }
        public Department  department { get; set; }
        public FunctionInfo  office { get; set; }
        public int? count { get; set; }
        public IEnumerable<EmployeeWithPostingPlacesViewModel> divEmpListVM { get; set; }
    }

    public class EmployeePostingPlaceViewModel
    {
        public EmployeePostingPlace posting { get; set; }
        public string duration { get; set; }
        
    }
    public class EmpPostingViewModel
    {
        public EmployeeInfo employee { get; set; }
        public EmployeePostingPlace employeePosting { get; set; }
    }
    public class EmployeeWithYearsTransfer
    {
        public EmployeeInfo employee { get; set; }
        public string duration { get; set; }
    }
    public class DesignationSeniorityViewModel
    {
        public Department department { get; set; }
        public Designation designation { get; set; }
        public int designationCount { get; set; } = 0;
        public IEnumerable<EmployeeInfo> employeeInfos { get; set; }
        public IEnumerable<SeniorityListViewModelNew> employees { get; set; }
    }
    public class EmpDiplomaViewModel
    {
        public EmployeeInfo employeeInfo { get; set; }
        public BankingDiploma bankingDiploma { get; set; }
        public Designation designation { get; set; }
        public CustomRole customRole { get; set; }
        public EmployeeAward employeeAward { get; set; }
    }

    public class EmpDiplomaVm
    {
        public string employeeCode { get; set; }
        public string nameEnglish { get; set; }
        public string designationName { get; set; }
        public string diplomaPart1 { get; set; }
        public string diplomaPart2 { get; set; }
        public string passingYear1 { get; set; }
        public string passingYear2 { get; set; }
    }
    public class EmployeeReligion
    {
        public Religion religion { get; set; }
        public IEnumerable<EmployeeInfo> employeeInfos { get; set; }
    }

    public class EmployeeDivision
    {
        public Division division { get; set; }
        public IEnumerable<EmployeeInfo> employeeInfos { get; set; }
    }
    public class EmployeeDistrict
    {
        public District district { get; set; }
        public IEnumerable<EmployeeInfo> employeeInfos { get; set; }
    }
    public class EmployeeBranch
    {
        public Location branch { get; set; }
        public IEnumerable<EmployeeInfo> employeeInfos { get; set; }
    }

    // public class EmployeeAllBranch
    //{
    //    public HrBranch branch { get; set; }
    //    public IEnumerable<EmployeeInfo> employeeInfos { get; set; }
    //}

  


    public class EmployeeGender
    {
        public string gender { get; set; }
        public IEnumerable<EmployeeInfo> employeeInfos { get; set; }
    }
    public class AwardWiseEmployee
    {
        public Award award { get; set; }
        public IEnumerable<EmployeeInfo> employeeInfos { get; set; }
        public IEnumerable<EmpDiplomaViewModel> employeeInfo { get; set; }
    }

	public class SeniorityListViewModelNew
	{
		public string employeeCode { get; set; }
		public string nameEnglish { get; set; }
		public string nameBangla { get; set; }
		public string designationName { get; set; }
		public string designationNameShort { get; set; }
		public string eduQualification { get; set; }
		public DateTime? dateOfBirth { get; set; }
		public DateTime? joiningDate { get; set; }
		public DateTime? lastPromotionDate { get; set; }
		public DateTime? lastTransferDate { get; set; }
		public DateTime? DateOfRetirement { get; set; }
		public string posting { get; set; }
		public string homeDistrict { get; set; }
		public int? designationId { get; set; }
		public int? designationCount { get; set; }
		public int? seniorityLevel { get; set; }
	}


    public class BranchManagerViewModel
    {
        public string employeeCode { get; set; }
        public string nameEnglish { get; set; }
        public string Designation { get; set; }
        public string Responsibility { get; set; }

        public string branchCode { get; set; }
        public string branchName { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
    }
}
