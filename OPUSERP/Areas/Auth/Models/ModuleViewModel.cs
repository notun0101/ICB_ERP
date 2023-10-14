using OPUSERP.Data.Entity.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Auth.Models
{
	public class ModuleViewModel
	{
		public int id { get; set; }
		public string nameEnglish { get; set; }
		public string nameBangla { get; set; }
		public int sortOrder { get; set; }

		public IEnumerable<ERPModule> eRPModules { get; set; }
	}
}
