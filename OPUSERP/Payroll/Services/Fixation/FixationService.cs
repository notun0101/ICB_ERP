using OPUSERP.Data;
using OPUSERP.Payroll.Services.Fixation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Payroll.Data.Entity.Fixation;
using OPUSERP.HRPMS.Data.Entity.Employee;
using Microsoft.EntityFrameworkCore;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Models;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace OPUSERP.Payroll.Services.Fixation
{
    public class FixationService : IFixationService
    {
        private readonly ERPDbContext _context;

        public FixationService(ERPDbContext context)
        {
            _context = context;
        }


        #region Fixation
        public async Task<IEnumerable<FixationIntegration>> GetAllFixationIntegration()
        {
            return await _context.fixationIntegrations.Include(a => a.employee).Include(a => a.lastDesignation).Include(x => x.employee.lastDesignation).Include(x => x.employee.hrBranch).Include(x => x.employee.currentGrade).OrderBy(a => Convert.ToInt32(a.employee.lastDesignation.designationCode)).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Signatory>> GetAllSignatory()
        {
            return await _context.Signatories.Include(x=>x.EmployeeInfo).Include(a => a.Designation).AsNoTracking().ToListAsync();
        }


        public async Task<Signatory> GetSignatoryByDesignationId(int designationId)
		{
			return await _context.Signatories.Include(x=>x.EmployeeInfo).Where(a => a.DesignationId == designationId).AsNoTracking().SingleOrDefaultAsync();
		}

		public async Task<SalaryProcessDataViewModel> UpdatePromotionFinally(int id)
        {
            return await _context.salaryProcessDataViewModels.FromSql($"sp_ProcessPromotionByFixId {id}").AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<decimal> UpdateEmpSalaryStructureByFixationId(int fixId)
        {
            var fixation = await _context.fixationIntegrations.FindAsync(fixId);
            var data = await _context.houseRentCalculates.FromSql($"sp_UpdateEmployeeStructureByBasic {fixation.employeeId},{fixation.amount}").AsNoTracking().Select(x => x.houseRent).FirstOrDefaultAsync();
            return Convert.ToDecimal(data);
        }
        //public async Task<IEnumerable<FixationIntegration>> GetAllPunishmentReFixationIntegration()
        //{
        //    return await _context.fixationIntegrations.Include(a => a.employee).Include(x=> x.employee.lastDesignation).Include(x=>x.employee.hrBranch).Include(x=>x.employee.currentGrade).Where(x=>x.fixaTypeId==2).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        //}
        public async Task<IEnumerable<LastFixationEmployeeWise>> GetAllPunishmentReFixationIntegration()
        {
            var fixations = new List<LastFixationEmployeeWise>();
            var empList = await _context.fixationIntegrations.Include(x => x.employee).AsNoTracking().Select(x => x.employee).Distinct().ToListAsync();
            foreach (var item in empList)
            {
                fixations.Add(new LastFixationEmployeeWise
                {
                    employee = item,
                    fixation = await _context.fixationIntegrations.AsNoTracking().Where(x => x.employeeId == item.Id).LastOrDefaultAsync()
                });
            }
            return fixations;
            //return await _context.fixationIntegrations.Include(a => a.employee).Include(x=> x.employee.lastDesignation).Include(x=>x.employee.hrBranch).Include(x=>x.employee.currentGrade).Where(x=>x.fixaTypeId==2).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<FixationListViewModel>> FilterFixationIntegration(int year, string code, int designationId,int zoneId,int branchId,int officeId, int departId, string fixationType)
        {
            var data = await _context.fixationListViewModels.FromSql($"sp_FilterFixationIntegration {year}, {code}, {designationId}, {zoneId}, {branchId}, {officeId},{departId}, {fixationType}").ToListAsync();
            return data;
        }

        public async Task<IEnumerable<FixationIntegration>> GetFixationIntegrationByEmpCode(string code)
        {
            var data = await _context.fixationIntegrations.Include(x => x.fixationGrade).Include(x => x.lastDesignation).Include(x => x.employee).Include(x => x.employee.currentGrade).Include(x => x.employee.joiningGrade).Where(x => x.empCode == code).AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<FixationIntegration> GetFixationIntegrationByEmpCodeForReFixation(string code)
        {
            var data = await _context.fixationIntegrations.Include(x => x.fixationGrade).Include(x => x.employee).Include(x => x.employee.currentGrade).Include(x => x.employee.joiningGrade).Where(x => x.empCode == code).AsNoTracking().LastOrDefaultAsync();
            return data;
        }
        public async Task<FixationIntegration> GetFixationIntegrationByFixIdForReFixation(int Id)
        {
            var data = await _context.fixationIntegrations.Include(x => x.fixationGrade).Include(x => x.employee).Include(x => x.employee.currentGrade).Include(x => x.employee.joiningGrade).Where(x => x.Id == Id).AsNoTracking().LastOrDefaultAsync();
            return data;
        }


        public async Task<SalaryGrade> GetNewGradeByEmpCode(string code)
        {
            var emp = await _context.employeeInfos.Where(x => x.employeeCode == code).FirstOrDefaultAsync();
            var newGrade = await _context.salaryGrades.Where(x => x.Id < emp.currentGradeId).LastOrDefaultAsync();
            return newGrade;
        }

        public async Task<FixationIntegration> GetLastFixationIntegrationByEmpCode(string code)
        {
            var data = await _context.fixationIntegrations.Where(x => x.empCode == code).AsNoTracking().LastOrDefaultAsync();
            return data;
        }
        public async Task<IEnumerable<FixationIntegration>> GetAllFixationIntegrationById(int id)
        {
            var data = await _context.fixationIntegrations.AsNoTracking().Where(x => x.fixaTypeId == 1 && x.empCode == _context.fixationIntegrations.AsNoTracking().Where(y => y.Id == id).Select(y => y.empCode).FirstOrDefault()).ToListAsync();
            return data;
            //return await _context.fixationIntegrations.Include(a => a.employee).Include(a => a.employee.lastDesignation).Include(x=>x.employee.currentGrade).Include(x=>x.employee.department).Include(x=>x.employee.hrBranch).Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<FixationIntegration> GetAllFixationIntegrationByIdNew(int id)
        {
            return await _context.fixationIntegrations.Include(a => a.employee).Include(a => a.employee.lastDesignation).Include(x => x.employee.currentGrade).Include(x => x.employee.department).Include(x => x.employee.hrDivision).Include(x => x.employee.location).Include(x => x.employee.functionInfo).Include(x => x.employee.hrUnit).Include(x => x.employee.hrBranch).Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<FixationIntegration>> GetAllFixationIntegrationByYear(int year)
        {
            return await _context.fixationIntegrations.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<EmployeePostingPlace>> GetPostingPlaceByFixationId(int id)
        {
            var empId = await _context.fixationIntegrations.Where(x => x.Id == id).Select(x => x.employeeId).FirstOrDefaultAsync();
            return await _context.employeePostings.Include(x => x.branch).Include(x => x.hrBranch).Include(x => x.office).Include(x => x.location).Include(x => x.hrUnit).Include(x => x.department).Where(x => x.employeeId == empId && x.status == 1 && x.type == 1).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<EmpPostingPlaceVm>> GetPostingPlaceByFixId(int fixId)
        {
            return await _context.empPostingPlaces.FromSql($"sp_EmpPostingByFixId @p0", fixId).ToListAsync();
        }

        public async Task<IEnumerable<EmpPostingPlaceVm>> GetPostingPlaceByEmpId(int empId)
        {
            return await _context.empPostingPlaces.FromSql($"sp_EmpPostingByEmpId @p0", empId).ToListAsync();
        }



		public async Task<IEnumerable<YearlyFixationVm>> GetYearlyFixation(int year, string type)
		{
			return await _context.yearlyFixationVms.FromSql($"sp_GetFixationYearlyReport {year}, {type}").ToListAsync();
		}
		public async Task<IEnumerable<FixationSummaryViewModel>> GetFixationSummaryByYearAndType(int year, string type)
		{
			return await _context.fixationSummaryViewModels.FromSql($"sp_FixationSummary {year}, {type}").ToListAsync();
		}
		public async Task<IEnumerable<FixationSummaryViewModel>> GetFixationSummaryStaffByYearAndType(int year, string type)
		{
			return await _context.fixationSummaryViewModels.FromSql($"sp_FixationSummaryStaff {year}, {type}").ToListAsync();
		}

        //Asad Added on 26.07.2023
        public async Task<CBSProcessSp> CreateIncrementProposal(int year, int employeeInfoId)
        {
            return await _context.cBSProcessSps.FromSql($"SP_CreateIncrementProposal {year}, {employeeInfoId}").FirstOrDefaultAsync();
        }

        public async Task<CBSProcessSp> ProcessIncrement(int year, int employeeInfoId)
		{
			return await _context.cBSProcessSps.FromSql($"sp_ProcessFixationIncrement {year}, {employeeInfoId}").FirstOrDefaultAsync();
		}

		public async Task<BranchManager> GetBranchManagerByBranchId(int id)
        {
            var manager = new BranchManager();
            var data = await _context.employeePostings.Where(x => x.hrBranchId == id && x.responsibilityId == 1).FirstOrDefaultAsync();
            if (data != null)
            {
                manager = await _context.employeeInfos.AsNoTracking().Include(x => x.lastDesignation).Where(x => x.Id == data.employeeId).Select(x => new BranchManager
                {
                    code = x.employeeCode,
                    name = x.nameEnglish,
                    designation = x.lastDesignation.designationName,
                    designationBn = x.lastDesignation.designationNameBN
                }).FirstOrDefaultAsync();
            }
            return manager;
        }

        public async Task<FixationIntegration> GetParticularInfoByfixId(int id)
        {
            return await _context.fixationIntegrations.Include(a => a.employee).Include(x => x.employee.currentGrade).Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetAllSalarySlabByGradeId(int gradeId)
        {
            return await _context.salarySlabs.Where(x => x.salaryGradeId == gradeId).Select(x => x.slabName).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<SalarySlab>> GetAllSalarySlabNameByGradeId(int gradeId)
        {
            return await _context.salarySlabs.Where(x => x.salaryGradeId == gradeId).OrderBy(x => x.orderNo).AsNoTracking().ToListAsync();
        }



        public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeeById(int employeeId)
        {
            return await _context.employeeInfos.Where(x => x.Id == employeeId).Include(a => a.employeeCode).AsNoTracking().ToListAsync();
        }
        //public async Task<int> GetSalarygradeByEmpId(int Id)
        //{
        //    var data= await _context.employeeInfos.Where(x => x.Id == Id).Select(x => x.currentGradeId).FirstOrDefaultAsync();
        //    return Convert.ToInt32(data);
        //}

        public async Task<int> GetSalarygradeByFixId(int Id)
        {
            var data = await _context.fixationIntegrations.Where(x => x.Id == Id).Select(x => x.fixationGradeId).FirstOrDefaultAsync();
            return Convert.ToInt32(data);
        }
        public async Task<int> GetEmplCodeByFixId(int id)
        {
            var data = await _context.fixationIntegrations.Where(x => x.Id == id).Select(x => x.empCode).FirstOrDefaultAsync();
            return Convert.ToInt32(data);
        }

		public async Task<bool> IsDuplicateSignatory(Signatory signatory)
		{
			var data = await _context.Signatories.Where(x => x.DesignationId == signatory.DesignationId  && x.Id != signatory.Id).ToListAsync();
			return data.Count()>0;
		}

		public async Task<int> SaveSignatory(Signatory signatory)
        {
            try
            {
                if (signatory.Id != 0)
                    _context.Signatories.Update(signatory);
                else
                    _context.Signatories.Add(signatory);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }

            return signatory.Id;
        }
        public async Task<int> SaveFixationIntegration(FixationIntegration fixationIntegration)
        {
            if (fixationIntegration.Id != 0)
                _context.fixationIntegrations.Update(fixationIntegration);
            else
                _context.fixationIntegrations.Add(fixationIntegration);
            await _context.SaveChangesAsync();
            return fixationIntegration.Id;
        }
        public async Task<IEnumerable<SalaryGrade>> GetSalarygrade()
        {
            return await _context.salaryGrades.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<FixaType>> GetAllFixType()
        {
            return await _context.fixaTypes.AsNoTracking().ToListAsync();
        }

        //new
        public async Task<DisciplinaryLog> GetDisciplinaryInfobyEmpID(int id)
        {

            return await _context.disciplinaryLogs.Where(x => Convert.ToDateTime(x.startingDate).Date >= Convert.ToDateTime(id).Date && Convert.ToDateTime(x.endDate).Date <= Convert.ToDateTime(id).Date && x.employeeId == id).AsNoTracking().FirstOrDefaultAsync();

        }


        public async Task<string> GetDisciplinaryIdbyEmpcode(string code)
        {
            var emp = await _context.employeeInfos.Where(x => x.employeeCode == code).FirstOrDefaultAsync();
            var data = await _context.disciplinaryLogs.Where(x => x.employeeId == emp.Id).Select(x => x.employeeId).FirstOrDefaultAsync();
            return Convert.ToString(data);
        }
        // public async Task<string> GetDisciplinaryIdbyEmpcode(string code)
        //{
        //    var emp = await _context.employeeInfos.Where(x => x.employeeCode == code).FirstOrDefaultAsync();
        //    var data = await _context.disciplinaryLogs.Where(x => x.employeeId == emp.Id && Convert.ToDateTime(x.endDate).Date <= Convert.ToDateTime(x.employeeId).Date).Select(x => x.employeeId).FirstOrDefaultAsync();
        //    return Convert.ToString(data);
        //}


        public async Task<FixationIntegration> GetfixationIntegrationInfoById(int id)
        {
            return await _context.fixationIntegrations.Include(x => x.employee).Include(x => x.employee.lastDesignation).Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();

        }
        public async Task<FixationIntegration> GetPunishmentReFixationInfoById(int id)
        {

            return await _context.fixationIntegrations.Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.employee.department).Include(x => x.employee.hrBranch).Where(x => x.Id == id && x.fixaTypeId == 2).AsNoTracking().FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<FixationIntegration>> GetAllFixForPunishmentById(int id)
        {
            var data = await _context.fixationIntegrations.AsNoTracking().Where(x => x.fixaTypeId == 2 && x.empCode == _context.fixationIntegrations.AsNoTracking().Where(y => y.Id == id).Select(y => y.empCode).FirstOrDefault()).ToListAsync();
            return data;
            //return await _context.fixationIntegrations.Include(a => a.employee).Include(a => a.employee.lastDesignation).Include(x=>x.employee.currentGrade).Include(x=>x.employee.department).Include(x=>x.employee.hrBranch).Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

		public async Task<int> SaveIncrementHeld(IncrementHeld model)
		{
			if (model.Id != 0)
				_context.IncrementHelds.Update(model);
			else
				_context.IncrementHelds.Add(model);
			await _context.SaveChangesAsync();
			return model.Id;
		}

		public async Task<int> DeleteIncrementHeld(int id)
		{
			var data = _context.IncrementHelds.Find(id);
			_context.IncrementHelds.Remove(data);
			return await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<IncrementHeld>> GetAllIncrementHeld()
		{
			var data = await _context.IncrementHelds.Include(x => x.employee).AsNoTracking().ToListAsync();
			return data;
		}


		public async Task<string> GetSalarygradeByEmpId(string code)
        {
            var data = await _context.employeeInfos.Where(x => x.employeeCode == code).Select(x => x.currentGradeId).FirstOrDefaultAsync();
            return Convert.ToString(data);
        }
        public async Task<string> GetSalarygradeIdByEmpCode(string code)
        {
            var emp = await _context.employeeInfos.Include(x => x.currentGrade).Where(x => x.employeeCode == code).FirstOrDefaultAsync();
            var nextGrade = await _context.salaryGrades.Where(x => x.Id < emp.currentGradeId).LastOrDefaultAsync();
            var newGradeId = await _context.salarySlabs.Where(x => x.salaryGradeId == nextGrade.Id).Select(x => x.salaryGradeId).FirstOrDefaultAsync();
            return Convert.ToString(newGradeId);
        }


        #endregion
    }
}
