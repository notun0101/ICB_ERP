using OPUSERP.Areas.HospitalManagement.Models;
using OPUSERP.SCM.Data.Entity.Hospital;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Services.Hospital.Interfaces
{
	public interface IHospitalSettings
	{
		Task<IEnumerable<Building>> GetAllBuildings();
		Task<IEnumerable<FloorWithCabin>> GetFloorsWithBuildingId(int id);
		Task<IEnumerable<CabinWithBed>> GetBedByFloor(int floorId);
		Task<Bed> GetBedInfoById(int id);

		#region Floor

		Task<bool> SaveFloor(Floor floor);
		Task<IEnumerable<Floor>> GetAllFloor();
		Task<IEnumerable<Floor>> GetAllFloorByFilter(string filter);
		Task<bool> DeleteFloorById(int id);
		#endregion
	}


}
