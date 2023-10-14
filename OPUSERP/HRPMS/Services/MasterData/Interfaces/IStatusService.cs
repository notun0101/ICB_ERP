using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Salary;

namespace OPUSERP.HRPMS.Services.MasterData.Interfaces
{
    public interface IStatusService
    {
        #region Activity Status
        Task<bool> SaveActivityStatus(ActivityStatus activityStatus);
        Task<IEnumerable<ActivityStatus>> GetActivityStatus();
        Task<ActivityStatus> GetActivityStatusById(int id);
        Task<bool> DeleteActivityStatusById(int id);

        #endregion Service Status

        #region Service Status
        Task<bool> SaveServiceStatus(ServiceStatus serviceStatus);
        Task<IEnumerable<ServiceStatus>> GetServiceStatus();
        Task<ServiceStatus> GetServiceStatusById(int id);
        Task<bool> DeleteServiceStatusById(int id);


        #endregion

        #region HrProgram
        Task<bool> SaveHrProgram(HrProgram hrProgram);
        Task<IEnumerable<HrProgram>> GetHrProgram();
        Task<HrProgram> GetHrProgramById(int id);
        Task<bool> DeleteHrProgramById(int id);

        #endregion

        #region HrUnit
        Task<bool> SaveHrUnit(HrUnit hrUnit);
        Task<IEnumerable<HrUnit>> GetHrUnit();
        Task<HrUnit> GetHrUnitById(int id);
        Task<bool> DeleteHrUnitById(int id);
        #endregion

        #region HrBranch
        Task<bool> SaveHrBranch(HrBranch hrBranch);
        Task<IEnumerable<HrBranch>> GetHrBranch();
        Task<HrBranch> GetHrBranchById(int id);
        Task<bool> DeleteHrBranchById(int id);

        #endregion

        #region HrDivision
        Task<bool> SaveHrDivision(HrDivision hrDivision);
        Task<IEnumerable<HrDivision>> GetHrDivision();
        Task<HrDivision> GetHrDivisionById(int id);
        Task<bool> DeleteHrDivisionById(int id);
        #endregion
        
        #region HrBranchType
        Task<bool> SaveHrBranchType(HrBranchType hrBranchType);
        Task<IEnumerable<HrBranchType>> GetHrBranchType();
        Task<HrBranchType> GetHrBranchTypeById(int id);
        Task<bool> DeleteHrBranchTypeById(int id);
        #endregion

        #region HrCommunication
        Task<bool> SaveHrCommunication(HrCommunication  hrCommunication);
        Task<IEnumerable<HrCommunication>> GetHrCommunication();
        Task<HrCommunication> GetHrCommunicationById(int id);
        Task<bool> DeleteHrCommunicationById(int id);
        Task<bool> DeleteRetirementById(int id);
        Task<bool> SaveHrCommunicationForRetirement(HrCommunication hrCommunication);
        #endregion



        Task<IEnumerable<FunctionInfo>> GetFunctionInfo();
        Task<IEnumerable<SalaryGrade>> GetJoiningGrades();
        Task<IEnumerable<SalaryGrade>> GetCurrentGrades();
        Task<IEnumerable<SalarySlab>> GetCurrentBasic();
        Task<IEnumerable<SalarySlab>> GetJoiningBasic();
        Task<IEnumerable<DisabilityType>> GeAlltDisabilityType();
        Task<bool> DeleteDisabilityTypeById(int id);
        Task<bool> SaveDisabilityType(DisabilityType disabilityType);
        Task<IEnumerable<Department>> GetAllDepartment();

    }
}
