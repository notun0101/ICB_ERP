﻿using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("BookCategory")]
    public class BookCategory:Base
    {
        [Required]
        public string bookCategoey { get; set; }
        public string bookCategoeyBn { get; set; }

        public string bookCategoryShortName { get; set; }
    }
}
