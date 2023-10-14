using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.FixedAsset.Data.Entity.AssetRegister
{
    [Table("AssetRetirementTypes")]
    public class AssetRetirementType : Base
    {
        [MaxLength(200)]
        public string assetRetirementTypeName { get; set; }       
       
    }
}
