using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.HRPMSAttendence;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Wages;
using OPUSERP.Models;
using OPUSERP.Payroll.Data.Entity;
using OPUSERP.Payroll.Data.Entity.Arrear;
using OPUSERP.Payroll.Data.Entity.Loan;
using OPUSERP.Payroll.Data.Entity.PF;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using OPUSERP.ProvidentFund.Data.Entity;
using OPUSERP.Utility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Services.Salary
{
	public class SalaryService : ISalaryService
	{
		private readonly ERPDbContext _context;

		public SalaryService(ERPDbContext context)
		{
			_context = context;
		}

		#region Gratuity Data Process

		public async Task<bool> SaveSendEmailLogStatus(SendEmailLogStatus sendmail)
		{
			if (sendmail.Id != 0)
			{
				_context.SendEmailLogStatus.Update(sendmail);
			}
			else
			{
				_context.SendEmailLogStatus.Add(sendmail);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<SendEmailLogStatus>> GetSendEmailLogStatus()
		{
			return await _context.SendEmailLogStatus.Include(x => x.employee).AsNoTracking().ToListAsync();
		}


		public async Task<bool> SaveGratiutyProcessData(GratiutyProcessData salaryYear)
		{
			if (salaryYear.Id != 0)
			{
				_context.gratiutyProcessDatas.Update(salaryYear);
			}
			else
			{
				_context.gratiutyProcessDatas.Add(salaryYear);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<GratiutyProcessData>> GetAllGratiutyProcessData()
		{
			return await _context.gratiutyProcessDatas.Include(x => x.employeeInfo).AsNoTracking().ToListAsync();
		}

		public List<DateTime?> GetAllGratiutyProcessDataDates()
		{
			return _context.gratiutyProcessDatas.Select(x => x.date).Distinct().ToList();
		}

		public async Task<IEnumerable<GratuityReportViewModel>> GetAllGratuityReportViewModel()
		{
			List<GratuityReportViewModel> data = new List<GratuityReportViewModel>();
			var processdata = await _context.gratiutyProcessDatas.Include(x => x.employeeInfo).AsNoTracking().ToListAsync();
			foreach (var list in processdata)
			{
				data.Add(new GratuityReportViewModel
				{
					employeeCode = list.employeeInfo.employeeCode,
					nameEnglish = list.employeeInfo.nameEnglish,
					designation = list.designation,
					joiningDate = list.employeeInfo.joiningDateGovtService?.ToString("dd-MM-yyyy"),
					uptoDate = list.date?.ToString("dd-MM-yyyy"),
					basicAmount = (int)list.basic,
					fractionalYear = list.year,
					gratuityAmount = list.gratuity
				});
			}
			return data;
		}

		public async Task<IEnumerable<GratuityReportViewModel>> GetAllGratuityReportViewModelByDate(DateTime date)
		{
			List<GratuityReportViewModel> data = new List<GratuityReportViewModel>();
			var processdata = await _context.gratiutyProcessDatas.Where(x => x.date == date).Include(x => x.employeeInfo).AsNoTracking().ToListAsync();
			foreach (var list in processdata)
			{
				data.Add(new GratuityReportViewModel
				{
					employeeCode = list.employeeInfo.employeeCode,
					nameEnglish = list.employeeInfo.nameEnglish,
					designation = list.designation,
					joiningDate = list.employeeInfo.joiningDateGovtService?.ToString("dd-MM-yyyy"),
					uptoDate = list.date?.ToString("dd-MM-yyyy"),
					basicAmount = (int)list.basic,
					fractionalYear = list.year,
					gratuityAmount = list.gratuity
				});
			}
			return data;
		}

		#endregion

		#region Salary Year

		public async Task<bool> SaveSalaryYear(SalaryYear salaryYear)
		{
			if (salaryYear.Id != 0)
			{
				_context.salaryYears.Update(salaryYear);
			}
			else
			{
				_context.salaryYears.Add(salaryYear);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<SalaryYear>> GetAllSalaryYear()
		{
			return await _context.salaryYears.OrderByDescending(x => x.yearName).AsNoTracking().ToListAsync();
		}

		public async Task<SalaryYear> GetSalaryYearById(int id)
		{
			return await _context.salaryYears.FindAsync(id);
		}

		public async Task<bool> DeleteSalaryYearById(int id)
		{
			_context.salaryYears.RemoveRange(_context.salaryYears.Where(a => a.Id == id));
			return 1 == await _context.SaveChangesAsync();
		}

		#endregion

		#region EmployeeIncomeTax

		public async Task<bool> SaveEmployeesTax(EmployeesTax employeesTax)
		{
			if (employeesTax.Id != 0)
			{
				_context.employeesTaxes.Update(employeesTax);
			}
			else
			{
				_context.employeesTaxes.Add(employeesTax);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<EmployeesTax>> GetAllEmployeesTax()
		{
			return await _context.employeesTaxes.Include(x => x.employeeInfo).Include(x => x.taxYear).Where(x => x.isActive == 1).AsNoTracking().ToListAsync();
		}

		public async Task<EmployeesTax> GetEmployeesTaxById(int id)
		{
			return await _context.employeesTaxes.FindAsync(id);
		}

		public async Task<bool> DeleteEmployeeTaxById(int id)
		{
			_context.employeesTaxes.RemoveRange(_context.employeesTaxes.Where(a => a.Id == id));
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<bool> UpdateEmployeesStatus(int? id, int taxyearid)
		{
			var VoucherMasters = _context.employeesTaxes.Where(x => x.employeeInfoId == id && x.taxYearId == taxyearid).FirstOrDefault();
			if (VoucherMasters != null)
			{
				//foreach (EmployeesTax tax in VoucherMasters)
				//{
				//    tax.isActive = 0;
				//    tax.updatedBy = updateBy;
				//    tax.updatedAt = DateTime.Now;
				//    _context.Entry(tax).State = EntityState.Modified;
				//    await _context.SaveChangesAsync();
				//}
				_context.employeesTaxes.Remove(_context.employeesTaxes.Find(VoucherMasters.Id));
				await _context.SaveChangesAsync();
			}

			return true;
		}

		#endregion

		#region Salary Grade
		public async Task<bool> SaveSalaryGrade(SalaryGrade grade)
		{
			if (grade.Id != 0)
				_context.salaryGrades.Update(grade);
			else
				_context.salaryGrades.Add(grade);
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<SalaryGrade>> GetAllSalaryGrade()
		{
			return await _context.salaryGrades.AsNoTracking().ToListAsync();
		}

		public async Task<EmployeesSalaryStructure> GetSalaryStructureByEmpAndHeadId(int empId, int headId)
		{
			var data = await _context.employeesSalaryStructures.AsNoTracking().Where(x => x.employeeInfoId == empId && x.salaryHeadId == headId).FirstOrDefaultAsync();
			return data;
		}

		public async Task<bool> UpdateHouseLoanDeductAmountBasedOnType(StaffLoan loan)
		{
			try
			{
				var head1 = await _context.employeesSalaryStructures.Where(x => x.employeeInfoId == loan.employeeInfoId && x.salaryHeadId == 132).FirstOrDefaultAsync();
				var head2 = await _context.employeesSalaryStructures.Where(x => x.employeeInfoId == loan.employeeInfoId && x.salaryHeadId == 162).FirstOrDefaultAsync();
				if (head1 != null)
				{
					head1.amount = loan.DeductionAmount.HasValue ? loan.DeductionAmount.Value : 0;
				}
				if (head2 != null)
				{
					head2.amount = 0;
				}
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				return false;
			}
			return true;
		}
		public async Task<SalaryGrade> GetSalaryGradeById(int id)
		{
			return await _context.salaryGrades.FindAsync(id);
		}

		public async Task<bool> DeleteSalaryGradeById(int id)
		{
			_context.salaryGrades.Remove(_context.salaryGrades.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		#endregion

		#region Tax Year
		public async Task<bool> SaveTaxYear(TaxYear taxYear)
		{
			if (taxYear.Id != 0)
			{
				_context.taxYears.Update(taxYear);
			}
			else
			{
				_context.taxYears.Add(taxYear);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<TaxYear>> GetAllTaxYear()
		{
			return await _context.taxYears.AsNoTracking().ToListAsync();
		}
		public async Task<TaxYear> TaxYearbyId(int id)
		{
			return await _context.taxYears.FindAsync(id);
		}
		#endregion

		#region Tax Slab
		public async Task<bool> SaveTaxSlab(SlabType slabType)
		{
			if (slabType.Id != 0)
			{
				_context.SlabTypes.Update(slabType);
			}
			else
			{
				_context.SlabTypes.Add(slabType);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<SlabType>> GetAllSlabType()
		{
			return await _context.SlabTypes.AsNoTracking().ToListAsync();
		}
		#endregion

		#region Rebate Slab Type
		public async Task<bool> SaveRebateSlab(RebateSlabType rebateSlabType)
		{
			if (rebateSlabType.Id != 0)
			{
				_context.RebateSlabTypes.Update(rebateSlabType);
			}
			else
			{
				_context.RebateSlabTypes.Add(rebateSlabType);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<RebateSlabType>> GetAllRebateSlabType()
		{
			return await _context.RebateSlabTypes.AsNoTracking().ToListAsync();
		}
		#endregion

		#region Investment Rebate Setting
		public async Task<bool> SaveRebateSetting(InvestmentRebateSettings investmentRebateSettings)
		{
			if (investmentRebateSettings.Id != 0)
			{
				_context.InvestmentRebateSettings.Update(investmentRebateSettings);
			}
			else
			{
				_context.InvestmentRebateSettings.Add(investmentRebateSettings);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<InvestmentRebateSettings>> GetAllRebateSetting()
		{
			return await _context.InvestmentRebateSettings.Include(x => x.taxYear).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<InvestmentRebateSettings>> GetAllRebateSettingbytaxyearid(int Id)
		{
			return await _context.InvestmentRebateSettings.Include(x => x.taxYear).Where(x => x.taxYearId == Id).AsNoTracking().ToListAsync();
		}
		#endregion

		#region Salary Head
		public async Task<bool> SaveSalaryHead(SalaryHead salaryHead)
		{
			if (salaryHead.Id != 0)
			{
				_context.salaryHeads.Update(salaryHead);
			}
			else
			{
				_context.salaryHeads.Add(salaryHead);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<string> GetHeadNameById(int? id)
		{
			var data = await _context.salaryHeads.AsNoTracking().Where(x => x.Id == id).Select(x => x.salaryHeadName).FirstOrDefaultAsync();
			return data;
		}


		public async Task<IEnumerable<SalaryHead>> GetAllSalaryHead()
		{
			return await _context.salaryHeads.AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<SalaryHead>> GetAllSalaryHeadForStruct()
		{
			return await _context.salaryHeads.AsNoTracking().OrderBy(x => x.isInvestments).ToListAsync();
		}

		public async Task<IEnumerable<SalaryHead>> GetAllSalaryHeadByFilter(string filter)
		{
			return await _context.salaryHeads.AsNoTracking().ToListAsync();
		}

		public async Task<bool> DeleteSalaryHeadById(int id)
		{
			_context.salaryHeads.Remove(_context.salaryHeads.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<bool> DeleteSalarySlabById(int id)
		{
			_context.salarySlabs.Remove(_context.salarySlabs.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<bool> DeleteSalaryGradeHeadById(int id)
		{
			_context.salaryGradePercents.Remove(_context.salaryGradePercents.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

		#endregion

		#region Salary Type
		public async Task<bool> SaveSalaryType(SalaryType salaryType)
		{
			if (salaryType.Id != 0)
			{
				_context.salaryTypes.Update(salaryType);
			}
			else
			{
				_context.salaryTypes.Add(salaryType);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<SalaryType>> GetAllSalaryType()
		{
			return await _context.salaryTypes.AsNoTracking().ToListAsync();
		}
		#endregion

		#region Bonus Type
		public async Task<bool> SaveBonusType(BonusType bonusType)
		{
			if (bonusType.Id != 0)
			{
				_context.bonusTypes.Update(bonusType);
			}
			else
			{
				_context.bonusTypes.Add(bonusType);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<BonusType>> GetAllBonusType()
		{
			return await _context.bonusTypes.AsNoTracking().ToListAsync();
		}
		#endregion

		#region Bonus Category

		public async Task<bool> SaveBonusCategory(BonousCategory bonusCategory)
		{
			if (bonusCategory.Id != 0)
			{
				_context.bonousCategories.Update(bonusCategory);
			}
			else
			{
				_context.bonousCategories.Add(bonusCategory);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<BonousCategory>> GetAllBonusCategory()
		{
			return await _context.bonousCategories.AsNoTracking().ToListAsync();
		}

		public async Task<bool> DeleteBonusCategoryById(int id)
		{
			_context.bonousCategories.Remove(_context.bonousCategories.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

		#endregion

		#region Bonus Sub Category

		public async Task<bool> SaveBonusSubCategory(BonousSubCategory bonusSubCategory)
		{
			if (bonusSubCategory.Id != 0)
			{
				_context.bonousSubCategories.Update(bonusSubCategory);
			}
			else
			{
				_context.bonousSubCategories.Add(bonusSubCategory);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<BonousSubCategory>> GetAllBonusSubCategory()
		{
			return await _context.bonousSubCategories.AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<BonousSubCategory>> GetBonusSubCategoryByMasterId(int masterId)
		{
			return await _context.bonousSubCategories.Where(a => a.bonousCategoryId == masterId).ToListAsync();
		}

		public async Task<bool> DeleteBonusSubCategoryById(int id)
		{
			_context.bonousSubCategories.Remove(_context.bonousSubCategories.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

		#endregion

		#region Bonous Structure
		public async Task<int> SaveBonousStructure(BonousStructure bonousStructure)
		{
			if (bonousStructure.Id != 0)
			{
				_context.bonousStructures.Update(bonousStructure);
			}
			else
			{
				_context.bonousStructures.Add(bonousStructure);
			}
			await _context.SaveChangesAsync();
			return bonousStructure.Id;
		}

		public async Task<IEnumerable<BonousStructure>> GetAllBonousStructure()
		{
			return await _context.bonousStructures.Include(a => a.bonousSubCategory).Include(a => a.period).Include(a => a.bonousSubCategory.bonousCategory).Include(a => a.salaryCalulationType).AsNoTracking().ToListAsync();
		}

		public async Task<bool> DeleteBonousStructureById(int id)
		{
			_context.bonousStructures.Remove(_context.bonousStructures.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

		#endregion

		#region Employees Bonous Structure
		public async Task<int> SaveEmployeesBonusStructure(EmployeesBonusStructure bonousStructure)
		{
			if (bonousStructure.Id != 0)
			{
				_context.employeesBonusStructures.Update(bonousStructure);
			}
			else
			{
				_context.employeesBonusStructures.Add(bonousStructure);
			}
			await _context.SaveChangesAsync();
			return bonousStructure.Id;
		}

		public async Task<IEnumerable<EmployeesBonusStructure>> GetEmployeesBonusStructureByBonusId(int bonusId)
		{
			return await _context.employeesBonusStructures.Include(a => a.employeeInfo).AsNoTracking().ToListAsync();
		}

		public async Task<bool> DeleteEmployeesBonusStructureBybonusId(int bonusId)
		{
			_context.employeesBonusStructures.RemoveRange(_context.employeesBonusStructures.Where(x => x.bonousStructureId == bonusId));
			return 1 == await _context.SaveChangesAsync();
		}

		#endregion

		#region Wallet Type
		public async Task<bool> SaveWalletType(WalletType walletType)
		{
			if (walletType.Id != 0)
			{
				_context.walletTypes.Update(walletType);
			}
			else
			{
				_context.walletTypes.Add(walletType);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<WalletType>> GetAllWalletType()
		{
			return await _context.walletTypes.OrderBy(x => x.walletTypeName).AsNoTracking().ToListAsync();
		}

		public async Task<bool> DeleteWalletTypeById(int id)
		{
			_context.walletTypes.Remove(_context.walletTypes.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		#endregion

		#region Employees Cash Setup
		public async Task<int> SaveEmployeesCashSetup(EmployeesCashSetup employeesCashSetup)
		{
			if (employeesCashSetup.Id != 0)
			{
				_context.employeesCashSetups.Update(employeesCashSetup);
			}
			else
			{
				_context.employeesCashSetups.Add(employeesCashSetup);
			}
			await _context.SaveChangesAsync();
			return employeesCashSetup.Id;
		}

		public async Task<IEnumerable<EmployeesCashSetup>> GetAllEmployeesCashSetup()
		{
			return await _context.employeesCashSetups.Include(a => a.employeeInfo).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<EmployeesCashSetup>> GetEmployeesCashSetupByEmployeeId(int empId)
		{
			return await _context.employeesCashSetups.Where(a => a.employeeInfoId == empId).AsNoTracking().ToListAsync();
		}

		public async Task<bool> DeleteEmployeesCashSetupById(int id)
		{
			_context.employeesCashSetups.Remove(_context.employeesCashSetups.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		#endregion

		#region Salary Calulation Type
		public async Task<bool> SaveSalaryCalulationType(SalaryCalulationType salaryCalulationType)
		{
			if (salaryCalulationType.Id != 0)
			{
				_context.salaryCalulationTypes.Update(salaryCalulationType);
			}
			else
			{
				_context.salaryCalulationTypes.Add(salaryCalulationType);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<SalaryCalulationType>> GetAllSalaryCalulationType()
		{
			return await _context.salaryCalulationTypes.AsNoTracking().ToListAsync();
		}
		#endregion

		#region Salary Period
		public async Task<int> SaveSalaryPeriod(SalaryPeriod salaryPeriod)
		{
			if (salaryPeriod.Id != 0)
			{
				_context.salaryPeriods.Update(salaryPeriod);
			}
			else
			{
				_context.salaryPeriods.Add(salaryPeriod);
			}
			await _context.SaveChangesAsync();
			return salaryPeriod.Id;
		}
		public async Task<DateTime> GetDisburseDate(int yearid, string monthname)
		{
			var year = await _context.salaryYears.Where(x => x.Id == yearid).Select(x => x.yearName).FirstOrDefaultAsync();
			var month = monthname == "January" ? 1
						: monthname == "February" ? 2
						: monthname == "March" ? 3
						: monthname == "April" ? 4
						: monthname == "May" ? 5
						: monthname == "June" ? 6
						: monthname == "July" ? 7
						: monthname == "August" ? 8
						: monthname == "September" ? 9
						: monthname == "October" ? 10
						: monthname == "November" ? 11
						: monthname == "December" ? 12
						: 0;


			var date = new DateTime(Convert.ToInt32(year), month, DateTime.DaysInMonth(Convert.ToInt32(year), month));

			return date;
		}

		public async Task<bool> EditSalaryPeriodForlockLabel(int Id, int lockLabel)
		{
			var SalaryPeriod = _context.salaryPeriods.Find(Id);
			SalaryPeriod.lockLabel = lockLabel;
			_context.Entry(SalaryPeriod).State = EntityState.Modified;
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<bool> SetSalaryPeriodLock(int Id, int lockLabel, string lockBy)
		{
			var SalaryPeriod = _context.salaryPeriods.Find(Id);
			SalaryPeriod.lockLabel = lockLabel;
			SalaryPeriod.lockDate = DateTime.Now;
			SalaryPeriod.lockBy = lockBy;
			_context.Entry(SalaryPeriod).State = EntityState.Modified;
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<SalaryPeriod>> GetAllSalaryPeriod()
		{
			return await _context.salaryPeriods.Include(x => x.salaryYear).Include(x => x.salaryType).Include(x => x.taxYear).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<ParticularNamesVm>> GetAllParticulars()
		{
			return await _context.staffLoanLogs.Select(x => new ParticularNamesVm
			{
				particular = x.particular
			}).Distinct().AsNoTracking().ToListAsync();
		}

		public async Task<StaffLoan> GetLoanNoByLoanId(int loanId)
		{
			return await _context.staffLoans.Where(x => x.Id == loanId).AsNoTracking().FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<PartialSalaryLog>> GetAllPartialSalaryLogs()
		{
			return await _context.PartialSalaryLogs.AsNoTracking().Include(x => x.employeeInfo).Include(x => x.salaryPeriod).ToListAsync();
		}
				
		public async Task<StaffLoanInfoViewModel> GetStaffLoanInfoExpSelfById(string transactionNo, int loanId)
		{
			try
			{
				var data = await _context.staffLoanInfoViewModels.FromSql($"sp_GetStaffLoanInfoExSelfById {transactionNo},{loanId}").AsNoTracking().FirstOrDefaultAsync();
				return data;
			}
			catch (Exception ex)
			{

				throw;
			}
		}
		public async Task<StaffLoanInfoViewModel> GetStaffLoanInfoById(int loanId)
		{
			try
			{
				var data = await _context.staffLoanInfoViewModels.FromSql($"sp_GetStaffLoanInfoById {loanId}").AsNoTracking().FirstOrDefaultAsync();
				return data;
			}
			catch (Exception ex)
			{

				throw;
			}
		}

		public async Task<IEnumerable<CheckLoansVm>> CheckLoans()
		{
			try
			{
				var data = await _context.checkLoans.FromSql($"sp_GetLoanAndStructureDifference").AsNoTracking().ToListAsync();
				return data;
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		public async Task<ResolveAllVm> ResolveStructureWithLoan()
		{
			return await _context.resolveAllVms.FromSql($"sp_ResolveStructureWithLoan").AsNoTracking().FirstOrDefaultAsync();
		}
        public async Task<ResolveAllVm> SetAllToSalaryStructure(int salaryPeriodId)
        {
            return await _context.resolveAllVms.FromSql($"sp_SetAllToSalaryStructure {salaryPeriodId}").AsNoTracking().FirstOrDefaultAsync();
        }
        
        public async Task<ResolveAllVm> ProcessPartialHouseRent(int periodId)
		{
			return await _context.resolveAllVms.FromSql($"sp_PartialHouseRent {periodId}").AsNoTracking().FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<PartialHouseRentVm>> GetPartialHouseRent(int periodId)
		{
			var houseRent = new List<PartialHouseRentVm>();
			try
			{
				 houseRent = await _context.partialHouseRents.FromSql($"sp_GetPartialHouseRent {periodId}").AsNoTracking().ToListAsync();
				return houseRent;
			}
			catch(Exception exp)
			{

			}
			return houseRent;
		}

		public async Task<IEnumerable<PartialSalaryLogViewModel>> GetAllPartialSalaryLog()
		{
			return await _context.PartialSalaryLogViewModels.FromSql($"sp_GetDistinctPartialSalary").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<PRLEmployeeViewModel>> GetAllPrlEmployees()
		{
			return await _context.pRLEmployeeViewModels.FromSql($"sp_GetPRLEmployees").AsNoTracking().ToListAsync();
		}
		public async Task<ArrearAmountVm> GetArrearAmountByEmpCode(int periodId, string empCode)
		{
			return await _context.arrearAmountVm.FromSql($"sp_GetArrearAmountByPeriodAndEmpCode {periodId}, {empCode}").AsNoTracking().FirstOrDefaultAsync();
		}


		public async Task<UpdateSalaryStructureExpire> UpdateSalaryStructureExpire()
		{
			return await _context.updateSalaryStructureExpires.FromSql($"sp_UpdateAllExpireSalaryStructure").AsNoTracking().FirstOrDefaultAsync();
		}

		public async Task<CBSProcessSp> CBSProcessSalarySheetByYearMonthAndSBU(int periodId, string sbu, string empCode, int hrBranchId)
		{
			return await _context.cBSProcessSps.FromSql($"sp_CBSProcessSalarySheetByYearMonthAndSBU {periodId}, {sbu}, {empCode}, {hrBranchId}").AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<CBSProcessSp> ProcessLoanByType(string loanType, int periodId, string empCode)
		{
			try
			{
				if (empCode == null)
				{
					empCode = "0";
				}
				if ((loanType == "HBA-B13" || loanType == "HBA-A13") && empCode == "0")
				{
					return await _context.cBSProcessSps.FromSql($"sp_LoanDeductionByHBA {periodId}, {loanType}").AsNoTracking().FirstOrDefaultAsync();
				}
				else if ((loanType == "HBA-B13" || loanType == "HBA-A13") && empCode != "0")
				{
					return await _context.cBSProcessSps.FromSql($"sp_LoanDeductionByHBAAndEmpCode  {periodId}, {loanType}, {empCode}").AsNoTracking().FirstOrDefaultAsync();
				}
				else
				{
					return await _context.cBSProcessSps.FromSql($"sp_LoanDeductionByType {periodId}, {loanType}, {empCode}").AsNoTracking().FirstOrDefaultAsync();
				}

			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public async Task<HrBranch> GetHrBranchById(int hrBranchId)
		{
			var data = await _context.hrBranches.Where(x => x.Id == hrBranchId).FirstOrDefaultAsync();
			return data;
		}
		public async Task<Location> GetZoneById(int zoneId)
		{
			var data = await _context.Locations.Where(x => x.Id == zoneId).FirstOrDefaultAsync();
			return data;
		}
		public async Task<CBSProcessSp> CBSProcessSalaryVoucher(int periodId, int hrBranchId, string type)
		{
			return await _context.cBSProcessSps.FromSql($"sp_CBSProcessSalaryVoucher {periodId}, {hrBranchId}, {type}").AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<CBSProcessSp> CBSProcessAllVoucher(int periodId)
		{
			var type = "";
			var result = new CBSProcessSp();
			var period = await _context.salaryPeriods.Where(x => x.Id == periodId).FirstOrDefaultAsync();
			try
			{
				if (period.salaryTypeId == 1)
				{
					var hrBranches = await _context.hrBranches.Select(x => x.Id).ToListAsync();
					foreach (var item in hrBranches)
					{

						if (item == 205)
						{
							type = "headoffice";
							result = await _context.cBSProcessSps.FromSql($"sp_CBSProcessSalaryVoucher {periodId}, {item}, {type}").AsNoTracking().FirstOrDefaultAsync();
						}
						else
						{
							type = "hrBranch";
							result = await _context.cBSProcessSps.FromSql($"sp_CBSProcessSalaryVoucher {periodId}, {item}, {type}").AsNoTracking().FirstOrDefaultAsync();
						}
					}

					var zones = await _context.Locations.Select(x => x.Id).ToListAsync();
					foreach (var item in zones)
					{
						type = "zone";
						result = await _context.cBSProcessSps.FromSql($"sp_CBSProcessSalaryVoucher {periodId}, {item}, {type}").AsNoTracking().FirstOrDefaultAsync();
					}
				}
				if (period.salaryTypeId == 2)
				{
					await _context.cBSProcessSps.FromSql($"sp_CBSProcessBonusVoucher {periodId}, {1000}, {"hrBranch"}").AsNoTracking().FirstOrDefaultAsync();
					await _context.cBSProcessSps.FromSql($"sp_CBSProcessBonusVoucher {periodId}, {1000}, {"zone"}").AsNoTracking().FirstOrDefaultAsync();
				}

				return result;
				//var result= await _context.cBSProcessSps.FromSql($"sp_ProcessAllVoucher {periodId}").AsNoTracking().FirstOrDefaultAsync();
				//return result;
			}
			catch (Exception ex)
			{
				return result;
			}

		}
		public async Task<CBSProcessSp> CBSProcessAllSalary(int periodId)
		{
			var sbu = "0";
			var empcode = "0";
			var result = new CBSProcessSp();
			var period = await _context.salaryPeriods.Where(x => x.Id == periodId).FirstOrDefaultAsync();
			try
			{
				if (period.salaryTypeId == 1)
				{
					var hrBranches = await _context.hrBranches.Select(x => x.Id).ToListAsync();
					foreach (var item in hrBranches)
					{

						if (item == 205)
						{
							result = await _context.cBSProcessSps.FromSql($"sp_CBSProcessSalarySheetByYearMonthAndSBU {periodId}, {sbu}, {empcode}, {item}").AsNoTracking().FirstOrDefaultAsync();
						}
						else
						{
							result = await _context.cBSProcessSps.FromSql($"sp_CBSProcessSalarySheetByYearMonthAndSBU {periodId}, {sbu}, {empcode}, {item}").AsNoTracking().FirstOrDefaultAsync();
						}
					}

					var zones = await _context.Locations.Select(x => x.Id).ToListAsync();
					foreach (var item in zones)
					{
						result = await _context.cBSProcessSps.FromSql($"sp_CBSProcessZoneSalarySheetByYearMonthAndSBU {periodId}, {sbu}, {empcode}, {item}").AsNoTracking().FirstOrDefaultAsync();
					}
				}
				if (period.salaryTypeId == 2)
				{
					result = await _context.cBSProcessSps.FromSql($"sp_ProcessAllSalaryBonus {periodId}").AsNoTracking().FirstOrDefaultAsync();
				}

				return result;
				//return await _context.cBSProcessSps.FromSql($"sp_CBSProcessSalarySheetByYearMonthAndSBU {periodId}").AsNoTracking().FirstOrDefaultAsync();
				//return result;
			}
			catch (Exception ex)
			{
				return result;
			}
		}
		public async Task<IEnumerable<CBSProcessLogViewModel>> GetProcessedSalaryLog(int periodId)
		{
			return await _context.CBSProcessLogs.FromSql($"sp_GetProcessedVouchers {periodId}").AsNoTracking().OrderBy(x => x.branchName).ToListAsync();
		}
		public async Task<CBSProcessSp> CBSProcessLoan(int periodId, int hrBranchId)
		{
			return await _context.cBSProcessSps.FromSql($"sp_ProcessLoanData {periodId}, {hrBranchId}").AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<IEnumerable<EmployeeInfoCarMaintanceVm>> GetEmployeeInfoById(int id)
		{
			return await _context.employeeInfoCarMaintances.FromSql($"sp_GetEmployeeInfoCarMaintenanceById {id}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<HrBranchSalary>> GetHrBranchForSalaryProcess(int periodId)
		{
			return await _context.hrBranchSalaries.FromSql($"sp_HrBranchForSalaryProcess {periodId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<HrBranchSalary>> GetHrZonesForSalaryProcess(int periodId)
		{
			return await _context.hrBranchSalaries.FromSql($"sp_HrZonesForSalaryProcess {periodId}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<CarMaintenance>> GetAllCarMaintenance()
		{
			return await _context.carMaintenances.AsNoTracking().Include(x => x.employeeInfo).Include(x => x.salaryPeriod).ToListAsync();
		}

		public async Task<IEnumerable<MobileAllowanceStructure>> GetAllMobileAllowanceStructure()
		{
			return await _context.mobileAllowanceStructures.AsNoTracking().Include(x => x.employeeInfo).Include(x => x.salaryPeriod).ToListAsync();
		}

		public async Task<int> DeleteCarMaintenance(int id)
		{
			var data = _context.carMaintenances.Remove(_context.carMaintenances.Find(id));
			return await _context.SaveChangesAsync();
		}

		public async Task<int> DeleteMobileAllowanceStructure(int id)
		{
			var data = _context.mobileAllowanceStructures.Remove(_context.mobileAllowanceStructures.Find(id));
			return await _context.SaveChangesAsync();
		}
		public async Task<int> DeleteArrearInfoById(int id)
		{
			var data = _context.EmployeeArrearInfo.Remove(_context.EmployeeArrearInfo.Find(id));
			return await _context.SaveChangesAsync();
		}

		public async Task<int> SaveCarMaintenance(CarMaintenance model)
		{
			if (model.Id == 0)
			{
				_context.carMaintenances.Add(model);
			}
			else
			{
				_context.carMaintenances.Update(model);
			}
			return await _context.SaveChangesAsync();
		}

		public async Task<int> SaveMobileAllowanceStructure(MobileAllowanceStructure model)
		{
			if (model.Id == 0)
			{
				_context.mobileAllowanceStructures.Add(model);
			}
			else
			{
				_context.mobileAllowanceStructures.Update(model);
			}
			return await _context.SaveChangesAsync();
		}
		public async Task<IEnumerable<SalaryStatusLog>> GetLockStatusLogs()
		{
			return await _context.SalaryStatusLogs.Include(x => x.salaryPeriod).Where(x => x.statusType == "Salary Period Lock" || x.statusType == "Salary Period Unlock").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<SalaryPeriod>> GetSalaryPeriodByLockLavel(int locklabel)
		{
			return await _context.salaryPeriods.Include(a => a.salaryType).Include(a => a.salaryYear).Include(a => a.taxYear).OrderByDescending(a => a.Id).Where(x => x.lockLabel == locklabel).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<SalaryPeriod>> GetAllSalaryPeriodByUserId(int userid, string type)
		{
			if (type == "made")
			{
				return await _context.salaryPeriods.Include(a => a.salaryType).Include(a => a.salaryYear).Include(a => a.taxYear).OrderByDescending(a => a.Id).Where(x => x.madeBy == userid).AsNoTracking().ToListAsync();
			}
			else if (type == "checked")
			{
				return await _context.salaryPeriods.Include(a => a.salaryType).Include(a => a.salaryYear).Include(a => a.taxYear).OrderByDescending(a => a.Id).Where(x => x.checkedBy == userid).AsNoTracking().ToListAsync();
			}
			else if (type == "approved")
			{
				return await _context.salaryPeriods.Include(a => a.salaryType).Include(a => a.salaryYear).Include(a => a.taxYear).OrderByDescending(a => a.Id).Where(x => x.approvedBy == userid).AsNoTracking().ToListAsync();
			}
			else if (type == "disbursed")
			{
				return await _context.salaryPeriods.Include(a => a.salaryType).Include(a => a.salaryYear).Include(a => a.taxYear).OrderByDescending(a => a.Id).Where(x => x.disbursedBy == userid).AsNoTracking().ToListAsync();
			}
			else
			{
				return await _context.salaryPeriods.Include(a => a.salaryType).Include(a => a.salaryYear).Include(a => a.taxYear).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
			}
		}

		public async Task<IEnumerable<SalaryPeriod>> GetAllSalaryPeriodbyid(int userid)
		{
			return await _context.salaryPeriods.Where(x => x.madeBy == userid).Include(a => a.salaryType).Include(a => a.salaryYear).Include(a => a.taxYear).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<PFSalaryContribution>> GetAllpfContribution()
		{
			return await _context.PFSalaryContributions.Include(a => a.employee).Include(a => a.employee.department).Include(a => a.employee.lastDesignation).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<SalaryPeriod>> GetSalaryPeriods()
		{
			return await _context.salaryPeriods.Include(a => a.salaryType).Include(a => a.salaryYear).Include(a => a.taxYear).AsNoTracking().ToListAsync();
		}

		public async Task<SalaryPeriod> GetSalaryPeriodById(int PeriodId)
		{
			return await _context.salaryPeriods.Include(x => x.salaryYear).Where(x => x.Id == PeriodId).AsNoTracking().FirstOrDefaultAsync();
		}

		public async Task<SalaryPeriod> GetSalaryPeriodByYearAndMonth(int year, string month)
		{
			var data = await _context.salaryPeriods.Where(x => x.salaryYear.yearName == year.ToString() && x.monthName == month).FirstOrDefaultAsync();
			return data;
		}
		public async Task<SalaryPeriod> GetSalaryPeriodById1(int PeriodId)
		{
			return await _context.salaryPeriods.Where(x => x.Id == PeriodId).Include(a => a.salaryType).Include(a => a.salaryYear).Include(a => a.taxYear).AsNoTracking().FirstOrDefaultAsync();
		}
		//new//
		public async Task<IEnumerable<SalaryPeriod>> GetSalaryPeriodByName(int periodName)
		{
			return await _context.salaryPeriods.Where(x => x.Id == periodName).Include(a => a.salaryType).Include(a => a.salaryYear).Include(a => a.taxYear).AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<SalaryPeriod>> GetDuplicateSalaryPeriodById(int PeriodId, int yearId, int typeId, string month)
		{
			return await _context.salaryPeriods.Where(x => x.Id != PeriodId && x.salaryYearId == yearId && x.salaryTypeId == typeId && x.monthName == month).AsNoTracking().ToListAsync();
		}

		public async Task<SalaryPeriod> GetSalaryPeriodNameById(int periodId)
		{
			return await _context.salaryPeriods.Where(x => x.Id == periodId).Include(a => a.salaryType).Include(a => a.salaryYear).Include(a => a.taxYear).AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<SalaryPeriod> GetSalaryPeriodmax()
		{
			var data = await _context.salaryPeriods.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
			var period = await _context.salaryPeriods.Where(x => x.Id == data.Id).Include(a => a.salaryType).Include(a => a.salaryYear).Include(a => a.taxYear).AsNoTracking().FirstOrDefaultAsync();
			return period;
		}

		public async Task<bool> DeleteSalaryPeriodById(int id)
		{
			_context.salaryPeriods.Remove(_context.salaryPeriods.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<List<int>> GetEmployeesWithSalaryStructure()
		{
			var data = await _context.employeesSalaryStructures.GroupBy(x => x.employeeInfoId).Select(x => x.Key).ToListAsync();
			return data;
		}
		public async Task<List<int>> GetBranchEmployeesWithSalaryStructure(int branchId)
		{
			var data = await _context.employeesSalaryStructures.AsNoTracking().Where(x => x.employeeInfo.hrBranchId == branchId).GroupBy(x => x.employeeInfoId).Select(x => x.Key).ToListAsync();
			return data;
		}


		//approved
		public async Task<IEnumerable<SalaryPeriod>> GetAllSalaryPeriodApproved(int userId, int lockLabel)
		{
			return await _context.salaryPeriods.Where(x => x.approvedBy == userId).Where(x => x.lockLabel == 3).Include(a => a.salaryType).Include(a => a.salaryYear).Include(a => a.taxYear).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<SalaryPeriod>> GetAllSalaryPeriodDisbursePending(int userId, int lockLabel)
		{
			return await _context.salaryPeriods.Where(x => x.disbursedBy == userId).Where(x => x.lockLabel == 4).Include(a => a.salaryType).Include(a => a.salaryYear).Include(a => a.taxYear).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<SalaryPeriod>> GetAllSalaryDisburse(int userId)
		{
			return await _context.salaryPeriods.Where(x => x.disbursedBy == userId).Include(a => a.salaryType).Include(a => a.salaryYear).Include(a => a.taxYear).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}




		#endregion

		#region Salary Slab
		public async Task<bool> SaveSalarySlab(SalarySlab salarySlab)
		{
			if (salarySlab.Id != 0)
			{
				_context.salarySlabs.Update(salarySlab);
			}
			else
			{
				_context.salarySlabs.Add(salarySlab);
			}
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<SalarySlab> GetSalarySlabById(int Id)
		{
			return await _context.salarySlabs.Include(a => a.salaryGrade).Where(x => x.Id == Id).AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<SalarySlab> GetSalarySlabByAmount(decimal amount)
		{
			return await _context.salarySlabs.Include(a => a.salaryGrade).Where(x => x.slabAmount == amount).AsNoTracking().FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<SalarySlab>> GetAllSalarySlab()
		{
			return await _context.salarySlabs.Include(a => a.salaryGrade).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<SalarySlab>> GetSalarySlabBysalaryGradeId(int salaryGradeId)
		{
			return await _context.salarySlabs.Include(a => a.salaryGrade).Where(x => x.salaryGradeId == salaryGradeId).OrderBy(x => x.orderNo).AsNoTracking().ToListAsync();
		}




		//slabHead
		public async Task<IEnumerable<SalaryGradePercent>> GetSalaryHeadBysalaryGradeId(int salaryGradeId)
		{
			return await _context.salaryGradePercents.Include(a => a.salaryHead).Include(a => a.salaryCalulationType).Where(x => x.salaryGradeId == salaryGradeId).AsNoTracking().ToListAsync();
		}
		//
		#endregion

		#region Salary Grade Percent
		public async Task<bool> SaveSalaryGradePercent(SalaryGradePercent salaryGradePercent)
		{
			if (salaryGradePercent.Id != 0)
			{
				_context.salaryGradePercents.Update(salaryGradePercent);
			}
			else
			{
				_context.salaryGradePercents.Add(salaryGradePercent);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<SalaryGradePercent>> GetAllSalaryGradePercent()
		{
			return await _context.salaryGradePercents.Include(a => a.salaryGrade).Include(a => a.salaryHead).Include(a => a.salaryCalulationType).AsNoTracking().ToListAsync();
		}
		public async Task<SalaryGradePercent> GetSalaryGradePercentBysalaryHeadId(int salaryGradeId, int salaryHeadId)
		{
			return await _context.salaryGradePercents.Include(a => a.salaryGrade).Include(a => a.salaryHead).Include(a => a.salaryCalulationType).Where(x => x.salaryGradeId == salaryGradeId && x.salaryHeadId == salaryHeadId).AsNoTracking().FirstOrDefaultAsync();
		}
		#endregion

		#region Employees Salary Structure
		public async Task<bool> SaveEmployeesSalaryStructure(EmployeesSalaryStructure employeesSalaryStructure)
		{
			if (employeesSalaryStructure.Id != 0)
			{
				_context.employeesSalaryStructures.Update(employeesSalaryStructure);
			}
			else
			{
				_context.employeesSalaryStructures.Add(employeesSalaryStructure);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<bool> EditEmployeesSalaryStructure(int Id, decimal amount, string status)
		{
			var SalaryStructur = _context.employeesSalaryStructures.Find(Id);
			SalaryStructur.amount = amount;
			SalaryStructur.isActive = status;
			_context.Entry(SalaryStructur).State = EntityState.Modified;
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<EmployeesSalaryStructure>> GetAllEmployeesSalaryStructure()
		{
			return await _context.employeesSalaryStructures.Include(x => x.salaryHead).Include(x => x.salarySlab).Include(x => x.employeeInfo).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<EmployeesSalaryStructure>> GetSalaryStructureByEmp(int empId)
		{
			return await _context.employeesSalaryStructures.Where(x => x.employeeInfoId == empId).Include(x => x.employeeInfo.department).Include(x => x.employeeInfo).Include(x => x.salaryHead).Include(x => x.salarySlab).AsNoTracking().ToListAsync();
		}
		public async Task<decimal> GetBasicFromSalaryStructure(int empId)
		{
			var data = await _context.employeesSalaryStructures.AsNoTracking().Where(x => x.employeeInfoId == empId && x.salaryHeadId == 119).Select(x => x.amount).FirstOrDefaultAsync();
			return data;
		}

		public async Task<GetLoansByEmpCodeAndTypeViewModel> GetLoansByEmpCodeAndType(string code, string type)
		{
			return await _context.getLoansByEmpCodes.FromSql($"sp_GetHBALoanByEmpCodeAndType {code},{type}").AsNoTracking().FirstOrDefaultAsync();
		}

		public async Task<PartialResultViewModel> SavePartialSalary(int type, int empId, int periodId, string fromDate, string toDate, int days)
		{
			return await _context.partialResultViewModel.FromSql($"sp_SavePartialSalary {type}, {empId},{periodId}, {fromDate}, {toDate}, {days}").AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<PartialResultViewModel> DeletePartialSalary(int empId, int periodId)
		{
			return await _context.partialResultViewModel.FromSql($"sp_DeletePartialSalary {empId},{periodId}").AsNoTracking().FirstOrDefaultAsync();
		}


		public async Task<IEnumerable<EmployeeInfo>> GetIndividualEmployeeFromSalaryStructure()
		{
			var distinctEmpId = _context.employeesSalaryStructures.Select(o => o.employeeInfoId).Distinct().ToList();
			var empList = new List<EmployeeInfo>();

			foreach (var item in distinctEmpId)
			{
				var data = await _context.employeeInfos.Include(x => x.department).Where(x => x.Id == item).FirstOrDefaultAsync();
				empList.Add(data);
			}
			return empList;
		}

		public async Task<IEnumerable<EmployeeInfo>> GetIndividualEmployeeFromSalaryStructure(int branchId, int departmentId, int designationId)
		{
			var distinctEmp = await _context.employeesSalaryStructures.Include(x => x.employeeInfo).Include(x => x.employeeInfo.department).Include(x => x.employeeInfo.lastDesignation).Select(x => x.employeeInfo).Distinct().ToListAsync();

			if (branchId != 0)
			{
				var data = distinctEmp.Where(x => x.hrBranchId == branchId).ToList();
				distinctEmp = data;
			}
			if (departmentId != 0)
			{
				var data = distinctEmp.Where(x => x.departmentId == departmentId).ToList();
				distinctEmp = data;
			}
			if (designationId != 0)
			{
				var data = distinctEmp.Where(x => x.lastDesignationId == designationId).ToList();
				distinctEmp = data;
			}

			return distinctEmp;
		}









		public async Task<IEnumerable<EmployeesSalaryStructure>> GetEmployeesSalaryStructureByEmpId(int empId)
		{
			return await _context.employeesSalaryStructures.Include(x => x.salaryHead).Include(x => x.salarySlab).Where(x => x.employeeInfoId == empId).OrderBy(x => x.salaryHead.isInvestments).AsNoTracking().ToListAsync();
		}

		public async Task<EmployeesSalaryStructure> GetEmpStructureByEmpId(int empId)
		{
			return await _context.employeesSalaryStructures.Include(x => x.employeeInfo.department).Include(x => x.salaryHead).Include(x => x.salarySlab).Include(x => x.salarySlab.salaryGrade).Where(x => x.employeeInfoId == empId).FirstOrDefaultAsync();
		}

		public async Task<bool> DeleteEmployeesSalaryStructureByempId(int empId)
		{
			_context.employeesSalaryStructures.RemoveRange(_context.employeesSalaryStructures.Where(x => x.employeeInfoId == empId));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<IEnumerable<PFYearlyIndividualVm>> GetIndividualYearlyPF(int pfMemberId, string year)
		{
			return await _context.pFYearlyIndividuals.FromSql($"sp_GetIndividualYearlyPF {pfMemberId},{year}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<FsSalaryStructureViewModel>> GetFsStructure(int empId, int day)
		{
			return await _context.fsSalaryStructureViewModels.FromSql($"SP_Get_FinalSettlementStructure {empId},{day}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<SalarySheetByYearMonthAndSBU>> sp_GetSalarySheetByYearMonthAndSBU(string year, string month, string sbu, string empId)
		{
			return await _context.salarySheetByYearMonthAndSBUs.FromSql($"sp_GetSalarySheetByYearMonthAndSBU {year},{month}, {sbu}, {empId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<NetPayByYearMonthAndSBU>> sp_GetNetPayByYearMonthAndSBU(string year, string month, string sbu, string empId)
		{
			return await _context.netPayByYearMonthAndSBUs.FromSql($"sp_GetNetPayByYearMonthAndSBU {year},{month}, {sbu}, {empId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<PostSalary>> sp_SaveVoucherDataByBranchId(int periodId, int branchId)
		{
			return await _context.postSalaries.FromSql($"sp_SaveVoucherDataByBranchId {branchId},{periodId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<PostedVoucherVm>> sp_GetPostedVoucher(int periodId, int branchId)
		{
			return await _context.postedVoucherVms.FromSql($"sp_GetPostedVoucher {periodId},{branchId}").AsNoTracking().ToListAsync();
		}
		public async Task<EmployeesSalaryStructure> GetPFByEmployeeId(int empId)
		{
			var data = await _context.employeesSalaryStructures.Where(x => x.employeeInfoId == empId && x.salaryHeadId == 149).FirstOrDefaultAsync();
			return data;
		}
		public async Task<decimal> GetBasicCalculation(int empId, decimal basic)
		{
			var data = await _context.houseRentCalculates.FromSql($"sp_GetHouseRentByEmpId {empId},{basic}").AsNoTracking().Select(x => x.houseRent).FirstOrDefaultAsync();
			return Convert.ToDecimal(data);
		}
		public async Task<decimal> GetBasicCalculationForArrear(int empId, decimal basic, decimal total)
		{
			var data = await _context.houseRentCalculates.FromSql($"sp_GetHouseRentByEmpIdForArrear {empId},{basic},{total}").AsNoTracking().Select(x => x.houseRent).FirstOrDefaultAsync();
			return Convert.ToDecimal(data);
		}
		public async Task<decimal> UpdateEmployeeStructureByBasic(int empId, decimal basic)
		{
			var data = await _context.houseRentCalculates.FromSql($"sp_UpdateEmployeeStructureByBasic {empId},{basic}").AsNoTracking().Select(x => x.houseRent).FirstOrDefaultAsync();
			return Convert.ToDecimal(data);
		}
		public async Task<decimal> GetPFSubsByEmpId(int empId, decimal basic, string pfType, int? pfPercentage)
		{
			var data = await _context.pfSubscriptionVms.FromSql($"sp_GetPFSubsByEmpId {empId},{basic}, {pfType}, {pfPercentage}").AsNoTracking().Select(x => x.pfAmount).FirstOrDefaultAsync();
			return Convert.ToDecimal(data);
		}

		#endregion

		#region Salary Structure History

		public async Task<IEnumerable<SalaryProcessDataViewModel>> SaveStructureHistory(int employeeInfoId, string changeBy)
		{
			return await _context.salaryProcessDataViewModels.FromSql($"SP_Save_SalaryStructureHistory {employeeInfoId},{changeBy}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<SalaryProcessDataViewModel>> UpdateStructureHistory(int structureId, string changeBy)
		{
			return await _context.salaryProcessDataViewModels.FromSql($"SP_Update_SalaryStructureHistory {structureId},{changeBy}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<System.Object>> GetStructureHistoryByEmpId(int empId)
		{
			return await _context.structureHistoryReportViewModels.FromSql($"SP_Get_SalaryStructureHistoryByEmpId {empId}").OrderByDescending(a => a.historyCode).AsNoTracking().ToListAsync();
		}

		#endregion

		#region Wages Salary Structure
		public async Task<bool> SaveWagesSalaryStructure(WagesSalaryStructure wagesSalaryStructure)
		{
			if (wagesSalaryStructure.Id != 0)
			{
				_context.wagesSalaryStructures.Update(wagesSalaryStructure);
			}
			else
			{
				_context.wagesSalaryStructures.Add(wagesSalaryStructure);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<bool> EditWagesSalaryStructure(int Id, decimal amount, string status)
		{
			var SalaryStructur = _context.wagesSalaryStructures.Find(Id);
			SalaryStructur.amount = amount;
			_context.Entry(SalaryStructur).State = EntityState.Modified;
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<WagesSalaryStructure>> GetAllWagesSalaryStructure()
		{
			return await _context.wagesSalaryStructures.Include(x => x.salaryHead).Include(x => x.employee).AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<WagesSalaryStructure>> GetWagesSalaryStructureByEmpId(int empId)
		{
			return await _context.wagesSalaryStructures.Include(x => x.salaryHead).Where(x => x.employeeId == empId).AsNoTracking().ToListAsync();
		}

		public async Task<bool> DeleteWagesSalaryStructureByempId(int empId)
		{
			_context.wagesSalaryStructures.RemoveRange(_context.wagesSalaryStructures.Where(x => x.employeeId == empId));
			return 1 == await _context.SaveChangesAsync();
		}


		public async Task<bool> DeleteWagesSalaryStructureById(int Id)
		{
			_context.wagesSalaryStructures.Remove(_context.wagesSalaryStructures.Where(x => x.Id == Id).FirstOrDefault());
			return 1 == await _context.SaveChangesAsync();
		}

		#endregion

		#region Leave Without Pay
		public async Task<bool> SaveLeaveWithoutPay(LeaveWithoutPay leaveWithoutPay)
		{
			if (leaveWithoutPay.Id != 0)
				_context.LeaveWithoutPays.Update(leaveWithoutPay);
			else
				_context.LeaveWithoutPays.Add(leaveWithoutPay);
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<LeaveWithoutPay>> GetAllLeaveWithoutPay()
		{
			return await _context.LeaveWithoutPays.Include(x => x.employeeInfo).Include(x => x.employeeInfo.department).Include(x => x.employeeInfo.lastDesignation).Include(x => x.salaryPeriod).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<LeaveWithoutPay>> GetLeaveWithoutPayBysalaryPeriodId(int salaryPeriodId)
		{
			return await _context.LeaveWithoutPays.Include(x => x.employeeInfo).Include(x => x.salaryPeriod).Where(x => x.salaryPeriodId == salaryPeriodId).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<LeaveWithoutPay>> GetLeaveWithoutPayByemployeeInfoId(int salaryPeriodId, int employeeInfoId)
		{
			return await _context.LeaveWithoutPays.Include(x => x.employeeInfo).Include(x => x.salaryPeriod).Where(x => x.salaryPeriodId == salaryPeriodId && x.employeeInfoId == employeeInfoId).AsNoTracking().ToListAsync();
		}

		public async Task<LeaveWithoutPay> GetLeaveWithoutPayById(int id)
		{
			return await _context.LeaveWithoutPays.FindAsync(id);
		}

		public async Task<bool> DeleteLeaveWithoutPayById(int id)
		{
			_context.LeaveWithoutPays.Remove(_context.LeaveWithoutPays.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		#endregion

		#region Process Emp Salary Structure
		public async Task<bool> SaveProcessEmpSalaryStructure(ProcessEmpSalaryStructure processEmpSalaryStructure)
		{
			if (processEmpSalaryStructure.Id != 0)
			{
				_context.ProcessEmpSalaryStructures.Update(processEmpSalaryStructure);
			}
			else
			{
				_context.ProcessEmpSalaryStructures.Add(processEmpSalaryStructure);
			}
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<bool> EditProcessEmpSalaryStructureForLeaveWithoutPay(int employeeInfoId, int salaryPeriodId, int noOfDays)
		{
			List<int> salaryHeads = new List<int> { 1, 2, 3, 4, 10, 11, 12, 14 };
			IEnumerable<ProcessEmpSalaryStructure> lstsalaryStructure = await _context.ProcessEmpSalaryStructures.Where(x => x.employeeInfoId == employeeInfoId && x.salaryPeriodId == salaryPeriodId && x.amount > 0 && salaryHeads.Contains(x.salaryHeadId)).AsNoTracking().ToListAsync();
			decimal? totalworkingdays = _context.salaryPeriods.Where(x => x.Id == salaryPeriodId).FirstOrDefault().daysWorking;

			foreach (ProcessEmpSalaryStructure ProcesssalaryStructure in lstsalaryStructure)
			{
				var SalaryStructur = _context.ProcessEmpSalaryStructures.Find(ProcesssalaryStructure.Id);
				SalaryStructur.amount = SalaryStructur.amount - SalaryStructur.amount * noOfDays / Convert.ToDecimal(totalworkingdays);
				_context.Entry(SalaryStructur).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<bool> EditProcessEmpSalaryStructureForAdvanceAdjustment(int employeeInfoId, int salaryPeriodId, int salaryHeadId, decimal amount)
		{
			var ProcesssalaryStructure = await _context.ProcessEmpSalaryStructures.Where(x => x.employeeInfoId == employeeInfoId && x.salaryPeriodId == salaryPeriodId && x.salaryHeadId == salaryHeadId).AsNoTracking().ToListAsync();
			if (ProcesssalaryStructure.Count() > 0)
			{
				var SalaryStructur = _context.ProcessEmpSalaryStructures.Find(ProcesssalaryStructure.FirstOrDefault().Id);
				SalaryStructur.amount = amount;
				_context.Entry(SalaryStructur).State = EntityState.Modified;
			}

			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<bool> EditProcessEmpSalaryStructureForEmployeeArrear(int employeeInfoId, int salaryPeriodId, decimal totalamount, decimal amount)
		{
			var ProcesssalaryStructure = await _context.ProcessEmpSalaryStructures.Where(x => x.employeeInfoId == employeeInfoId && x.salaryPeriodId == salaryPeriodId && x.salaryHeadId == 18).AsNoTracking().ToListAsync();
			if (ProcesssalaryStructure.Count() > 0)
			{
				var SalaryStructur = _context.ProcessEmpSalaryStructures.Find(ProcesssalaryStructure.FirstOrDefault().Id);
				SalaryStructur.amount = totalamount;
				_context.Entry(SalaryStructur).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			var ProcesssalaryStructurePFOwn = await _context.ProcessEmpSalaryStructures.Where(x => x.employeeInfoId == employeeInfoId && x.salaryPeriodId == salaryPeriodId && x.salaryHeadId == 5 && x.amount > 0).AsNoTracking().ToListAsync();
			var salaryGrade = await _context.employeesSalaryStructures.Include(x => x.salarySlab).Where(x => x.employeeInfoId == employeeInfoId && x.salaryHeadId == 1).AsNoTracking().ToListAsync();
			if (ProcesssalaryStructurePFOwn.Count() > 0)
			{
				var SalaryStructurPFOwn = _context.ProcessEmpSalaryStructures.Find(ProcesssalaryStructurePFOwn.FirstOrDefault().Id);
				var percentAmount = await _context.salaryGradePercents.Where(x => x.salaryHeadId == SalaryStructurPFOwn.salaryHeadId && x.salaryGradeId == salaryGrade.FirstOrDefault().salarySlab.salaryGradeId).AsNoTracking().ToListAsync();
				decimal? percent = percentAmount.FirstOrDefault().percentAmount / 100;
				SalaryStructurPFOwn.amount = SalaryStructurPFOwn.amount + amount * Convert.ToDecimal(percent);
				_context.Entry(SalaryStructurPFOwn).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			var ProcesssalaryStructurePFEmployer = await _context.ProcessEmpSalaryStructures.Where(x => x.employeeInfoId == employeeInfoId && x.salaryPeriodId == salaryPeriodId && x.salaryHeadId == 29 && x.amount > 0).AsNoTracking().ToListAsync();
			if (ProcesssalaryStructurePFEmployer.Count() > 0)
			{
				var SalaryStructurPFEmployer = _context.ProcessEmpSalaryStructures.Find(ProcesssalaryStructurePFEmployer.FirstOrDefault().Id);
				var percentAmountPFEmployer = await _context.salaryGradePercents.Where(x => x.salaryHeadId == SalaryStructurPFEmployer.salaryHeadId && x.salaryGradeId == salaryGrade.FirstOrDefault().salarySlab.salaryGradeId).AsNoTracking().ToListAsync();
				decimal? percentPFEmployer = percentAmountPFEmployer.FirstOrDefault().percentAmount / 100;
				SalaryStructurPFEmployer.amount = SalaryStructurPFEmployer.amount + amount * Convert.ToDecimal(percentPFEmployer);
				_context.Entry(SalaryStructurPFEmployer).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}

			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<IEnumerable<ProcessEmpSalaryStructure>> GetProcessEmpSalaryStructureByemployeeInfoId(int salaryPeriodId, int employeeInfoId)
		{
			return await _context.ProcessEmpSalaryStructures.Include(a => a.salaryPeriod).Include(a => a.salaryHead).Include(a => a.employeeInfo).Where(x => x.salaryPeriodId == salaryPeriodId && x.employeeInfoId == employeeInfoId).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<ProcessEmpSalaryStructure>> GetProcessEmpSalaryStructureBysalaryPeriodId(int salaryPeriodId)
		{
			return await _context.ProcessEmpSalaryStructures.Include(a => a.salaryPeriod).Include(a => a.employeeInfo).Where(x => x.salaryPeriodId == salaryPeriodId).AsNoTracking().ToListAsync();
		}

		public async Task<bool> DeleteProcessEmpSalaryStructureByempId(int empId, int salaryPeriodId)
		{
			_context.ProcessEmpSalaryStructures.RemoveRange(_context.ProcessEmpSalaryStructures.Where(x => x.employeeInfoId == empId && x.salaryPeriodId == salaryPeriodId));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<bool> DeleteProcessEmpSalaryStructureBysalaryPeriodId(int salaryPeriodId)
		{
			_context.ProcessEmpSalaryStructures.RemoveRange(_context.ProcessEmpSalaryStructures.Where(x => x.salaryPeriodId == salaryPeriodId));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<decimal> GetNetPayableByemployeeInfoId(int employeeInfoId, int salaryPeriodId)
		{
			IEnumerable<ProcessEmpSalaryStructure> salaryMasters = await _context.ProcessEmpSalaryStructures.Include(x => x.employeeInfo).Include(x => x.salaryPeriod).Include(x => x.salaryHead).Where(x => x.employeeInfoId == employeeInfoId && x.salaryPeriodId == salaryPeriodId).AsNoTracking().ToListAsync();

			decimal additions = salaryMasters.Where(x => x.salaryHead.salaryHeadType == "Addition").Sum(x => x.amount);
			decimal deductions = salaryMasters.Where(x => x.salaryHead.salaryHeadType == "Deduction").Sum(x => x.amount);

			decimal netPayable = additions - deductions;

			return netPayable;
		}

		#endregion

		#region Process Emp Salary Master
		public async Task<bool> SaveProcessEmpSalaryMaster(ProcessEmpSalaryMaster processEmpSalaryMaster)
		{
			if (processEmpSalaryMaster.Id != 0)
			{
				_context.ProcessEmpSalaryMasters.Update(processEmpSalaryMaster);
			}
			else
			{
				_context.ProcessEmpSalaryMasters.Add(processEmpSalaryMaster);
			}
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<IEnumerable<ProcessEmpSalaryMaster>> GetProcessEmpSalaryMasterByemployeeInfoId(int salaryPeriodId, int employeeInfoId)
		{
			return await _context.ProcessEmpSalaryMasters.Include(a => a.salaryPeriod).Include(a => a.employeeInfo).Where(x => x.salaryPeriodId == salaryPeriodId && x.employeeInfoId == employeeInfoId).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<ProcessEmpSalaryMaster>> GetProcessEmpSalaryMasterBysalaryPeriodId(int salaryPeriodId)
		{
			return await _context.ProcessEmpSalaryMasters.Include(a => a.salaryPeriod).Include(a => a.employeeInfo).Where(x => x.salaryPeriodId == salaryPeriodId).AsNoTracking().ToListAsync();
		}
		public async Task<bool> DeleteProcessEmpSalaryMasterByempId(int empId, int salaryPeriodId)
		{
			_context.ProcessEmpSalaryMasters.RemoveRange(_context.ProcessEmpSalaryMasters.Where(x => x.employeeInfoId == empId && x.salaryPeriodId == salaryPeriodId));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<bool> DeleteProcessEmpSalaryMasterBysalaryPeriodId(int salaryPeriodId)
		{
			_context.ProcessEmpSalaryMasters.RemoveRange(_context.ProcessEmpSalaryMasters.Where(x => x.salaryPeriodId == salaryPeriodId));
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<SalaryProcessDataViewModel>> ProcessEmpSalaryMasterBySp(int? salaryPeriodId, int? employeeInfoId)
		{
			return await _context.salaryProcessDataViewModels.FromSql($"SP_ProcessEmployeeSalaryMaster {salaryPeriodId},{employeeInfoId}").AsNoTracking().ToListAsync();
		}
		#endregion

		#region Salary Process Log

		public async Task<bool> SaveSalaryProcessLog(SalaryProcessLog salaryProcessLog)
		{
			if (salaryProcessLog.Id != 0)
			{
				_context.SalaryProcessLogs.Update(salaryProcessLog);
			}
			else
			{
				_context.SalaryProcessLogs.Add(salaryProcessLog);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<SalaryProcessLog>> GetAllSalaryProcessLog()
		{
			return await _context.SalaryProcessLogs.Include(a => a.salaryPeriod).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}
		#endregion

		#region Salary Status Log

		public async Task<int> SaveSalaryStatusLog(SalaryStatusLog salaryStatusLog)
		{
			if (salaryStatusLog.Id != 0)
			{
				_context.SalaryStatusLogs.Update(salaryStatusLog);
			}
			else
			{
				_context.SalaryStatusLogs.Add(salaryStatusLog);
			}
			await _context.SaveChangesAsync();
			return salaryStatusLog.Id;
		}

		public async Task<int> SaveSalaryContribution(PFSalaryContribution pFSalaryContribution)
		{
			if (pFSalaryContribution.Id != 0)
			{
				_context.PFSalaryContributions.Update(pFSalaryContribution);
			}
			else
			{
				_context.PFSalaryContributions.Add(pFSalaryContribution);
			}
			await _context.SaveChangesAsync();
			return pFSalaryContribution.Id;
		}

		public async Task<IEnumerable<SalaryStatusLog>> GetSalaryStatusLogByPeriodId(int periodId)
		{
			return await _context.SalaryStatusLogs.Include(a => a.salaryPeriod).Where(a => a.salaryPeriodId == periodId).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}
		#endregion

		#region Process Salary Remarks

		public async Task<int> SaveProcessSalaryRemarks(ProcessSalaryRemarks processSalaryRemarks)
		{
			if (processSalaryRemarks.Id != 0)
			{
				_context.ProcessSalaryRemarks.Update(processSalaryRemarks);
			}
			else
			{
				_context.ProcessSalaryRemarks.Add(processSalaryRemarks);
			}
			await _context.SaveChangesAsync();
			return processSalaryRemarks.Id;
		}

		public async Task<bool> DeleteProcessSalaryRemarks(int? empId, int? periodId)
		{
			_context.ProcessSalaryRemarks.RemoveRange(_context.ProcessSalaryRemarks.Where(a => a.employeeInfoId == empId && a.salaryPeriodId == periodId));
			return 1 == await _context.SaveChangesAsync();
		}

		#endregion

		#region Employee Arrear
		public async Task<bool> SaveEmployeeArrear(EmployeeArrear employeeArrear)
		{
			if (employeeArrear.Id != 0)
				_context.EmployeeArrears.Update(employeeArrear);
			else
				_context.EmployeeArrears.Add(employeeArrear);
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<bool> SaveEmployeeArrearInfo(EmployeeArrearInfo employeeArrear)
		{
			if (employeeArrear.Id != 0)
				_context.EmployeeArrearInfo.Update(employeeArrear);
			else
				_context.EmployeeArrearInfo.Add(employeeArrear);
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<EmployeeArrear>> GetAllEmployeeArrear()
		{
			return await _context.EmployeeArrears.Include(x => x.employeeInfo).Include(x => x.employeeInfo.department).Include(x => x.salaryPeriod).Include(x => x.salaryHead).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<EmployeeArrearInfo>> GetAllEmployeeArrearInfo()
		{
			return await _context.EmployeeArrearInfo.Include(x => x.employee).Include(x => x.period).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<EmployeeArrear>> GetEmployeeArrearBysalaryPeriodId(int salaryPeriodId)
		{
			return await _context.EmployeeArrears.Include(x => x.salaryHead).Include(x => x.employeeInfo).Where(x => x.salaryPeriodId == salaryPeriodId).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<EmployeeArrear>> GetEmployeeArrearByEmpAndPeriodId(int empId, int periodId)
		{
			return await _context.EmployeeArrears.Include(x => x.salaryHead).Include(x => x.employeeInfo).Where(x => x.employeeInfoId == empId && x.salaryPeriodId == periodId).AsNoTracking().ToListAsync();
		}

		public async Task<EmployeeArrear> GetEmployeeArrearById(int id)
		{
			return await _context.EmployeeArrears.FindAsync(id);
		}

		public async Task<bool> DeleteEmployeeArrearByEmpAndPeriodId(int empId, int periodId)
		{
			_context.EmployeeArrears.RemoveRange(_context.EmployeeArrears.Where(x => x.employeeInfoId == empId && x.salaryPeriodId == periodId));
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<EmployeeArrear>> EmployeeArrearCalculationByEmpId(int empId, int periodId, decimal? totalAmount, decimal? bonusAmount)
		{
			List<EmployeeArrear> EmployeeArrear = new List<EmployeeArrear>();
			List<int> salaryHeadId = new List<int>();

			decimal? GrossAmount = 0;
			decimal? additions = 0;
			var data = _context.employeesSalaryStructures.Include(x => x.salaryHead).Where(x => x.employeeInfoId == empId).ToList();
			if (bonusAmount > 0)
			{
				additions = data.Where(x => x.salaryHead.salaryHeadType == "Addition" && x.isActive == "Active").Sum(x => x.amount);
				salaryHeadId = _context.salaryHeads.Where(x => x.isArrear == "Yes").Select(x => x.Id).ToList();
			}
			else
			{
				additions = data.Where(x => x.salaryHead.salaryHeadType == "Addition" && x.isActive == "Active" && x.salaryHead.isBonus != "Yes").Sum(x => x.amount);
				salaryHeadId = _context.salaryHeads.Where(x => x.isArrear == "Yes" && x.isBonus != "Yes").Select(x => x.Id).ToList();
			}
			decimal? deductions = data.Where(x => x.salaryHead.salaryHeadType == "Deduction" && x.isActive == "Active").Sum(x => x.amount);
			GrossAmount = additions - deductions;


			List<EmployeesSalaryStructure> lstStructure = _context.employeesSalaryStructures.Include(x => x.employeeInfo).Include(x => x.salaryHead).Where(x => salaryHeadId.Contains(x.salaryHeadId) && x.employeeInfoId == empId).ToList();



			foreach (EmployeesSalaryStructure item in lstStructure)
			{
				EmployeeArrear.Add(new EmployeeArrear
				{
					employeeInfoId = item.employeeInfoId,
					salaryPeriodId = periodId,
					salaryHeadId = item.salaryHeadId,
					salaryHeadName = item.salaryHead.salaryHeadName,
					arrearAmount = Convert.ToDecimal((item.amount * totalAmount) / GrossAmount),
					ratio = Convert.ToDecimal((item.amount / GrossAmount) * 100)
				});
			}

			return EmployeeArrear;
		}

		#endregion

		#region Advance Adjustment

		public async Task<int> SaveAdvanceAdjustment(AdvanceAdjustment advanceAdjustment)
		{
			if (advanceAdjustment.Id != 0)
				_context.AdvanceAdjustments.Update(advanceAdjustment);
			else
				_context.AdvanceAdjustments.Add(advanceAdjustment);
			await _context.SaveChangesAsync();
			return advanceAdjustment.Id;
		}

		public async Task<IEnumerable<AdvanceAdjustment>> GetAllAdvanceAdjustment()
		{
			return await _context.AdvanceAdjustments.Include(x => x.employeeInfo).Include(x => x.salaryPeriod).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<AdvanceAdjustment>> GetAdvanceAdjustmentBysalaryPeriodId(int salaryPeriodId)
		{
			return await _context.AdvanceAdjustments.Include(x => x.employeeInfo).Include(x => x.salaryPeriod).Where(x => x.salaryPeriodId <= salaryPeriodId).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<AdvanceAdjustment>> GetAdvanceAdjustmentByemployeeInfoId(int salaryPeriodId, int employeeInfoId)
		{
			return await _context.AdvanceAdjustments.Include(x => x.employeeInfo).Include(x => x.salaryPeriod).Where(x => x.salaryPeriodId <= salaryPeriodId && x.employeeInfoId == employeeInfoId).AsNoTracking().ToListAsync();
		}
		public async Task<AdvanceAdjustment> GetAdvanceAdjustmentById(int id)
		{
			return await _context.AdvanceAdjustments.FindAsync(id);
		}

		public async Task<bool> DeleteAdvanceAdjustmentById(int id)
		{
			_context.AdvanceAdjustments.Remove(_context.AdvanceAdjustments.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<bool> SaveAdvanceAdjustmentDetail(AdvanceAdjustmentDetail advanceAdjustment)
		{
			if (advanceAdjustment.Id != 0)
				_context.advanceAdjustmentDetails.Update(advanceAdjustment);
			else
				_context.advanceAdjustmentDetails.Add(advanceAdjustment);
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<AdvanceAdjustmentDetail>> GetAllAdvanceAdjustmentDetailByMasterId(int id)
		{
			return await _context.advanceAdjustmentDetails.Where(x => x.advanceAdjustmentId == id).Include(x => x.advanceAdjustment.employeeInfo.department).Include(x => x.advanceAdjustment.salaryHead).AsNoTracking().ToListAsync();
		}

		public void UpdateAdvanceAdjustmentDetail(int docId, decimal? amount)
		{
			var user = _context.advanceAdjustmentDetails.Find(docId);
			user.scheduleAmount = amount;
			user.updatedAt = DateTime.Now;
			_context.Entry(user).State = EntityState.Modified;
			_context.SaveChanges();
		}

		#endregion

		#region Loan Policy

		public async Task<bool> SaveLoanPolicy(LoanPolicy loanPolicy)
		{
			if (loanPolicy.Id != 0)
				_context.LoanPolicies.Update(loanPolicy);
			else
				_context.LoanPolicies.Add(loanPolicy);
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<bool> SaveCarLoanPolicy(LoanPolicy loanPolicy)
		{
			if (loanPolicy.Id != 0)
				_context.LoanPolicies.Update(loanPolicy);
			else
				_context.LoanPolicies.Add(loanPolicy);
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<int> SaveCarLoanPolicyNew(LoanPolicyNew loanPolicyNew)
		{
			if (loanPolicyNew.Id != 0)
				_context.loanPolicyNews.Update(loanPolicyNew);
			else
				_context.loanPolicyNews.Add(loanPolicyNew);
			await _context.SaveChangesAsync();
			return loanPolicyNew.Id;
		}
		public async Task<int> SaveLoanPolicyDetail(LoanPolicyDetail loanPolicyDetail)
		{
			if (loanPolicyDetail.Id != 0)
				_context.loanPolicyDetails.Update(loanPolicyDetail);
			else
				_context.loanPolicyDetails.Add(loanPolicyDetail);
			await _context.SaveChangesAsync();
			return loanPolicyDetail.Id;
		}
		public async Task<int> DeleteLoanPolicyDetailsByMasterId(int id)
		{
			_context.loanPolicyDetails.RemoveRange(_context.loanPolicyDetails.Where(x => x.loanPolicyId == id));
			await _context.SaveChangesAsync();
			return id;
		}
		public async Task<bool> SaveHomeLoanPolicy(LoanPolicy loanPolicy)
		{
			if (loanPolicy.Id != 0)
				_context.LoanPolicies.Update(loanPolicy);
			else
				_context.LoanPolicies.Add(loanPolicy);
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<bool> SavePFLoanPolicy(LoanPolicy loanPolicy)
		{
			if (loanPolicy.Id != 0)
				_context.LoanPolicies.Update(loanPolicy);
			else
				_context.LoanPolicies.Add(loanPolicy);
			return 1 == await _context.SaveChangesAsync();
		}


		public async Task<IEnumerable<LoanPolicy>> GetAllLoanPolicy()
		{
			return await _context.LoanPolicies.Include(x => x.salaryGrade).Include(x => x.salaryHead).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<LoanPolicyNew>> GetAllLoanPolicyNew()
		{
			return await _context.loanPolicyNews.Include(x => x.designation).Include(x => x.employeeInfo).AsNoTracking().ToListAsync();
		}

		public async Task<LoanPolicy> GetLoanPolicyById(int id)
		{
			return await _context.LoanPolicies.FindAsync(id);
		}

		public async Task<bool> DeleteLoanPolicyById(int id)
		{
			_context.LoanPolicies.Remove(_context.LoanPolicies.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<bool> DeleteHomeLoanPolicyById(int id)
		{
			_context.LoanPolicies.Remove(_context.LoanPolicies.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<bool> DeleteCarLoanPolicyById(int id)
		{
			if (_context.loanPolicyDetails.Where(x => x.loanPolicyId == id).ToList().Count() > 0)
			{
				_context.loanPolicyDetails.RemoveRange(_context.loanPolicyDetails.Where(x => x.loanPolicyId == id).ToList());
				_context.SaveChanges();
			}

			_context.loanPolicyNews.Remove(_context.loanPolicyNews.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<bool> DeletePFLoanPolicyById(int id)
		{
			_context.LoanPolicies.Remove(_context.LoanPolicies.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

		#endregion

		#region Loan Schedule
		public async Task<int> SaveLoanScheduleMaster(LoanScheduleMaster advanceAdjustment)
		{
			if (advanceAdjustment.Id != 0)
				_context.loanScheduleMasters.Update(advanceAdjustment);
			else
				_context.loanScheduleMasters.Add(advanceAdjustment);
			await _context.SaveChangesAsync();
			return advanceAdjustment.Id;
		}

		public async Task<bool> SaveLoanScheduleDetail(LoanScheduleDetail advanceAdjustment)
		{
			if (advanceAdjustment.Id != 0)
				_context.loanScheduleDetails.Update(advanceAdjustment);
			else
				_context.loanScheduleDetails.Add(advanceAdjustment);
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<LoanScheduleMaster>> GetAllLoanScheduleMaster()
		{
			return await _context.loanScheduleMasters.Include(x => x.employeeInfo).Include(x => x.loanPolicy.salaryHead).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<LoanScheduleMaster>> GetAllLoanScheduleMasterByEmpId(int? empId)
		{
			return await _context.loanScheduleMasters.Include(x => x.employeeInfo).Include(x => x.loanPolicy.salaryHead).Where(a => a.employeeInfoId == empId).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<LoanScheduleDetail>> GetAllLoanScheduleDetailByMasterId(int id)
		{
			return await _context.loanScheduleDetails.Where(x => x.loanScheduleMasterId == id).Include(x => x.loanScheduleMaster.employeeInfo.department).Include(x => x.loanScheduleMaster.loanPolicy.salaryHead).AsNoTracking().ToListAsync();
		}

		public async Task<bool> DeleteLoanScheduleDetailById(int id)
		{
			_context.loanScheduleDetails.RemoveRange(_context.loanScheduleDetails.Where(x => x.loanScheduleMasterId == id).ToList());
			return 1 == await _context.SaveChangesAsync();
		}

		public void UpdateLoanScheduleDetail(int docId, decimal? amount)
		{
			var user = _context.loanScheduleDetails.Find(docId);
			user.scheduleAmount = amount;
			user.updatedAt = DateTime.Now;
			_context.Entry(user).State = EntityState.Modified;
			_context.SaveChanges();
		}

		public async Task<bool> UpdateLoanScheduleMasterApproval(int Id, int status)
		{
			LoanScheduleMaster data = await _context.loanScheduleMasters.FindAsync(Id);
			if (data != null)
			{
				data.approveStatus = status;
				_context.loanScheduleMasters.Update(data);
				return 1 == await _context.SaveChangesAsync();
			}
			return false;
		}

		public async Task<IEnumerable<LoanScheduleReportViewModel>> GetLoanReportById(int masterId)
		{
			return await _context.loanScheduleReportViewModels.FromSql($"SP_LoanScheduleById {masterId}").AsNoTracking().ToListAsync();
		}

		#endregion

		#region Loan Route

		public async Task<bool> SaveLoanRoute(LoanRoute loanRoute)
		{
			if (loanRoute.Id != 0)
				_context.LoanRoutes.Update(loanRoute);
			else
				_context.LoanRoutes.Add(loanRoute);
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<LoanRoute>> GetLoanRouteByEmpId(int empId)
		{
			return await _context.LoanRoutes.Include(x => x.loanScheduleMaster.employeeInfo.department).Include(x => x.loanScheduleMaster.loanPolicy.salaryHead).Where(x => x.employeeId == empId && x.isActive == 1).AsNoTracking().ToListAsync();
		}

		public async Task<LoanRoute> GetLoanRouteById(int id)
		{
			return await _context.LoanRoutes.FindAsync(id);
		}

		public async Task<bool> UpdateLoanRoute(int Id, int Type)
		{
			LoanRoute data = await _context.LoanRoutes.FindAsync(Id);
			if (data != null)
			{
				data.isActive = Type;
				_context.LoanRoutes.Update(data);
				return 1 == await _context.SaveChangesAsync();
			}
			return false;
		}

		public async Task<LoanRoute> GetLoanRouteByRouteOrder(int id, int order)
		{

			return await _context.LoanRoutes.Where(x => x.loanScheduleMasterId == id && x.routeOrder == order).FirstOrDefaultAsync();
		}

		#endregion

		#region Lfa Info

		public async Task<bool> SaveLfaInfo(LfaInfo lfaInfo)
		{
			if (lfaInfo.Id != 0)
				_context.LfaInfos.Update(lfaInfo);
			else
				_context.LfaInfos.Add(lfaInfo);
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<LfaInfo>> GetAllLfaInfo()
		{
			return await _context.LfaInfos.Include(x => x.employeeInfo).Include(x => x.salaryPeriod).AsNoTracking().ToListAsync();
		}

		public async Task<LfaInfo> GetLfaInfoByEmpId(int empId)
		{
			int? lfaIds = _context.LfaInfos.Where(a => a.employeeInfoId == empId).Count();

			return await _context.LfaInfos.Where(a => a.Id == lfaIds).FirstOrDefaultAsync();
		}

		public async Task<bool> DeleteLfaInfoById(int id)
		{
			_context.LfaInfos.Remove(_context.LfaInfos.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<ProcessEmpSalaryStructure> GetLastYearBasic(int empId, int periodId)
		{
			int yearName = Convert.ToInt32(_context.salaryPeriods.Include(x => x.salaryYear).Where(x => x.Id == periodId).Select(x => x.salaryYear.yearName).FirstOrDefault());
			int minusYear = yearName - 1;

			//var yearNames = _context.salaryPeriods.Include(x => x.salaryYear).Where(x => x.Id == periodId).ToList();
			////int minusYear =Convert.ToInt32(yearName) - 1;
			//string yearName = "";
			//foreach (SalaryPeriod item in yearNames)
			//{
			//    yearName = item.salaryYear.yearName;
			//}

			int periodIds = _context.salaryPeriods.Include(x => x.salaryYear).Where(x => x.Id == periodId && x.salaryTypeId == 1 && x.salaryYear.yearName == yearName.ToString()).Max(x => x.Id);

			return await _context.ProcessEmpSalaryStructures.Where(a => a.employeeInfoId == empId && a.salaryHeadId == 1 && a.salaryPeriodId == periodIds).FirstOrDefaultAsync();
		}
		#endregion

		#region Salary Process
		public async Task<IEnumerable<SalaryProcessDataViewModel>> EmpSalaryProcess(int salaryPeriodId, int employeeInfoId)
		{
			return await _context.salaryProcessDataViewModels.FromSql($"spProcessEmpSalary {salaryPeriodId},{employeeInfoId}").AsNoTracking().ToListAsync();
		}
		public async Task<SalaryProcessDataViewModel> UpdatePfTransactionByPeriodId(int salaryPeriodId)
		{
			return await _context.salaryProcessDataViewModels.FromSql($"sp_UpdatePFBySalaryPeriod {salaryPeriodId}").AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<IEnumerable<SalaryProcessDataViewModel>> BranchWiseEmpSalaryProcess(int salaryPeriodId, int hrBranchId)
		{
			return await _context.salaryProcessDataViewModels.FromSql($"spBranchProcessEmpSalary {salaryPeriodId},{hrBranchId}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<SalaryProcessDataViewModel>> ProcessPartialSalary(int salaryPeriodId, int type, int employeeInfoId)
		{
			return await _context.salaryProcessDataViewModels.FromSql($"spProcessEmpSalaryPartial {salaryPeriodId}, {type},{employeeInfoId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<SalaryProcessDataViewModel>> GetAllEmployeesByType(int salaryPeriodId, int type)
		{
			return await _context.salaryProcessDataViewModels.FromSql($"spGetAllEmployeesByPartialType {salaryPeriodId}, {type}").AsNoTracking().ToListAsync();
		}
		public async Task<SalaryProcessDataViewModel> ProcessIndividualEmployeeSalaryForLongReport(int salaryPeriodId, int empId)
		{
			return await _context.salaryProcessDataViewModels.FromSql($"spProcessIndividualEmployeeSalaryForLongReport {salaryPeriodId}, {empId}").AsNoTracking().FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<SalaryProcessDataViewModel>> ZoneWiseEmpSalaryProcess(int salaryPeriodId, int locationId)
		{
			return await _context.salaryProcessDataViewModels.FromSql($"spZoneProcessEmpSalary {salaryPeriodId},{locationId}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<SalaryProcessDataViewModel>> WagesSalaryProcess(int salaryPeriodId, int employeeInfoId)
		{
			return await _context.salaryProcessDataViewModels.FromSql($"spProcessWagesSalary {salaryPeriodId},{employeeInfoId}").AsNoTracking().ToListAsync();
		}

		public int ProcessSalaryBackgroundTasks(int periodId, int empId)
		{
			_context.Database.ExecuteSqlCommand($"sp_ProcessSalaryBackgroundTask {periodId}, {empId}");
			return 1;
		}

		public async Task<int> UpdateLockStatusBranchWise(int lockId)
		{
			var lck = await _context.branchWiseSalaryLocks.FindAsync(lockId);
			if (lck.status == 0)
			{
				lck.status = 1;
			}
			else
			{
				lck.status = 0;
			}
			_context.branchWiseSalaryLocks.Update(lck);
			await _context.SaveChangesAsync();
			return Convert.ToInt32(lck.status);
		}

		public async Task<IEnumerable<SalaryProcessDataViewModel>> EmpBonusProcess(int salaryPeriodId, int salaryHeadId, int employeeInfoId, DateTime? lastDayofPeriod, string userName, string bonusFor)
		{
			return await _context.salaryProcessDataViewModels.FromSql($"spProcessEmpBonusSalary {salaryPeriodId},{salaryHeadId},{employeeInfoId},{lastDayofPeriod},{userName},{bonusFor}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<SalaryProcessDataViewModel>> BonusProcess(string username, int salaryPeriodId, int hrBranch, int zoneId, string empCode)
		{
			var period = await _context.salaryPeriods.Where(x => x.Id == salaryPeriodId).FirstOrDefaultAsync();
			if (period.bonusTypeId == 4)
			{
				//New
				//return await _context.salaryProcessDataViewModels.FromSql($"sp_ProcessIncentiveBonus {salaryPeriodId} ").AsNoTracking().ToListAsync();
				//Old
				return await _context.salaryProcessDataViewModels.FromSql($"sp_ProcessIncentiveBonus {username},{salaryPeriodId},{hrBranch},{zoneId},{empCode}").AsNoTracking().ToListAsync();
			}
			else
			{
				return await _context.salaryProcessDataViewModels.FromSql($"sp_ProcessEmployeesBonus {username},{salaryPeriodId},{hrBranch},{zoneId},{empCode}").AsNoTracking().ToListAsync();
			}
		}

		public int LoanDeductionByPeriodId(int salaryPeriodId)
		{
			_context.Database.ExecuteSqlCommand($"sp_LoanDeduction {salaryPeriodId}");
			return 1;
		}

		#endregion

		#region Payroll Report

		public async Task<IEnumerable<PayslipReportViewModel>> GetPaySlipByEmpId(int employeeInfoId, int salaryPeriodId)
		{
			try
			{
				var data = await _context.payslipReportViewModels.FromSql($"spRptPaySlip {employeeInfoId}, {salaryPeriodId}").AsNoTracking().ToListAsync();
				return data;
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public async Task<IEnumerable<SalaryCertificateReportViewModel>> GetSalaryCertificateByEmpId(int employeeInfoId, int salaryPeriodId)
		{
			var data = await _context.salaryCertificateReportViewModels.FromSql($"spRptSalaryCertificate {employeeInfoId}, {salaryPeriodId}").AsNoTracking().ToListAsync();
			return data;
		}



		public async Task<string> GetPostingByEmpId(int employeeId)
		{
			var emp = await _context.employeeInfos.Where(x => x.Id == employeeId).FirstOrDefaultAsync();
			var placename = "";
			if (emp.hrBranchId == 205)
			{
				if (emp.departmentId != null)
				{
					placename = await _context.departments.Where(x => x.Id == emp.departmentId).Select(x => x.deptName).FirstOrDefaultAsync();
				}
				else if (emp.functionInfoId != null)
				{
					placename = await _context.FunctionInfos.Where(x => x.Id == emp.functionInfoId).Select(x => x.branchUnitName).FirstOrDefaultAsync();
				}
				else if (emp.hrDivisionId != null)
				{
					placename = await _context.hrDivisions.Where(x => x.Id == emp.hrDivisionId).Select(x => x.divName).FirstOrDefaultAsync();
				}
				else if (emp.hrUnitId != null)
				{
					placename = await _context.hrUnits.Where(x => x.Id == emp.hrUnitId).Select(x => x.unitName).FirstOrDefaultAsync();
				}
				else
				{
					placename = "Head Office";
				}
			}
			else if (emp.locationId != null)
			{
				placename = await _context.Locations.Where(x => x.Id == emp.locationId).Select(x => x.branchUnitName).FirstOrDefaultAsync();
			}
			else if (emp.hrBranchId != 205 && emp.hrBranchId != null)
			{
				placename = await _context.hrBranches.Where(x => x.Id == emp.hrBranchId).Select(x => x.branchName).FirstOrDefaultAsync();
			}
			else
			{
				placename = "";
			}
			//var hrBranch = await _context.employeeInfos.AsNoTracking().Include(x => x.hrBranch).Where(x => x.Id == employeeId).Select(x => x.hrBranch.branchName).FirstOrDefaultAsync();
			//var department = await _context.employeeInfos.AsNoTracking().Include(x => x.department).Where(x => x.Id == employeeId).Select(x => x.department.deptName).FirstOrDefaultAsync();
			//var zone = await _context.employeeInfos.AsNoTracking().Include(x => x.location).Where(x => x.Id == employeeId).Select(x => x.location.branchUnitName).FirstOrDefaultAsync();
			//var office = await _context.employeeInfos.AsNoTracking().Include(x => x.functionInfo).Where(x => x.Id == employeeId).Select(x => x.functionInfo.branchUnitName).FirstOrDefaultAsync();
			//var unit = await _context.employeeInfos.AsNoTracking().Include(x => x.hrUnit).Where(x => x.Id == employeeId).Select(x => x.hrUnit.unitName).FirstOrDefaultAsync();
			//if (hrBranch != null)
			//{
			//    placename = hrBranch;
			//}
			//if (department != null)
			//{
			//    placename = department;
			//}
			//if (zone != null)
			//{
			//    placename = zone;
			//}
			//if (office != null)
			//{
			//    placename = office;
			//}
			//if (unit != null)
			//{
			//    placename = unit;
			//}
			return placename;
		}


		public async Task<IEnumerable<PayslipReportViewModel>> GetWagesPaySlipByEmpId(int employeeInfoId, int salaryPeriodId)
		{
			return await _context.payslipReportViewModels.FromSql($"spWagesRptPaySlip {employeeInfoId}, {salaryPeriodId}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<PayslipReportViewModelApi>> GetPaySlipAdditionByEmpIdApi(int employeeInfoId, int salaryPeriodId)
		{
			return await _context.payslipReportViewModelApis.FromSql($"spRptPaySlipAdditionApi {employeeInfoId}, {salaryPeriodId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<PayslipReportViewModelApi>> GetPaySlipDeductionByEmpIdApi(int employeeInfoId, int salaryPeriodId)
		{
			return await _context.payslipReportViewModelApis.FromSql($"spRptPaySlipDeductionApi {employeeInfoId}, {salaryPeriodId}").AsNoTracking().ToListAsync();
		}

		//New from bellow
		public async Task<IEnumerable<SalaryCertificateViewModel>> GetSalaryCertificateReportByEmpId(int employeeInfoId, int salaryPeriodId)
		{
			try
			{
				var data = await _context.SalaryCertificateViewModel.FromSql($"spRptPaySlipAddition {employeeInfoId}, {salaryPeriodId}").AsNoTracking().ToListAsync();
				return data;
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}


		//Asad Added above from this one for certificate
		public async Task<IEnumerable<PayslipReportViewModel>> GetPaySlipAdditionByEmpId(int employeeInfoId, int salaryPeriodId)
		{
			try
			{
				var data = await _context.payslipReportViewModels.FromSql($"spRptPaySlipAddition {employeeInfoId}, {salaryPeriodId}").AsNoTracking().ToListAsync();
				return data;
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

		public async Task<IEnumerable<PayslipReportViewModel>> GetWagesPaySlipAdditionByEmpId(int employeeInfoId, int salaryPeriodId)
		{
			return await _context.payslipReportViewModels.FromSql($"spWagesRptPaySlipAddition {employeeInfoId}, {salaryPeriodId}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<PayslipReportViewModel>> GetPaySlipDeductionByEmpId(int employeeInfoId, int salaryPeriodId)
		{
			try
			{
				var data = await _context.payslipReportViewModels.FromSql($"spRptPaySlipDeduction {employeeInfoId}, {salaryPeriodId}").AsNoTracking().ToListAsync();
				return data;
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

		public async Task<IEnumerable<PayslipReportViewModel>> GetWagesPaySlipDeductionByEmpId(int employeeInfoId, int salaryPeriodId)
		{
			return await _context.payslipReportViewModels.FromSql($"spWagesRptPaySlipDeduction {employeeInfoId}, {salaryPeriodId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<salaryAddDiductionViewModel>> GetSalaryAddiDidinfoByPeriodNbranchId(int salaryPeriodId, int branchId)
		{
			return await _context.salaryAddDiductionVM.FromSql($"sp_GetSalaryVoucherList {salaryPeriodId}, {branchId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<SalaryVoucherViewModel>> GetSalaryVoucherByPeriodIdAndSbu(int periodId, string sbu, string type)
		{
			int sbuid = _context.SpecialBranchUnits.Where(x => x.branchUnitName == sbu).Select(x => x.Id).FirstOrDefault();
			return await _context.salaryVoucherVm.FromSql($"sp_GenerateSalaryVoucherNew {periodId}, {sbuid}, {type}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<SalaryVoucherViewModel>> GetSalaryVoucherByPeriodId(int periodId, int id, string type)
		{
			return await _context.salaryVoucherVm.FromSql($"sp_GenerateSalaryVoucherNew {periodId}, {id}, {type}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<GetMobileOrCarAllowanceVm>> GetMobileOrCarAllowanceVms(int periodId, string headName)
		{
			return await _context.mobileOrCarAllowanceVms.FromSql($"sp_GetMobileOrCarAllowance {periodId}, {headName}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<SalaryVoucherAllViewModel>> GetSalaryVoucherAllByPeriodId(int periodId, string type)
		{
			return await _context.salaryVoucherAllVm.FromSql($"sp_GetAllSalaryVoucher {periodId}, {type}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<BonusVoucherViewModel>> GetBonusVoucherByPeriodId(int periodId, int hrBranchId, int zoneId)
		{
			return await _context.bonusVoucherViewModels.FromSql($"sp_BonusVoucherByBranchOrZone {periodId}, {hrBranchId}, {zoneId}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<BonusVoucherViewModel>> GetBonusVoucherByPeriodIdAndType(int periodId, string type)
		{
			return await _context.bonusVoucherViewModels.FromSql($"sp_BonusVoucherByBranchOrZoneByType {periodId}, {type}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<AllLoanViewModel>> GetAllLoanByTypeAndId(string type, int id)
		{
			return await _context.allLoanViewModels.FromSql($"sp_GetLoanSchedules {type}, {id}").AsNoTracking().ToListAsync();
		}



		public async Task<string> GetTypeName(string type, int id)
		{
			var data = "";
			if (type == "department")
			{
				data = await _context.departments.AsNoTracking().Where(x => x.Id == id).Select(x => x.deptName).FirstOrDefaultAsync();
			}
			else if (type == "branch")
			{
				data = await _context.hrBranches.AsNoTracking().Where(x => x.Id == id).Select(x => x.branchName).FirstOrDefaultAsync();
			}
			else if (type == "zone")
			{
				data = await _context.Locations.AsNoTracking().Where(x => x.Id == id).Select(x => x.branchUnitName).FirstOrDefaultAsync();
			}
			else if (type == "office")
			{
				data = await _context.FunctionInfos.AsNoTracking().Where(x => x.Id == id).Select(x => x.branchUnitName).FirstOrDefaultAsync();
			}
			else if (type == "unit")
			{
				data = await _context.hrUnits.AsNoTracking().Where(x => x.Id == id).Select(x => x.unitName).FirstOrDefaultAsync();
			}
			else if (type == "division")
			{
				data = await _context.hrDivisions.AsNoTracking().Where(x => x.Id == id).Select(x => x.divName).FirstOrDefaultAsync();
			}
			else if (type == "sbu")
			{
				data = await _context.SpecialBranchUnits.AsNoTracking().Where(x => x.Id == id).Select(x => x.branchUnitName).FirstOrDefaultAsync();
			}
			else
			{

			}
			return data;
		}
		public async Task<IEnumerable<SalaryLedgerViewModel>> GetSalaryLedgerReports(int? salaryPeriodId, int? salaryHeadId)
		{
			return await _context.salaryLedgerViewModels.FromSql($"sp_getSalaryLedgerReport {salaryPeriodId}, {salaryHeadId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<SalaryLedgerViewModel>> GetSalaryNetPayReports(int? salaryPeriodId)
		{
			return await _context.salaryLedgerViewModels.FromSql($"sp_getSalaryNetPayReport {salaryPeriodId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<IndLoanViewModel>> GetIndividualLoanByEmpId(int empId)
		{
			return await _context.indLoanViewModels.FromSql($"sp_getIndividualLoanByEmpId {empId}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<StaffLoanViewModel>> GetStaffLoanLogByDateRange(string empCode, int loanId, DateTime? startDate, DateTime? endDate)
		{
			return await _context.staffLoanViewModels.FromSql($"sp_GetLoanLog {empCode}, {loanId}, {startDate}, {endDate}").AsNoTracking().ToListAsync();
		}
		//
		public async Task<IEnumerable<StaffLoanTransactions>> GetStaffLoanTransactionsByDateRange(string empCode, int loanId, DateTime? startDate)
		{
			try
			{
				var data = await _context.staffLoanTransactions.FromSql($"sp_GetLoanTransactions {empCode}, {loanId}, {startDate?.ToString("yyyy-MM-dd")}").AsNoTracking().ToListAsync();
				return data;
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		//
		public async Task<IEnumerable<StaffLoanTransactions>> GetStaffLoanTransactionsByFormDateToDate(string empCode, int loanId, DateTime? startDate, DateTime? endDate)
		{
			try
			{
				var data = await _context.staffLoanTransactions.FromSql($"sp_GetLoanTransactionsByDateRange {empCode}, {loanId}, {startDate?.ToString("yyyy-MM-dd")}, {endDate?.ToString("yyyy-MM-dd")}").AsNoTracking().ToListAsync();
				return data;
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public async Task<IEnumerable<StaffLoanTransactions>> GetStaffLoanStatementByDateRange(string empCode, int loanId, DateTime? startDate, DateTime? endDate)
		{
			return await _context.staffLoanTransactions.FromSql($"sp_GetStaffLoanTransactions {empCode}, {loanId}, {startDate?.ToString("yyyy-MM-dd")}, {endDate?.ToString("yyyy-MM-dd")}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<StaffLoanScheduleViewModel>> GetStaffLoanScheduleByDateRange(DateTime? startDate, DateTime? endDate, int hrBranchId, int zoneId, string empCode)
		{
			return await _context.staffLoanScheduleViewModels.FromSql($"sp_GetAllIndividualLoanSchedules {startDate}, {endDate}, {hrBranchId}, {zoneId}, {empCode}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<StaffLoanScheduleViewModel>> GetStaffLoanRecoveryScheduleByDateRange(DateTime? startDate, DateTime? endDate, int hrBranchId, int zoneId, string empCode, string loanType)
		{
			return await _context.staffLoanScheduleViewModels.FromSql($"GetAllIndividualLoanRecovery {startDate}, {endDate}, {hrBranchId}, {zoneId}, {empCode}, {loanType}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<StaffLoanInterestVm>> GetStaffLoanInterestScheduleByDateRange(DateTime? startDate, DateTime? endDate, int hrBranchId, int zoneId, string loanType)
		{
			return await _context.staffLoanInterestVms.FromSql($"sp_GetInterestByDate {startDate}, {endDate}, {hrBranchId}, {zoneId}, {loanType}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<StaffLoanInterestChargeVm>> GetStaffLoanInterestChargeByDateRange(string asOndate, int hrBranchId, string zoneorBranch)
		{
			return await _context.staffLoanInterestChargeVms.FromSql($"sp_GetInterestChargeByDate {asOndate}, {hrBranchId},{zoneorBranch}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<StaffLoanAllInterestVm>> GetAllInterestByDate(DateTime? startDate, DateTime? endDate)
		{
			return await _context.staffLoanAllInterestVms.FromSql($"sp_GetAllInterestByDate {startDate}, {endDate}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<AllStaffLoanVm>> GetaLLStaffLoanByDateRange(DateTime? startDate, DateTime? endDate)
		{
			return await _context.allStaffLoanVms.FromSql($"sp_GetDisbursmentWithOutinstalment {startDate}, {endDate}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<SpecialReportVm>> GetSecuritiesLtdReprotByTypeAndPeriod(string type, int periodId)
		{
			return await _context.specialReportVms.FromSql($"sp_GetSecuritiesLtdReprotByTypeAndPeriod {type}, {periodId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<LoanRecoveryotherSalaryVm>> GetloanrecoveryotherthansalaryByDateRange(DateTime? startDate, DateTime? endDate)
		{
			return await _context.loanRecoveryotherSalaryVms.FromSql($"sp_GetLoanRecoveryOtherThanSalary {startDate}, {endDate}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<SalariedLoanDeductionVm>> GetSalariedLoanDeductionByperiodid(int periodid)
		{
			return await _context.salariedLoanDeductionVms.FromSql($"sp_GetSalariedLoanDeduction {periodid}").OrderBy(x => x.designationCode).ThenBy(x => x.senioritylevel).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<StaffLoanEmployeesVm>> GetEmpInfoByLoan()
		{
			var data = await (from l in _context.staffLoans
							  join e in _context.employeeInfos on l.employeeInfoId equals e.Id
							  select new StaffLoanEmployeesVm
							  {
								  empCode = e.employeeCode,
								  empName = e.nameEnglish,
								  loanId = l.Id,
								  loanNo = l.NewLoanNo,
								  loanType = l.loanType
							  }).ToListAsync();
			return data;
		}

		public async Task<IEnumerable<StaffLoanAsOnViewModel>> GetStaffLoanBalanceAsOnByType(string asOndate, int hrBranchId, string type)
		{
			return await _context.staffLoanAsOns.FromSql($"sp_StaffLoanBalanceAsOnByType {asOndate}, {hrBranchId}, {type}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<StaffLoanAsOnViewModel>> GetStaffLoanBalanceAsOn(string asOndate, int hrBranchId)
		{
			return await _context.staffLoanAsOns.FromSql($"sp_StaffLoanBalanceAsOn {asOndate}, {hrBranchId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<StaffLoanAsOnViewModel>> GetStaffLoanBalanceAsOn2(string asOndate, int hrBranchId, string zoneorBranch)
		{
			return await _context.staffLoanAsOns.FromSql($"sp_StaffLoanBalanceAsOn {asOndate}, {hrBranchId},{zoneorBranch}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<BonusReportViewModel>> GetBonusData(int periodId, int hrBranchId, int zoneId)
		{
			return await _context.bonusReportViewModels.FromSql($"sp_GetBonusDataByPeriodId {periodId}, {hrBranchId}, {zoneId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<BonusReportViewModel>> GetBonusSundryData(int periodId, int hrBranchId, int zoneId)
		{
			return await _context.bonusReportViewModels.FromSql($"sp_GetBonusSundryDataByPeriodId {periodId}, {hrBranchId}, {zoneId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<AllSheetBonusVm>> GetAllBonusData(int periodId, string type)
		{
			return await _context.allSheetBonusVms.FromSql($"sp_GetAllBonusSheetByPeriod {periodId}, {type}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<BonusSummaryReportViewModel>> GetBonusSummaryData(int periodId, int hrBranchId, int zoneId)
		{
			return await _context.bonusSummaryReportViewModels.FromSql($"sp_GetSummarySheetForBranch {periodId}, {hrBranchId}, {zoneId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<BranchWiseBonusSummary>> GetBonusSummaryDataByBranch(int periodId, string type)
		{
			return await _context.branchWiseBonusSummaries.FromSql($"sp_GetBonusSummarySheetBranchOrZoneWise {periodId}, {type}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<HeadOfficeWiseBonusSummary>> GetBonusSummaryDataByHeadOffice(int periodId, string type)
		{
			var data = await _context.headOfficeWiseBonusSummaries.FromSql($"sp_GetDesignationWiseBonusSummarySheetForHO {periodId}, {type}").AsNoTracking().ToListAsync();
			return data;
		}

		public async Task<IEnumerable<EmployeeInfoMinimum>> GetEmployeesAll()
		{

			var data = await _context.employeeInfos.Include(x => x.lastDesignation).AsNoTracking().Select(x => new EmployeeInfoMinimum
			{
				Id = x.Id,
				empName = x.nameEnglish,
				empCode = x.employeeCode,
				designation = x.lastDesignationId == null ? null : x.lastDesignation.shortName,
				activityStatus = x.activityStatus,
				seniorityLevel = Convert.ToInt32(x.seniorityLevel)
			}).OrderBy(x => Convert.ToInt32(x.seniorityLevel)).ToListAsync();

			return data;
		}


        public async Task<IEnumerable<EmployeeInfoMinimumNew>> GetUserWiseEmployee(string username)
        {
            var data = await _context.EmployeeInfoMinimumNews.FromSql($"sp_GetUserWiseEmployee {username}").AsNoTracking().ToListAsync();
            return data;
        }



        public async Task<IEnumerable<EmployeeInfoMinimumPF>> GetEmployeesforPFAll()
		{
			try
			{
				var data = await _context.employeeInfoMinimumPFs.FromSql($"sp_GetllEmployeeforpf").AsNoTracking().ToListAsync();
				return data;
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public async Task<IEnumerable<BranchandZoneVm>> GetBranchandZone()
		{
			return await _context.branchandZoneVms.FromSql($"sp_GetBranchandZone").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<SBSReportViewModel>> GetSBSReportAsOn(string startDate, string endDate, string loanType)
		{
			return await _context.sBSReportViewModels.FromSql($"sp_GetSBSReportByDateRange {startDate}, {endDate}, {loanType}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<EmployeeLoans>> GetLoansByEmployeeCode(string empCode)
		{
			return await _context.employeeLoans.FromSql($"sp_GetLoansByEmployeeCode {empCode}").AsNoTracking().ToListAsync();
		}
		public async Task<int> ProcessStaffLoanInterest(DateTime? processDate, int hrBranchId, string type)
		{
			var periodId = await _context.salaryPeriods.Select(x => x.Id).FirstOrDefaultAsync();

			var pDate = processDate?.ToString("yyyy-MM-dd");

			_context.Database.ExecuteSqlCommand($"sp_ProcessStaffLoanInterest {pDate}, {hrBranchId}, {type}");

			//if (hrBranchId == 1000)
			//{
			//    //_context.Database.ExecuteSqlCommand($"sp_ProcessInterest {pDate}");
			//    _context.Database.ExecuteSqlCommand($"sp_ProcessInterestByDate {pDate}");
			//}
			//else
			//{
			//    _context.Database.ExecuteSqlCommand($"sp_ProcessInterestByBranch {pDate}, {hrBranchId}");
			//}
			//var periodId = await _context.ProcessEmpSalaryStructures.AsNoTracking().Where(x => Convert.ToDateTime(x.createdAt).Date <= Convert.ToDateTime(processDate).Date).OrderByDescending(x => x.salaryPeriodId).Select(x => x.salaryPeriodId).Take(1).FirstOrDefaultAsync();
			return 1;
		}

		public async Task<LoanInterest> StaffLoanInterest(DateTime? processDate, string code, string type)
		{

			try
			{
				var data = await _context.loanInterests.FromSql($"sp_ProcessStaffLoanInterestIndividualNew {Convert.ToDateTime(processDate):yyyy-MM-dd}, {code}, {type}").AsNoTracking().FirstOrDefaultAsync();
				return data;
			}
			catch (Exception ex)
			{

				throw ex;
			}




		}




		public async Task<int> ProcessStaffLoanInterestIndividual(DateTime? processDate, string empCode, string type)
		{
			var periodId = await _context.salaryPeriods.Select(x => x.Id).FirstOrDefaultAsync();

			var pDate = processDate?.ToString("yyyy-MM-dd");

			_context.Database.ExecuteSqlCommand($"sp_ProcessStaffLoanInterestIndividual {pDate}, {empCode}, {type}");

			return 1;
		}

		public async Task<IEnumerable<MonthlySalaryReportViewModel>> GetMonthlySalaryReportByPeriodId(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			List<MonthlySalaryReportViewModel> result = await _context.monthlySalaryReportViewModels.FromSql($"spRptMonthlySalarySheet {salaryPeriodId}, {sbuId}, {pnsId}").AsNoTracking().ToListAsync();
			return result;
		}
		public async Task<IEnumerable<MonthlySalaryReportViewModel>> GetMonthlySalaryReportByPeriodIdDept(int salaryPeriodId, int? departmentId, int? pnsId)
		{
			List<MonthlySalaryReportViewModel> result = await _context.monthlySalaryReportViewModels.FromSql($"spRptMonthlySalarySheetDept {salaryPeriodId}, {departmentId}, {pnsId}").AsNoTracking().ToListAsync();
			return result;
		}
		public async Task<IEnumerable<MonthlySalaryReportViewModel>> GetMonthlySalaryReportByPeriodIdBranch(int salaryPeriodId, int? hrBranchId, int? pnsId)
		{
			List<MonthlySalaryReportViewModel> result = await _context.monthlySalaryReportViewModels.FromSql($"spRptMonthlySalarySheetBranch {salaryPeriodId}, {hrBranchId}, {pnsId}").AsNoTracking().ToListAsync();
			return result;
		}
		//31.07.2023
		//public async Task<IEnumerable<MonthlySalarySummaryReportViewModel>> GetMonthlySalarySummaryReportByPeriodId(int salaryPeriodId, int? hrBranchId, int? pnsId)
		//{
		//	List<MonthlySalarySummaryReportViewModel> result = await _context.MonthlySalarySummaryReportViewModels.FromSql($"spRptMonthlySalarySheetBranchNew {salaryPeriodId}, {hrBranchId}, {pnsId}").AsNoTracking().ToListAsync();
		//	return result;
		//}

		public async Task<IEnumerable<BranchMonthlySalaryReportViewModel>> GetMonthlySalaryReportByPeriodIdBranchNew(int salaryPeriodId, int? hrBranchId, int? pnsId)
		{
			List<BranchMonthlySalaryReportViewModel> result = await _context.branchMonthlySalaryReportViewModels.FromSql($"spRptMonthlySalarySheetBranchNew {salaryPeriodId}, {hrBranchId}, {pnsId}").AsNoTracking().ToListAsync();
			return result;
		}
		public async Task<IEnumerable<BranchMonthlySalaryReportViewModel>> GetMonthlySalaryReportByPeriodIdDeptNew(int salaryPeriodId, int? departmentId, int? pnsId)
		{
			List<BranchMonthlySalaryReportViewModel> result = await _context.branchMonthlySalaryReportViewModels.FromSql($"spRptMonthlySalarySheetDeptNew {salaryPeriodId}, {departmentId}, {pnsId}").AsNoTracking().ToListAsync();
			return result;
		}
		public async Task<IEnumerable<BranchMonthlySalaryReportViewModel>> GetMonthlySalaryReportByPeriodIdOfficeNew(int salaryPeriodId, int? officeId, int? pnsId)
		{
			List<BranchMonthlySalaryReportViewModel> result = await _context.branchMonthlySalaryReportViewModels.FromSql($"spRptMonthlySalarySheetOfficeNew {salaryPeriodId}, {officeId}, {pnsId}").AsNoTracking().ToListAsync();
			return result;
		}
		public async Task<IEnumerable<BranchMonthlySalaryReportViewModel>> GetMonthlySalaryReportByPeriodIdUnitNew(int salaryPeriodId, int? unitId, int? pnsId)
		{
			List<BranchMonthlySalaryReportViewModel> result = await _context.branchMonthlySalaryReportViewModels.FromSql($"spRptMonthlySalarySheetUnitNew {salaryPeriodId}, {unitId}, {pnsId}").AsNoTracking().ToListAsync();
			return result;
		}

		public async Task<IEnumerable<BranchMonthlySalaryReportViewModel>> GetMonthlySalaryReportByPeriodIdZoneNew(int salaryPeriodId, int? zoneId, int? pnsId)
		{
			List<BranchMonthlySalaryReportViewModel> result = await _context.branchMonthlySalaryReportViewModels.FromSql($"spRptMonthlySalarySheetZoneNew {salaryPeriodId}, {zoneId}, {pnsId}").AsNoTracking().ToListAsync();
			return result;
		}

		public async Task<IEnumerable<IndivisualEmpSalaryReportViewModel>> GetIndivisualSalaryReportByEmpIdandDate(string empCode, DateTime fDate, DateTime tDate)
		{
			List<IndivisualEmpSalaryReportViewModel> result = await _context.indivisualEmpSalaryReportVM.FromSql($"sp_GetSalaryStatement {empCode}, {fDate}, {tDate}").AsNoTracking().ToListAsync();
			return result;
		}
		public async Task<IEnumerable<IndivisualEmpSalaryReportViewModel>> GetIndivisualSalaryReportByDesigIdandDate(int desigId, DateTime fDate, DateTime tDate)
		{
			List<IndivisualEmpSalaryReportViewModel> result = await _context.indivisualEmpSalaryReportVM.FromSql($"sp_GetSalaryStatementDesigId {desigId}, {fDate}, {tDate}").AsNoTracking().ToListAsync();
			return result;
		}
		public async Task<IEnumerable<SalaryPeriod>> GetSalaryPeriodByDateRange(DateTime fDate, DateTime tDate)
		{
			List<SalaryPeriod> result = await _context.salaryPeriods.FromSql($"SP_GetSalaryPeriodByDateRange {fDate}, {tDate}").AsNoTracking().ToListAsync();
			return result;
		}
		public async Task<IEnumerable<BonusTaxReport>> GetBonusTaxReportByEmpCode(string empCode, DateTime fDate, DateTime tDate)
		{
			List<BonusTaxReport> result = await _context.bonusTaxReports.FromSql($"sp_GetBonusStatement {empCode}, {fDate}, {tDate}").AsNoTracking().ToListAsync();
			return result;
		}

		public async Task<IEnumerable<BonusTaxReport>> GetBonusTaxReportByDesig(int desigId, DateTime fDate, DateTime tDate)
		{
			List<BonusTaxReport> result = await _context.bonusTaxReports.FromSql($"sp_GetBonusStatementByDesigId {desigId}, {fDate}, {tDate}").AsNoTracking().ToListAsync();
			return result;
		}

		public async Task<BetonVataViewModel> GetBetonVataAll(DateTime fDate, DateTime tDate)
		{
			var result = await _context.betonVataViewModels.FromSql($"sp_BetonVataDateRange {fDate}, {tDate}").AsNoTracking().FirstOrDefaultAsync();
			return result;
		}

		public async Task<IEnumerable<AllBranchSalarySheetVm>> GetAllSalarySheetVms(int periodId, string type)
		{
			var result = await _context.allBranchSalarySheetVms.FromSql($"sp_GetAllSalarySheetByType {periodId}, {type}").AsNoTracking().ToListAsync();
			return result;
		}

        public async Task<EmployeeInfo> GetDesignationByEmployeeId(int employeeId)
        {
            var data = await _context.employeeInfos.Where(x=>x.Id == employeeId).Include(x=> x.lastDesignation).SingleOrDefaultAsync();
            return data;
        }
        public async Task<IEnumerable<Designation>> GetAllDesignations()
		{
			var data = await _context.designations.OrderBy(x => Convert.ToInt32(x.designationCode)).ToListAsync();
			return data;
		}

        public async Task<ExecuteNoneQuery> CollectPayrollContributionByPeriodId(int salaryPeriodId, string createBy)
        {

			var result = new ExecuteNoneQuery();
			try
			{
				result = await _context.ExecuteNoneQuery.FromSql($"CollectPayrollContribution {salaryPeriodId}, {createBy}").AsNoTracking().FirstOrDefaultAsync();  //.ToListAsync()
			}
			catch(Exception exp)
			{
			}
            return result;
        }

        public async Task<IEnumerable<EmpContributionViewModel>> GetPayrollContributionByPeriodId(int salaryPeriodId)
        {

            var data = await _context.EmpContributionViewModels.FromSql($"SP_GetPayrollContribution {salaryPeriodId}").AsNoTracking().ToListAsync();  //.ToListAsync()

            return data;
        }
        public async Task<IEnumerable<EmpContributionViewModel>> GetProcessedDataByYearAndMonth(string year, string month)
		{
			var data = new List<EmpContributionViewModel>();
			var empStructure = await _context.ProcessEmpSalaryStructures.Include(x => x.employeeInfo).Include(x => x.employeeInfo.department).Include(x => x.employeeInfo.lastDesignation).Include(x => x.salaryHead).Include(x => x.salaryPeriod).Include(x => x.salaryPeriod.salaryYear).Where(x => x.salaryPeriod.salaryYear.yearName == year && x.salaryPeriod.monthName == month && x.salaryHead.Id == 44).AsNoTracking().ToListAsync();
			foreach (var item in empStructure)
			{
				data.Add(new EmpContributionViewModel
				{
					employeeId = item.employeeInfoId,
					employeeCode = item.employeeInfo.employeeCode,
					employeeName = item.employeeInfo.nameEnglish,
					department = item.employeeInfo.department.deptName,
					lastDesignation = item.employeeInfo.lastDesignation.designationName,
					year = item.salaryPeriod.salaryYear.yearName,
					month = item.salaryPeriod.monthName,
					company = item.amount,
					own = await _context.ProcessEmpSalaryStructures.Include(x => x.salaryHead).AsNoTracking().Where(x => x.salaryHead.Id == 64 && x.employeeInfoId == item.employeeInfoId).Select(x => x.amount).FirstOrDefaultAsync()
				});
			}
			return data;
		}


		public async Task<IEnumerable<BankStatementReportViewModel>> GetBankStatementByPeriodId(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			return await _context.bankStatementReportViewModels.FromSql($"SP_RPT_BankStatement {salaryPeriodId}, {sbuId}, {pnsId}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<ReconciliationReportViewModel>> GetReconcilationStatement(int? empId, int? salaryPeriodId, int? presalaryPeriodId, int? typeId)
		{
			return await _context.reconciliationReportViewModels.FromSql($"spRptTotalReconciliationStatement {empId}, {salaryPeriodId}, {presalaryPeriodId},{typeId}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<MonthlySalaryReportViewModel>> GetWagesMonthlySalaryReportByPeriodId(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			return await _context.monthlySalaryReportViewModels.FromSql($"spWagesRptMonthlySalarySheet {salaryPeriodId}, {sbuId}, {pnsId}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<GratuityReportViewModel>> GetGratuityReport()
		{
			return await _context.gratuityReportViewModels.FromSql("spRptGratuityNew").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<EmpTaxDetailsViewModel>> GetEmpTaxDetails(int employeeInfoId, int taxYearId)
		{
			return await _context.empTaxDetailsViewModels.FromSql($"spRptEmpTaxDetails {employeeInfoId}, {taxYearId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<EmpTaxSlabViewModel>> GetEmpTaxSlab(int employeeInfoId, int taxYearId)
		{
			return await _context.empTaxSlabViewModels.FromSql($"spRptTaxSlab {employeeInfoId}, {taxYearId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<EmpRebatableTaxViewModel>> GetEmpRebatableTax(int employeeInfoId, int taxYearId)
		{
			return await _context.empRebatableTaxViewModels.FromSql($"spRptRebatableTax {employeeInfoId}, {taxYearId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<EmpTaxDeductFinalViewModel>> GetEmpTaxDeductFinal(int employeeInfoId, int taxYearId)
		{
			return await _context.empTaxDeductFinalViewModels.FromSql($"RPT_FinalTaxDeduct {employeeInfoId}, {taxYearId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<UniversalCodaXLTempleteViewModel>> GetUniversalCodaXLTempleteViewModels(int PeriodId, int EmployeeId)
		{

			List<UniversalCodaXLTempleteViewModel> universalCodaXLTempleteViewModels = new List<UniversalCodaXLTempleteViewModel>();
			List<ProcessEmpSalaryStructure> GetPocessEmplSalStructure = _context.ProcessEmpSalaryStructures.Include(x => x.employeeInfo).Include(x => x.salaryPeriod.salaryYear).Include(x => x.salaryHead).Where(x => x.salaryPeriodId == PeriodId).ToList();
			List<int> lstemployeeId = new List<int>();
			List<FinanceCode> lstFinancialCode = _context.financeCodes.ToList();
			if (EmployeeId > 0)
			{

				lstemployeeId = GetPocessEmplSalStructure.Where(x => x.employeeInfoId == EmployeeId).Select(x => x.employeeInfoId).Distinct().ToList();
			}
			else
			{
				lstemployeeId = GetPocessEmplSalStructure.Select(x => x.employeeInfoId).Distinct().ToList();
			}

			List<EmployeeProjectActivity> employeeProjectActivities = _context.employeeProjectActivities.Include(x => x.hRActivity).Include(x => x.hRProject).Include(x => x.hRDoner).Where(x => lstemployeeId.Contains((int)x.employeeId) && x.isActive == 1).ToList();
			int slno = 0;
			foreach (int id in lstemployeeId)
			{
				string fnCode = lstFinancialCode.Where(x => x.employeeId == id).Select(x => x.fnCode).FirstOrDefault();
				EmployeeProjectActivity employeeProjectActivity = employeeProjectActivities.Where(x => x.employeeId == id).FirstOrDefault();
				decimal gross = GetPocessEmplSalStructure.Where(x => x.employeeInfoId == id && x.salaryHead.salaryHeadName == "Gross Salary").FirstOrDefault().amount;
				decimal PF = GetPocessEmplSalStructure.Where(x => x.employeeInfoId == id && x.salaryHead.salaryHeadName == "PF Deduction Staff").FirstOrDefault().amount;
				decimal TDS = GetPocessEmplSalStructure.Where(x => x.employeeInfoId == id && x.salaryHead.salaryHeadName == "Income Tax").FirstOrDefault().amount;
				int line = 0;
				slno++;
				foreach (ProcessEmpSalaryStructure pos in GetPocessEmplSalStructure.Where(x => x.employeeInfoId == id && x.amount > 0))
				{
					string el1 = pos?.salaryHead?.salaryHeadCode;
					if (el1 == null)
					{
						el1 = "";
					}

					string el2 = employeeProjectActivity?.hRProject?.code;
					if (el2 == null)
					{
						el2 = "";
					}
					string el3 = employeeProjectActivity?.hRActivity?.code;
					if (el3 == null)
					{
						el3 = "";
					}
					string el4 = employeeProjectActivity?.hRDoner?.code;
					if (el4 == null)
					{
						el4 = "";
					}
					string grossstring = "";


					if (pos.salaryHead.salaryHeadName == "Gross Salary")
					{

						gross = pos.amount;

					}

					if (pos.salaryHead.salaryHeadName == "PF Deduction Staff")
					{

						grossstring = "PF Salary " + pos.salaryPeriod.monthName + "-" + pos.salaryPeriod.salaryYear.yearName + "- " + pos.employeeInfo.nameEnglish;

					}
					else if (pos.salaryHead.salaryHeadName == "Income Tax")
					{

						grossstring = "TDS Salary " + pos.salaryPeriod.monthName + "-" + pos.salaryPeriod.salaryYear.yearName + "- " + pos.employeeInfo.nameEnglish;
					}
					else
					{
						grossstring = "Salary " + pos.salaryPeriod.monthName + "-" + pos.salaryPeriod.salaryYear.yearName + "- " + pos.employeeInfo.nameEnglish;
					}
					line++;
					if (line == 1)
					{
						universalCodaXLTempleteViewModels.Add(new UniversalCodaXLTempleteViewModel
						{

							date = DateTime.Now.ToString("dd-MM-yyyy"),
							year = pos?.salaryPeriod?.salaryYear?.yearName,
							salaryPeriodId = pos?.salaryPeriodId,
							documentDescription = "Salary " + pos?.salaryPeriod?.monthName + "-" + pos?.salaryPeriod?.salaryYear.yearName + "- " + pos?.employeeInfo?.nameEnglish,
							cc = "BD",
							doc = "PAYF",
							docnum = slno.ToString(),
							line = line.ToString(),
							el1 = "2521",//el1, //pos?.salaryHead?.salaryHeadCode,
							el2 = fnCode, //el2, //employeeProjectActivity?.hRProject?.code,
							el3 = "",// employeeProjectActivity?.hRActivity?.code,
							el4 = "",// employeeProjectActivity?.hRDoner?.code,
							cur = "BDT",
							// debit = 0,
							credit = gross - PF - TDS,
							linedescription = "Salary " + pos?.salaryPeriod?.monthName + "-" + pos?.salaryPeriod?.salaryYear?.yearName + "- " + pos?.employeeInfo?.nameEnglish,




						});

						line++;
						universalCodaXLTempleteViewModels.Add(new UniversalCodaXLTempleteViewModel
						{

							date = DateTime.Now.ToString("dd-MM-yyyy"),
							year = pos?.salaryPeriod?.salaryYear?.yearName,
							salaryPeriodId = pos?.salaryPeriodId,
							documentDescription = "Salary " + pos?.salaryPeriod?.monthName + "-" + pos?.salaryPeriod?.salaryYear.yearName + "- " + pos?.employeeInfo?.nameEnglish,
							cc = "BD",
							doc = "PAYF",
							docnum = slno.ToString(),
							line = line.ToString(),
							//el1 = pos?.salaryHead?.salaryHeadCode,
							//el2 = employeeProjectActivity?.hRProject?.code,
							//el3 = employeeProjectActivity?.hRActivity?.code,
							//el4 = employeeProjectActivity?.hRDoner?.code,
							el1 = el1, //pos?.salaryHead?.salaryHeadCode,
							el2 = el2, //employeeProjectActivity?.hRProject?.code,
							el3 = el3,// employeeProjectActivity?.hRActivity?.code,
							el4 = el4,// employeeProjectActivity?.hRDoner?.code,
							cur = "BDT",
							debit = gross,
							// credit = 0,
							linedescription = grossstring




						});

					}
					if (pos.salaryHead.salaryHeadName != "Gross Salary")
					{
						universalCodaXLTempleteViewModels.Add(new UniversalCodaXLTempleteViewModel
						{

							date = DateTime.Now.ToString("dd-MM-yyyy"),
							year = pos?.salaryPeriod?.salaryYear?.yearName,
							salaryPeriodId = pos?.salaryPeriodId,
							documentDescription = "Salary " + pos?.salaryPeriod?.monthName + "-" + pos?.salaryPeriod?.salaryYear?.yearName + "- " + pos?.employeeInfo?.nameEnglish,
							cc = "BD",
							doc = "PAYF",
							docnum = slno.ToString(),
							line = line.ToString(),
							//el1=pos?.salaryHead?.salaryHeadCode,
							//el2= employeeProjectActivity?.hRProject?.code,
							//el3= employeeProjectActivity?.hRActivity?.code,
							//el4=employeeProjectActivity?.hRDoner?.code,
							el1 = el1, //pos?.salaryHead?.salaryHeadCode,
							el2 = fnCode, //el2, //employeeProjectActivity?.hRProject?.code,
							el3 = "XBDT", //el3,// employeeProjectActivity?.hRActivity?.code,
							el4 = "",// employeeProjectActivity?.hRDoner?.code,
							cur = "BDT",
							//  debit = 0,
							credit = pos.amount,
							linedescription = grossstring




						});
					}

				}



			}
			return universalCodaXLTempleteViewModels;


		}

		public async Task<IEnumerable<TaxableamountViewModel>> TaxableamountViewModels(int employeeInfoId, int taxYearId)
		{
			EmployeeInfo employee = _context.employeeInfos.Where(x => x.Id == employeeInfoId).FirstOrDefault();
			List<TaxableamountViewModel> taxableamountViewModels = new List<TaxableamountViewModel>();
			List<EmployeesSalaryStructure> employeesSalaryStructures = _context.employeesSalaryStructures.Include(x => x.salaryHead).Where(x => x.employeeInfoId == employeeInfoId).ToList();
			List<IncomeTaxSetup> incomeTaxSetups = _context.IncomeTaxSetups.Include(x => x.salaryHead).Where(x => x.taxYearId == taxYearId).ToList();
			string stringdate = "30 Jun 2020";
			if (DateTime.Now.Month >= 7)
			{
				stringdate = "30 Jun " + (DateTime.Now.Year + 1);
			}
			else
			{
				stringdate = "30 Jun " + (DateTime.Now.Year);
			}

			DateTime dateTime = DateTime.Parse(stringdate);
			decimal monthNo = Math.Round((dateTime.Subtract((DateTime)employee?.joiningDateGovtService).Days + 1) / (decimal)(365.25 / 12), 2);
			if (monthNo >= 12)
			{
				monthNo = 12;
			}
			foreach (IncomeTaxSetup data in incomeTaxSetups.OrderBy(x => x.salaryHead.sortOrder))
			{
				decimal? amount = employeesSalaryStructures.Where(x => x.salaryHeadId == data.salaryHeadId).Select(x => x.amount).FirstOrDefault() * monthNo;
				decimal? basicamount = employeesSalaryStructures.Where(x => x.salaryHead.salaryHeadName == "Basic Salary").Select(x => x.amount).FirstOrDefault() * monthNo;
				decimal? examtionamount = 0;
				if (data.exemption == "No")
				{
					examtionamount = 0;
				}
				else
				{
					if (data.exemptionPercent > 0)
					{
						decimal? exemtiononpercent = basicamount * data.exemptionPercent / 100;
						if (data.exemptionAmount > 0)
						{
							if (data.exemptionAmount > exemtiononpercent)
							{
								examtionamount = exemtiononpercent;
							}
							else
							{
								examtionamount = data.exemptionAmount;

							}
						}
						else
						{
							examtionamount = exemtiononpercent;

						}

					}
					else
					{
						examtionamount = data.exemptionAmount;
					}
				}
				if (data.salaryHead.salaryHeadName.Contains("Bonus"))
				{
					amount = basicamount * 2 / monthNo;
					examtionamount = 0;
				}
				if (data.salaryHead.salaryHeadName.Contains("Provident"))
				{

					amount = _context.employeesSalaryStructures.Where(x => x.salaryHead.headShortName == "PFOwn").Where(x => x.employeeInfoId == employeeInfoId)?.FirstOrDefault()?.amount * monthNo;
					examtionamount = 0;
				}
				if (data.salaryHead.salaryHeadName.Contains("LFA"))
				{
					examtionamount = amount * data.exemptionPercent / 100;
					//examtionamount = 0;
				}
				decimal taxableamountf = Math.Round((decimal)amount, 2, MidpointRounding.AwayFromZero) - Math.Round((decimal)examtionamount, 2, MidpointRounding.AwayFromZero);
				if (taxableamountf <= 0)
				{
					taxableamountf = 0;
				}
				taxableamountViewModels.Add(new TaxableamountViewModel
				{
					accountName = data.salaryHead.salaryHeadName,
					exemtedrule = data.exemptionRule,
					amount = Math.Round((decimal)amount, 2, MidpointRounding.AwayFromZero),
					exemAmount = Math.Round((decimal)examtionamount, 2, MidpointRounding.AwayFromZero),
					taxableAmount = taxableamountf, //Math.Round((decimal)amount, 2, MidpointRounding.AwayFromZero) - Math.Round((decimal)examtionamount, 2, MidpointRounding.AwayFromZero),
					monthNo = monthNo

				});

			}
			return taxableamountViewModels;
		}

		public async Task<IEnumerable<TaxableSlabViewModel>> TaxableslabViewModels(int employeeInfoId, int taxYearId)
		{

			List<TaxableamountViewModel> taxableamountViewModels = new List<TaxableamountViewModel>();
			List<TaxableSlabViewModel> taxableSlabViewModels = new List<TaxableSlabViewModel>();
			List<EmployeesSalaryStructure> employeesSalaryStructures = _context.employeesSalaryStructures.Include(x => x.salaryHead).Where(x => x.employeeInfoId == employeeInfoId).ToList();
			List<IncomeTaxSetup> incomeTaxSetups = _context.IncomeTaxSetups.Include(x => x.salaryHead).Where(x => x.taxYearId == taxYearId).ToList();
			EmployeeInfo employeeInfo = _context.employeeInfos.Where(x => x.Id == employeeInfoId).FirstOrDefault();
			string stringdate = "30 Jun 2020";
			if (DateTime.Now.Month >= 7)
			{
				stringdate = "30 Jun " + (DateTime.Now.Year + 1);
			}
			else
			{
				stringdate = "30 Jun " + (DateTime.Now.Year);
			}

			DateTime dateTime = DateTime.Parse(stringdate);
			decimal monthNo = Math.Round((dateTime.Subtract((DateTime)employeeInfo?.joiningDateGovtService).Days + 1) / (decimal)(365.25 / 12), 2);
			if (monthNo >= 12)
			{
				monthNo = 12;
			}
			foreach (IncomeTaxSetup data in incomeTaxSetups)
			{
				decimal? amount = employeesSalaryStructures.Where(x => x.salaryHeadId == data.salaryHeadId).Select(x => x.amount).FirstOrDefault() * monthNo;
				decimal? basicamount = employeesSalaryStructures.Where(x => x.salaryHead.salaryHeadName == "Basic Salary").Select(x => x.amount).FirstOrDefault() * monthNo;
				decimal? examtionamount = 0;
				if (data.exemption == "No")
				{
					examtionamount = 0;
				}
				else
				{
					if (data.exemptionPercent > 0)
					{
						decimal? exemtiononpercent = basicamount * data.exemptionPercent / 100;
						if (data.exemptionAmount > 0)
						{
							if (data.exemptionAmount > exemtiononpercent)
							{
								examtionamount = exemtiononpercent;
							}
							else
							{
								examtionamount = data.exemptionAmount;

							}
						}
						else
						{
							examtionamount = exemtiononpercent;

						}
					}
					else
					{
						examtionamount = data.exemptionAmount;
					}
				}
				if (data.salaryHead.salaryHeadName.Contains("Bonus"))
				{
					amount = basicamount * 2 / monthNo;
					examtionamount = 0;
				}
				if (data.salaryHead.salaryHeadName.Contains("Provident"))
				{
					//amount = basicamount * 10 / 100;
					amount = _context.employeesSalaryStructures.Where(x => x.salaryHead.headShortName == "PFOwn").Where(x => x.employeeInfoId == employeeInfoId)?.FirstOrDefault()?.amount * monthNo;
					examtionamount = 0;
				}
				if (data.salaryHead.salaryHeadName.Contains("LFA"))
				{
					examtionamount = amount * data.exemptionPercent / 100;
					// examtionamount = 0;
				}
				decimal taxableamountf = Math.Round((decimal)amount, 2, MidpointRounding.AwayFromZero) - Math.Round((decimal)examtionamount, 2, MidpointRounding.AwayFromZero);
				if (taxableamountf <= 0)
				{
					taxableamountf = 0;
				}
				taxableamountViewModels.Add(new TaxableamountViewModel
				{
					accountName = data.salaryHead.salaryHeadName,
					exemtedrule = data.exemptionRule,
					amount = Math.Round((decimal)amount, 2, MidpointRounding.AwayFromZero),
					exemAmount = Math.Round((decimal)examtionamount, 2, MidpointRounding.AwayFromZero),
					taxableAmount = taxableamountf

				});

			}


			decimal? taxableamount = taxableamountViewModels.Sum(x => x.taxableAmount);
			SlabIncomeTaxAssign slabIncomeTaxAssign = _context.SlabIncomeTaxAssigns.Include(x => x.slabType).Where(x => x.employeeInfoId == employeeInfo.Id).LastOrDefault();
			if (slabIncomeTaxAssign != null)
			{
				employeeInfo.gender = slabIncomeTaxAssign.slabType.slabTypeName;
			}
			List<SlabIncomeTax> slabIncomeTaxes = _context.SlabIncomeTaxes.Include(x => x.slabType).Where(x => x.slabType.slabTypeName.ToLower() == employeeInfo.gender.ToLower() && x.taxYearId == taxYearId).ToList();
			foreach (SlabIncomeTax tx in slabIncomeTaxes.Where(x => x.slabType.slabTypeName == employeeInfo.gender).OrderBy(x => x.sortOrder))
			{
				decimal? slabamountc = 0;
				decimal? taxableamountcurrent = 0;
				if (taxableamount > 0)
				{
					if (tx.slabAmount <= taxableamount && tx.slabAmount != 0)
					{
						slabamountc = tx.slabAmount;
						taxableamountcurrent = tx.slabAmount * tx.taxRate / 100;

						taxableSlabViewModels.Add(new TaxableSlabViewModel
						{

							calculationOfInvestment = tx.slabText,
							rate = tx.taxRate,
							slabamount = slabamountc,
							taxAmount = taxableamountcurrent

						});
						taxableamount = taxableamount - tx.slabAmount;
					}
					else if (tx.slabAmount <= taxableamount && tx.slabAmount == 0)
					{
						slabamountc = taxableamount;
						taxableamountcurrent = taxableamount * tx.taxRate / 100;

						taxableSlabViewModels.Add(new TaxableSlabViewModel
						{

							calculationOfInvestment = tx.slabText,
							rate = tx.taxRate,
							slabamount = slabamountc,
							taxAmount = taxableamountcurrent

						});
						taxableamount = 0;
					}
					else
					{
						slabamountc = taxableamount;
						taxableamountcurrent = taxableamount * tx.taxRate / 100;

						taxableSlabViewModels.Add(new TaxableSlabViewModel
						{

							calculationOfInvestment = tx.slabText,
							rate = tx.taxRate,
							slabamount = slabamountc,
							taxAmount = taxableamountcurrent

						});
						taxableamount = 0;
					}

				}


			}
			return taxableSlabViewModels;
		}
		public async Task<IEnumerable<TaxablePFViewModel>> TaxablePFViewModels(int employeeInfoId, int taxYearId)
		{

			List<TaxablePFViewModel> TaxablePFViewModels = new List<TaxablePFViewModel>();
			List<EmployeesSalaryStructure> employeesSalaryStructures = _context.employeesSalaryStructures.Include(x => x.salaryHead).Where(x => x.employeeInfoId == employeeInfoId).ToList();
			List<IncomeTaxSetup> incomeTaxSetups = _context.IncomeTaxSetups.Include(x => x.salaryHead).Where(x => x.taxYearId == taxYearId).ToList();
			EmployeeInfo employeeInfo = _context.employeeInfos.Where(x => x.Id == employeeInfoId).FirstOrDefault();

			TaxablePFViewModels.Add(new TaxablePFViewModel
			{

				accountName = "Provident Fund - Employer",
				investmentAmount = employeesSalaryStructures.Where(x => x.salaryHead.headShortName == "PFOwn").Select(x => x.amount).FirstOrDefault() * 12

			});
			TaxablePFViewModels.Add(new TaxablePFViewModel
			{

				accountName = "Provident Fund - Own",
				investmentAmount = employeesSalaryStructures.Where(x => x.salaryHead.headShortName == "PFOwn").Select(x => x.amount).FirstOrDefault() * 12

			});

			return TaxablePFViewModels;
		}
		public async Task<IEnumerable<EmployeesTax>> TaxCalculateforall(int taxYearId)
		{

			List<EmployeesSalaryStructure> employeesSalaryStructures = _context.employeesSalaryStructures.Include(x => x.salaryHead).ToList();
			List<IncomeTaxSetup> incomeTaxSetups = _context.IncomeTaxSetups.Include(x => x.salaryHead).Where(x => x.taxYearId == taxYearId).ToList();
			IEnumerable<EmployeeInfo> lstemployeeInfo = _context.employeeInfos.AsNoTracking().ToList();
			IEnumerable<SlabIncomeTaxAssign> slabIncomeTaxAssigns = _context.SlabIncomeTaxAssigns.Include(x => x.slabType).ToList();
			foreach (EmployeeInfo employeeInfo in lstemployeeInfo.Where(x => x.joiningDateGovtService != null))
			{
				string stringdate = "30 Jun 2020";
				if (DateTime.Now.Month >= 7)
				{
					stringdate = "30 Jun " + (DateTime.Now.Year + 1);
				}
				else
				{
					stringdate = "30 Jun " + (DateTime.Now.Year);
				}

				DateTime dateTime = DateTime.Parse(stringdate);
				decimal monthNo = Math.Round((dateTime.Subtract((DateTime)employeeInfo?.joiningDateGovtService).Days + 1) / (decimal)(365.25 / 12), 2);
				if (monthNo >= 12)
				{
					monthNo = 12;
				}
				await UpdateEmployeesStatus(employeeInfo.Id, taxYearId);
				// _context.employeesTaxes.Remove(_context.employeesTaxes.Find(id));
				List<TaxableamountViewModel> taxableamountViewModels = new List<TaxableamountViewModel>();
				List<TaxableSlabViewModel> taxableSlabViewModels = new List<TaxableSlabViewModel>();
				List<EmployeesSalaryStructure> employeesSalaryStructuresf = employeesSalaryStructures.Where(x => x.employeeInfoId == employeeInfo.Id).ToList();
				foreach (IncomeTaxSetup data in incomeTaxSetups)
				{
					decimal? amount = employeesSalaryStructuresf.Where(x => x.salaryHeadId == data.salaryHeadId).Select(x => x.amount).FirstOrDefault() * monthNo;
					decimal? basicamount = employeesSalaryStructuresf.Where(x => x.salaryHead.salaryHeadName == "Basic Salary").Select(x => x.amount).FirstOrDefault() * monthNo;
					decimal? examtionamount = 0;
					if (data.exemption == "No")
					{
						examtionamount = 0;
					}
					else
					{
						if (data.exemptionPercent > 0)
						{
							decimal? exemtiononpercent = basicamount * data.exemptionPercent / 100;
							if (data.exemptionAmount > 0)
							{
								if (data.exemptionAmount > exemtiononpercent)
								{
									examtionamount = exemtiononpercent;
								}
								else
								{
									examtionamount = data.exemptionAmount;

								}
							}
							else
							{
								examtionamount = exemtiononpercent;

							}
						}
						else
						{
							examtionamount = data.exemptionAmount;
						}
					}
					if (data.salaryHead.salaryHeadName.Contains("Bonus"))
					{
						amount = basicamount * 2 / monthNo;
						examtionamount = 0;
					}
					if (data.salaryHead.salaryHeadName.Contains("Provident"))
					{
						//amount = basicamount * 10 / 100;
						amount = _context.employeesSalaryStructures.Where(x => x.salaryHead.headShortName == "PFOwn").Where(x => x.employeeInfoId == employeeInfo.Id)?.FirstOrDefault()?.amount * monthNo;
						examtionamount = 0;
					}
					if (data.salaryHead.salaryHeadName.Contains("LFA"))
					{
						examtionamount = amount * data.exemptionPercent / 100;
						// examtionamount = 0;
					}
					decimal taxableamountf = Math.Round((decimal)amount, 2, MidpointRounding.AwayFromZero) - Math.Round((decimal)examtionamount, 2, MidpointRounding.AwayFromZero);
					if (taxableamountf <= 0)
					{
						taxableamountf = 0;
					}
					taxableamountViewModels.Add(new TaxableamountViewModel
					{
						accountName = data.salaryHead.salaryHeadName,
						exemtedrule = data.exemptionRule,
						amount = Math.Round((decimal)amount, 2, MidpointRounding.AwayFromZero),
						exemAmount = Math.Round((decimal)examtionamount, 2, MidpointRounding.AwayFromZero),
						taxableAmount = taxableamountf //Math.Round((decimal)amount, 2, MidpointRounding.AwayFromZero) - Math.Round((decimal)examtionamount, 2, MidpointRounding.AwayFromZero)



					});

				}
				if (employeeInfo.gender == null)
				{
					employeeInfo.gender = "Male";
				}
				SlabIncomeTaxAssign slabIncomeTaxAssign = slabIncomeTaxAssigns.Where(x => x.employeeInfoId == employeeInfo.Id).LastOrDefault();
				if (slabIncomeTaxAssign != null)
				{
					employeeInfo.gender = slabIncomeTaxAssign.slabType.slabTypeName;
				}
				List<SlabIncomeTax> slabIncomeTaxes = _context.SlabIncomeTaxes.Include(x => x.slabType).Where(x => x.slabType.slabTypeName.ToLower() == employeeInfo.gender.ToLower() && x.taxYearId == taxYearId).ToList();
				decimal? taxableamount = taxableamountViewModels.Sum(x => x.taxableAmount);
				foreach (SlabIncomeTax tx in slabIncomeTaxes.OrderBy(x => x.sortOrder))
				{
					decimal? slabamountc = 0;
					decimal? taxableamountcurrent = 0;
					if (taxableamount > 0)
					{
						if (tx.slabAmount <= taxableamount && tx.slabAmount != 0)
						{
							slabamountc = tx.slabAmount;
							taxableamountcurrent = tx.slabAmount * tx.taxRate / 100;

							taxableSlabViewModels.Add(new TaxableSlabViewModel
							{

								calculationOfInvestment = tx.slabText,
								rate = tx.taxRate,
								slabamount = slabamountc,
								taxAmount = taxableamountcurrent

							});
							taxableamount = taxableamount - tx.slabAmount;
						}
						else if (tx.slabAmount <= taxableamount && tx.slabAmount == 0)
						{
							slabamountc = taxableamount;
							taxableamountcurrent = taxableamount * tx.taxRate / 100;

							taxableSlabViewModels.Add(new TaxableSlabViewModel
							{

								calculationOfInvestment = tx.slabText,
								rate = tx.taxRate,
								slabamount = slabamountc,
								taxAmount = taxableamountcurrent

							});
							taxableamount = 0;
						}
						else
						{
							slabamountc = taxableamount;
							taxableamountcurrent = taxableamount * tx.taxRate / 100;

							taxableSlabViewModels.Add(new TaxableSlabViewModel
							{

								calculationOfInvestment = tx.slabText,
								rate = tx.taxRate,
								slabamount = slabamountc,
								taxAmount = taxableamountcurrent

							});
							taxableamount = 0;
						}

					}


				}
				decimal? PFamount = employeesSalaryStructuresf.Where(x => x.salaryHead.salaryHeadName == "Basic Salary").Select(x => x.amount).FirstOrDefault() * monthNo * 10 / 100;
				var investment = _context.InvestmentRebateSettings.Where(x => x.taxYearId == taxYearId);

				decimal? investmetrebatepercent = 0;
				decimal? orinvestmetrebatepercent = 0;
				int i = 0;
				foreach (var invrebate in investment)
				{

					if (i == 0)
					{
						if (invrebate.orInvestmentRebate >= taxableamountViewModels.Sum(x => x.taxableAmount))
						{
							investmetrebatepercent = invrebate.investmentRebate;
							orinvestmetrebatepercent = invrebate.orInvestmentRebate;
							i = 1;
						}
					}

				}

				if (i == 0)
				{
					investmetrebatepercent = investment.LastOrDefault().investmentRebate;
					orinvestmetrebatepercent = investment.LastOrDefault().orInvestmentRebate;
				}


				decimal? reabtabletax = taxableSlabViewModels.Sum(x => x.taxAmount) - ((taxableamountViewModels.Sum(x => x.taxableAmount)) * investment.FirstOrDefault().allowableInvestment / 100) * investmetrebatepercent / 100;
				if (reabtabletax <= 5000)
				{
					if (reabtabletax <= 0)
					{
						reabtabletax = 0;
					}
					else
					{
						reabtabletax = 5000;
					}

				}
				decimal? taxpermonth = reabtabletax / monthNo;
				var taxableamounts = taxableamountViewModels.Sum(x => x.taxableAmount);
				if (taxableamounts > 0 && taxpermonth > 0)
				{

					EmployeesTax employeesTax = new EmployeesTax
					{
						employeeInfoId = employeeInfo.Id,
						taxYearId = taxYearId,
						amount = taxpermonth,
						yearlyTaxableincome = taxableamounts,
						yearlyTaxableamount = reabtabletax,
						isActive = 1

					};
					await SaveEmployeesTax(employeesTax);
				}


			}
			var emplyeetaxes = _context.employeesTaxes.Include(x => x.employeeInfo).Include(x => x.taxYear).ToList();
			return emplyeetaxes;
		}
		#endregion

		#region Tax

		public async Task<bool> UpdateTaxInSalary(int PeriodId, int employeeId, int Id)
		{
			var data = _context.employeesTaxes.Find(Id);
			var SalaryPeriod = _context.ProcessEmpSalaryStructures.Where(x => x.employeeInfoId == employeeId && x.salaryPeriodId == PeriodId && x.salaryHead.salaryHeadName == "Income Tax").FirstOrDefault();
			if (SalaryPeriod != null)
			{
				SalaryPeriod.amount = (decimal)data.amount;
				_context.Entry(SalaryPeriod).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			return true;


		}
		public async Task<bool> UpdateTaxInstruc(int employeeId, int Id)
		{
			var data = _context.employeesTaxes.Find(Id);
			var SalaryPeriod = _context.employeesSalaryStructures.Where(x => x.employeeInfoId == employeeId && x.salaryHead.salaryHeadName == "Income Tax").OrderByDescending(c => c.Id).FirstOrDefault();
			if (SalaryPeriod != null)
			{
				SalaryPeriod.amount = (decimal)data.amount;
				_context.Entry(SalaryPeriod).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			return true;


		}

		public async Task<bool> UpdateTaxInSalaryall(int PeriodId)
		{
			var salaryperiod = _context.salaryPeriods.Find(PeriodId);

			var data = _context.employeesTaxes.Where(x => x.taxYearId == salaryperiod.taxYearId && x.isActive == 1).ToList();
			foreach (var x in data)
			{
				var salstruc = _context.ProcessEmpSalaryStructures.Where(y => y.employeeInfoId == x.employeeInfoId && y.salaryPeriodId == PeriodId && y.salaryHead.salaryHeadName == "Income Tax").FirstOrDefault();
				if (salstruc != null)
				{
					salstruc.amount = (decimal)x?.amount;
					_context.Entry(salstruc).State = EntityState.Modified;
					await _context.SaveChangesAsync();
				}

			}


			return true;

		}
		public async Task<bool> UpdateTaxInSalaryallstruc()
		{
			var salaryperiod = _context.taxYears.OrderByDescending(x => x.Id).FirstOrDefault();

			var data = _context.employeesTaxes.Where(x => x.taxYearId == salaryperiod.Id && x.isActive == 1).ToList();
			foreach (var x in data)
			{
				var salstruc = _context.employeesSalaryStructures.Where(y => y.employeeInfoId == x.employeeInfoId && y.salaryHead.salaryHeadName == "Income Tax").OrderByDescending(y => y.Id).FirstOrDefault();
				if (salstruc != null)
				{
					salstruc.amount = (decimal)x.amount;
					_context.Entry(salstruc).State = EntityState.Modified;
					await _context.SaveChangesAsync();
				}

			}


			return true;

		}

		#endregion

		#region Advance Payment

		public async Task<bool> SaveAdvancePayment(AdvancePayment advancePayment)
		{
			if (advancePayment.Id != 0)
				_context.AdvancePayments.Update(advancePayment);
			else
				_context.AdvancePayments.Add(advancePayment);
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<AdvancePayment>> GetAllAdvancePayment()
		{
			return await _context.AdvancePayments.Include(x => x.advanceAdjustment).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<AdvancePayment>> GetAdvancePaymentBysalaryPeriodId(int salaryPeriodId)
		{
			return await _context.AdvancePayments.Include(x => x.advanceAdjustment).Where(x => x.salaryPeriodId == salaryPeriodId).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<AdvancePayment>> GetAdvancePaymentByadvanceAdjustmentId(int advanceAdjustmentId)
		{
			return await _context.AdvancePayments.Include(x => x.advanceAdjustment).Where(x => x.advanceAdjustmentId == advanceAdjustmentId).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<AdvancePayment>> GetAdvancePaymentByemployeeInfoId(int salaryPeriodId, int employeeInfoId)
		{
			return await _context.AdvancePayments.Include(x => x.advanceAdjustment).Where(x => x.salaryPeriodId == salaryPeriodId && x.advanceAdjustment.employeeInfoId == employeeInfoId).AsNoTracking().ToListAsync();
		}
		public async Task<AdvancePayment> GetAdvancePaymentById(int id)
		{
			return await _context.AdvancePayments.FindAsync(id);
		}

		public async Task<bool> DeleteAdvancePaymentBysalaryPeriodId(int salaryPeriodId)
		{
			_context.AdvancePayments.RemoveRange(_context.AdvancePayments.Where(x => x.salaryPeriodId == salaryPeriodId));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<bool> DeleteAdvancePaymentByemployeeInfoId(int employeeInfoId, int salaryPeriodId)
		{
			_context.AdvancePayments.RemoveRange(_context.AdvancePayments.Where(x => x.advanceAdjustment.employeeInfoId == employeeInfoId && x.salaryPeriodId == salaryPeriodId));
			return 1 == await _context.SaveChangesAsync();
		}
		#endregion

		#region reportformat
		public async Task<bool> SaveReportFormat(ReportFormat reportFormat)
		{
			if (reportFormat.Id != 0)
				_context.ReportFormats.Update(reportFormat);
			else
				_context.ReportFormats.Add(reportFormat);
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<ReportFormat>> GetReportFormat()
		{
			return await _context.ReportFormats.AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<ReportFormat>> GetReportFormatByReportType(string reportType)
		{
			return await _context.ReportFormats.Where(a => a.reportTypeName == reportType).ToListAsync();
		}

		public async Task<bool> DeleteformatById(string reportType)
		{
			_context.ReportFormats.Remove(_context.ReportFormats.Where(x => x.reportTypeName == reportType).FirstOrDefault());
			return 1 == await _context.SaveChangesAsync();
		}
		#endregion

		#region salaryActivityPerchect
		public async Task<bool> SavesalaryActivityPercent(SalaryActivityPercent salaryActivityPercent)
		{
			if (salaryActivityPercent.Id != 0)
				_context.salaryActivityPercents.Update(salaryActivityPercent);
			else
				_context.salaryActivityPercents.Add(salaryActivityPercent);
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<SalaryActivityPercent>> GetsalaryActivityPercent()
		{
			return await _context.salaryActivityPercents.Include(x => x.employeeInfo).Include(x => x.employeeProjectActivity.hRDoner).Include(x => x.employeeProjectActivity.hRProject).Include(x => x.employeeProjectActivity.hRActivity).AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<SalaryActivityPercent>> GetsalaryActivityPercentByEmpId(int empId)
		{


			return await _context.salaryActivityPercents.Where(a => a.employeeInfoId == empId).ToListAsync();
		}

		public async Task<bool> DeletesalaryActivityPercentById(int empId)
		{
			_context.salaryActivityPercents.Remove(_context.salaryActivityPercents.Where(x => x.Id == empId).FirstOrDefault());
			return 1 == await _context.SaveChangesAsync();
		}
		#endregion

		#region salaryActivityPerchect
		public async Task<bool> SaveCodeManagement(CodeManagement codeManagement)
		{
			if (codeManagement.Id != 0)
				_context.CodeManagements.Update(codeManagement);
			else
				_context.CodeManagements.Add(codeManagement);
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<CodeManagement>> GetCodeManagement()
		{
			return await _context.CodeManagements.Include(x => x.employeeInfo).Include(x => x.salaryHead).Include(x => x.salaryPeriod).AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<CodeManagement>> GetCodeManagementByEmpPeriodId(int empId, int PeriodId)
		{


			return await _context.CodeManagements.Where(a => a.employeeInfoId == empId && a.salaryPeriodId == PeriodId).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<CodeManagement>> GetCodeManagementByPeriodId(int PeriodId)
		{


			return await _context.CodeManagements.Where(a => a.salaryPeriodId == PeriodId).AsNoTracking().ToListAsync();
		}

		public async Task<bool> DeleteCodeManagementsByEmpPeriodId(int empId, int PeriodId)
		{
			_context.CodeManagements.RemoveRange(_context.CodeManagements.Where(x => x.Id == empId && x.salaryPeriodId == PeriodId).AsNoTracking().ToList());
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<bool> DeleteCodeManagementsByPeriodId(int PeriodId)
		{
			_context.CodeManagements.RemoveRange(_context.CodeManagements.Where(x => x.salaryPeriodId == PeriodId).AsNoTracking().ToList());
			return 1 == await _context.SaveChangesAsync();
		}
		#endregion

		#region Monthly Allowance

		public async Task<bool> SaveMonthlyAllowance(MonthlyAllowance monthlyAllowance)
		{
			if (monthlyAllowance.Id != 0)
				_context.MonthlyAllowances.Update(monthlyAllowance);
			else
				_context.MonthlyAllowances.Add(monthlyAllowance);
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<MonthlyAllowance>> GetAllMonthlyAllowance()
		{
			return await _context.MonthlyAllowances.Include(x => x.employeeInfo).Include(x => x.salaryPeriod).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<MonthlyAllowance>> GetMonthlyAllowanceBysalaryPeriodId(int salaryPeriodId)
		{
			return await _context.MonthlyAllowances.Include(x => x.employeeInfo).Include(x => x.salaryPeriod).Where(x => x.salaryPeriodId <= salaryPeriodId).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<MonthlyAllowance>> GetMonthlyAllowanceByemployeeInfoId(int salaryPeriodId, int employeeInfoId)
		{
			return await _context.MonthlyAllowances.Include(x => x.employeeInfo).Include(x => x.salaryPeriod).Where(x => x.salaryPeriodId <= salaryPeriodId && x.employeeInfoId == employeeInfoId).AsNoTracking().ToListAsync();
		}
		public async Task<MonthlyAllowance> GetMonthlyAllowanceById(int id)
		{
			return await _context.MonthlyAllowances.FindAsync(id);
		}

		public async Task<bool> DeleteMonthlyAllowanceById(int id)
		{
			_context.MonthlyAllowances.Remove(_context.MonthlyAllowances.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<MealChargeViewModel>> GetMealChargeByPeriod(int salaryPeriodId)
		{
			return await _context.mealChargeViewModels.FromSql($"SP_GETMonthlyMealCharge {salaryPeriodId}").AsNoTracking().ToListAsync();
		}
		#endregion

		#region Attendance Summary

		public async Task<int> SaveAttendanceSummary(EmpAttendanceSummary empAttendanceSummary)
		{
			if (empAttendanceSummary.Id != 0)
				_context.EmpAttendanceSummaries.Update(empAttendanceSummary);
			else
				_context.EmpAttendanceSummaries.Add(empAttendanceSummary);
			await _context.SaveChangesAsync();
			return empAttendanceSummary.Id;
		}

		public async Task<IEnumerable<EmpAttendanceSummary>> GetAttendanceSummary()
		{
			return await _context.EmpAttendanceSummaries.Include(x => x.employeeInfo).Include(x => x.salaryPeriod).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<EmpAttendanceSummary>> GetAttendanceSummaryBysalaryPeriodId(int salaryPeriodId)
		{
			return await _context.EmpAttendanceSummaries.Include(x => x.employeeInfo).Include(x => x.salaryPeriod).Where(x => x.salaryPeriodId <= salaryPeriodId).AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<EmpAttendanceSummary>> GetDuplicateAttendanceSummary(int id, int salaryPeriodId, int employeeInfoId)
		{
			return await _context.EmpAttendanceSummaries.Include(x => x.employeeInfo).Include(x => x.salaryPeriod).Where(x => x.Id != id && x.salaryPeriodId == salaryPeriodId && x.employeeInfoId == employeeInfoId).AsNoTracking().ToListAsync();
		}

		public async Task<EmpAttendanceSummary> GetAttendanceSummaryById(int id)
		{
			return await _context.EmpAttendanceSummaries.FindAsync(id);
		}

		public async Task<IEnumerable<Areas.HRPMSAttendence.Models.AttendanceSummaryViewModel>> GetAttendanceSummaryByPeriod(int? id, int? salaryPeriodId)
		{
			return await _context.attendanceSummaryViewModels.FromSql($"SP_GETAttendanceSummary {id},{salaryPeriodId}").AsNoTracking().ToListAsync();
		}
		
		//public async Task<IEnumerable<ManualAttendanceViewModel>> GetManualAttendanceByAnyKey(string employeeCode, int? approverId, string fromDate, string toDate)
		//{
		//	return await _context.ManualAttendanceViewModels.FromSql($"SP_GetAttendanceByAnyKey {employeeCode},{approverId}, {fromDate}, {toDate}").AsNoTracking().ToListAsync();
		//}

		public async Task<bool> DeleteAttendanceSummaryById(int id)
		{
			_context.EmpAttendanceSummaries.Remove(_context.EmpAttendanceSummaries.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		#endregion

		#region ForfeitAccount
		public async Task<bool> SaveForfeitAccount(ForfeitAccount forfeitAccount)
		{
			if (forfeitAccount.Id != 0)
			{
				_context.forfeitAccounts.Update(forfeitAccount);
			}
			else
			{
				_context.forfeitAccounts.Add(forfeitAccount);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<ForfeitAccount>> GetAllForfeitAccount()
		{
			return await _context.forfeitAccounts.Include(x => x.pf).Include(x => x.employee).AsNoTracking().ToListAsync();
		}

		public async Task<ForfeitAccount> GetForfeitAccountById(int id)
		{
			return await _context.forfeitAccounts.FindAsync(id);
		}

		public async Task<bool> DeleteForfeitAccountById(int id)
		{
			_context.forfeitAccounts.Remove(_context.forfeitAccounts.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<PFMemberInfo> GetPFMemberInfoByEmployeeId(int empId)
		{
			return await _context.pfMemberInfos.Where(x => x.employeeInfoId == empId).FirstOrDefaultAsync();
		}
		#endregion
		#region FinalSettlement
		public async Task<bool> SavepfFinalSettleMent(PfFinalSettlement pfFinalSettlement)
		{
			if (pfFinalSettlement.Id != 0)
			{
				_context.pfFinalSettlements.Update(pfFinalSettlement);
			}
			else
			{
				_context.pfFinalSettlements.Add(pfFinalSettlement);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<PfFinalSettlement>> GetAllpfFinalSettlement()
		{
			return await _context.pfFinalSettlements.Include(x => x.pfMember).AsNoTracking().ToListAsync();
		}

		public async Task<PfFinalSettlement> GetpfFinalSettlementById(int id)
		{
			return await _context.pfFinalSettlements.FindAsync(id);
		}

		public async Task<bool> DeletepfFinalSettlementById(int id)
		{
			_context.pfFinalSettlements.Remove(_context.pfFinalSettlements.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}


		#endregion


		#region masterData
		public async Task<IEnumerable<HrBranch>> GetAllHrBranchs()
		{
			return await _context.hrBranches.AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<Department>> GetAllDepartments()
		{
			return await _context.departments.AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<FunctionInfo>> GetAllOffices()
		{
			return await _context.FunctionInfos.AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<HrUnit>> GetAllHrUnits()
		{
			return await _context.hrUnits.AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<Location>> GetAllZones()
		{
			return await _context.Locations.AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<SpecialBranchUnit>> GetAllSbus()
		{
			return await _context.SpecialBranchUnits.AsNoTracking().ToListAsync();
		}
		#endregion

		#region StaffLoan
		public async Task<IEnumerable<StaffLoan>> GetAllStaffLoans()
		{
			return await _context.staffLoans.AsNoTracking().ToListAsync();
		}
		public async Task<DateTime?> GetPRLDateByEmpId(int empId)
		{
			var data = await _context.employeeInfos.Where(x => x.Id == empId).AsNoTracking().Select(x => x.prlDate).FirstOrDefaultAsync();
			return data;
		}
		public async Task<int> SaveStaffLoan(StaffLoan model)
		{
			if (model.Id != 0)
			{
				_context.staffLoans.Update(model);
			}
			else
			{
				_context.staffLoans.Add(model);
			}
			await _context.SaveChangesAsync();
			return model.Id;
		}

		public async Task<CheckLoanDedVm> RecoverFromPrincipalOrInterestExpSelf(string transactionNo, int loanId)
		{
			var result = await _context.checkLoanDedVms.FromSql($"sp_CheckLoanDeductionExSelf {transactionNo}, {loanId}").AsNoTracking().FirstOrDefaultAsync();
			return result;
		}
		//aaa
		public async Task<List<StaffLoanLog>> GetStaffLoanLogByTransactionNo(string transactionNo)
		{
			var data = await _context.staffLoanLogs.Where(x => x.trxNo == transactionNo).AsNoTracking().ToListAsync();
			return data;
		}

		public async Task<CheckLoanDedVm> RecoverFromPrincipalOrInterest(int loanId)
		{
			var result = await _context.checkLoanDedVms.FromSql($"sp_CheckLoanDeduction {loanId}").AsNoTracking().FirstOrDefaultAsync();
			return result;
		}

		public async Task<StaffLoanLog> GetStaffLoanLogByLoanId(int loanId)
		{
			var data = await _context.staffLoanLogs.Where(x => x.staffLoanId == loanId && x.particular == "Opening Balance").FirstOrDefaultAsync();
			return data;
		}

		public async Task<int> DeleteStaffLoanLogById(int id)
		{
			try
			{
				var obj = _context.staffLoanLogs.Where(a => a.Id == id).FirstOrDefault();
				_context.Remove(obj);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return id;
		}
		
		public async Task<int> SaveStaffLoanLog(StaffLoanLog model)
		{
			
			try
			{
				if (model.Id != 0)
				{
					_context.staffLoanLogs.Update(model);
				}
				else
				{
					_context.staffLoanLogs.Add(model);
				}
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{

				throw;
			}
			return model.Id;
		}

		public async Task<IEnumerable<StaffLoan>> GetAvailableLoans(string empcode)
		{
			var data = await _context.staffLoans.Where(x => x.empCode == empcode).ToListAsync();
			return data;
		}

		public async Task<int> SaveStaffLoanErrors(StaffLoanUploadFailed model)
		{
			if (model.Id != 0)
			{
				_context.staffLoanUploadFaileds.Update(model);
			}
			else
			{
				_context.staffLoanUploadFaileds.Add(model);
			}
			await _context.SaveChangesAsync();
			return model.Id;
		}
		public async Task<int> saveUploadLoanExcelLog(UploadExcelLog model)
		{
			if (model.Id != 0)
			{
				_context.uploadExcelLogs.Update(model);
			}
			else
			{
				_context.uploadExcelLogs.Add(model);
			}
			await _context.SaveChangesAsync();
			return model.Id;
		}

		public async Task<int> DeleteStaffLoanLogsByLoanId(int id)
		{
			var logs = await _context.staffLoanLogs.Where(x => x.staffLoanId == id).ToListAsync();
			foreach (var item in logs)
			{
				_context.staffLoanLogs.Remove(item);
				await _context.SaveChangesAsync();
			}
			return 1;
		}

		public async Task<int> DeleteStaffLoanByLoanId(int id)
		{
			var log = await _context.staffLoanLogs.FindAsync(id);
			_context.staffLoanLogs.Remove(log);
			return await _context.SaveChangesAsync();
		}
		public async Task<IEnumerable<StaffLoan>> GetAllStaffLoansByDate(DateTime? fromDate, DateTime toDate)
		{
			try
			{
				var data = await _context.staffLoans.Where(x => Convert.ToDateTime(x.loanDate).Date >= Convert.ToDateTime(fromDate).Date && Convert.ToDateTime(x.loanDate).Date <= Convert.ToDateTime(toDate).Date).AsNoTracking().ToListAsync();
				return data;
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		//AsadAdded On 18.08.2023
		public async Task<StaffLoanLog> GetStaffLoanLogById(int id)
		{
			try
			{
				var data = await _context.staffLoanLogs.Include(x => x.staffLoan).Include(x => x.staffLoan.employeeInfo).Include(x => x.staffLoan.employeeInfo.lastDesignation).Where(x => x.Id == id).OrderBy(x=> x.effectiveDate).AsNoTracking().SingleOrDefaultAsync();
				return data;
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public async Task<IEnumerable<StaffLoanLog>> GetStaffLoansLogAllByDate(DateTime? fromDate, DateTime toDate)
		{
			try
			{
				var data = await _context.staffLoanLogs.Include(x => x.staffLoan).Include(x => x.staffLoan.employeeInfo).Include(x => x.staffLoan.employeeInfo.lastDesignation).Where(x => Convert.ToDateTime(x.createdAt).Date >= Convert.ToDateTime(fromDate).Date && Convert.ToDateTime(x.createdAt).Date <= Convert.ToDateTime(toDate).Date).AsNoTracking().ToListAsync();
				return data;
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		//Asad added on 18082023
		public async Task<IEnumerable<StaffLoanLog>> GetStaffLoanLogByTrxNo(string trxNo)
		{
			var data = await _context.staffLoanLogs.Where(x => x.type != null && x.trxNo == trxNo).Include(x => x.staffLoan).Include(x => x.staffLoan.employeeInfo).Include(x => x.staffLoan.employeeInfo.lastDesignation).OrderBy(x=>x.effectiveDate).AsNoTracking().ToListAsync();
			return data;
		}

		//Asad Added on 07.08.2023
		public async Task<IEnumerable<StaffLoanLog>> GetStaffLoanLog()
		{
			var data = await _context.staffLoanLogs.Where(x => x.type != null).Include(x => x.staffLoan).Include(x => x.staffLoan.employeeInfo).Include(x => x.staffLoan.employeeInfo.lastDesignation).AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<StaffLoanLog>> GetAllStaffLoansLog()
        {
            var data = await _context.staffLoanLogs.Include(x => x.staffLoan).Include(x => x.staffLoan.employeeInfo).Include(x => x.staffLoan.employeeInfo.lastDesignation).AsNoTracking().ToListAsync();
            return data;
        }

        #endregion

        #region SalarySummarySheet

        public async Task<IEnumerable<SalarySummarySheetSpViewModel>> GetSalarySummarySheetHeadOffice(int salaryPeriodId)
		{
			List<SalarySummarySheetSpViewModel> result = await _context.salarySummarySheetSpViewModels.FromSql($"sp_SalarySummarySheetHeadOffice {salaryPeriodId}").AsNoTracking().ToListAsync();
			return result;
		}

		public async Task<GenerateTrxNumberForLoan> GenerateLoanTransactionNo(string type, string loanName)
		{
			var result = await _context.generateTrxNumberForLoans.FromSql($"sp_GenerateLoanTransactionNo {type}, {loanName}").AsNoTracking().FirstOrDefaultAsync();
			return result;
		}

		public async Task<string> GetLoanNameById(int id)
		{
			var data = await _context.staffLoans.Where(x => x.Id == id).AsNoTracking().Select(x => x.loanType).FirstOrDefaultAsync();
			return data;
		}

		public async Task<IEnumerable<BranchSalarySummaryViewModel>> GetSalarySummarySheetBranch(int salaryPeriodId)
		{
			List<BranchSalarySummaryViewModel> result = await _context.branchSalarySummaryViewModels.FromSql($"sp_SalarySummarySheet {salaryPeriodId}").AsNoTracking().ToListAsync();
			return result;
		}
		public async Task<IEnumerable<BranchSalarySummaryViewModel>> GetYearlySalarySummarySheetBranch(string fromDate, string toDate)
		{
			List<BranchSalarySummaryViewModel> result = await _context.branchSalarySummaryViewModels.FromSql($"[sp_SalaryYearlySummarySheet] {fromDate}, {toDate}").AsNoTracking().ToListAsync();
			return result;
		}

		public async Task<IEnumerable<BranchSalarySummaryViewModel>> GetSalarySummarySheetZone(int salaryPeriodId)
		{
			List<BranchSalarySummaryViewModel> result = await _context.branchSalarySummaryViewModels.FromSql($"sp_SalarySummarySheetZone {salaryPeriodId}").AsNoTracking().ToListAsync();
			return result;
		}

		public async Task<LoanNumberGenerate> GetStaffLoanNo(string empCode, string loanType)
		{
			var result = await _context.loanNumberGenerates.FromSql($"sp_GenerateLoanNumber {empCode}, {loanType}").AsNoTracking().FirstOrDefaultAsync();
			return result;
		}

		public async Task<StaffLoan> GetStaffLoanByEmpIdAndType(int empId, string type)
		{
			var data = await _context.staffLoans.Where(x => x.employeeInfoId == empId && x.loanType == type && x.isDelete != 1).AsNoTracking().FirstOrDefaultAsync();
			return data;
		}

		public async Task<LoanNumberGenerate> DeleteStaffLoanMasterDetail(int loanId)
		{
			var result = await _context.loanNumberGenerates.FromSql($"sp_DeleteStaffLoan {loanId}").AsNoTracking().FirstOrDefaultAsync();
			return result;
		}
        //Asad added on 27.07.2023
        public async Task<LoanPaid> IsFullPaid(int loanId)
        {
            var result = await _context.LoanPaid.FromSql($"SP_IsFullPaid {loanId}").AsNoTracking().FirstOrDefaultAsync();
            return result;
        }
        //Asad added on 27.07.2023
        public async Task<LoanPaid> CloseFullPaidStaffLoan(int loanId)
        {
            var result = await _context.LoanPaid.FromSql($"SP_CloseFullPaidStaffLoan {loanId}").AsNoTracking().FirstOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<UploadExcelLog>> GetAllExcelUploadLogsByType(string type)
		{
			var data = await _context.uploadExcelLogs.AsNoTracking().Where(x => x.type == type).ToListAsync();
			return data;
		}
		public async Task<IEnumerable<FilterStaffLoanViewModel>> FilterStaffLoan(int branchId, int zoneId, int empId, string loanType)
		{
			try
			{
				var result = await _context.filterStaffLoan.FromSql($"sp_FilterStaffLoan {branchId}, {zoneId} ,{empId},{loanType}").AsNoTracking().ToListAsync();
				return result;
			}
			catch (Exception ex)
			{

				return new List<FilterStaffLoanViewModel>();
			}

		}
		public async Task<IEnumerable<LoanNumberGenerate>> DeleteExistingStaffLoan(int empId, string loanType, string CBSAc, string LoanAc)
		{
			var result = await _context.loanNumberGenerates.FromSql($"sp_RemoveStaffLoanPrev {empId}, {loanType}, {CBSAc}, {LoanAc}").AsNoTracking().ToListAsync();
			return result;
		}
		public async Task<IEnumerable<SalaryLockVm>> PopulateBranchSalaryLock(int periodId)
		{
			var result = await _context.salaryLockVms.FromSql($"sp_PopulateBranchSalaryLock {periodId}").AsNoTracking().ToListAsync();
			return result;
		}
		public async Task<IEnumerable<SalaryLockWithStatusVm>> GetAllSalaryLocksWithStatus()
		{
			var result = await _context.salaryLockWithStatusVms.FromSql($"sp_GetAllSalaryLockWithStatus").AsNoTracking().ToListAsync();
			return result;
		}
		public async Task<IEnumerable<SalaryLockWithStatusVm>> GetAllBadgeLock(int periodId, string type)
		{
			var result = await _context.salaryLockWithStatusVms.FromSql($"sp_GetAllBadgeLockByType {periodId}, {type}").AsNoTracking().ToListAsync();
			return result;
		}
		public async Task<SalaryLockVm> UpdateBadgeLock(int periodId, string type, int status)
		{
			var result = await _context.salaryLockVms.FromSql($"sp_BadgeLock {periodId}, {type}, {status}").AsNoTracking().FirstOrDefaultAsync();
			return result;
		}


		#endregion


		public async Task<IEnumerable<ReconciallationVm>> GetReconcillationData(string particular, string fDate, string tDate)
		{
			return await _context.reconciallations.FromSql($"SP_GetReconcilliationReport {particular}, {fDate}, {tDate}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<QuarterlyVm>> GetQuarterlyReport(string type, string fDate, string tDate)
		{
			return await _context.quarterlyVms.FromSql($"sp_GetQuarterlyReport {fDate}, {tDate}, {type}").AsNoTracking().ToListAsync();
		}

        //Asad added on 08.06.2023
        public async Task<IEnumerable<StaffLoanInfoVm>> GetNewLoans(int salaryPeriodId)
        {
            try
            {
                var data = await _context.staffLoanInfo.FromSql($"SP_GET_NEW_LOANS {salaryPeriodId}").AsNoTracking().ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<ResolveAllVm> GetNumberOfNewLoans(int salaryPeriodId)
        {
            return await _context.resolveAllVms.FromSql($"SP_GET_NUMBER_OF_NEW_LOANS {salaryPeriodId}").AsNoTracking().FirstOrDefaultAsync();
        }

        //Added by Asad on 13.06.2023
        public async Task<IEnumerable<IndividualLoanSummaryVm>> GetIndividualLoanSummaryByChargeDate(string chargeDate)
        {
            return await _context.IndividualLoanSummaryVms.FromSql($"SP_GetIndLoanSummaryByChargeDate {chargeDate}").AsNoTracking().ToListAsync();
        }
        //Asad added on 15.06.2023
        public async Task<IEnumerable<VoucherVm>>  GetVoucherByParticular(string particular, string startDate, string endDate)
        {
            return await _context.VoucherVms.FromSql($"GetVoucherByParticular {particular}, {startDate}, {endDate}").AsNoTracking().ToListAsync();
        }

        #region PF
        public async Task<ResponseObject> CollectPayrollLoanEmiByPeriodId(int salaryPeriodId, string createBy)
        {

            var result = new ExecuteNoneQuery();

            try
            {
                result = await _context.ExecuteNoneQuery.FromSql($"CollectPayrollLoanEmi {salaryPeriodId}, {createBy}").AsNoTracking().FirstOrDefaultAsync();  //.ToListAsync()
            }
            catch (Exception exp)
            {

            }


            string message = string.Empty;
            string status = string.Empty;

            if (result != null)
            {
                message = result.Message;
                status = result.Status;
            }
            else
            {
                message = "Unable to Collect ACR from Payroll.";
                status = "NOK";
            }
            return new ResponseObject { Status = status, Message = message };
        }

        public async Task<IEnumerable<PayrollLoanEmiViewModel>> GetPayrollLoanEmiByPeriodId(int salaryPeriodId)
        {
            var data = await _context.PayrollLoanEmiViewModels.FromSql($"SP_GetPayrollLoanEmi {salaryPeriodId}").AsNoTracking().ToListAsync();  //.ToListAsync()

            return data;
        }
        public async Task<ResponseObject> GenerateVoucherOnPayrollLoanEmiByPeriodId(int salaryPeriodId, string createBy)
        {
            string vDate = DateTime.Now.ToString();
            var result = new ExecuteNoneQuery();

            try
            {
                result = await _context.ExecuteNoneQuery.FromSql($"GenerateVoucherOnPayrollLoanEmi {salaryPeriodId}, {vDate}, {createBy}").AsNoTracking().FirstOrDefaultAsync();  //.ToListAsync()
            }
            catch (Exception exp)
            {

            }


            string message = string.Empty;
            string status = string.Empty;

            if (result != null)
            {
                message = result.Message;
                status = result.Status;
            }
            else
            {
                message = "Unable to Generate Voucher from Payroll.";
                status = "NOK";
            }
            return new ResponseObject { Status = status, Message = message };
        }

        #endregion

    }
}