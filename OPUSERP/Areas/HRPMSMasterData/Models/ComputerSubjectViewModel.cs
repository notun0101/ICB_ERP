using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class ComputerSubjectViewModel
    {
        public int CsId { get; set; }
        public string name { get; set; }

        public int? status { get; set; }
        public int? isActive { get; set; }
        public string remarks { get; set; }
        public IEnumerable<ComputerSubject> ComputerSubjects { get; set; }
    }
}
