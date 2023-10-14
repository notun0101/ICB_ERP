﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Workflow.Models
{
    public class DocForwardViewmodel
    {
        public string comment { get; set; }
        public string submit { get; set; }
        public int docId { get; set; }
        public int docRouteID { get; set; }
        public int? ReviewId { get; set; }

        public int? ReturnTo { get; set; }
    }
}
