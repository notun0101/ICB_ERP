using System.ComponentModel.DataAnnotations.Schema;
using OPUSERP.Data.Entity;

namespace OPUSERP.Production.Data.Entity
{
    [Table("ProductionTypes")]
    public class ProductionType:Base
    {
        [Column(TypeName = "nvarchar(100)")]
        public string productionTypeName { get; set; }
        public int? shortOrder { get; set; }
    }
}
