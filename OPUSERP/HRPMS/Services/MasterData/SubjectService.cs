using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.EntityFrameworkCore;
using OPUSERP.HRPMS.Data.Entity.HrComputerLiteracy;
using OPUSERP.Areas.HRPMSMasterData.Models;

namespace OPUSERP.HRPMS.Services.MasterData
{
    public class SubjectService : ISubjectService
    {
        private readonly ERPDbContext _context;

        public SubjectService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveSubject(Subject subject)
        {
            if (subject.Id != 0)
                _context.subjects.Update(subject);
            else
                _context.subjects.Add(subject);
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<int> SaveSubject1(Subject subject)
        {
            if (subject.Id != 0)
                _context.subjects.Update(subject);
            else
                _context.subjects.Add(subject);
            await _context.SaveChangesAsync();
            return subject.Id;
        }

        public async Task<int> UpdateSubjectDegreeRelation(int subjectId, int degreeId)
        {
            var data = await _context.relDegreeSubjects.Where(x => x.subjectId == subjectId).FirstOrDefaultAsync();
            if (data == null)
            {
                var model = new RelDegreeSubject
                {
                    subjectId = subjectId,
                    degreeId = degreeId
                };
                _context.relDegreeSubjects.Add(model);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (data.degreeId != degreeId)
                {
                    data.degreeId = degreeId;
                    _context.relDegreeSubjects.Update(data);
                    await _context.SaveChangesAsync();
                }
            }
            return 1;

        }

		public async Task<IEnumerable<SubjectVm>> GetAllSubjectVm()
		{
			var data = await _context.subjectVms.FromSql($"sp_GetAllSubjects").AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<bool> SaveComputerSubject(ComputerSubject computerSubject)
        {
            if (computerSubject.Id != 0)
                _context.computerSubjects.Update(computerSubject);
            else
                _context.computerSubjects.Add(computerSubject);
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<bool> SaveHrComputerLiteracy(HrComputerLiteracy hrComputerLiteracy)
        {
            if (hrComputerLiteracy.Id != 0)
                _context.hrComputerLiteracies.Update(hrComputerLiteracy);
            else
                _context.hrComputerLiteracies.Add(hrComputerLiteracy);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Subject>> GetAllSubject()
        {
            return await _context.subjects.OrderBy(x => x.subjectName).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<Degree>> GetAllDegrees()
        {
            return await _context.degrees.OrderBy(x => x.degreeName).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<ComputerSubject>> GetAllComputerSubject()
        {
            return await _context.computerSubjects.OrderBy(x => x.name).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<HrComputerLiteracy>> GetAllcomputerLiteracy()
        {
            return await _context.hrComputerLiteracies.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<HrComputerLiteracy>> GetcomputerLiteracyByEmpId(int empId)
        {
            return await _context.hrComputerLiteracies.Where(x => x.employeeId == empId).AsNoTracking().ToListAsync();
        }
        public async Task<Subject> GetSubjectById(int id)
        {
            return await _context.subjects.FindAsync(id);
        }

        public async Task<bool> DeleteSubjectById(int id)
        {
            _context.subjects.Remove(_context.subjects.Find(id));
            //_context.employeeTypes.Remove(_context.employeeTypes.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<int> GetDegreeBySubjectId(int id)
        {
            return await _context.relDegreeSubjects.Where(x => x.subjectId == id).Select(x => x.degreeId).FirstOrDefaultAsync();
        }
        public async Task<bool> DeleteComputerSubjectById(int id)
        {
            _context.computerSubjects.Remove(_context.computerSubjects.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteComputerLiteracyById(int id)
        {
            _context.hrComputerLiteracies.Remove(_context.hrComputerLiteracies.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }


    }
}
