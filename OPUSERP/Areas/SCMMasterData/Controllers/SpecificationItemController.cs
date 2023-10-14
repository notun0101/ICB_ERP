using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.SCMMasterData.Models;
using OPUSERP.SCM.Data.Entity.MasterData;
using OPUSERP.SCM.Services.MasterData.Interfaces;

namespace OPUSERP.Areas.SCMMasterData.Controllers
{
    [Area("SCMMasterData")]
    public class SpecificationItemController : Controller
    {
        private readonly ISpecificationService _specificationService;
        private readonly IItemsService _itemsService;

        public SpecificationItemController(ISpecificationService specificationService, IItemsService itemsService)
        {
            _specificationService = specificationService;
            _itemsService = itemsService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new SpecificationViewModel
            {
                itemSpecifications = await _specificationService.GetSpecifications()
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditItem(SpecificationViewModel specification)
        {
            var spec = await _specificationService.GetSpecificationsById(specification.specId);

            spec.Id = specification.specId;
            spec.itemId = specification.itemId;
            spec.SKUNumber = specification.skuNumber;
            spec.specificationName = specification.specificationName;
            await _specificationService.SaveSpecification(spec);

            var item = await _itemsService.GetItemById(specification.itemId);
            item.Id = specification.itemId;
            item.itemName = specification.itemName;
            await _itemsService.SaveItem(item);

            return RedirectToAction(nameof(Index));
        }
    }
}