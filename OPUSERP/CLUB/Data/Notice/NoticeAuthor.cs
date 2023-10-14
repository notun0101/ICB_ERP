using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.CLUB.Data.Notice
{
    [Table("NoticeAuthor")]
    public class NoticeAuthor : Base
    {
        public string name { get; set; }
        public string designation { get; set; }
        public string signature { get; set; }
    }
}
