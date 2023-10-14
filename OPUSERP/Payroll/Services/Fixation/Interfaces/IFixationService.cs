using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Payroll.Data.Entity.Fixation;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Models;

namespace OPUSERP.Payroll.Services.Fixation.Interfaces
{
	public interface IFixationService
	{
        Task<IEnumerable<FixationIntegration>> GetAllFixationIntegration();
        Task<Signatory> GetSignatoryByDesignationId(int designationId);
        Task<IEnumerable<Signatory>> GetAllSignatory();

        Task<IEnumerable<EmployeeInfo>> GetAllEmployeeById(int employeeId);
        Task<bool> IsDuplicateSignatory(Signatory signatory);

		Task<int> SaveSignatory(Signatory signatory);
        Task<int> SaveFixationIntegration(FixationIntegration fixationIntegration);
        Task<IEnumerable<FixationIntegration>> GetAllFixationIntegrationById(int id);
		Task<FixationIntegration> GetfixationIntegrationInfoById(int id);
        Task<SalaryProcessDataViewModel> UpdatePromotionFinally(int id);
        Task<IEnumerable<SalaryGrade>> GetSalarygrade();
        Task<IEnumerable<FixationIntegration>> GetFixationIntegrationByEmpCode(string code);
        Task<FixationIntegration> GetLastFixationIntegrationByEmpCode(string code);
		Task<SalaryGrade> GetNewGradeByEmpCode(string code);
        Task<BranchManager> GetBranchManagerByBranchId(int id);
        Task<IEnumerable<FixaType>> GetAllFixType();
        Task<IEnumerable<EmployeePostingPlace>> GetPostingPlaceByFixationId(int id);
        Task<FixationIntegration> GetFixationIntegrationByEmpCodeForReFixation(string code);
        Task<IEnumerable<LastFixationEmployeeWise>> GetAllPunishmentReFixationIntegration();
        //Task<FixationIntegration> GetAllPunishmentReFixationIntegration();
        Task<FixationIntegration> GetPunishmentReFixationInfoById(int id);
        Task<List<string>> GetAllSalarySlabByGradeId(int gradeId);
        //Task<int> GetSalarygradeByEmpId(int Id);
        Task<int> GetSalarygradeByFixId(int Id);
        Task<IEnumerable<SalarySlab>> GetAllSalarySlabNameByGradeId(int gradeId);
        Task<int> GetEmplCodeByFixId(int id);
        Task<string> GetSalarygradeByEmpId(string code);
        Task<string> GetSalarygradeIdByEmpCode(string code);
        Task<FixationIntegration> GetFixationIntegrationByFixIdForReFixation(int Id);
        Task<FixationIntegration> GetParticularInfoByfixId(int id);
        Task<FixationIntegration> GetAllFixationIntegrationByIdNew(int id);
        Task<IEnumerable<FixationIntegration>> GetAllFixForPunishmentById(int id);
        Task<DisciplinaryLog> GetDisciplinaryInfobyEmpID(int id);
        Task<string> GetDisciplinaryIdbyEmpcode(string code);
        Task<IEnumerable<EmpPostingPlaceVm>> GetPostingPlaceByFixId(int fixId);
        Task<IEnumerable<YearlyFixationVm>> GetYearlyFixation(int year, string type);
		Task<IEnumerable<FixationSummaryViewModel>> GetFixationSummaryByYearAndType(int year, string type);
		Task<IEnumerable<FixationSummaryViewModel>> GetFixationSummaryStaffByYearAndType(int year, string type);
        Task<CBSProcessSp> CreateIncrementProposal(int year, int employeeInfoId);
		Task<CBSProcessSp> ProcessIncrement(int year, int employeeInfoId);

		Task<decimal> UpdateEmpSalaryStructureByFixationId(int fixId);
		Task<int> SaveIncrementHeld(IncrementHeld model);
		Task<IEnumerable<IncrementHeld>> GetAllIncrementHeld();
		Task<int> DeleteIncrementHeld(int id);
        Task<IEnumerable<FixationListViewModel>> FilterFixationIntegration(int year, string code, int designationId, int zoneId, int branchId, int officeId,int departId, string fixationType);

        Task<IEnumerable<FixationIntegration>> GetAllFixationIntegrationByYear(int year);
        Task<IEnumerable<EmpPostingPlaceVm>> GetPostingPlaceByEmpId(int empId);
    }
}
