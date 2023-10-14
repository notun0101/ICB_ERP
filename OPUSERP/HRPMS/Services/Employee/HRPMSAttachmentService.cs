using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee
{
    public class HRPMSAttachmentService: IHRPMSAttachmentService
    {
        private readonly ERPDbContext _context;

        public HRPMSAttachmentService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteHRPMSAttachmentById(int id)
        {
            _context.hRPMSAttachments.Remove(_context.hRPMSAttachments.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HRPMSAttachment>> GetHRPMSAttachment()
        {
            return await _context.hRPMSAttachments.Include(x => x.employee).Include(x=>x.atttachmentCategory).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<HRPMSAttachment>> GetHRPMSAttachmentByFile()
        {
            return await _context.hRPMSAttachments.Include(x=>x.employee).Include(x=>x.atttachmentCategory).Where(x => x.fileUrl != null ).AsNoTracking().ToListAsync();
        }

    

        public async Task<IEnumerable<HRPMSAttachment>> GetHRPMSAttachmentByEmpId(int empId)
        {
            return await _context.hRPMSAttachments.Where(x => x.employeeId == empId).Include(x => x.employee).Include(x => x.atttachmentCategory).Include(x => x.atttachmentCategory.atttachmentGroup).AsNoTracking().ToListAsync();
        }

        public async Task<HRPMSAttachment> GetHRPMSAttachmentById(int id)
        {
            return await _context.hRPMSAttachments.FindAsync(id);
        }

        public async Task<int> SaveHRPMSAttachment(HRPMSAttachment hRPMSAttachment)
        {
            if (hRPMSAttachment.Id != 0)
                _context.hRPMSAttachments.Update(hRPMSAttachment);
            else
                _context.hRPMSAttachments.Add(hRPMSAttachment);
                await _context.SaveChangesAsync();
            return hRPMSAttachment.Id;

        }

        public async Task<string> GetAttachmentUrlById(int id)
        {
            return await _context.hRPMSAttachments.Where(x => x.Id == id).Select(x => x.fileUrl).FirstOrDefaultAsync();
        }
        public async Task<string> GetAttachmentUrlByIdcv(int id)
        {
            return await _context.cvBlackLists.Where(x => x.Id == id).Select(x => x.attatchment).FirstOrDefaultAsync();
        }
    }
}
