using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.OtherSales.Models;
using OPUSERP.CRM.Data.Entity.Lead;
using OPUSERP.CRM.Services.Lead.Interfaces;
using OPUSERP.Data.Entity.Master;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.OtherSales.Data.Entity.Sales;
using OPUSERP.OtherSales.Services.Sales.Interfaces;
using OPUSERP.Rental.Services.Sales.Interfaces;
using OPUSERP.SCM.Data.Entity.Supplier;
using OPUSERP.SCM.Services.Hospital.Interfaces;

namespace OPUSERP.Areas.HospitalManagement.Controllers
{
    [Area("HospitalManagement")]
    public class PatientController : Controller
    {
        private readonly ICustomerService customerService;
        private readonly IPatient patientService;
        private readonly IAddressService addressService;
       
        private readonly IAddressCategoryService addressCategoryService;
       
        private readonly IContactsService contactsService;
        private readonly IResourceService resourceService;
        private readonly ILeadsService leadsService;
        private readonly IERPModuleService eRPModuleService;
      
        private readonly IOSalesInvoiceMasterService salesInvoiceMasterService;
       



        public PatientController(IOSalesInvoiceMasterService salesInvoiceMasterService, IPatient patientService, IERPModuleService eRPModuleService, ILeadsService leadsService, IContactsService contactsService, IResourceService resourceService,  ICustomerService customerService, IAddressService addressService,  IAddressCategoryService addressCategoryService)
		{
            this.customerService = customerService;
            this.addressService = addressService;
            this.patientService = patientService;
            this.addressCategoryService = addressCategoryService;
            this.contactsService = contactsService;
            this.resourceService = resourceService;
            this.leadsService = leadsService;
            this.salesInvoiceMasterService = salesInvoiceMasterService;
            this.eRPModuleService = eRPModuleService;
      

        }
        //Patient List
        //public IActionResult Index()
        //      {
        //          return View();
        //      }
        [HttpGet]
        public async Task<IActionResult> Index(int? id, string actionType)
        {
            if (actionType == string.Empty || actionType == null)
            {
                actionType = "defaultOpen";
            }
            int viid = Convert.ToInt32(id);
            int relSupplierCustomerResourseId = 0;
            if (viid != 0)
            {
                var relSupplierCustomerResourse = await customerService.GetRelSupplierCustomerResourseByResourceId(viid);
                relSupplierCustomerResourseId = relSupplierCustomerResourse.Id;
            }
            try
            {
                var orgAddress = await addressService.GetAddressByResourceId(viid);
                if (orgAddress == null)
                {
                    orgAddress = new CRM.Data.Entity.Lead.Address();
                    orgAddress.division = new Division();
                    orgAddress.district = new District();
                    orgAddress.thana = new Thana();
                }
                ViewBag.addcount = orgAddress;
                CustomerViewModel model = new CustomerViewModel
                {
                    organizations = await customerService.GetAllOrganization(),
                 
                    GetResourcebyId = await customerService.GetResourceById(viid),
                    GetAddress = orgAddress,
                    relSupplierCustomerResourseId = relSupplierCustomerResourseId,
                    addressCategories = await addressCategoryService.GetAddressCategory()

                };
                model.actionType = actionType;
                if (model.GetResourcebyId == null) model.GetResourcebyId = new Resource();


                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] CustomerViewModel model)
        {
          
            var relresource = model.relSupplierCustomerResourseId;

            try
            {
                string userId = HttpContext.User.Identity.Name;

                var patientData = new SCM.Data.Entity.Hospital.Patient
				{
                    Id = Convert.ToInt32(model.resourceId),
                    nameEnglish = model.nameEnglish,
                    email = model.email,
                    alternativeEmail = model.alternativeEmail,
                    mobile = model.mobile,
                    phone = model.phone,
                    gender=model.gender,
                    bloodGroup=model.bloodGroup,
                    skypeId=model.skypeId,
                    linkedInId=model.linkedInId,
                    departments=model.departments,
                    maritalStatus=model.maritalStatus,
                    contactRelation=model.contactRelation
                };

                int rid = await patientService.SavePatient(patientData);
				
               
                //if (model.resourceId != 0)
                //{ 
                //    await customerService.DeleteAddressByResourceId(rid);
                //    await customerService.DeleteContactByResourceId(rid);
                //}

				Contact contact = new Contact
				{
					Id = 0,
					resourceId = rid,
					//personName = model.contactall[i],
					//Department = model.,
					//Designation = model.desall[i],
					phoneNumber = model.phone,
					mobileNumber = model.mobile,
					Department = model.contactRelation
				};
				int specid = await customerService.Savecontact(contact);

				CRM.Data.Entity.Lead.Address address = new CRM.Data.Entity.Lead.Address
				{
					Id = Convert.ToInt32(model.orgAddressID),
					resourceId = rid,
					countryId = 1,
					//addressCategoryId = model.addressCategoryId[i],
					//divisionId = Convert.ToInt32(model.orgDivision[i]),
					//districtId = Convert.ToInt32(model.orgDistrict[i]),
					//thanaId = Convert.ToInt32(model.orgUpazila[i]),
					//union = model.orgUnion[i],
					////postOffice = model.orgPostOffice[i],
					//postCode = model.orgPostCode[i],
					//blockSector = model.orgBlockSector[i],
					//houseVillage = model.orgHouseVillage[i],

					type = "Customer"
				};
				int vetId = await addressService.SaveAddress(address);

				return RedirectToAction("PatientList");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //Patient Create
        public async Task<IActionResult> PatientList()
        {
            var data = await customerService.GetAllRelSupplierPatientResourse();
            if (data == null)
            {
                data = new List<RelSupplierCustomerResourse>();
            }

            var dataaddress = await addressService.GetAddress();
            List<int?> relids = data.Select(x => x.resourceId).ToList();
            List<int?> leadsid = data.Select(x => x.LeadsId).ToList();
            dataaddress = dataaddress.Where(x => leadsid.Contains(x.leadsId));
            var contacts = await contactsService.GetAllContacts();
            contacts = contacts.Where(x => relids.Contains(x.resourceId));
            var resources = await resourceService.GetAllResource();
            resources = resources.Where(x => relids.Contains(x.Id));
            var model = new CustomerViewModel()
            {
                relSupplierCustomerResourses = data.Where(x => x.LeadsId != null),
                GetAddresses = dataaddress,
                Contacts = contacts,
                Resources = resources
            };

            return View(model);
        }
    }
}