﻿using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.CLUB.Data.Member
{
    [Table("MemberSpouse")]
    public class MemberSpouse: Base
    {
        //Foreign Reliation
        public int employeeId { get; set; }
        public MemberInfo employee { get; set; }

        public string spouseName { get; set; }

        public string email { get; set; }

        public string spouseNameBN { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dateOfBirth { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dateOfMarriage { get; set; }

        public string occupation { get; set; }

        public string gender { get; set; }

        public string designation { get; set; }

        public string organization { get; set; }

        public string bin { get; set; }

        public string nid { get; set; }

        public string bloodGroup { get; set; }

        public string contact { get; set; }

        public string highestEducation { get; set; }

        public string homeDistrict { get; set; }

        //SpecialSkill
        public string spacialSkillIds { get; set; }
        public string spacialSkills { get; set; }
    }
}
