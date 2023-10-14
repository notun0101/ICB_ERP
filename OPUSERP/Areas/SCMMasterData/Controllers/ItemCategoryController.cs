
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OPUSERP.Areas.SCMMasterData.Models;
using OPUSERP.Areas.SCMMasterData.Models.Lang;
using OPUSERP.Helpers;
using OPUSERP.SCM.Data.Entity.MasterData;
using OPUSERP.SCM.Services.MasterData.Interfaces;

namespace OPUSERP.Areas.SCMMasterData.Controllers
{
	[Area("SCMMasterData")]
	public class ItemCategoryController : Controller
	{
		private readonly LangGenerate<ItemCategoryLn> _lang;
		private readonly IItemsService ItemsService;
		private int Depth;
		public ItemCategoryController(IHostingEnvironment hostingEnvironment, IItemsService ItemsService)
		{
			_lang = new LangGenerate<ItemCategoryLn>(hostingEnvironment.ContentRootPath);
			this.ItemsService = ItemsService;

		}
		//GET: Item
		public async Task<IActionResult> Index()
		{
			IEnumerable<ItemCategory> itemCategorywithunit = await ItemsService.GetAllItemCategory();
			if (itemCategorywithunit == null)
			{
				itemCategorywithunit = new List<ItemCategory>();
			}

			

			var allCate = await ItemsService.GetAllItemCategory();
			var catParent = new List<ItemCategoryWithParent>();
			foreach (var item in allCate)
			{
				var cat = await ItemsService.GetAllItemCategoriesById(item.Id);
				catParent.Add(new ItemCategoryWithParent
				{
					itemCategory = item,
					fullparent = cat
				});
			}

			ItemCategoryViewModel model = new ItemCategoryViewModel
			{
				fLang = _lang.PerseLang("MasterData/ItemCategoryEN.json", "MasterData/ItemCategoryBN.json", Request.Cookies["lang"]),
				//itemCategories = await ItemsService.GetAllItemCategory()
				itemCategories = catParent
			};

			return View(model);
		}
		public async Task<IActionResult> getParentCategories(int Id)
		{
			//var str1 = "";

			//var cat = await ItemsService.GetParentCategoriesByCatId(Id);
			//var cat = await ItemsService.GetAllItemCategoriesByParentId(Id);
			var cat1 = await ItemsService.GetAllItemCategoriesById(Id);
			//var lst = new List<string>();
			//foreach (var item in cat)
			//{
			//	lst.Add(item.categoryName);
			//}
			//lst.Reverse();

			//foreach (var item in lst)
			//{
			//	str1 += item + ">";
			//}
			return Json(cat1);
		}
		// POST: Item/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Index([FromForm] ItemCategoryViewModel model)
		{

			//if (!ModelState.IsValid)
			//{
			//    model.fLang = _lang.PerseLang("MasterData/ItemEN.json", "MasterData/ItemBN.json", Request.Cookies["lang"]);
			//    model.items = await ItemsService.GetAllItem();
			//    return View(model);
			//}
			if (model.parentId == 0)
			{
				model.parentId = null;
				model.categoryPrefix = null;
			}
			ItemCategory itemcategory = new ItemCategory
			{
				Id = model.itemcategoryId,
				parentId = model.parentId,

				categoryName = model.categoryName,
				categoryPrefix = model.categoryPrefix,
				categoryDescription = model.categoryDescription,

				isDelete = 0,

				createdBy = HttpContext.User.Identity.Name,
				createdAt = DateTime.Now
			};

			try
			{
				int instId = await ItemsService.SaveItemCategory(itemcategory);
			}
			catch (Exception ex)
			{

				throw;
			}

			//if (itemsepccategorry.Count() == 0)
			//{
			//await ItemsService.DeleteSpecificationCategoryBycatId(instId);
			//var itemsepccategorry = await ItemsService.GetAllSpecificationCategorybycatid(instId);
			////if (model.descriptions.Length > 0)
			//if (model.descriptions != null)
			//{

			//    for (int i = 0; i < model.descriptions.Length; i++)
			//    {
			//        itemsepccategorry = itemsepccategorry.Where(x => x.specificationCategoryName == model.descriptions[i]).ToList();
			//        if (itemsepccategorry.Count() == 0)
			//        {
			//            SpecificationCategory itemcategoryspec = new SpecificationCategory
			//            {
			//                Id = 0,
			//                specificationCategoryName = model.descriptions[i],

			//                itemCategoryId = instId,

			//                isDelete = 0,

			//                createdBy = HttpContext.User.Identity.Name,
			//                createdAt = DateTime.Now
			//            };

			//            int instIds = await ItemsService.SaveSpecificationCategory(itemcategoryspec);
			//        }


			//    }
			//}

			//}

			return RedirectToAction(nameof(Index));

		}
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await ItemsService.DeleteItemCategoryById(id);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return RedirectToAction(nameof(Index));
			}
		}

		[HttpPost]
		public async Task<JsonResult> DeleteItemCategoryById(int Id)
		{
			await ItemsService.DeleteItemCategoryById(Id);
			return Json(true);
		}

		#region

		[HttpGet]
		[Route("/global/api/patentItemCategory/")]
		public async Task<JsonResult> GetParentItemCategory()
		{

			var result = await ItemsService.GetAllItemCategory();
			return Json(result);
		}


		[HttpGet]
		[Route("/global/api/ItemCategory/{id}")]
		public async Task<JsonResult> GetItemCategory(int id)
		{

			var result = await ItemsService.GetItemCategoryByParentId(id);
			return Json(result);
		}
		[HttpGet]
		[Route("/global/api/ItemSpecCategory/{id}")]
		public async Task<JsonResult> GetItemSpecCategory(int id)
		{

			var result = await ItemsService.GetAllSpecificationCategorybycatid(id);
			return Json(result);
		}

		#endregion
		#region tree
		[HttpGet]
		public async Task<IActionResult> GetCategoriesJson(int org)
		{
			Depth = 4;
			string s, tm;
			int Id = await ItemsService.GetRootId(org);

			tm = await this.GenerateTree(Id, "Start", 0);
			ItemCategory tempData = await ItemsService.GetItemCategoryById(Id);

			//if (tm == "")
			s = "{" + string.Format("\"data\":{0},\"name\":\"{1}\",\"nameBN\":\"{2}\",\"parent\":\"{3}\",\"head\":{4},\"children\":[{5}]", Id, tempData.categoryName, tempData.categoryName, "null", 23, tm) + "}";
			//else s = tm;

			dynamic data = new JObject();
			data.menus = s;
			data.depth = Depth;
			return Json(data);
		}
		private async Task<string> GenerateTree(int parrentid, string parrentName, int level)
		{
			int isHead = 2;
			Depth = Math.Max(level, Depth);
			string data = "";

			IEnumerable<ItemCategory> itemCategories = await ItemsService.GetCategoryByParrentId(parrentid);

			if (itemCategories.Count() <= 0) return data;
			int last = itemCategories.Last().Id;

			foreach (ItemCategory menu in itemCategories)
			{
				string child = await GenerateTree(menu.Id, menu.categoryName, level + 1);
				string name = menu.categoryName;

				string S = "{" + string.Format("\"data\":{0},\"name\":\"{1}\",\"nameBN\":\"{2}\",\"parent\":\"{3}\",\"head\":{4},\"children\":[{5}]", menu.Id, name, menu.categoryName, parrentid, isHead, child) + "}";

				if (menu.Id != last)
				{
					S += ",";
				}
				data += S;
			}
			return data;
		}
		#endregion
	}
}