using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.CRM.Data.Entity.MasterData
{
    [Table("RatingCategory")]
    public class RatingCategory : Base
    {
        [Required]
        public string ratingCategoryName { get; set; }
    }
}
