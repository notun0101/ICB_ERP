using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Workflow.Data.Entity
{
    [Table("DraftAttachment")]
    public class DraftAttachment:Base
    {
        public int? draftDocId { get; set; }
        public DraftDoc draftDoc { get; set; }

        public string fileUrl { get; set; }

        public string title { get; set; }
    }
}
