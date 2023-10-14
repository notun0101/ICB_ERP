using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.IELTS;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee
{
    public class NomineeService : INomineeService
    {
        private readonly ERPDbContext _context;

        public NomineeService(ERPDbContext context)
        {
            _context = context;
        }

        #region Nominee

        public async Task<bool> DeleteNomineeById(int id)
        {
            _context.nominees.Remove(_context.nominees.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Nominee>> GetNominee()
        {
            return await _context.nominees.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Nominee>> GetNomineeByEmpId(int empId)
        {
            return await _context.nominees.Where(x => x.employeeID == empId).AsNoTracking().ToListAsync();
        }
        //Occupation,relation
        public async Task<IEnumerable<Occupation>> GetOccupation()
        {
            return await _context.occupations.AsNoTracking().OrderBy(x=>x.occupationName).ToListAsync();
        }
        public async Task<IEnumerable<Degree>> GetDegree()
        {
            return await _context.degrees.OrderBy(x => x.degreeName).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<LevelofEducation>> GetLevelofEducation()
        {
            return await _context.levelofEducations.OrderBy(x => x.levelofeducationName).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<Occupation>> GetFatherOccupation()
        {
            return await _context.occupations.AsNoTracking().OrderBy(x=>x.occupationName).ToListAsync();
        }
        public async Task<IEnumerable<Occupation>> GetMotherOccupation()
        {
            return await _context.occupations.AsNoTracking().OrderBy(x=>x.occupationName).ToListAsync();
        }

        public async Task<IEnumerable<Relation>> GetRelation()
        {
            return await _context.relations.AsNoTracking().OrderBy(x => x.relationName).ToListAsync();
        }
        //

         public async Task <Nominee> GetNomineeByEmpId1(int empId)
        {
            return await _context.nominees.Where(x => x.employeeID == empId).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Nominee> GetNomineeById(int id)
        {
            return await _context.nominees.FindAsync(id);
        }
        public async Task<EmployeeAward> GetAwardById(int id)
        {
            return await _context.employeeAwards.Include(x => x.employee).AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<TraveInfo> GetTravelById(int id)
        {
            return await _context.traveInfos.FindAsync(id);
        }
        public async Task<IeltsInfo> GetIeltsById(int id)
        {
            return await _context.ieltsInfos.FindAsync(id);
        }
        public async Task<HRPMSAttachment> GetAttachmentById(int id)
        {
            return await _context.hRPMSAttachments.FindAsync(id);
        }
        public async Task<EmployeeInsurance> GetInsuranceById(int id)
        {
            return await _context.employeeInsurances.FindAsync(id);
        }
        public async Task<PassportDetails> GetPassportById(int id)
        {
            return await _context.passportDetails.FindAsync(id);
        }

        public async Task<int> SaveNominee(Nominee nominee)
        {
            if (nominee.Id != 0)
                _context.nominees.Update(nominee);
            else
                _context.nominees.Add(nominee);

            await _context.SaveChangesAsync();
            return nominee.Id;
        }
        public async Task<int> SaveRelation(Relation relation)
        {
            if (relation.Id != 0)
                _context.relations.Update(relation);
            else
                _context.relations.Add(relation);

            await _context.SaveChangesAsync();
            return relation.Id;
        }

        public async Task<int> SaveOccupation(Occupation occupation)
        {
            if (occupation.Id != 0)
                _context.occupations.Update(occupation);
            else
                _context.occupations.Add(occupation);

            await _context.SaveChangesAsync();
            return occupation.Id;
        }


        #endregion

        #region Employee Insurance

        public async Task<bool> DeleteEmployeeInsuranceById(int id)
        {
            _context.employeeInsurances.Remove(_context.employeeInsurances.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }       

        public async Task<IEnumerable<EmployeeInsurance>> GetEmployeeInsuranceByEmpId(int empId)
        {
            return await _context.employeeInsurances.Where(x => x.employeeInfoId == empId).AsNoTracking().ToListAsync();
        }      

        public async Task<int> SaveEmployeeInsurance(EmployeeInsurance employeeInsurance)
        {
            if (employeeInsurance.Id != 0)
                _context.employeeInsurances.Update(employeeInsurance);
            else
                _context.employeeInsurances.Add(employeeInsurance);

            await _context.SaveChangesAsync();
            return employeeInsurance.Id;
        }

        #endregion
        public async Task<string> GetNomineeImgUrlById(int id)
        {
            return await _context.employeeInsurances.Where(x => x.Id == id).Select(x => x.imageUrl).FirstOrDefaultAsync();
        }
    }
}
