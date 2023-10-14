using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.HospitalManagement.Models;
using OPUSERP.Data;
using OPUSERP.SCM.Data.Entity.Hospital;
using OPUSERP.SCM.Services.Hospital.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Services.Hospital
{
	public class HospitalSettingsService:IHospitalSettings
	{
		private readonly ERPDbContext _context;
		
		public HospitalSettingsService(ERPDbContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<Building>> GetAllBuildings()
		{
			return await _context.Buildings.ToListAsync();
		}
		public async Task<IEnumerable<FloorWithCabin>> GetFloorsWithBuildingId(int id)
		{
			var data = await _context.Floors.Where(x => x.buildingId == id).Include(x => x.building).AsNoTracking().ToListAsync();
			var data1 = new List<FloorWithCabin>();
			foreach (var item in data)
			{
				data1.Add(new FloorWithCabin
				{
					floor = item,
					cabins = await GetBedByFloor(item.Id)
					//cabins = await _context.Cabins.Include(x => x.bed).Where(x => x.floorId == item.Id).AsNoTracking().ToListAsync()
				});
			}
			return data1;
		}
		public async Task<Bed> GetBedInfoById(int id)
		{
			return await _context.Beds.Where(x => x.Id == id).FirstOrDefaultAsync();
		}
		public async Task<IEnumerable<CabinWithBed>> GetBedByFloor(int floorId)
		{
			var cabins = await _context.Cabins.Where(x => x.floorId == floorId).ToListAsync();

			var data1 = new List<CabinWithBed>();
			foreach (var item in cabins)
			{
				data1.Add(new CabinWithBed
				{
					cabin = item,
					beds = await _context.Beds.Where(x => x.cabinId == item.Id).ToListAsync()
				});
			}
			return data1;
		}

		#region Floor
		public async Task<bool> SaveFloor(Floor floor)
		{
			if (floor.Id != 0)
			{
				_context.Floors.Update(floor);
			}
			else
			{
				_context.Floors.Add(floor);
			}
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Floor>> GetAllFloor()
		{
			return await _context.Floors.AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<Floor>> GetAllFloorByFilter(string filter)
		{
			return await _context.Floors.AsNoTracking().ToListAsync();
		}

		public async Task<bool> DeleteFloorById(int id)
		{
			_context.Floors.Remove(_context.Floors.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

        #endregion
    }
}
