using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Assignment;
using OPUSERP.HRPMS.Services.Assignment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Assignment
{
    public class TransferJoinService : ITransferJoinService
    {
        private readonly ERPDbContext _context;

        public TransferJoinService(ERPDbContext context)
        {
            _context = context;
        }


        #region Employee Release
        public async Task<bool> DeleteEmployeeReleaseById(int id)
        {
            _context.employeeReleaseInfos.Remove(_context.employeeReleaseInfos.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        //public async Task<IEnumerable<Data.Entity.Assignment.Assignment>> GetAssignmentByEmpId(int empId)
        //{
        //    return await _context.assignments.Where(x => x.employeeId == empId).OrderBy(x => x.StartDate).AsNoTracking().ToListAsync();
        //}



        public async Task<EmployeeReleaseInfo> GetEmployeeReleaseInfoById(int id)
        {
            return _context.employeeReleaseInfos.Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task<IEnumerable<EmployeeReleaseInfo>> GetAllReleasedEmployee(int? departmentId)
        {
            var ids = _context.employeeJoiningLetters.Select(x=>x.employeeReleaseInfoId).ToList();
            return await _context.employeeReleaseInfos.Where(x => x.toDepartmentId == departmentId && !ids.Contains(x.Id)).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<EmployeeJoiningLetter>> GetAllJoiningEmployee(int? departmentId)
        {
            return await _context.employeeJoiningLetters.Where(x => x.joingDepartmentId == departmentId).AsNoTracking().ToListAsync();
        }



        //public async Task<EmployeeReleaseInfo> GetAllReleasedEmployee(int? departmentId)
        //{
        //    var result = (from R in _context.employeeReleaseInfos
        //                  join D in _context.departments on R.toDepartmentId equals D.Id
        //                  where R.toDepartmentId == departmentId
        //                  select new EmployeeReleaseInfo
        //                  {
        //                      employeeName = R.employeeName,
        //                      designation = R.designation,
        //                      fromDepartment = R.fromDepartment,
        //                      fromDepartmentId = R.fromDepartmentId,
        //                      toDepartment = D.deptName,
        //                      toDepartmentId = R.toDepartmentId,
        //                      transferOrderNo = R.transferOrderNo,
        //                      transferOrderDate = R.transferOrderDate,
        //                      releaseOrderNo = R.releaseOrderNo,
        //                      releaseOrderDate = R.releaseOrderDate,
        //                  }).FirstOrDefaultAsync();
        //    return await result;
        //}

        public async Task<int> SaveEmployeeRelease(EmployeeReleaseInfo employeeReleaseInfo)
        {
            if (employeeReleaseInfo.Id != 0)
                _context.employeeReleaseInfos.Update(employeeReleaseInfo);
            else
                _context.employeeReleaseInfos.Add(employeeReleaseInfo);

            await _context.SaveChangesAsync();
            return employeeReleaseInfo.Id;
        }
        #endregion

        #region Employee Joining
        public async Task<bool> DeleteJoiningById(int id)
        {
            _context.employeeReleaseInfos.Remove(_context.employeeReleaseInfos.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        //public async Task<IEnumerable<Data.Entity.Assignment.Assignment>> GetAssignmentByEmpId(int empId)
        //{
        //    return await _context.assignments.Where(x => x.employeeId == empId).OrderBy(x => x.StartDate).AsNoTracking().ToListAsync();
        //}



        public async Task<EmployeeReleaseInfo> GetEmployeeJoiningById(int id)
        {
            return _context.employeeReleaseInfos.Where(x => x.Id == id).FirstOrDefault();
        }

        //public async Task<IEnumerable<EmployeeReleaseInfo>> GetAssignmentByTypeAndEmpId(int empId, int assignmentType)
        //{
        //    return await _context.assignments.Where(x => x.employeeId == empId && x.assignmentTypeId == assignmentType).Include(x => x.designation).Include(x => x.department).AsNoTracking().ToListAsync();
        //}

        public async Task<int> SaveEmployeeJoining(EmployeeJoiningLetter employeeJoiningLetter)
        {
            if (employeeJoiningLetter.Id != 0)
                _context.employeeJoiningLetters.Update(employeeJoiningLetter);
            else
                _context.employeeJoiningLetters.Add(employeeJoiningLetter);

            await _context.SaveChangesAsync();
            return employeeJoiningLetter.Id;
        }
        #endregion
    }
}
