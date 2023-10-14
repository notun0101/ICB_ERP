using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
    [Table("QuantitativeEvaluationComments")]
    public class QuantitativeEvaluationComments:Base
    {
        public int? assessmentId { get; set; }
        public Assessment assessment { get; set; }
        public int? evaluationCommentsHeadId { get; set; }
        public EvaluationCommentsHead evaluationCommentsHead { get; set; }
        public string evaluationComments { get; set; }
    }
}
