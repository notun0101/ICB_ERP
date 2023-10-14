using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Data.Entity.Auth
{
	public class MasterMenu:Base
	{
		public string name { get; set; }
		public string aliasName { get; set; }
		public string description { get; set; }
		public string areaName { get; set; }
		public string controllerName { get; set; }
		public string methodName { get; set; }
		public string company { get; set; }
	}
}
