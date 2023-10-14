using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.EntityFrameworkCore;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.HRPMS.Data.Entity.Employee;

namespace OPUSERP.HRPMS.Services.MasterData
{
    public class StatusService : IStatusService
    {
        private readonly ERPDbContext _context;

        public StatusService(ERPDbContext context)
        {
            _context = context;
        }

        #region Activity Status
        public async Task<bool> DeleteActivityStatusById(int id)
        {
            _context.activityStatuses.Remove(_context.activityStatuses.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ActivityStatus>> GetActivityStatus()
        {
            return await _context.activityStatuses.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<DisabilityType>> GeAlltDisabilityType()
        {
            return await _context.disabilityTypes.AsNoTracking().ToListAsync();
        }

        public async Task<ActivityStatus> GetActivityStatusById(int id)
        {
            return await _context.activityStatuses.FindAsync(id);
        }

        public async Task<bool> SaveActivityStatus(ActivityStatus activityStatuse)
        {
            if (activityStatuse.Id != 0)
                _context.activityStatuses.Update(activityStatuse);
            else
                _context.activityStatuses.Add(activityStatuse);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveDisabilityType(DisabilityType disabilityType)
        {
            if (disabilityType.Id != 0)
                _context.disabilityTypes.Update(disabilityType);
            else
                _context.disabilityTypes.Add(disabilityType);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteDisabilityTypeById(int id)
        {
            _context.disabilityTypes.Remove(_context.disabilityTypes.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        #endregion 

        #region ServiceStatus
        public async Task<bool> DeleteServiceStatusById(int id)
        {
            _context.serviceStatuses.Remove(_context.serviceStatuses.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        
        public async Task<IEnumerable<ServiceStatus>> GetServiceStatus()
        {
            return await _context.serviceStatuses.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<SalaryGrade>> GetJoiningGrades()
        {
            return await _context.salaryGrades.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<SalarySlab>> GetJoiningBasic()
        {
            return await _context.salarySlabs.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<SalaryGrade>> GetCurrentGrades()
        {
            return await _context.salaryGrades.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<SalarySlab>> GetCurrentBasic()
        {
            return await _context.salarySlabs.AsNoTracking().ToListAsync();
        }

        public async Task<ServiceStatus> GetServiceStatusById(int id)
        {
            return await _context.serviceStatuses.FindAsync(id);
        }

        public async Task<bool> SaveServiceStatus(ServiceStatus serviceStatus)
        {
            if (serviceStatus.Id != 0)
                _context.serviceStatuses.Update(serviceStatus);
            else
                _context.serviceStatuses.Add(serviceStatus);
            return 1 == await _context.SaveChangesAsync();
        }


        #endregion 

        #region HrProgram
        public async Task<bool> DeleteHrProgramById(int id)
        {
            _context.hrPrograms.Remove(_context.hrPrograms.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HrProgram>> GetHrProgram()
        {
            return await _context.hrPrograms.AsNoTracking().ToListAsync();
        }

        public async Task<HrProgram> GetHrProgramById(int id)
        {
            return await _context.hrPrograms.FindAsync(id);
        }

        public async Task<bool> SaveHrProgram(HrProgram hrProgram)
        {
            if (hrProgram.Id != 0)
                _context.hrPrograms.Update(hrProgram);
            else
                _context.hrPrograms.Add(hrProgram);
            return 1 == await _context.SaveChangesAsync();
        }


        #endregion

        #region HrUnit
        public async Task<bool> DeleteHrUnitById(int id)
        {
            _context.hrUnits.Remove(_context.hrUnits.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HrUnit>> GetHrUnit()
        {
            return await _context.hrUnits.Include(x=>x.department).OrderBy(x => x.unitName).AsNoTracking().ToListAsync();
        }
         public async Task<IEnumerable<Department>> GetAllDepartment()
        {
            return await _context.departments.OrderBy(x => x.deptName).AsNoTracking().ToListAsync();
        }

        public async Task<HrUnit> GetHrUnitById(int id)
        {
            return await _context.hrUnits.FindAsync(id);
        }

        public async Task<bool> SaveHrUnit(HrUnit hrUnit)
        {
            if (hrUnit.Id != 0)
                _context.hrUnits.Update(hrUnit);
            else
                _context.hrUnits.Add(hrUnit);
            return 1 == await _context.SaveChangesAsync();
        }


        #endregion

        #region HrBranch
        public async Task<bool> DeleteHrBranchById(int id)
        {
            _context.hrBranches.Remove(_context.hrBranches.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HrBranch>> GetHrBranch()
        {
            return await _context.hrBranches.OrderBy(x => x.branchName).AsNoTracking().ToListAsync();
        }

        public async Task<HrBranch> GetHrBranchById(int id)
        {
            return await _context.hrBranches.FindAsync(id);
        }

        public async Task<bool> SaveHrBranch(HrBranch hrBranch)
        {
            if (hrBranch.Id != 0)
                _context.hrBranches.Update(hrBranch);
            else
                _context.hrBranches.Add(hrBranch);
            return 1 == await _context.SaveChangesAsync();
        }


        #endregion

        #region HrDivision
        public async Task<bool> DeleteHrDivisionById(int id)
        {
            _context.hrDivisions.Remove(_context.hrDivisions.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HrDivision>> GetHrDivision()
        {
            return await _context.hrDivisions.Include(x=> x.functionInfo).OrderBy(x => x.divName).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<FunctionInfo>> GetFunctionInfo()
        {
            return await _context.FunctionInfos.AsNoTracking().ToListAsync();
        }

        public async Task<HrDivision> GetHrDivisionById(int id)
        {
            return await _context.hrDivisions.FindAsync(id);
        }

        public async Task<bool> SaveHrDivision(HrDivision hrDivision)
        {
            if (hrDivision.Id != 0)
                _context.hrDivisions.Update(hrDivision);
            else
                _context.hrDivisions.Add(hrDivision);
            return 1 == await _context.SaveChangesAsync();
        }


        #endregion

        #region HrBranchType
        public async Task<bool> DeleteHrBranchTypeById(int id)
        {
            _context.hrBranchTypes.Remove(_context.hrBranchTypes.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HrBranchType>> GetHrBranchType()
        {
            return await _context.hrBranchTypes.AsNoTracking().ToListAsync();
        }

        public async Task<HrBranchType> GetHrBranchTypeById(int id)
        {
            return await _context.hrBranchTypes.FindAsync(id);
        }

        public async Task<bool> SaveHrBranchType(HrBranchType hrBranchType)
        {
            if (hrBranchType.Id != 0)
                _context.hrBranchTypes.Update(hrBranchType);
            else
                _context.hrBranchTypes.Add(hrBranchType);
            return 1 == await _context.SaveChangesAsync();
        }


        #endregion


        #region HrCommunication
        public async Task<bool> DeleteHrCommunicationById(int id)
        {
            _context.hrCommunications.Remove(_context.hrCommunications.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteRetirementById(int id)
        {
            _context.hrCommunications.Remove(_context.hrCommunications.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HrCommunication>> GetHrCommunication()
        {
            return await _context.hrCommunications.Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.employee.department).AsNoTracking().ToListAsync();
        }

        public async Task<HrCommunication> GetHrCommunicationById(int id)
        {
            return await _context.hrCommunications.Include(x => x.employee).Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<bool> SaveHrCommunication(HrCommunication hrCommunication)
        {
            if (hrCommunication.Id != 0)
                _context.hrCommunications.Update(hrCommunication);
            else
                _context.hrCommunications.Add(hrCommunication);
            return 1 == await _context.SaveChangesAsync();
        }
          public async Task<bool> SaveHrCommunicationForRetirement(HrCommunication hrCommunication)
        {
            if (hrCommunication.Id != 0)
                _context.hrCommunications.Update(hrCommunication);
            else
                _context.hrCommunications.Add(hrCommunication);
            return 1 == await _context.SaveChangesAsync();
        }


        #endregion

    }
}
