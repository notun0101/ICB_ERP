using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee
{
    public class MobileBenifitService : IMobileBenifitService
    {
        private readonly ERPDbContext _context;

        public MobileBenifitService(ERPDbContext context)
        {
            _context = context;
        }

        #region MobileBenifit
        public async Task<bool> DeleteMobileBenifitById(int id)
        {
            _context.mobileBenifits.Remove(_context.mobileBenifits.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<int> SaveMobileBenifit(MobileBenifit mobileBenifit)
        {
            if (mobileBenifit.Id != 0)
                _context.mobileBenifits.Update(mobileBenifit);
            else
                _context.mobileBenifits.Add(mobileBenifit);
            await _context.SaveChangesAsync();
            return mobileBenifit.Id;
        }

        public async Task<IEnumerable<MobileBenifit>> GetMobileBenifit()
        {
            return await _context.mobileBenifits.AsNoTracking().ToListAsync();
        }


        public async Task<IEnumerable<MobileBenifit>> GetMobileBenifitByEmpId(int empId)
        {
            return await _context.mobileBenifits.Include(x => x.employee).Where(x => x.employeeId == empId).AsNoTracking().ToListAsync();

        }

        public async Task<MobileBenifit> GetMobileBenifitById(int id)
        {
            var data = await _context.mobileBenifits.Where(x => x.Id == id).FirstOrDefaultAsync();
            return data;
        }


        #endregion


    }
}
