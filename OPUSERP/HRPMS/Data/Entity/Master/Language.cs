using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("Language")]
    public class Language:Base
    {
        [Required]
        public string languageName { get; set; }
        public string languageNameBn { get; set; }

        public string languageShortName { get; set; }
    }
}
