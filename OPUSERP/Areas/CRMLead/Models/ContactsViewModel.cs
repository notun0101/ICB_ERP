using Microsoft.AspNetCore.Http;
using OPUSERP.CRM.Data.Entity.Lead;
using OPUSERP.CRM.Data.Entity.MasterData;
using OPUSERP.CRM.Data.SPModel;
using OPUSERP.Data.Entity.AttachmentComment;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Master;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace OPUSERP.Areas.CRMLead.Models
{
    public class ContactsViewModel
    {
        public int id { get; set; }
        public int CId { get; set; }
        public int resourceId { get; set; }
        public int contactId { get; set; }

        [Display(Name = "Contact Name")]
        public string contactName { get; set; }

        [Display(Name = "Department")]
        public int? department { get; set; }

        [Display(Name = "Designation")]
        public int? designation { get; set; }

        public int? age { get; set; }

        public string gender { get; set; }


        public string image { get; set; }

        public string phone { get; set; }
        public string otherPhone { get; set; }

        public string mobile { get; set; }

        public string email { get; set; }

        public string companyName { get; set; }

        public string alternativeEmail { get; set; }


        public string fax { get; set; }
        public string skypeId { get; set; }
        public string linkedInId { get; set; }

        public string leadId { get; set; }

        public string leadName { get; set; }
        public string sourceDescription { get; set; }

        [Display(Name = "Contact Photo")]
        public IFormFile empPhoto { get; set; }

        public Resource resource { get; set; }

        public IEnumerable<Contacts> contacts { get; set; }
        public Contacts contact { get; set; }

        public IEnumerable<CRMDepartment> departments { get; set; }
        public IEnumerable<CRMDesignation> designations { get; set; }
        public IEnumerable<Sector> sectors { get; set; }
        public IEnumerable<CompanyGroup> companyGroups { get; set; }
        public IEnumerable<OwnerShipType> ownerShipTypes { get; set; }
        public IEnumerable<Source> sources { get; set; }
        public LeadAutoNumber leadAutoNumbers { get; set; }
        public Leads leadsingal { get; set; }
        public IEnumerable<Leads> leads { get; set; }
        public IEnumerable<FIType> fITypes { get; set; }
        public Contacts contactsById { get; set; }
        public DocumentPhotoAttachment contactImage { get; set; }
        public DocumentPhotoAttachment visitingCardFrontImage { get; set; }
        public DocumentPhotoAttachment visitingCardBackImage { get; set; }

        //For Client or lead save
        public int Id { get; set; }
        public int leadOrClientId { get; set; }
        public int sourceId { get; set; }
        public int ownerShipTypeId { get; set; }
        public int companyGroupId { get; set; }
        public int totalemployee { get; set; }
        public int sector { get; set; }
        public string leadShortName { get; set; }
        public string leadNumber { get; set; }
    }
}
