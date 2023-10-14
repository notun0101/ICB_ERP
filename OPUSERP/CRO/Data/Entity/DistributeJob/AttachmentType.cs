﻿using OPUSERP.CRM.Data.Entity.Lead;
using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.CRO.Data.Entity.DistributeJob
{
    [Table("AttachmentType", Schema = "CRO")]
    public class AttachmentType : Base
    {
        [MaxLength(250)]
        public string attachmentTypeName { get; set; }

    }
}
