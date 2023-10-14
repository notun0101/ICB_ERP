using OPUSERP.Data.Entity.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Auth.Models
{
	public class MasterMenuViewModel
	{
		public int id { get; set; }
		public string name { get; set; }
		public string aliasName { get; set; }
		public string description { get; set; }
		public string areaName { get; set; }
		public string controllerName { get; set; }
		public string methodName { get; set; }
		public string company { get; set; }

		public IEnumerable<MasterMenu> masterMenus { get; set; }
	}
}
