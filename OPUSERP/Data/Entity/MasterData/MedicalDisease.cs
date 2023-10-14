using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Data.Entity.MasterData
{
    public class MedicalDisease:Base
    {
        public string name { get; set; }

        public int? level { get; set; }
    }
}
