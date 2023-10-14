﻿using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.CRMLead.Models;
using OPUSERP.CRM.Data.Entity.Lead;
using OPUSERP.CRM.Services.Lead.Interfaces;
using OPUSERP.Data;
using OPUSERP.Data.Entity.AttachmentComment;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.CRM.Services.Lead
{

    public class ContactsService : IContactsService
    {
        private readonly ERPDbContext _context;

        public ContactsService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveContacts(Contacts Contacts)
        {
            if (Contacts.Id != 0)
                _context.Contact.Update(Contacts);
            else
                _context.Contact.Add(Contacts);
            await _context.SaveChangesAsync();
            return Contacts.Id;
        }

        public async Task<IEnumerable<Contacts>> GetAllContacts()
        {
            return await _context.Contact.Include(x=>x.leads).Include(x => x.resource).Include(x => x.resource.crmdesignations).Include(x => x.resource.crmdesignations).ToListAsync();
        }
        public async Task<IEnumerable<Contacts>> GetAllContactsWithImage()
        {
            var contacts = await _context.Contact.Include(x => x.leads).Include(x => x.resource).Include(x => x.resource.crmdesignations).Include(x => x.resource.crmdepartments).ToListAsync();
            var data = new List<Contacts>();
            foreach (var item in contacts)
            {
                data.Add(new Contacts
                {
                    Id = item.Id,
                    contactOwner = item.contactOwner,
                    resource = item.resource,
                    leads = item.leads,
                    departmentsName = await _context.DocumentPhotoAttachments.Where(x => x.actionTypeId == item.Id && x.actionType == "Contact" && x.documentType == "photo" && x.moduleId == 2).Select(x => x.filePath).FirstOrDefaultAsync()
                });
            }
            return data.OrderByDescending(x=>x.Id);
        }

        public async Task<DocumentPhotoAttachment> GetContactsImage(int? id,string actionType,string documentType)
        {
            return await _context.DocumentPhotoAttachments.Where(x => x.actionTypeId == id && x.actionType == actionType && x.documentType == documentType && x.moduleId == 2).FirstOrDefaultAsync();
            
        }

        public async Task<IEnumerable<Contacts>> GetAllContactsbyLeadId(int id)
        {
            return await _context.Contact.Where(x => x.leadsId == id).Include(x => x.resource.crmdesignations).Include(x => x.resource.designations).Include(x => x.leads).ToListAsync();
        }
        public async Task<Contacts> GetContactsbyLeadId(int id)
        {
            return await _context.Contact.Where(x => x.leadsId == id).Include(x => x.resource.crmdesignations).Include(x => x.resource.designations).Include(x => x.leads).FirstOrDefaultAsync();
        }

        public async Task<Contacts> GetContactbyLeadId(int id)
        {
            return await _context.Contact.Where(x => x.leadsId == id && x.isLead==1).Include(x => x.resource.crmdesignations).Include(x => x.resource.crmdepartments).Include(x => x.leads).Include(x=>x.resource).Include(x=>x.resource.professionType).FirstOrDefaultAsync();
        }

        public async Task<Contacts> GetContactsbyLeadIdForLeadClient(int id)
        {
            return await _context.Contact.Where(x => x.leadsId == id && x.isLead==1).Include(x => x.resource.crmdesignations).Include(x => x.resource.designations).Include(x => x.leads).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Contacts>> GetOwnContactsbyLeadId(int id)
        {
            return await _context.Contact.Where(x => x.leadsId == id && x.isLead == 1).Include(x => x.resource.crmdesignations).Include(x => x.resource.designations).Include(x => x.leads).ToListAsync();
        }

        public async Task<Contacts> GetContactsById(int id)
        {
            return await _context.Contact.AsNoTracking().Include(x=>x.leads).Include(x=>x.resource).Include(x => x.resource.crmdesignations).Include(x => x.resource.crmdepartments).Where(x=>x.Id==id).FirstOrDefaultAsync();
        }

        public async Task<Contacts> GetContactsForPersonalLeadById(int id)
        {
            return await _context.Contact.AsNoTracking().Include(x => x.leads).Include(x => x.resource).Include(x => x.resource.crmdesignations).Include(x => x.resource.crmdepartments).Where(x => x.leadsId == id && x.isLead==1).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ContactsSpViewModel>> GetContactsSpViewModel(string TeamLeader, int Fa, string BD, string LeadId)
        {
            return await _context.GetContactsSpViewModels.FromSql($"getallcontact {TeamLeader},{Fa},{BD},{LeadId}").AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<BankContactsSpViewModel>> GetBankContactsSpViewModel(string TeamLeader, int Fa, string BD, string LeadId,int bankId,int branchId)
        {
            return await _context.GetBankContactsSpViewModels.FromSql($"getallcontactbank {TeamLeader},{Fa},{BD},{LeadId},{bankId},{branchId}").AsNoTracking().ToListAsync();
        }

        public async Task<bool> DeleteContactsById(int id)
        {
            _context.Contact.Remove(_context.Contact.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Contacts>> GetAllContactsByLeadsId(int leadId)
        {
            return await _context.Contact.AsNoTracking().Include(x => x.resource.crmdesignations).Include(x => x.resource.crmdepartments).Include(x => x.resource.professionType).Where(x => x.leadsId == leadId).ToListAsync();
        }

        public async Task<IEnumerable<Contacts>> GetAllContactsByLeadsIdforPersonal(int leadId)
        {
            return await _context.Contact.AsNoTracking().Include(x => x.resource.crmdesignations).Include(x => x.resource.crmdepartments).Where(x => x.leadsId == leadId && x.isLead != 1).ToListAsync();
        }

        public async Task<IEnumerable<Contacts>> GetAllContactsByLeadsIdforPersonalWithImage(int leadId)
        {
            var leads =await _context.Contact.AsNoTracking().Include(x => x.resource.crmdesignations).Include(x => x.resource.crmdepartments).Include(x=>x.leads).Where(x => x.leadsId == leadId && x.isLead != 1).ToListAsync();
            int? actiontypeId = 0;
            var data = new List<Contacts>();
            foreach (var item in leads)
            {                
                data.Add(new Contacts
                {
                    Id=item.Id,
                    contactOwner = item.contactOwner,
                    resource = item.resource,
                    leads = item.leads,
                    departmentsName = await _context.DocumentPhotoAttachments.Where(x => x.actionTypeId == item.Id && x.actionType == "Contact" && x.documentType == "photo" && x.moduleId==2).Select(x=>x.filePath).FirstOrDefaultAsync()
                });
            }
            return data;
        }

    }
}
