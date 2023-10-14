using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Suspensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.BoardofDirector.Interfaces
{
    public interface IBoardofDirector
    {
        Task<IEnumerable<Company>> GetAllCompanyList();
        Task<string> GetPhotoUrlById(int id);
        Task<string> GetDirectorPhotoUrlById(int id);
        Task<int> SaveBoardOfDirector(BoardOfDirector boardofDirector);
		Task<IEnumerable<ComapnyAndYear>> GetAllCompanyListWithBOD();
		Task<IEnumerable<ComapnyAndYear>> GetAllCompanyListWithBOD(int companyId, int year);

		Task<ComapnyAndBOD> GetChairmenByCompanyIdAndYear(int companyId, int year);
		Task<IEnumerable<ComapnyAndBOD>> GetDirectorsByCompanyIdAndYear(int companyId, int year);
		Task<ComapnyAndBOD> GetCeoByCompanyIdAndYear(int companyId, int year);
        Task<bool> DeleteBoardOfDirectorById(int companyId, int year);
		Task<int> DeleteBoardOfDirectorByCompanyIdAndYear(int companyId, int year);

	}
}
