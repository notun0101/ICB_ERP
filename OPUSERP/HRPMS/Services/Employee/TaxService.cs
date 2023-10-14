using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee
{
    public class TaxService : ITaxService
    {
        private readonly ERPDbContext _context;

        public TaxService(ERPDbContext context)
        {
            _context = context;
        }

        #region Tax
        public async Task<bool> DeleteTaxById(int id)
        {
            _context.taxes.Remove(_context.taxes.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<int> SaveTax(Tax tax)
        {
            if (tax.Id != 0)
                _context.taxes.Update(tax);
            else
                _context.taxes.Add(tax);
            await _context.SaveChangesAsync();
            return tax.Id;
        }

        public async Task<IEnumerable<Tax>> GetTax()
        {
            return await _context.taxes.AsNoTracking().ToListAsync();
        }


        public async Task<IEnumerable<Tax>> GetTaxByEmpId(int empId)
        {
            return await _context.taxes.Include(x => x.employee).Where(x => x.employeeId == empId).AsNoTracking().ToListAsync();

        }
        public async Task<IEnumerable<EmployeePostingPlace>> GetEmpPostingByEmpId(int empId)
        {
            return await _context.employeePostings.Include(x => x.employee).Where(x => x.employeeId == empId).Include(x => x.branch).Include(x=> x.department).Include(x=> x.hrDivision).Include(x=> x.office).Include(x=>x.location).Include(x=> x.hrUnit).Include(x=> x.hrBranch).AsNoTracking().ToListAsync();

        }
        public async Task<EmployeePostingPlace> GetEmpPostingById(int id)
        {
            return await _context.employeePostings.FindAsync(id);

        }
        public async Task<int> SavePosting(EmployeePostingPlace employeePosting)
        {
            if (employeePosting.Id != 0)
                _context.employeePostings.Update(employeePosting);
            else
                _context.employeePostings.Add(employeePosting);
            await _context.SaveChangesAsync();
            return employeePosting.Id;
        }
        public async Task<int> DeletePostingById(int id)
        {
            _context.employeePostings.Remove(_context.employeePostings.Find(id));
            return await _context.SaveChangesAsync();
        }

        public async Task<Tax> GetTaxById(int id)
        {
            var data = await _context.taxes.Where(x => x.Id == id).FirstOrDefaultAsync();
            return data;
        }


        #endregion

        #region DuelResidence


        public async Task<bool> DeleteDuelResidenceById(int id)
        {
            _context.duelResidences.Remove(_context.duelResidences.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<int> SaveDuelResidence(DuelResidence duelResidence)
        {
            if (duelResidence.Id != 0)
                _context.duelResidences.Update(duelResidence);
            else
                _context.duelResidences.Add(duelResidence);
            await _context.SaveChangesAsync();
            return duelResidence.Id;
        }

        public async Task<IEnumerable<DuelResidence>> GetDuelResidence()
        {
            return await _context.duelResidences.Include(x => x.employee).Include(x => x.duelCountry).AsNoTracking().ToListAsync();
        }


        public async Task<IEnumerable<DuelResidence>> GetDuelResidenceByEmpId(int empId)
        {
            return await _context.duelResidences.Include(x => x.employee).Include(x => x.duelCountry).Where(x => x.employeeId == empId).AsNoTracking().ToListAsync();

        }

        public async Task<DuelResidence> GetDuelResidenceById(int id)
        {
            var data = await _context.duelResidences.Where(x => x.Id == id).FirstOrDefaultAsync();
            return data;
        }

       
        #endregion

    }
}
