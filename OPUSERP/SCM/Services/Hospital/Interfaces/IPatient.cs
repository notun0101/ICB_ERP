using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Services.Hospital.Interfaces
{
	public interface IPatient
	{
		Task<int> SavePatient(SCM.Data.Entity.Hospital.Patient resource);
	}
}
