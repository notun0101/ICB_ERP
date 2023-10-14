using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Master;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
    [Table("TrainingTitlelog", Schema = "HR")]
    public class TrainingTitlelog : Base
    {
		public string trainingTitle { get; set; }
		public int? status { get; set; }

	}
}
