using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models.Dashboard
{
    public class MainDashboardViewModel
    {
        public ActiveEmployeeCountViewModel activeEmployeeCountViewModel { get; set; }
		public MaleFemalCount maleFemalCount { get; set; }
	}
	public class MaleFemalCount
	{
		public int MaleCount { get; set; }
		public int FemaleCount { get; set; }
	}
}
