using OPUSERP.Areas.HRPMSACR.Models.Lang;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.ACR;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSACR.Models
{
    public class thirdClaricalReportViewModel
    {
        public IEnumerable<Company> companies { get; set; }

        public AcrEvaluationDetailLn fLang { get; set; }

        public IEnumerable<AcrEvaluationDetail> acrEvaluationDetails { get; set; }

        public AcrEvaluationDetail acrEvaluationDetail { get; set; }

        public IEnumerable<AcrEvaluationMaster> acrEvaluationMasters { get; set; }
    }
}
