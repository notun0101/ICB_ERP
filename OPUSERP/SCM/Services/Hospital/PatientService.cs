using OPUSERP.Data;
using OPUSERP.SCM.Services.Hospital.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Services.Hospital
{
	public class PatientService:IPatient
	{
		private readonly ERPDbContext _context;

		public PatientService(ERPDbContext context)
		{
			_context = context;
		}
		public async Task<int> SavePatient(SCM.Data.Entity.Hospital.Patient resource)
		{
			if (resource.Id != 0)
			{
				resource.updatedBy = resource.updatedBy;
				resource.updatedAt = DateTime.Now;
				_context.Patients.Update(resource);
			}
			else
			{
				resource.createdBy = resource.createdBy;
				resource.createdAt = DateTime.Now;
				_context.Patients.Add(resource);
			}
			//return resource.Id == await _context.SaveChangesAsync();
			await _context.SaveChangesAsync();
			return resource.Id;
		}
	}
}
