using OPUSERP.SCM.Data.Entity.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.SCMMasterData.Models
{
    public class SpecificationViewModel
    {
        public int itemId { get; set; }
        public int specId { get; set; }
        public string skuNumber { get; set; }
        public string specificationName { get; set; }
        public string itemName { get; set; }

        public IEnumerable<ItemSpecification> itemSpecifications { get; set; }
    }
}
