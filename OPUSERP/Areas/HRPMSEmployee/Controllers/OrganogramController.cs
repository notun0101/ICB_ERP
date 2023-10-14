using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OPUSERP.Areas.HRPMSAssignment.Helpers;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Data.Entity;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.HRPMS.Data.Entity.Organogram;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.Organogram.Interfaces;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
	[Authorize]
	[Area("HRPMSEmployee")]
	public class OrganogramController : Controller
    {
		private readonly LangGenerate<Address> _lang;
		private readonly IEmployeeInfoService employeeInfoService;
		private readonly IWagesEmployeeInfoService wagesEmployeeInfoService;
		private readonly IPersonalInfoService personalInfoService;
		private readonly IPhotographService photographService;
		private readonly IWagesPersonalInfoService wagesPersonalInfoService;
		private readonly IAddressService addressService;
		private readonly IOrganizationTypeService organizationTypeService;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<ApplicationRole> _roleManager;

		public OrganogramController(IHostingEnvironment hostingEnvironment, IOrganizationTypeService organizationTypeService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPhotographService photographService, IWagesEmployeeInfoService wagesEmployeeInfoService, IPersonalInfoService personalInfoService, IEmployeeInfoService employeeInfoService, IWagesPersonalInfoService wagesPersonalInfoService, IAddressService addressService)
		{
			_lang = new LangGenerate<Address>(hostingEnvironment.ContentRootPath);
			this.employeeInfoService = employeeInfoService;
			this.personalInfoService = personalInfoService;
			this.photographService = photographService;
			this.wagesEmployeeInfoService = wagesEmployeeInfoService;
			this.wagesPersonalInfoService = wagesPersonalInfoService;
			this.addressService = addressService;
			this.organizationTypeService = organizationTypeService;
			_roleManager = roleManager;
			_userManager = userManager;
		}
		public IActionResult Index()
        {
            return View();
        }

		public async Task<IActionResult> OrganogramView()
		{
			return View();
		}
		public JsonResult OrganogramApi()
		{
			List<OrganogramRelationViewModel> organograms = new List<OrganogramRelationViewModel>();

			var data = organizationTypeService.GetAllOrganogramList();

			foreach (var item in data)
			{
				var img = organizationTypeService.GetPhotoByEmpId(Convert.ToInt32(item.headId));
				organograms.Add(new OrganogramRelationViewModel()
				{
					Id = item.Id,
					title = item.title,
					subTitle = item.subTitle,
					url = img,
					children = organizationTypeService.GetChildrenByOrganogramId(item.Id),
					parentId = item.parentId == null ? 0 : item.parentId,
					//isActive = item.isActive == null ? 0 : item.isActive
				});
			}

			//return Json(categories, JsonRequestBehavior.AllowGet);
			return Json(organograms);
		}
        public async Task<IActionResult> OrganogramListView()
        {
            var model = new OrganogramViewModel()
            {
                organogramRelations = await organizationTypeService.GetAllOrganogram()
            };
            return View(model);
        }

        public async Task<IActionResult> OrganogramSet()
        {
            var model = new OrganogramViewModel()
            {
                organogramRelations = await organizationTypeService.GetAllOrganogram()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrganogramSet([FromForm] OrganogramViewModel model)
        {
            try
            {
                // var emp = await personalInfoService.GetEmployeeInfoById((int)model.employeeId);
                string PhotoUrl;
                var Photo = await photographService.GetPhotographByEmpIdAndType((int)model.employeeId, "profile");
                if (Photo == null )
                {
                    PhotoUrl = "";
                }
                else
                {
                    PhotoUrl = Photo.url;
                }

                OrganogramRelation data = new OrganogramRelation
                {
                    Id = (int)model.OrgRelationId,
                    title = model.title,
                    subTitle = model.subTitle,
                    parentId = model.parentId,
                    isDelete = model.isActive,
                    headId = model.employeeId,
                    url = PhotoUrl,
                };
                if (model.officeId != null)
                {
                    data.officeId = model.officeId;
                }
                else if (model.hrdivisionId != null)
                {
                    data.divisionId = model.hrdivisionId;
                }
                else if (model.departmentId != null)
                {
                    data.departmentId = model.departmentId;
                }
                else
                {
                    return RedirectToAction(nameof(OrganogramSet));
                }
                
                var chilId = await organizationTypeService.SaveOrganogramRelation(data);
                if (model.OrgChildId != null)
                {
                    for (int i = 0; i < model.OrgChildId.Length; i++)
                    {
                        var child = new OrganogramChild
                        {
                            Id = (int)model.OrgChildId[i],
                            OrgRelationId = chilId,
                            designationId = model.designationId[i],
                            memberCount = model.memberCount[i],
                           
                        };
                        
                        await organizationTypeService.SaveOrganogramChild(child);

                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }

            return RedirectToAction(nameof(OrganogramSet));
        }


        public async Task<IActionResult> Delete(int id)
        {
            await organizationTypeService.DeleteOrgChildrenByRelationId(id);
            await organizationTypeService.DeleteOrgRelationById(id);
            return RedirectToAction(nameof(OrganogramSet));
        }
        public async Task<IActionResult> GetOrganogramChildById(int id)
        {
            var data = await organizationTypeService.GetChildrenByOrganogramId1(id);
            return Json(data);
        }

         public async Task<IActionResult> GetEmployeeInfoById(int id)
        {
            var data = await personalInfoService.GetEmployeeInfoById(id); 
            return Json(data);
        }


        public async Task<IActionResult> DeleteOrganogramChild(int id)
        {
            var data = await organizationTypeService.DeleteOrgChildrenById(id);
            return Json(data);
        }
    }
}