using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Training
{
    [Table("ModuleTrainingCategory")]
    public class ModuleTrainingCategory : Base
    {
        public string nameBN { get; set; }
        public string nameEN { get; set; }
    }
}
