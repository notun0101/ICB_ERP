using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.POS.Data.Entity
{
    [Table("CardType")]
    public class CardType : Base
    {
        [MaxLength(100)]
        public string CardTypeName { get; set; }
    }
}
