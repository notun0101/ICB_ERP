using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Suspensions;
using OPUSERP.HRPMS.Services.SuspensionReport.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.SuspensionReport
{
    public class SuspensionReportService:ISuspension
    {
        private readonly ERPDbContext _context;

        public SuspensionReportService(ERPDbContext context)
        {
            _context = context;
        }
        public async Task<int> SaveSuspension(Suspension suspension)
        {
            if (suspension.Id != 0)
                _context.suspensions.Update(suspension);
            else
                _context.suspensions.Add(suspension);

            await _context.SaveChangesAsync();
            return suspension.Id;
        }

        public async Task<IEnumerable<Suspension>> GetAllSuspension()
        {
            return await _context.suspensions.AsNoTracking().ToListAsync();
        }
        public async Task<string> GetsuspensionImgUrlById(int id)
        {
            return await _context.suspensions.Where(x => x.Id == id).Select(x => x.susFile).FirstOrDefaultAsync();
        }
        public async Task<string> GetChargeImgUrlById(int id)
        {
            return await _context.suspensions.Where(x => x.Id == id).Select(x => x.chargeSheetFile).FirstOrDefaultAsync();
        }
        public async Task<string> GetHearingImgUrlById(int id)
        {
            return await _context.suspensions.Where(x => x.Id == id).Select(x => x.hearingReportFile).FirstOrDefaultAsync();
        }
        public async Task<bool> DeleteSuspensionById(int id)
        {
            _context.suspensions.Remove(_context.suspensions.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }


        public async Task<Suspension> GetSuspensionById(int id)
        {
            return await _context.suspensions.Where(x => x.employeeId == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Suspension>> GetSuspensionsByEmployeeId(int id)
        {
            return await _context.suspensions.Where(x => x.employeeId == id).ToListAsync();
        }
        public async Task<Suspension> GetSuspensionsByEmpId(int id)
        {
            return await _context.suspensions.Where(x => x.employeeId == id).FirstOrDefaultAsync();
        }
        //public async Task<IEnumerable<Suspension>> GetSuspensionById(int empId)
        //{
        //    return await _context.suspensions.Include(x => x.employee).Where(x => x.employeeId == empId).AsNoTracking().ToListAsync();
        //}
    }
}
