using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Organogram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class OrganogramViewModel
    {
        public int? OrgRelationId { get; set; }
        public int?[] OrgChildId { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
        public int? parentId { get; set; }
        public int? employeeId { get; set; }
        public int? officeId { get; set; }
        public int? hrdivisionId { get; set; }
        public int? departmentId { get; set; }
        public string url { get; set; }
        public int? isActive { get; set; }
        public int?[] designationId { get; set; }
        public int?[] memberCount { get; set; }
        public IEnumerable<OrganogramRelation> organogramRelations { get; set; }
        public IEnumerable<OrganogramChild> organogramChildren { get; set; }
    }
}
