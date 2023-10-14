using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSACR.Models
{
    public class QuantitativeEvaluationsViewModel
    {        
        public string indicatorNameBn { get; set; }
        public string HeadName { get; set; }
        public decimal? target { get; set; }
        public decimal? achievement { get; set; }
        public decimal? achievementPercent { get; set; }
        public decimal? achievementSign { get; set; }
        public decimal? achievementPercentSign { get; set; }
        public decimal? averagePercent { get; set; }      
        public decimal? averagePercentSignatory { get; set; }
        public decimal? acrCounter { get; set; }
		public string posting { get; set; }
	}
}
