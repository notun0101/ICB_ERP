﻿using System.ComponentModel.DataAnnotations;

namespace OPUSERP.Data.Entity.Master
{
    public class District : Base
    {
        [Required]
        public string districtCode { get; set; }

        [Required]
        public string districtName { get; set; }
        public string districtNameBn { get; set; }

        public string shortName { get; set; }

        public int divisionId { get; set; }

        public Division division { get; set; }
    }
}
