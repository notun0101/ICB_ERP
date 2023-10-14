using OPUSERP.SCM.Data.Entity.Hospital;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HospitalManagement.Models
{
	public class HospitalSettingsViewModel
	{
		public HospitalBuildingsWithFloor hospitalBuildingsWithFloor { get; set; }
	}
	public class HospitalBuildingsWithFloor
	{
		public Building building { get; set; }
		public IEnumerable<FloorWithCabin> floors { get; set; }
	}
	public class FloorWithCabin
	{
		public Floor floor { get; set; }
		public IEnumerable<CabinWithBed> cabins { get; set; }
	}
	public class CabinWithBed
	{
		public Cabin cabin { get; set; }
		public IEnumerable<Bed> beds { get; set; }
	}
}
