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
    public class PhotographService : IPhotographService
    {
        private readonly ERPDbContext _context;

        public PhotographService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeletePhotographById(int id)
        {
            _context.photographs.Remove(_context.photographs.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }


        public async Task<Photograph> GetPhotographByEmpIdAndType(int empId, string type)
        {
            return await _context.photographs.Where(x => x.type == type && x.employeeId == empId).FirstOrDefaultAsync();
        }

        public async Task<Photograph> GetPhotographByEmpId(int empId)
        {
            return await _context.photographs.Where(x => x.employeeId == empId).FirstOrDefaultAsync();
        }

        public async Task<Photograph> GetPhotographByEmpIdNew(int empId)
        {
            var data=await _context.photographs.Where(x => x.Id == empId).FirstOrDefaultAsync();
            return data;
        }

         public async Task<TransferLog> GetTransferAttachmentByEmpId(int id)
        {
            return await _context.transferLogs.Where(x=>x.employeeId == id).FirstOrDefaultAsync();
        }

        public async Task<Photograph> GetPhotographByEmpCodeAndType(string code, string type)
        {
            return await _context.photographs.Where(x => x.type == type && x.employee.employeeCode == code).FirstOrDefaultAsync();
        }

        public async Task<Photograph> GetPhotographById(int id)
        {
            return await _context.photographs.FindAsync(id);
        }

        public async Task<IEnumerable<Photograph>> GetPhotographs()
        {
            return await _context.photographs.AsNoTracking().ToListAsync();
        }

        public async Task<int> SavePhotograph(Photograph photograph)
        {
            try
            {
                if (photograph.Id != 0)
                    _context.photographs.Update(photograph);
                else
                    _context.photographs.Add(photograph);
                await _context.SaveChangesAsync();
                return photograph.Id;
            }
            catch (System.Exception ex)
            {

                throw;
            }
            //if (photograph.Id != 0)
            //    _context.photographs.Update(photograph);
            //else
            //    _context.photographs.Add(photograph);

            //return 1 == await _context.SaveChangesAsync();

        }

        public async Task<int> SaveTransferAttachment(TransferLog transferLog)
        {

            if (transferLog.Id != 0)
                _context.transferLogs.Update(transferLog);
            else
                _context.transferLogs.Add(transferLog);
            await _context.SaveChangesAsync();
            return transferLog.Id;



        }

        public int UpdatePostingPlace(int empId, DateTime? fromDate)
        {
            _context.Database.ExecuteSqlCommand($"sp_UpdatePosting {empId}, {fromDate}");
            return 1;
        }



        public async Task<bool> DeleteEmployeeSignatureById(int id)
        {
            _context.employeeSignatures.Remove(_context.employeeSignatures.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<EmployeeSignature> GetEmployeeSignatureByEmpId(int empId)
        {
            var data = await _context.employeeSignatures.Where(x => x.employeeId == empId).FirstOrDefaultAsync();
            return data;
        }
        public async Task<EmployeeSignature> GetEmployeeSignatureByEmpCode(string code)
        {
            var data = await _context.employeeSignatures.Where(x => x.employee.employeeCode == code).FirstOrDefaultAsync();
            return data;
        }

        public async Task<EmployeeSignature> GetEmployeeSignatureById(int id)
        {
            return await _context.employeeSignatures.FindAsync(id);
        }

        public async Task<IEnumerable<EmployeeSignature>> GetEmployeeSignature()
        {
            return await _context.employeeSignatures.AsNoTracking().ToListAsync();
        }

        //public async Task<bool> SaveEmployeeSignature(EmployeeSignature photograph)
        //{
        //    if (photograph.Id != 0)
        //        _context.employeeSignatures.Update(photograph);
        //    else
        //        _context.employeeSignatures.Add(photograph);

        //    return 1 == await _context.SaveChangesAsync();
        //}
        public async Task<int> SaveEmployeeSignature(EmployeeSignature photograph)
        {
            if (photograph.Id != 0)
                _context.employeeSignatures.Update(photograph);
            else
                _context.employeeSignatures.Add(photograph);
            await _context.SaveChangesAsync();
            return photograph.Id;
        }
       

        public async Task<EmployeeSignature> GetEmpSignatureById(int id)
        {
            var data = await _context.employeeSignatures.Where(x => x.Id == id).FirstOrDefaultAsync();
            return data;
        }




        public async Task<EmployeeSignature> GetEmpSignatureByEmployeeId(int id)
        {
            return await _context.employeeSignatures.Where(x => x.employeeId == id).FirstOrDefaultAsync();
        }
    }
}
