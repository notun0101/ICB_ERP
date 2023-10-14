using OPUSERP.CRM.Data.Entity.Lead;
using OPUSERP.Data.Entity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.Patient.Data.Entity
{
    [Table("PatientHistory")]
    public class PatientHistory : Base
    {
        public int? leadsId { get; set; }
        public Leads leads { get; set; }

        public int? diseaseInfoId { get; set; }
        public DiseaseInfo diseaseInfo { get; set; }

        public DateTime? diseaseStartDate { get; set; }

    }
}
