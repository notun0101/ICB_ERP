using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
	//1=Inactive, 0=Active
    [Table("EmployeeInfo")]
    public class EmployeeInfo : Base
    {
        [Required]
        [MaxLength(50)]
        public string employeeCode { get; set; }

        [MaxLength(100)]
        public string nationalID { get; set; }

        public string PassportNo { get; set; }
        public string PassportIssueDate { get; set; }
        public string PassportExDate { get; set; }

        [MaxLength(100)]
        public string birthIdentificationNo { get; set; }

        [MaxLength(250)]
        public string govtID { get; set; }
        [MaxLength(250)]
        public string gpfNomineeName { get; set; }
        [MaxLength(50)]
        public string gpfAcNo { get; set; }
        [MaxLength(250)]
        public string nameEnglish { get; set; }
        [MaxLength(250)]
        public string nameBangla { get; set; }
       
        [MaxLength(250)]
        public string fatherNameEnglish { get; set; }
        [MaxLength(250)]
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

		[MaxLength(250)]
		public string motherNameEnglish { get; set; }
		[MaxLength(250)]
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


		[MaxLength(100)]
        public string nationality { get; set; }
        [MaxLength(3)]
        public string disability { get; set; }

		public int? disabilityTypeId { get; set; }
		public DisabilityType disabilityType { get; set; }

		public string favouriteColor { get; set; }

		[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dateOfBirth { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? joiningDatePresentWorkstation { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? joiningDateGovtService { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dateofregularity { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dateOfPermanent { get; set; }
        
        public DateTime? LPRDate { get; set; } //calculative From Date of Birth

        public DateTime? PRLStartDate { get; set; } //calculative From Date of Birth
        public DateTime? PRLEndDate { get; set; } //calculative From Date of Birth
        [MaxLength(10)]
        public string gender { get; set; }
        [MaxLength(250)]
        public string birthPlace { get; set; }
        [MaxLength(20)]
        public string maritalStatus { get; set; }

        public string qualification { get; set; }

        public int?  religionId { get; set; }
        public Religion religion { get; set; }

        public int? employeeTypeId { get; set; }
        public EmployeeType employeeType { get; set; }

        public string contructualType { get; set; }

        public int? activityStatus { get; set; }

        public int? departmentId { get; set; }
        public Department department { get; set; }
        [MaxLength(30)]
        public string tin { get; set; }

        public string batch { get; set; }
        [MaxLength(20)]
        public string bloodGroup { get; set; }

        public bool? freedomFighter { get; set; }
        [MaxLength(150)]
        public string freedomFighterNo { get; set; }
        [MaxLength(50)]
        public string telephoneOffice { get; set; }
        [MaxLength(50)]
        public string telephoneOfficeBn { get; set; }
        
        [MaxLength(50)]
        public string telephoneResidence { get; set; }
        [MaxLength(50)]
        public string pabx { get; set; }
        [MaxLength(150)]
        public string emailAddress { get; set; }
        [MaxLength(150)]
        public string emailAddressPersonal { get; set; } // Next generated not planned

        [MaxLength(50)]
        public string mobileNumberOffice { get; set; }

        [MaxLength(50)]
        public string mobileNumberPersonal { get; set; }
        [MaxLength(250)]
        public string specialSkill { get; set; }

		public int? height { get; set; }
		public int? weight { get; set; }

		[MaxLength(50)]
        public string seniorityNumber { get; set; }
        public string designation { get; set; }
        [MaxLength(150)]
        public string skypeId { get; set; }
		[MaxLength(150)]
		public string FacebookId { get; set; }
		[MaxLength(150)]
		public string TwitterId { get; set; }
		[MaxLength(150)]
		public string InstagramId { get; set; }
		[MaxLength(150)]
		public string LinkedInId { get; set; }
		[MaxLength(150)]
		public string WhatsAppId { get; set; }

		public int? post { get; set; } // Related PostID But Not FK Referenced 

        public int? designationCheck { get; set; }//Current Charged Checked
        [MaxLength(250)]
        public string joiningDesignation { get; set; }


        public int? JoinDesignationId { get; set; }
        public Designation JoinDesignation { get; set; }


        public DateTime? DateOfRetirement { get; set; }

		[MaxLength(100)]
        public string natureOfRequitment { get; set; } // Direct Or Absorbed
        [MaxLength(100)]
        public string homeDistrict { get; set; }
		public int? DivisionId { get; set; }
		public Division Division { get; set; }
		public string BranchName { get; set; }

        public string problem { get; set; }

        public int? branchId { get; set; }
        public SpecialBranchUnit branch { get; set; }

		public int? hrBranchId { get; set; }
        public HrBranch hrBranch { get; set; }
        public int? isManager { get; set; }//0=no; 1=yes;

        public int? hrDivisionId { get; set; }
        public HrDivision hrDivision { get; set; }

        public int? pNSId { get; set; }
        public PNS pNS { get; set; }

        //For Type Managing 
        [MaxLength(100)]
        public string orgType { get; set; }

        //Application User LInk
        public String ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int? isApproved { get; set; }//0= not approved,1=approved

        public int? shiftGroupId { get; set; }
        public ShiftGroupMaster shiftGroup { get; set; }

        public int? employeeStatusId { get; set; }
        public ServiceStatus employeeStatus { get; set; }

        public int? hrProgramId { get; set; }
        public HrProgram hrProgram { get; set; }

        public int? hrUnitId { get; set; }
        public HrUnit hrUnit { get; set; }

        public int? locationId { get; set; }
        public Location location { get; set; }
        public int? functionInfoId { get; set; }
        public FunctionInfo functionInfo { get; set; }

		public int? isEthnicMinority { get; set; }
		public string ethnicMinorityType { get; set; }

		public string disablityType { get; set; }
        [DefaultValue(1)]
        public int? salaryStatus { get; set; }
        public string salaryStatusComment { get; set; }

		public int? bloodDonateStatus { get; set; } //0 or 1

        public int? approveStatus { get; set; }

        
        public DateTime? prlDate { get; set; }
        public string refLetterNo { get; set; }
        public string fileNo { get; set; }
        public string presentPosting { get; set; }

        public DateTime? lastPromotionDate { get; set; }
        public DateTime? lastWorkingDate { get; set; }
        public int? lastDesignationId { get; set; }
        public Designation lastDesignation { get; set; }
        public DateTime? lastTransferDate { get; set; }
        public decimal? lastSalary { get; set; }
		public string identificationSign { get; set; }


		public string prevOrganisationResp { get; set; } //textarea
        public string significantAchivement { get; set; } //textarea
        public string areaOfProficiency { get; set; } //textarea
        public string joiningLocation { get; set; } //dropdown
        public string placeOfPosting { get; set; } //dropdown
		public DateTime? postingDate { get; set; }

		public DateTime? probationStart { get; set; }
        public DateTime? probationEnd { get; set; }

        public DateTime? bondDateStart { get; set; }
        public DateTime? bondDateEnd { get; set; }

        public int? customRoleId { get; set; }
        public CustomRole customRole { get; set; }

        public int? isFestival { get; set; } //Yes=1, No=0
        public string DesiCode { get; set; }

        public string PRLreportDateBn { get; set; }
        public DateTime? PRLreportDateEn { get; set; }

		public int? joiningGradeId { get; set; }
		public SalaryGrade joiningGrade { get; set; }
        public string joiningGradeName { get; set; }
        public decimal? joiningBasic { get; set; }

		public int? currentGradeId { get; set; }
		public SalaryGrade currentGrade { get; set; }
		public decimal? currentBasic { get; set; }

		public string seniorityLevel { get; set; }

        public int? expertiseId { get; set; }
        public EmpExpertise expertise { get; set; }

        public string sbAccount { get; set; }
        public string specialRole { get; set; }
        public string SupervisorCode { get; set; }
        public string SupervisorName { get; set; }

        public DateTime? dateOfResignation { get; set; }

       // public string HouseLoanDeductFrom { get; set; } = ""; //""= no active loan, house= deducted from Salary rent, lakh= Amount Per Lakh 
        //public DateTime? dateOfDeath { get; set; }
    }
}
