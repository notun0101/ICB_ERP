using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Fixation;
using OPUSERP.Payroll.Data.Entity.Salary;

namespace OPUSERP.Areas.Payroll.Models
{
    public class FixationIntegrationViewModel
    {
        public int fixationintegrationId { get; set; }
        public int? employeeId { get; set; }
        public string empCode { get; set; }
        public string employeeName { get; set; }
        public string grade { get; set; }
        public string currentGrade { get; set; }
        public string remarks { get; set; }
        public DateTime? lastIncrementDate { get; set; }
       // public DateTime? punishmentDate { get; set; }
        public string postingPlace { get; set; }
        //public decimal? initialBasic { get; set; }
        public decimal? increment { get; set; }
        //public decimal? startingBasic { get; set; }
        //public decimal? newBasicPoint { get; set; }
        public DateTime? lastPromotionDate { get; set; }
        public DateTime? punishmentDate { get; set; }
        //public string brachCode { get; set; }
        public string promotionScale { get; set; }
        public decimal? amount { get; set; }
        public decimal? NbasicPay { get; set; }
        public decimal?[] amountMultiple { get; set; }
        public int? fixationTypeId { get; set; }
        public int? fixaTypeId { get; set; }

        public string fixationType { get; set; }
        public int? fixationGradeId { get; set; }
        public DateTime? effectiveDate { get; set; }
        public DateTime?[] effectiveDateMultiple { get; set; }
        public string refNo { get; set; }
        public DateTime? nextIncrementDate { get; set; }
        public int? slNo { get; set; }
        public DateTime letterDate { get; set; }
        public int? batchId { get; set; }
        public int? isApproved { get; set; }
        public string reportHeader { get; set; }
        public DateTime? joiningDate { get; set; }
        public DateTime? dateOfBirth { get; set; }
        //public decimal? basicPay { get; set; }
        public string designation { get; set; }
        public int? lastDesignationId { get; set; }
        public int? fileNo { get; set; }
        public string fixationGrade { get; set; }
        public string[] fixationGradeMultiple { get; set; }
      
        public string particular { get; set; }
        public string[] particularMultiple { get; set; }

        public int?[] chkFixId { get; set; }

        //public string currentGradeId { get; set; }
        public int? statusId { get; set; }
        public int? categoryId { get; set; }
        public string hrBranchId { get; set; }
        
        public IEnumerable<FixationIntegration> fixationIntegrationList { get; set; }
        public IEnumerable<LastFixationEmployeeWise> fixationIntegrationLists { get; set; }
        public FixationIntegration fixationIntegration { get; set; }

        public IEnumerable<SalaryGrade> salaryGrades { get; set; }
        public IEnumerable<FixaType> fixaTypes { get; set; }
        public IEnumerable<Designation> designations { get; set; }
        public IEnumerable<FunctionInfo> functionInfos { get; set; }

        public IEnumerable<HrBranch> hrBranches { get; set; }
        public IEnumerable<Location> zoneLocation { get; set; }
        public IEnumerable<Department> departments { get; set; }

        public IEnumerable<FixationListViewModel> fixationLists { get; set; }

        public EmployeeInfo employee { get; set; }


    }
    public class LastFixationEmployeeWise
    {
        public EmployeeInfo employee { get; set; }
        public FixationIntegration fixation { get; set; }
    }
    public class FixationCurrentSalaryViewModel
    {
        public string empCode { get; set; }
        public int gradeId { get; set; }
        public decimal? salaryAmount { get; set; }
    }
    public class SalaryGradeJoiningDateViewModel
    {
        public EmployeeInfo employee { get; set; }
        public decimal? jobDuration { get; set; }
        public int gradeId { get; set; }
        public int gradeBasic { get; set; }
        public string ngradeName { get; set; }
        public int ngradeId { get; set; }
    }

    public class FixationListViewModel
    {
        public int? Id { get; set; }
        public int? employeeId { get; set; }
        public string currentGrade { get; set; }
        public string empCode { get; set; }
        public string nameEnglish { get; set; }
        public string payScale { get; set; }
        public string brachCode { get; set; }
        public DateTime? effectiveDate { get; set; }
        public string designation { get; set; }
        public decimal? amount { get; set; }
        public string fixationType { get; set; }
        public int? fixaTypeId { get; set; }
        public DateTime? joiningDate { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public DateTime? lastIncrementDate { get; set; }
        public string postingPlace { get; set; }
        public DateTime? lastPromotionDate { get; set; }
        public int? fixationGradeId { get; set; }
        public int? statusId { get; set; }
        public string refNo { get; set; }
        public int? fileNo { get; set; }
        public int? lastDesignationId { get; set; }
        public DateTime? letterDate { get; set; }
        public DateTime? nextIncrementDate { get; set; }
        public string empGrade { get; set; }
        public decimal? empBasic { get; set; }
        public string newSignatory { get; set; }
        public string newSignatoryName { get; set; }
        public string newSignatoryDesig { get; set; }

	}
}
