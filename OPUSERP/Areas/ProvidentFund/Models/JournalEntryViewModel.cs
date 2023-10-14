using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class JournalEntryViewModel
    {
        public int journalEntryId { get; set; }
        public DateTime tranDate { get; set; }
        public string journalNo { get; set; }
        public string description { get; set; }
        public string attachment { get; set; }
        public string account { get; set; }
        public int debits { get; set; }
        public int credits { get; set; }
        public string tbldescription { get; set; }
        public string projRef { get; set; }
        public string partyRef { get; set; }

        public IEnumerable<JournalEntryViewModel> journalEntryViewModels { get; set; }
    }
}
