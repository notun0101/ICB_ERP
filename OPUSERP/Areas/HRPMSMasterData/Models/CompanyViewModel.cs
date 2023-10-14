using Microsoft.AspNetCore.Http;
using OPUSERP.CRM.Data.Entity.MasterData;
using OPUSERP.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class CompanyViewModel
    {
        public int companyId { get; set; }
        public string companyName { get; set; }
        public string ownerName { get; set; }
        public string managerName { get; set; }
        public string tradeLicense { get; set; }
        public string businessNature { get; set; }
        public string officeTelephone { get; set; }
        public string certificateOfInspir { get; set; }
        public string permission { get; set; }
        public string generation { get; set; }
        public string vision { get; set; }
        public string mission { get; set; }
        public string vatNo { get; set; }
        public string tinNo { get; set; }
        public string binNo { get; set; }
        public string swiftCode { get; set; }
        public DateTime? dateOfAgm { get; set; }
        public int? bankBranchDetailsId { get; set; }
        public BankBranchDetails bankBranchDetails { get; set; }
        public DateTime? dateOfEstablishment { get; set; }
        public int? permanentEmployee { get; set; }
        public string companyEmail { get; set; }
        public string alternetEmail { get; set; }
        public decimal? liquidityRatio { get; set; }
        public string fileName { get; set; }
        public string filePath { get; set; }
        public IFormFile companyFilePath { get; set; }
        public string addressLine { get; set; }
        public string filePathTwo { get; set; }
        public string filePathThree { get; set; }
        public string companyShortName { get; set; }
        public string height { get; set; }
        public string width { get; set; }
        public string labelColor { get; set; }
        public Company allCompany { get; set; }
      

    }
}
