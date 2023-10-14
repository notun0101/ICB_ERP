using OPUSERP.SCM.Data.Entity.Hospital;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HospitalManagement.Models
{
    public class FloorViewModel
    {
        public int Id { get; set; }
        public int? buildingId { get; set; }
        public string nameEn { get; set; }
        public string nameBn { get; set; }
        public string type { get; set; }
        public int? status { get; set; }
        public string remarks { get; set; }
        public IEnumerable<Floor> floorList { get; set; }
        public Floor floor { get; set; }


    }
}
