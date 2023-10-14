﻿using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
	[Table("DisabilityType", Schema = "HR")]
	public class DisabilityType:Base
	{
		public string name { get; set; }

		public string remarks { get; set; }
	}
}
