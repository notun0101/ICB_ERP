using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.CRM.Data.Entity.MasterData
{
    [Table("AgreementType")]
    public class AgreementType : Base
    {
        [Required]
        public string agreementTypeName { get; set; }
    }
}
