//using UMBRELLA.Areas.MasterData.Models.Lang;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Master;

namespace OPUSERP.Areas.MasterData.Models
{
    public class ExtraCurricularTypeViewModel
    {


        public int ExtraCurricularTypeId { get; set; }
        public string name { get; set; }

        public IEnumerable<ExtraCurricularType> extraCurricularTypes { get; set; }

    }
}
