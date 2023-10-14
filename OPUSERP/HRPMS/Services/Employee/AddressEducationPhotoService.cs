using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee
{
    public class AddressEducationPhotoService : IEmployeeInfoService
    {
        private readonly ERPDbContext _context;

        public AddressEducationPhotoService(ERPDbContext context)
        {
            _context = context;
        }

        #region Address 

        public async Task<bool> SaveAddress(Address address)
        {
            if (address.Id != 0)
                _context.addresses.Update(address);
            else
                _context.addresses.Add(address);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Address>> GetAllAddress()
        {
            return await _context.addresses.AsNoTracking().ToListAsync();
        }

        public async Task<Address> GetAddressById(int id)
        {
            return await _context.addresses.FindAsync(id);
        }

        public async Task<bool> DeleteAddressById(int id)
        {
            _context.addresses.Remove(_context.addresses.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Address>> GetAddressByEmpId(int empId)
        {
            return await _context.addresses.Where(x => x.employeeId == empId).AsNoTracking().ToListAsync();
        }

        public async Task<Address> GetAddressByTypeAndEmpId(int empId, string type)
        {
            return await _context.addresses.Where(x => x.employeeId == empId && x.type == type).Include(x=> x.division).Include(x=>x.district).Include(x=>x.thana).FirstOrDefaultAsync();
        }

		public async Task<int> DeletePrevJobHistoryById(int id)
		{
			_context.prevJobHistories.Remove(_context.prevJobHistories.Find(id));
			return await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<EmployeeInfo>> GetAllProfileUpdateReqEmp()
        {
            return await _context.employeeInfos.Where(x => x.isApproved == 0 && x.activityStatus == 3).ToListAsync();
        }
        public async Task<EmployeeInfo> GetEmployeeById(int id)
        {
            return await _context.employeeInfos.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> ChangeEmployeeActivityStatus(int empId, int status)
        {
            var emp = await _context.employeeInfos.FindAsync(empId);
            emp.activityStatus = status;
            return emp.Id;
        }
        #endregion

        #region EducationalQualification

        public async Task<bool> DeleteEducationalQualificationById(int id)
        {
            _context.educationalQualifications.Remove(_context.educationalQualifications.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EducationalQualification>> GetAllEducationalQualification()
        {
            return await _context.educationalQualifications.AsNoTracking().ToListAsync();
        }

        public async Task<EducationalQualification> GetEducationalQualificationById(int id)
        {
            return await _context.educationalQualifications.FindAsync(id);
        }
      
        public async Task<bool> SaveEducationalQualification(EducationalQualification educationalQualification)
        {
            if (educationalQualification.Id != 0)
                _context.educationalQualifications.Update(educationalQualification);
            else
                _context.educationalQualifications.Add(educationalQualification);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EducationalQualification>> GetEducationalQualificationByEmpId(int empId)
        {
            return await _context.educationalQualifications.OrderByDescending(x => x.passingYear).Where(x => x.employeeId == empId).Include(x => x.result).Include(x => x.degree).Include(x => x.reldegreesubject).Include(x => x.organization).Include(x => x.degree.levelofeducation).Include(x => x.reldegreesubject.subject).AsNoTracking().ToListAsync();
        }
		public async Task<IEnumerable<LevelofEducation>> GetLevelOfEducationList()
        {
            return await _context.levelofEducations.ToListAsync();
        }
        public async Task<IEnumerable<EducationalQualification>> GetEducationalQualificationListByEmpId(int? empId)
        {
            return await _context.educationalQualifications.Where(x => x.employeeId == empId).Include(x => x.result).Include(x => x.degree).Include(x => x.reldegreesubject).Include(x => x.organization).Include(x => x.degree.levelofeducation).Include(x => x.reldegreesubject.subject).AsNoTracking().ToListAsync();
        }
        #endregion
        public async Task<bool> DeleteJobHistoryByEmpId(int employeeId)
        {
            _context.prevJobHistories.RemoveRange(_context.prevJobHistories.Where(x => x.employeeId == employeeId).ToList());
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteBDBLJobHistoryByEmpId(int employeeId)
        {
            _context.employeePostings.RemoveRange(_context.employeePostings.Where(x => x.employeeId == employeeId).ToList());
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<bool> SaveJobHistory(PrevJobHistory PrevJobHistory)
        {
            if (PrevJobHistory.Id != 0)
                _context.prevJobHistories.Update(PrevJobHistory);
            else
                _context.prevJobHistories.Add(PrevJobHistory);
            return 1 == await _context.SaveChangesAsync();
        }

		public async Task<IEnumerable<PrevJobHistory>> getAllPreviousJobHistory(int empId)
		{
			return await _context.prevJobHistories.Where(x => x.employeeId == empId).ToListAsync();
		}
		public async Task<IEnumerable<EmployeePostingPlace>> GetBdblJobInfoByEmpId(int empId)
		{
			var data = await _context.employeePostings.AsNoTracking().Where(x => x.employeeId == empId).OrderByDescending(x => Convert.ToDateTime(x.startDate).Date).ToListAsync();
			return data;
		}

		public async Task<int> DeleteBDBLPosting(int id)
		{
			var data = await _context.employeePostings.FindAsync(id);
			_context.employeePostings.Remove(data);
			return await _context.SaveChangesAsync();
		}

		public async Task<int> SaveBDBLJobHistory(EmployeePostingPlace employeePostingPlace)
        {
            var dateEnd = Convert.ToDateTime(employeePostingPlace.endDate).Date;
            employeePostingPlace.endDate = dateEnd == new DateTime(1, 1, 1).Date ? null : employeePostingPlace.endDate;
            if (employeePostingPlace.Id != 0)
            {
                _context.employeePostings.Update(employeePostingPlace);
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.employeePostings.Add(employeePostingPlace);
                await _context.SaveChangesAsync();
            }
            return employeePostingPlace.Id;
        }

        #region Photograph

        public async Task<bool> SaveImage(Photograph photograph)
        {
            if (photograph.Id != 0)
                _context.photographs.Update(photograph);
            else
                _context.photographs.Add(photograph);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Photograph>> GetPhotograph()
        {
            return await _context.photographs.AsNoTracking().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Photograph>> GetPhotographByEmpId(int empId)
        {
            return await _context.photographs.Where(x => x.employeeId == empId).AsNoTracking().AsNoTracking().ToListAsync();
        }

        public async Task<bool> DeletePhotographById(int id)
        {
            _context.photographs.Remove(_context.photographs.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<Photograph> GetPhotographByTypeAndEmpId(int empId, string type)
        {
            return await _context.photographs.Where(x => x.employeeId == empId && x.type == type).AsNoTracking().FirstOrDefaultAsync();
        }

       



        #endregion
    }
}
