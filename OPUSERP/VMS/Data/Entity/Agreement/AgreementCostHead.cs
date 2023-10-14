using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.VMS.Data.Entity.Agreement
{
    [Table("AgreementCostHead")]
    public class AgreementCostHead:Base
    {
        public string agreementHeadName { get; set; }
    }
}
