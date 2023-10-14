using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.Models;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using ServiceReference1;

namespace OPUSERP.Controllers
{
	[Authorize]
	public class CBSPostingController : Controller
	{
		private readonly DataInsertMainClient client = new DataInsertMainClient();
		private readonly ICBSPostingService cBSPostingService;
		private readonly ISalaryService salaryService;
		private readonly IERPCompanyService eRPCompanyService;
		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly string rootPath;
		private readonly MyPDF myPDF;
		public string FileName;
		private readonly string userId;
		private readonly string password;
		private readonly IConfiguration _configuration;
		private readonly string chkSum;
		public CBSPostingController(DataInsertMainClient client, IConfiguration configuration, IERPCompanyService eRPCompanyService, IHostingEnvironment _hostingEnvironment, IConverter converter, ISalaryService salaryService, ICBSPostingService cBSPostingService)
		{
			this.client = client;
			this.salaryService = salaryService;
			this.cBSPostingService = cBSPostingService;
			this.eRPCompanyService = eRPCompanyService;

			this._hostingEnvironment = _hostingEnvironment;
			myPDF = new MyPDF(_hostingEnvironment, converter);
			rootPath = _hostingEnvironment.ContentRootPath;

			_configuration = configuration;
			//Test
			//this.userId = "EHR";
			//this.password = "123456";
			//this.chkSum = "2731C9E8471B1B1F6E977E7A00ABEC4EE8376268";

			//Live
			this.userId = "EHR";
			this.password = "6543210";
			this.chkSum = "2731C9E8471B1B1F6E977E7A00ABEC4EE8376268";
		}

		public IActionResult Index()
		{
			return View();
		}


		public async Task<IActionResult> CBSProcess()
		{
			var model = new CBSProcessViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriod(),
				hrBranches = await salaryService.GetAllHrBranchs(),
				zones = await salaryService.GetAllZones(),
				sbus = await salaryService.GetAllSbus()
			};
			return View(model);
		}

		public async Task<IActionResult> GetAllPostedVoucherApi(int periodId)
		{
			var data = await cBSPostingService.GetAllPostedVoucher(periodId);
			return Json(data);
		}

		public async Task<IActionResult> GetAllPostedSalaryApi(int periodId)
		{
			var data = await cBSPostingService.GetAllPostedSalary(periodId);
			return Json(data);
		}
		public async Task<IActionResult> GetTypeByPeriodId(int Id)
		{
			var data = await cBSPostingService.GetTypeByPeriodId(Id);
			return Json(data);
		}

		public async Task<IActionResult> GetAllPostedUnionApi(int periodId)
		{
			var data = await cBSPostingService.GetAllPostedUnion(periodId);
			return Json(data);
		}
		public async Task<IActionResult> GetAllPostedPayOrderApi(int periodId)
		{
			var data = await cBSPostingService.GetAllPostedPayOrder(periodId);
			return Json(data);
		}

		public async Task<IActionResult> GetProcessedVoucherApi(int periodId)
		{
			var data = new CBSProcessViewModel
			{
				processedVouchersVms = await cBSPostingService.GetProcessedVoucher(periodId)
			};
			return Json(data);
		}

		public async Task<IActionResult> GetProcessedSalaryApi(int periodId)
		{
			var data = new CBSProcessViewModel
			{
				processedSalaryVms = await cBSPostingService.GetProcessedSalary(periodId)
			};
			return Json(data);
		}

		[HttpPost]
		public async Task<IActionResult> CBSProcess(CBSProcessViewModel model)
		{
			if (model.empCode == null)
			{
				model.empCode = "0";
			}
			if (model.Type == "Salary")
			{
				if (model.hrBranchId != 1000)
				{
					await salaryService.CBSProcessSalarySheetByYearMonthAndSBU(Convert.ToInt32(model.periodId), model.sbu, model.empCode, Convert.ToInt32(model.hrBranchId));
				}
				else
				{
					await salaryService.CBSProcessAllSalary(Convert.ToInt32(model.periodId));

					await cBSPostingService.GenerateSalaryXML(Convert.ToInt32(model.periodId));
					await cBSPostingService.GenerateUnionXML(Convert.ToInt32(model.periodId));
				}
			}
			if (model.Type == "Voucher")
			{
				if (model.hrBranchId != 1000)
				{
					await salaryService.CBSProcessSalaryVoucher(Convert.ToInt32(model.periodId), Convert.ToInt32(model.hrBranchId), Convert.ToInt32(model.hrBranchId) == 205 ? "headoffice" : "hrBranch");
				}
				else
				{
					await salaryService.CBSProcessAllVoucher(Convert.ToInt32(model.periodId));

					await cBSPostingService.GenerateVoucherXML(Convert.ToInt32(model.periodId));

					//var allBranches = await salaryService.GetAllHrBranchs();
					//foreach (var item in allBranches.Where(x => x.Id != 205))
					//{
					//	await salaryService.CBSProcessSalaryVoucher(Convert.ToInt32(model.periodId), Convert.ToInt32(item.Id), Convert.ToInt32(item.Id) == 205 ? "headoffice" : "hrBranch");
					//}
				}
			}
			if (model.Type == "Loan")
			{
				await salaryService.CBSProcessLoan(Convert.ToInt32(model.periodId), Convert.ToInt32(model.hrBranchId));
			}

			return RedirectToAction(nameof(CBSProcess));
		}

		public async Task<IActionResult> ProcessVoucherByBranchApi(int hrbranchId, int periodId)
		{
			var data = await salaryService.CBSProcessSalaryVoucher(periodId, hrbranchId, hrbranchId == 205 ? "headoffice" : "hrBranch");
			return Json(data);
		}

		public async Task<IActionResult> GetProcessedSalaryLog(int periodId)
		{
			var data = await salaryService.GetProcessedSalaryLog(periodId);
			return Json(data);
		}

		public async Task<IActionResult> GetAllHrBranches()
		{
			var data = await salaryService.GetAllHrBranchs();
			return Json(data);
		}


		public async Task<IActionResult> PostToCBS()
		{
			var model = new CBSProcessViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriod(),
				hrBranches = await salaryService.GetAllHrBranchs(),
				zones = await salaryService.GetAllZones(),
				sbus = await salaryService.GetAllSbus(),
			};
			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> PostSalaryToCBS(CBSProcessViewModel vm)
		{
			string userName = HttpContext.User.Identity.Name;
			var hrBranch = await salaryService.GetHrBranchById(Convert.ToInt32(vm.hrBranchId));
			var salaryPeriod = await salaryService.GetSalaryPeriodById(Convert.ToInt32(vm.periodId));

			var checkPostedLog = await cBSPostingService.checkSalaryPostedLog(Convert.ToInt32(vm.hrBranchId), Convert.ToInt32(vm.periodId), "SALARY");
			if (checkPostedLog != null)
			{
				return Json("Already Posted");
			}
			else
			{

				var xmlData = await cBSPostingService.GetCBSXMLData(salaryPeriod.salaryYear.yearName, salaryPeriod.monthName, vm.sbu, vm.empCode == null ? "0" : vm.empCode, hrBranch.branchCode);

				if (xmlData.Count() > 0)
				{
					try
					{
						var model = new CBSDataInsertModel
						{
							in_user_id = _configuration["CBS:user_id"],
							in_Auth_Key = _configuration["CBS:Password"],
							in_ChkSum = _configuration["CBS:ChkSum"],
							in_request_id = _configuration["CBS:request_id"],
							in_xml_data = xmlData.FirstOrDefault().XML_DATA,
							in_Bin_data = "",
							in_request_type = "req_transfer",
							in_Add_Param_One = "100",
							in_Add_Param_Two = ""
						};
						var test = await client.dataInsertAsync(model.in_user_id, model.in_Auth_Key, model.in_ChkSum, model.in_request_id, model.in_xml_data, model.in_Bin_data, model.in_request_type, model.in_Add_Param_One, model.in_Add_Param_Two);
						if (test.Body.dataInsertReturn.out_Status == "Y")
						{
							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

							XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
							XmlNodeList TRN_REF = xmlDoc.GetElementsByTagName("TRN_REF");
							XmlNodeList MESSAGE = xmlDoc.GetElementsByTagName("MESSAGE");

							string successResult = ISSUCCESS[0].InnerXml;
							string tranrefResult = TRN_REF[0].InnerXml;
							string messageResult = MESSAGE[0].InnerXml;

							await cBSPostingService.UpdateCBSVoucherStatus(xmlData.FirstOrDefault().uniqueKey, "POSTED");
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, null, hrBranch.branchCode, "SALARY", xmlData.FirstOrDefault().uniqueKey, successResult, tranrefResult, messageResult);
							return Json("Success");
						}
						else
						{
							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

							XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
							XmlNodeList REASON = xmlDoc.GetElementsByTagName("REASON");
							XmlNodeList WARNING = xmlDoc.GetElementsByTagName("WARNING");

							string successResult = ISSUCCESS[0].InnerXml;
							string reasonResult = REASON[0].InnerXml;
							string warningResult = WARNING[0].InnerXml;

							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, null, hrBranch.branchCode, "SALARY", xmlData.FirstOrDefault().uniqueKey, successResult, reasonResult, warningResult);
							return Json("Failed-" + warningResult);
						}
					}
					catch (Exception ex)
					{
						await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, null, hrBranch.branchCode, "SALARY", xmlData.FirstOrDefault().uniqueKey, null, null, ex.Message);
						return Json("Something Wrong-" + ex.Message);
					}
				}
				else
				{
					await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, null, hrBranch.branchCode, "SALARY", null, null, null, "Process Incomplete");
					return Json("Process Not Done");
				}
			}
		}

		public async Task<IActionResult> PostAllToCBSInd(CBSProcessViewModel vm)
		{
			var result = new List<CBSVoucherPostingResponseVm>();

			string userName = HttpContext.User.Identity.Name;
			var salaryPeriod = await salaryService.GetSalaryPeriodById(Convert.ToInt32(vm.periodId));

			var processAllInd = await cBSPostingService.ProcessAllForIndividual(Convert.ToInt32(vm.periodId));


			//Voucher Posting
			var VoucherXml = await cBSPostingService.GetVoucherXmlInd(Convert.ToInt32(vm.periodId));

			var checkPostedLogV = await cBSPostingService.checkSalaryPostedLog(11, Convert.ToInt32(vm.periodId), "INDVOUCHER");
			if (checkPostedLogV != null)
			{
				result.Add(new CBSVoucherPostingResponseVm
				{
					status = 1,
					trxcode = checkPostedLogV,
					message = "Previous Posted",
					branchCode = null,
					branchName = "BDBL Securities Ltd, Ctg"
				});
			}
			else
			{
				try
				{
					var model = new CBSDataInsertModel
					{
						in_user_id = _configuration["CBS:user_id"],
						in_Auth_Key = _configuration["CBS:Password"],
						in_ChkSum = _configuration["CBS:ChkSum"],
						in_request_id = _configuration["CBS:request_id"],
						in_xml_data = VoucherXml.XML_DATA,
						in_Bin_data = "",
						in_request_type = "req_transfer",
						in_Add_Param_One = "100",
						in_Add_Param_Two = ""
					};

					var test = await client.dataInsertAsync(model.in_user_id, model.in_Auth_Key, model.in_ChkSum, model.in_request_id, model.in_xml_data, model.in_Bin_data, model.in_request_type, model.in_Add_Param_One, model.in_Add_Param_Two);
					if (test.Body.dataInsertReturn.out_Status == "Y")
					{
						XmlDocument xmlDoc = new XmlDocument();
						xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

						XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
						XmlNodeList TRN_REF = xmlDoc.GetElementsByTagName("TRN_REF");
						XmlNodeList MESSAGE = xmlDoc.GetElementsByTagName("MESSAGE");

						string successResult = ISSUCCESS[0].InnerXml;
						string tranrefResult = TRN_REF[0].InnerXml;
						string messageResult = MESSAGE[0].InnerXml;

						await cBSPostingService.UpdateCBSVoucherStatus(VoucherXml.uniqueKey, "POSTED");
						await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), null, null, VoucherXml.BranchId, "", "INDVOUCHER", VoucherXml.uniqueKey, successResult, tranrefResult, messageResult);
						result.Add(new CBSVoucherPostingResponseVm
						{
							status = 1,
							trxcode = tranrefResult,
							message = messageResult,
							branchCode = "",
							branchName = "BDBL Securities Ltd, Ctg"
						});
					}
					else
					{
						XmlDocument xmlDoc = new XmlDocument();
						xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

						XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
						XmlNodeList REASON = xmlDoc.GetElementsByTagName("REASON");
						XmlNodeList WARNING = xmlDoc.GetElementsByTagName("WARNING");

						string successResult = ISSUCCESS[0].InnerXml;
						string reasonResult = REASON[0].InnerXml;
						string warningResult = WARNING[0].InnerXml;

						await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), null, null, VoucherXml.BranchId, "", "INDVOUCHER", VoucherXml.uniqueKey, successResult, reasonResult, warningResult);

						result.Add(new CBSVoucherPostingResponseVm
						{
							status = 0,
							trxcode = reasonResult,
							message = warningResult,
							branchCode = VoucherXml.branchCode,
							branchName = VoucherXml.branchName
						});
					}
				}
				catch (Exception ex)
				{

				}
			}

			//Salary Posting
			var SalaryXml = await cBSPostingService.GetSalaryXmlInd(Convert.ToInt32(vm.periodId));

			var checkPostedLogS = await cBSPostingService.checkSalaryPostedLog(11, Convert.ToInt32(vm.periodId), "INDSALARY");
			if (checkPostedLogS != null)
			{
				result.Add(new CBSVoucherPostingResponseVm
				{
					status = 1,
					trxcode = checkPostedLogS,
					message = "Previous Posted",
					branchCode = null,
					branchName = "BDBL Securities Ltd, Ctg"
				});
			}
			else
			{
				try
				{
					var model = new CBSDataInsertModel
					{
						in_user_id = _configuration["CBS:user_id"],
						in_Auth_Key = _configuration["CBS:Password"],
						in_ChkSum = _configuration["CBS:ChkSum"],
						in_request_id = _configuration["CBS:request_id"],
						in_xml_data = SalaryXml.XML_DATA,
						in_Bin_data = "",
						in_request_type = "req_transfer",
						in_Add_Param_One = "100",
						in_Add_Param_Two = ""
					};

					var test = await client.dataInsertAsync(model.in_user_id, model.in_Auth_Key, model.in_ChkSum, model.in_request_id, model.in_xml_data, model.in_Bin_data, model.in_request_type, model.in_Add_Param_One, model.in_Add_Param_Two);
					if (test.Body.dataInsertReturn.out_Status == "Y")
					{
						XmlDocument xmlDoc = new XmlDocument();
						xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

						XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
						XmlNodeList TRN_REF = xmlDoc.GetElementsByTagName("TRN_REF");
						XmlNodeList MESSAGE = xmlDoc.GetElementsByTagName("MESSAGE");

						string successResult = ISSUCCESS[0].InnerXml;
						string tranrefResult = TRN_REF[0].InnerXml;
						string messageResult = MESSAGE[0].InnerXml;

						await cBSPostingService.UpdateCBSVoucherStatus(SalaryXml.uniqueKey, "POSTED");
						await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), null, null, SalaryXml.BranchId, "", "INDSALARY", SalaryXml.uniqueKey, successResult, tranrefResult, messageResult);
						result.Add(new CBSVoucherPostingResponseVm
						{
							status = 1,
							trxcode = tranrefResult,
							message = messageResult,
							branchCode = "",
							branchName = "BDBL Securities Ltd, Ctg"
						});
					}
					else
					{
						XmlDocument xmlDoc = new XmlDocument();
						xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

						XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
						XmlNodeList REASON = xmlDoc.GetElementsByTagName("REASON");
						XmlNodeList WARNING = xmlDoc.GetElementsByTagName("WARNING");

						string successResult = ISSUCCESS[0].InnerXml;
						string reasonResult = REASON[0].InnerXml;
						string warningResult = WARNING[0].InnerXml;

						await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), null, null, SalaryXml.BranchId, "", "INDSALARY", SalaryXml.uniqueKey, successResult, reasonResult, warningResult);

						result.Add(new CBSVoucherPostingResponseVm
						{
							status = 0,
							trxcode = reasonResult,
							message = warningResult,
							branchCode = SalaryXml.branchCode,
							branchName = SalaryXml.branchName
						});
					}
				}
				catch (Exception ex)
				{

				}
			}

			//Union Posting
			var UnionXml = await cBSPostingService.GetUnionXmlInd(Convert.ToInt32(vm.periodId));

			var checkPostedLogU = await cBSPostingService.checkSalaryPostedLog(11, Convert.ToInt32(vm.periodId), "INDUNION");
			if (checkPostedLogU != null)
			{
				result.Add(new CBSVoucherPostingResponseVm
				{
					status = 1,
					trxcode = checkPostedLogU,
					message = "Previous Posted",
					branchCode = null,
					branchName = "BDBL Securities Ltd, Ctg"
				});
			}
			else
			{
				try
				{
					var model = new CBSDataInsertModel
					{
						in_user_id = _configuration["CBS:user_id"],
						in_Auth_Key = _configuration["CBS:Password"],
						in_ChkSum = _configuration["CBS:ChkSum"],
						in_request_id = _configuration["CBS:request_id"],
						in_xml_data = UnionXml.XML_DATA,
						in_Bin_data = "",
						in_request_type = "req_transfer",
						in_Add_Param_One = "100",
						in_Add_Param_Two = ""
					};

					var test = await client.dataInsertAsync(model.in_user_id, model.in_Auth_Key, model.in_ChkSum, model.in_request_id, model.in_xml_data, model.in_Bin_data, model.in_request_type, model.in_Add_Param_One, model.in_Add_Param_Two);
					if (test.Body.dataInsertReturn.out_Status == "Y")
					{
						XmlDocument xmlDoc = new XmlDocument();
						xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

						XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
						XmlNodeList TRN_REF = xmlDoc.GetElementsByTagName("TRN_REF");
						XmlNodeList MESSAGE = xmlDoc.GetElementsByTagName("MESSAGE");

						string successResult = ISSUCCESS[0].InnerXml;
						string tranrefResult = TRN_REF[0].InnerXml;
						string messageResult = MESSAGE[0].InnerXml;

						await cBSPostingService.UpdateCBSVoucherStatus(VoucherXml.uniqueKey, "POSTED");
						await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), null, null, UnionXml.BranchId, "", "INDUNION", UnionXml.uniqueKey, successResult, tranrefResult, messageResult);
						result.Add(new CBSVoucherPostingResponseVm
						{
							status = 1,
							trxcode = tranrefResult,
							message = messageResult,
							branchCode = "",
							branchName = "BDBL Securities Ltd, Ctg"
						});
					}
					else
					{
						XmlDocument xmlDoc = new XmlDocument();
						xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

						XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
						XmlNodeList REASON = xmlDoc.GetElementsByTagName("REASON");
						XmlNodeList WARNING = xmlDoc.GetElementsByTagName("WARNING");

						string successResult = ISSUCCESS[0].InnerXml;
						string reasonResult = REASON[0].InnerXml;
						string warningResult = WARNING[0].InnerXml;

						await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), null, null, UnionXml.BranchId, "", "INDUNION", UnionXml.uniqueKey, successResult, reasonResult, warningResult);

						result.Add(new CBSVoucherPostingResponseVm
						{
							status = 0,
							trxcode = reasonResult,
							message = warningResult,
							branchCode = UnionXml.branchCode,
							branchName = UnionXml.branchName
						});
					}
				}
				catch (Exception ex)
				{

				}
			}

			return Json(result);
		}

		[HttpPost]
		public async Task<IActionResult> PostIndSalaryToCBS(CBSProcessViewModel vm)
		{
			var result = new List<CBSVoucherPostingResponseVm>();

			string userName = HttpContext.User.Identity.Name;
			var salaryPeriod = await salaryService.GetSalaryPeriodById(Convert.ToInt32(vm.periodId));


			var allSalaryXmls = await cBSPostingService.GetAllCBSSalaryXmls(Convert.ToInt32(vm.periodId));

			foreach (var item in allSalaryXmls)
			{
				var checkPostedLog = await cBSPostingService.checkSalaryPostedLog(Convert.ToInt32(item.BranchId), Convert.ToInt32(vm.periodId), "SALARY");
				if (checkPostedLog != null)
				{
					result.Add(new CBSVoucherPostingResponseVm
					{
						status = 1,
						trxcode = checkPostedLog,
						message = "Previous Posted",
						branchCode = item.branchCode,
						branchName = item.branchName
					});
				}
				else
				{
					try
					{
						var model = new CBSDataInsertModel
						{
							in_user_id = _configuration["CBS:user_id"],
							in_Auth_Key = _configuration["CBS:Password"],
							in_ChkSum = _configuration["CBS:ChkSum"],
							in_request_id = _configuration["CBS:request_id"],
							in_xml_data = item.XML_DATA,
							in_Bin_data = "",
							in_request_type = "req_transfer",
							in_Add_Param_One = "100",
							in_Add_Param_Two = ""
						};

						var test = await client.dataInsertAsync(model.in_user_id, model.in_Auth_Key, model.in_ChkSum, model.in_request_id, model.in_xml_data, model.in_Bin_data, model.in_request_type, model.in_Add_Param_One, model.in_Add_Param_Two);

						if (test.Body.dataInsertReturn.out_Status == "Y")
						{
							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

							XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
							XmlNodeList TRN_REF = xmlDoc.GetElementsByTagName("TRN_REF");
							XmlNodeList MESSAGE = xmlDoc.GetElementsByTagName("MESSAGE");

							string successResult = ISSUCCESS[0].InnerXml;
							string tranrefResult = TRN_REF[0].InnerXml;
							string messageResult = MESSAGE[0].InnerXml;

							//<QRYRESULT><ISSUCCESS>Y</ISSUCCESS><TRN_REF>100MB000070</TRN_REF><MESSAGE>Transaction Completed</MESSAGE></QRYRESULT>
							await cBSPostingService.UpdateCBSVoucherStatus(item.uniqueKey, "POSTED");
							if (item.type == "zone")
							{
								await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), null, item.BranchId, null, item.branchCode, "SALARY", item.uniqueKey, successResult, tranrefResult, messageResult);
							}
							else
							{
								await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), item.BranchId, null, null, item.branchCode, "SALARY", item.uniqueKey, successResult, tranrefResult, messageResult);
							}
							result.Add(new CBSVoucherPostingResponseVm
							{
								status = 1,
								trxcode = tranrefResult,
								message = messageResult,
								branchCode = item.branchCode,
								branchName = item.branchName
							});
						}
						else
						{
							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

							XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
							XmlNodeList REASON = xmlDoc.GetElementsByTagName("REASON");
							XmlNodeList WARNING = xmlDoc.GetElementsByTagName("WARNING");

							string successResult = ISSUCCESS[0].InnerXml;
							string reasonResult = REASON[0].InnerXml;
							string warningResult = WARNING[0].InnerXml;

							//<QRYRESULT><ISSUCCESS>Y</ISSUCCESS><TRN_REF>100MB000070</TRN_REF><MESSAGE>Transaction Completed</MESSAGE></QRYRESULT>
							if (item.type == "zone")
							{
								await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), null, item.BranchId, null, item.branchCode, "SALARY", item.uniqueKey, successResult, reasonResult, warningResult);
							}
							else
							{
								await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), item.BranchId, null, null, item.branchCode, "SALARY", item.uniqueKey, successResult, reasonResult, warningResult);
							}

							result.Add(new CBSVoucherPostingResponseVm
							{
								status = 0,
								trxcode = reasonResult,
								message = warningResult,
								branchCode = item.branchCode,
								branchName = item.branchName
							});
						}
					}
					catch (Exception ex)
					{
						if (item.type == "zone")
						{
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), null, item.BranchId, null, item.branchCode, "SALARY", item.uniqueKey, null, null, ex.Message);
						}
						else
						{
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), item.BranchId, null, null, item.branchCode, "SALARY", item.uniqueKey, null, null, ex.Message);
						}

						result.Add(new CBSVoucherPostingResponseVm
						{
							status = 0,
							trxcode = "",
							message = "Something Wrong",
							branchCode = item.branchCode,
							branchName = item.branchName
						});
					}
				}




			}

			//var allHrBranches = await salaryService.GetAllHrBranchs();
			//foreach (var hrBranch in allHrBranches)
			//{
			//	var checkPostedLog = await cBSPostingService.checkSalaryPostedLog(Convert.ToInt32(hrBranch.Id), Convert.ToInt32(vm.periodId), "SALARY");
			//	if (checkPostedLog != null)
			//	{
			//		result.Add(new CBSVoucherPostingResponseVm
			//		{
			//			status = 1,
			//			trxcode = checkPostedLog,
			//			message = "Previous Posted",
			//			branchCode = hrBranch.branchCode,
			//			branchName = hrBranch.branchName
			//		});
			//	}
			//	else
			//	{
			//		IEnumerable<CBSXMLDataViewModel> xmlData = new List<CBSXMLDataViewModel>();
			//		if (vm.hrBranchId == 205)
			//		{
			//			xmlData = await cBSPostingService.GetCBSXMLDataHeadOffice(salaryPeriod.salaryYear.yearName, salaryPeriod.monthName, vm.sbu, vm.empCode == null ? "0" : vm.empCode, hrBranch.branchCode);
			//		}
			//		else
			//		{
			//			xmlData = await cBSPostingService.GetCBSXMLData(salaryPeriod.salaryYear.yearName, salaryPeriod.monthName, vm.sbu, vm.empCode == null ? "0" : vm.empCode, hrBranch.branchCode);
			//		}
			//		//var in_xml_data = "<![CDATA[" + xmlData.FirstOrDefault().XML_DATA + "]]>";

			//		if (xmlData.Count() > 0)
			//		{
			//			try
			//			{
			//				var model = new CBSDataInsertModel
			//				{
			//					in_user_id = "EHR",
			//					in_Auth_Key = _configuration["CBS:Password"],
			//					in_ChkSum = "2731C9E8471B1B1F6E977E7A00ABEC4EE8376268",
			//					in_request_id = "HR1",
			//					in_xml_data = xmlData.FirstOrDefault().XML_DATA,
			//					in_Bin_data = "",
			//					in_request_type = "req_transfer",
			//					in_Add_Param_One = "100",
			//					in_Add_Param_Two = ""
			//				};

			//				var test = await client.dataInsertAsync(model.in_user_id, model.in_Auth_Key, model.in_ChkSum, model.in_request_id, model.in_xml_data, model.in_Bin_data, model.in_request_type, model.in_Add_Param_One, model.in_Add_Param_Two);

			//				if (test.Body.dataInsertReturn.out_Status == "Y")
			//				{
			//					XmlDocument xmlDoc = new XmlDocument();
			//					xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

			//					XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
			//					XmlNodeList TRN_REF = xmlDoc.GetElementsByTagName("TRN_REF");
			//					XmlNodeList MESSAGE = xmlDoc.GetElementsByTagName("MESSAGE");

			//					string successResult = ISSUCCESS[0].InnerXml;
			//					string tranrefResult = TRN_REF[0].InnerXml;
			//					string messageResult = MESSAGE[0].InnerXml;

			//					//<QRYRESULT><ISSUCCESS>Y</ISSUCCESS><TRN_REF>100MB000070</TRN_REF><MESSAGE>Transaction Completed</MESSAGE></QRYRESULT>
			//					await cBSPostingService.UpdateCBSVoucherStatus(xmlData.FirstOrDefault().uniqueKey, "SALARY POSTED");
			//					await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, hrBranch.branchCode, "SALARY", xmlData.FirstOrDefault().uniqueKey, successResult, tranrefResult, messageResult);
			//					result.Add(new CBSVoucherPostingResponseVm
			//					{
			//						status = 1,
			//						trxcode = tranrefResult,
			//						message = messageResult,
			//						branchCode = hrBranch.branchCode,
			//						branchName = hrBranch.branchName
			//					});
			//				}
			//				else
			//				{
			//					XmlDocument xmlDoc = new XmlDocument();
			//					xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

			//					XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
			//					XmlNodeList REASON = xmlDoc.GetElementsByTagName("REASON");
			//					XmlNodeList WARNING = xmlDoc.GetElementsByTagName("WARNING");

			//					string successResult = ISSUCCESS[0].InnerXml;
			//					string reasonResult = REASON[0].InnerXml;
			//					string warningResult = WARNING[0].InnerXml;

			//					//<QRYRESULT><ISSUCCESS>Y</ISSUCCESS><TRN_REF>100MB000070</TRN_REF><MESSAGE>Transaction Completed</MESSAGE></QRYRESULT>
			//					await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, hrBranch.branchCode, "SALARY", xmlData.FirstOrDefault().uniqueKey, successResult, reasonResult, warningResult);
			//					result.Add(new CBSVoucherPostingResponseVm
			//					{
			//						status = 0,
			//						trxcode = reasonResult,
			//						message = warningResult,
			//						branchCode = hrBranch.branchCode,
			//						branchName = hrBranch.branchName
			//					});
			//				}
			//			}
			//			catch (Exception ex)
			//			{
			//				await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, hrBranch.branchCode, "SALARY", xmlData.FirstOrDefault().uniqueKey, null, null, ex.Message);
			//				result.Add(new CBSVoucherPostingResponseVm
			//				{
			//					status = 0,
			//					trxcode = "",
			//					message = "Something Wrong",
			//					branchCode = hrBranch.branchCode,
			//					branchName = hrBranch.branchName
			//				});
			//			}
			//		}
			//		else
			//		{
			//			await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, hrBranch.branchCode, "SALARY", null, null, null, "Process Incomplete");
			//			result.Add(new CBSVoucherPostingResponseVm
			//			{
			//				status = 0,
			//				trxcode = "",
			//				message = "Process Not Done",
			//				branchCode = hrBranch.branchCode,
			//				branchName = hrBranch.branchName
			//			});
			//		}
			//	}
			//}

			return Json(result);
		}

		public async Task<IActionResult> ProcessAllSalary(CBSProcessViewModel model)
		{
			var result = "";
			if (model.hrBranchId != null)
			{
				var branches = await salaryService.GetAllHrBranchs();
				foreach (var item in branches)
				{
					if (model.empCode == null)
					{
						model.empCode = "0";
					}
					if (model.Type == "Salary")
					{
						await salaryService.CBSProcessSalarySheetByYearMonthAndSBU(Convert.ToInt32(model.periodId), model.sbu, model.empCode, Convert.ToInt32(model.hrBranchId));
					}
					if (model.Type == "Voucher")
					{
						var status = await salaryService.CBSProcessSalaryVoucher(Convert.ToInt32(model.periodId), Convert.ToInt32(model.hrBranchId), Convert.ToInt32(model.hrBranchId) == 205 ? "headoffice" : "hrBranch");
					}
				}
				result = "";
			}

			if (model.zoneId != null)
			{

			}
			return Json("ok");
		}
		[HttpPost]
		public async Task<IActionResult> PostSalaryAndVoucherToCBS(CBSProcessViewModel vm)
		{
			var hrBranch = await salaryService.GetHrBranchById(Convert.ToInt32(vm.hrBranchId));
			var salaryPeriod = await salaryService.GetSalaryPeriodById(Convert.ToInt32(vm.periodId));

			var xmlData = await cBSPostingService.GetSalaryAndVoucherCBSXMLData(salaryPeriod.salaryYear.yearName, salaryPeriod.monthName, hrBranch.branchCode);

			var model = new CBSDataInsertModel
			{
				in_user_id = _configuration["CBS:user_id"],
				in_Auth_Key = _configuration["CBS:Password"],
				in_ChkSum = _configuration["CBS:ChkSum"],
				in_request_id = _configuration["CBS:request_id"],
				in_xml_data = "<![CDATA[" + xmlData.FirstOrDefault().XML_DATA + "]]>",
				in_Bin_data = "",
				in_request_type = "req_transfer",
				in_Add_Param_One = "100",
				in_Add_Param_Two = ""
			};
			var test = await client.dataInsertAsync(model.in_user_id, model.in_Auth_Key, model.in_ChkSum, model.in_request_id, model.in_xml_data, model.in_Bin_data, model.in_request_type, model.in_Add_Param_One, model.in_Add_Param_Two);

			ViewBag.error = "";
			if (test.Body.dataInsertReturn.out_Status == "Y")
			{
				await cBSPostingService.UpdateCBSSalaryStatus(xmlData.FirstOrDefault().uniqueKey, "POSTED");
				return Json(true);
			}
			else
			{
				ViewBag.error = test.Body.dataInsertReturn.out_XML_Data;
				return Json(false);
			}
		}

		//public XDocument GetXmlDataForCBSPosting()
		//{

		//	string data = await cBSPostingService.GetCBSXMLData(salaryPeriod.salaryYear.yearName, salaryPeriod.monthName, vm.sbu, vm.empCode == null ? 0 : vm.empCode, hrBranch.branchCode);

		//	Response.AddHeader("Content-Type", "text/xml");
		//	return XDocument.Parse(data);

		//}

		[HttpPost]
		public async Task<IActionResult> PostVoucherToCBS(CBSProcessViewModel vm)
		{
			string userName = HttpContext.User.Identity.Name;
			var hrBranch = await salaryService.GetHrBranchById(Convert.ToInt32(vm.hrBranchId));
			var salaryPeriod = await salaryService.GetSalaryPeriodById(Convert.ToInt32(vm.periodId));

			var checkPostedLog = await cBSPostingService.checkSalaryPostedLog(Convert.ToInt32(vm.hrBranchId), Convert.ToInt32(vm.periodId), "VOUCHER");
			if (checkPostedLog != null)
			{
				return Json("Already Posted");
			}
			else
			{
				IEnumerable<CBSXMLDataViewModel> xmlData = new List<CBSXMLDataViewModel>();
				if (vm.hrBranchId != 205)
				{
					xmlData = await cBSPostingService.GetCBSVoucherXMLData(salaryPeriod.salaryYear.yearName, salaryPeriod.monthName, hrBranch.branchCode);
				}
				else
				{
					xmlData = await cBSPostingService.GetCBSVoucherXMLData(salaryPeriod.salaryYear.yearName, salaryPeriod.monthName, hrBranch.branchCode);
				}
				//var in_xml_data = "<![CDATA[" + xmlData.FirstOrDefault().XML_DATA + "]]>";

				if (xmlData.Count() > 0 && hrBranch.branchCode != "100")
				{
					try
					{
						var model = new CBSDataInsertModel
						{
							in_user_id = _configuration["CBS:user_id"],
							in_Auth_Key = _configuration["CBS:Password"],
							in_ChkSum = _configuration["CBS:ChkSum"],
							in_request_id = _configuration["CBS:request_id"],
							in_xml_data = xmlData.FirstOrDefault().XML_DATA,
							in_Bin_data = "",
							in_request_type = "req_transfer",
							in_Add_Param_One = "100",
							in_Add_Param_Two = ""
						};

						var test = await client.dataInsertAsync(model.in_user_id, model.in_Auth_Key, model.in_ChkSum, model.in_request_id, model.in_xml_data, model.in_Bin_data, model.in_request_type, model.in_Add_Param_One, model.in_Add_Param_Two);

						if (test.Body.dataInsertReturn.out_Status == "Y")
						{
							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

							XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
							XmlNodeList TRN_REF = xmlDoc.GetElementsByTagName("TRN_REF");
							XmlNodeList MESSAGE = xmlDoc.GetElementsByTagName("MESSAGE");

							string successResult = ISSUCCESS[0].InnerXml;
							string tranrefResult = TRN_REF[0].InnerXml;
							string messageResult = MESSAGE[0].InnerXml;

							//<QRYRESULT><ISSUCCESS>Y</ISSUCCESS><TRN_REF>100MB000070</TRN_REF><MESSAGE>Transaction Completed</MESSAGE></QRYRESULT>
							await cBSPostingService.UpdateCBSVoucherStatus(xmlData.FirstOrDefault().uniqueKey, "POSTED");
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, null, hrBranch.branchCode, "VOUCHER", xmlData.FirstOrDefault().uniqueKey, successResult, tranrefResult, messageResult);
							return Json("Success");
						}
						else
						{
							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

							XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
							XmlNodeList REASON = xmlDoc.GetElementsByTagName("REASON");
							XmlNodeList WARNING = xmlDoc.GetElementsByTagName("WARNING");

							string successResult = ISSUCCESS[0].InnerXml;
							string reasonResult = REASON[0].InnerXml;
							string warningResult = WARNING[0].InnerXml;

							//<QRYRESULT><ISSUCCESS>Y</ISSUCCESS><TRN_REF>100MB000070</TRN_REF><MESSAGE>Transaction Completed</MESSAGE></QRYRESULT>
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, null, hrBranch.branchCode, "VOUCHER", xmlData.FirstOrDefault().uniqueKey, successResult, reasonResult, warningResult);
							return Json("Failed-" + warningResult);
						}
					}
					catch (Exception ex)
					{
						await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, null, hrBranch.branchCode, "VOUCHER", xmlData.FirstOrDefault().uniqueKey, null, null, ex.Message);
						return Json("Something Wrong-" + ex.Message);
					}
				}
				else
				{
					await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, null, hrBranch.branchCode, "VOUCHER", null, null, null, "Process Incomplete");
					return Json("Process Not Done");
				}
			}

		}

		[HttpPost]
		public async Task<IActionResult> PostIndVoucherToCBS(CBSProcessViewModel vm)
		{
			string userName = HttpContext.User.Identity.Name;

			var salaryPeriod = await salaryService.GetSalaryPeriodById(Convert.ToInt32(vm.periodId));


			var checkPostedLog = await cBSPostingService.checkSalaryPostedLog(500, Convert.ToInt32(vm.periodId), "VOUCHER");
			if (checkPostedLog != null)
			{
				return Json("Already Posted");
			}
			else
			{
				IEnumerable<CBSXMLDataViewModel> xmlData = new List<CBSXMLDataViewModel>();
				xmlData = await cBSPostingService.GetIndCBSVoucherXMLData(salaryPeriod.salaryYear.yearName, salaryPeriod.monthName, "2411");

				if (xmlData.Count() > 0)
				{
					try
					{
						var model = new CBSDataInsertModel
						{
							in_user_id = _configuration["CBS:user_id"],
							in_Auth_Key = _configuration["CBS:Password"],
							in_ChkSum = _configuration["CBS:ChkSum"],
							in_request_id = _configuration["CBS:request_id"],
							in_xml_data = xmlData.FirstOrDefault().XML_DATA,
							in_Bin_data = "",
							in_request_type = "req_transfer",
							in_Add_Param_One = "100",
							in_Add_Param_Two = ""
						};

						var test = await client.dataInsertAsync(model.in_user_id, model.in_Auth_Key, model.in_ChkSum, model.in_request_id, model.in_xml_data, model.in_Bin_data, model.in_request_type, model.in_Add_Param_One, model.in_Add_Param_Two);

						if (test.Body.dataInsertReturn.out_Status == "Y")
						{
							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

							XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
							XmlNodeList TRN_REF = xmlDoc.GetElementsByTagName("TRN_REF");
							XmlNodeList MESSAGE = xmlDoc.GetElementsByTagName("MESSAGE");

							string successResult = ISSUCCESS[0].InnerXml;
							string tranrefResult = TRN_REF[0].InnerXml;
							string messageResult = MESSAGE[0].InnerXml;

							await cBSPostingService.UpdateCBSVoucherStatus(xmlData.FirstOrDefault().uniqueKey, "POSTED");
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), 500, null, null, "200", "VOUCHER", xmlData.FirstOrDefault().uniqueKey, successResult, tranrefResult, messageResult);
							return Json("Success");
						}
						else
						{
							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

							XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
							XmlNodeList REASON = xmlDoc.GetElementsByTagName("REASON");
							XmlNodeList WARNING = xmlDoc.GetElementsByTagName("WARNING");

							string successResult = ISSUCCESS[0].InnerXml;
							string reasonResult = REASON[0].InnerXml;
							string warningResult = WARNING[0].InnerXml;

							//<QRYRESULT><ISSUCCESS>Y</ISSUCCESS><TRN_REF>100MB000070</TRN_REF><MESSAGE>Transaction Completed</MESSAGE></QRYRESULT>
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), 500, null, null, "200", "VOUCHER", xmlData.FirstOrDefault().uniqueKey, successResult, reasonResult, warningResult);
							return Json("Failed-" + warningResult);
						}
					}
					catch (Exception ex)
					{
						await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), 500, null, null, "200", "VOUCHER", xmlData.FirstOrDefault().uniqueKey, null, null, ex.Message);
						return Json("Something Wrong-" + ex.Message);
					}
				}
				else
				{
					await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), 500, null, null, "200", "VOUCHER", null, null, null, "Process Incomplete");
					return Json("Process Not Done");
				}
			}

		}

		public async Task<IActionResult> PostSingleVoucherByLogIdAPI(string unique, string type, int? periodId)
		{
			var result = new List<CBSVoucherPostingResponseVm>();

			string userName = HttpContext.User.Identity.Name;
			var voucherXml = new CBSVoucherViewModels();
			string checkPostedLog = "";
			string bType = "";

			if (type == "voucher")
			{
				bType = "VOUCHER";
				voucherXml = await cBSPostingService.GetVoucherXMLByKeyAndId(unique, periodId);
				checkPostedLog = await cBSPostingService.checkSalaryPostedLog(Convert.ToInt32(voucherXml.BranchId), Convert.ToInt32(voucherXml.periodId), "VOUCHER");
			}
			else if (type == "salary")
			{
				bType = "SALARY";
				voucherXml = await cBSPostingService.GetSalaryXMLById(unique);
				checkPostedLog = await cBSPostingService.checkSalaryPostedLog(Convert.ToInt32(voucherXml.BranchId), Convert.ToInt32(voucherXml.periodId), "SALARY");
			}
			else if (type == "union")
			{
				bType = "UNION";
				voucherXml = await cBSPostingService.GetUnionXMLById(unique);
				checkPostedLog = await cBSPostingService.checkSalaryPostedLog(Convert.ToInt32(voucherXml.BranchId), Convert.ToInt32(voucherXml.periodId), "UNION");
			}
			else
			{

			}

			if (checkPostedLog != null)
			{
				result.Add(new CBSVoucherPostingResponseVm
				{
					status = 1,
					trxcode = checkPostedLog,
					message = "Previous Posted",
					branchCode = voucherXml.branchCode,
					branchName = voucherXml.branchName
				});
			}
			else
			{
				try
				{
					var model = new CBSDataInsertModel
					{
						in_user_id = _configuration["CBS:user_id"],
						in_Auth_Key = _configuration["CBS:Password"],
						in_ChkSum = _configuration["CBS:ChkSum"],
						in_request_id = _configuration["CBS:request_id"],
						in_xml_data = voucherXml.XML_DATA,
						in_Bin_data = "",
						in_request_type = "req_transfer",
						in_Add_Param_One = "100",
						in_Add_Param_Two = ""
					};

					var test = await client.dataInsertAsync(model.in_user_id, model.in_Auth_Key, model.in_ChkSum, model.in_request_id, model.in_xml_data, model.in_Bin_data, model.in_request_type, model.in_Add_Param_One, model.in_Add_Param_Two);

					if (test.Body.dataInsertReturn.out_Status == "Y")
					{
						XmlDocument xmlDoc = new XmlDocument();
						xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

						XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
						XmlNodeList TRN_REF = xmlDoc.GetElementsByTagName("TRN_REF");
						XmlNodeList MESSAGE = xmlDoc.GetElementsByTagName("MESSAGE");

						string successResult = ISSUCCESS[0].InnerXml;
						string tranrefResult = TRN_REF[0].InnerXml;
						string messageResult = MESSAGE[0].InnerXml;

						if (type == "voucher")
						{
							await cBSPostingService.UpdateCBSVoucherStatusNew(voucherXml.uniqueKey, "POSTED", "voucher");
						}
						else if (type == "salary")
						{
							await cBSPostingService.UpdateCBSVoucherStatusNew(voucherXml.uniqueKey, "POSTED", "salary");
						}
						else if (type == "union")
						{
							await cBSPostingService.UpdateCBSVoucherStatusNew(voucherXml.uniqueKey, "POSTED", "union");
						}
						else
						{

						}

						if (voucherXml.type == "zone")
						{
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(voucherXml.periodId), null, voucherXml.BranchId, null, voucherXml.branchCode, bType, voucherXml.uniqueKey, successResult, tranrefResult, messageResult);
						}
						else
						{
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(voucherXml.periodId), voucherXml.BranchId, null, null, voucherXml.branchCode, bType, voucherXml.uniqueKey, successResult, tranrefResult, messageResult);
						}

						result.Add(new CBSVoucherPostingResponseVm
						{
							status = 1,
							trxcode = tranrefResult,
							message = messageResult,
							branchCode = voucherXml.branchCode,
							branchName = voucherXml.branchName
						});
					}
					else
					{
						XmlDocument xmlDoc = new XmlDocument();
						xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

						XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
						XmlNodeList REASON = xmlDoc.GetElementsByTagName("REASON");
						XmlNodeList WARNING = xmlDoc.GetElementsByTagName("WARNING");

						string successResult = ISSUCCESS[0].InnerXml;
						string reasonResult = REASON[0].InnerXml;
						string warningResult = WARNING[0].InnerXml;

						//<QRYRESULT><ISSUCCESS>Y</ISSUCCESS><TRN_REF>100MB000070</TRN_REF><MESSAGE>Transaction Completed</MESSAGE></QRYRESULT>
						if (voucherXml.type == "zone")
						{
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(voucherXml.periodId), null, voucherXml.BranchId, null, voucherXml.branchCode, bType, voucherXml.uniqueKey, successResult, reasonResult, warningResult);
						}
						else
						{
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(voucherXml.periodId), voucherXml.BranchId, null, null, voucherXml.branchCode, bType, voucherXml.uniqueKey, successResult, reasonResult, warningResult);
						}
						result.Add(new CBSVoucherPostingResponseVm
						{
							status = 0,
							trxcode = reasonResult,
							message = warningResult,
							branchCode = voucherXml.branchCode,
							branchName = voucherXml.branchName
						});
					}
				}
				catch (Exception ex)
				{
					if (voucherXml.type == "zone")
					{
						await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(voucherXml.periodId), null, null, voucherXml.BranchId, voucherXml.branchCode, bType, voucherXml.uniqueKey, null, null, ex.Message);
					}
					else
					{
						await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(voucherXml.periodId), voucherXml.BranchId, null, null, voucherXml.branchCode, bType, voucherXml.uniqueKey, null, null, ex.Message);
					}


					result.Add(new CBSVoucherPostingResponseVm
					{
						status = 0,
						trxcode = "",
						message = "Something Wrong",
						branchCode = voucherXml.branchCode,
						branchName = voucherXml.branchName
					});
				}
			}
			return Json("ok");
		}


		public async Task<IActionResult> PostSingleBonusByLogIdAPI(string unique, string type)
		{
			var result = new List<CBSVoucherPostingResponseVm>();

			string userName = HttpContext.User.Identity.Name;
			var voucherXml = new CBSVoucherViewModels();
			string checkPostedLog = "";
			string bType = "";

			if (type == "voucher")
			{
				bType = "VOUCHER";
				voucherXml = await cBSPostingService.GetVoucherXMLById(unique);
				checkPostedLog = await cBSPostingService.checkSalaryPostedLog(Convert.ToInt32(voucherXml.BranchId), Convert.ToInt32(voucherXml.periodId), "VOUCHER");
			}
			else if (type == "salary")
			{
				bType = "SALARY";
				voucherXml = await cBSPostingService.GetSalaryXMLById(unique);
				checkPostedLog = await cBSPostingService.checkSalaryPostedLog(Convert.ToInt32(voucherXml.BranchId), Convert.ToInt32(voucherXml.periodId), "SALARY");
			}
			else if (type == "union")
			{
				bType = "UNION";
				voucherXml = await cBSPostingService.GetUnionXMLById(unique);
				checkPostedLog = await cBSPostingService.checkSalaryPostedLog(Convert.ToInt32(voucherXml.BranchId), Convert.ToInt32(voucherXml.periodId), "UNION");
			}
			else
			{

			}

			if (checkPostedLog != null)
			{
				result.Add(new CBSVoucherPostingResponseVm
				{
					status = 1,
					trxcode = checkPostedLog,
					message = "Previous Posted",
					branchCode = voucherXml.branchCode,
					branchName = voucherXml.branchName
				});
			}
			else
			{
				try
				{
					var model = new CBSDataInsertModel
					{
						in_user_id = _configuration["CBS:user_id"],
						in_Auth_Key = _configuration["CBS:Password"],
						in_ChkSum = _configuration["CBS:ChkSum"],
						in_request_id = _configuration["CBS:request_id"],
						in_xml_data = voucherXml.XML_DATA,
						in_Bin_data = "",
						in_request_type = "req_transfer",
						in_Add_Param_One = "100",
						in_Add_Param_Two = ""
					};

					var test = await client.dataInsertAsync(model.in_user_id, model.in_Auth_Key, model.in_ChkSum, model.in_request_id, model.in_xml_data, model.in_Bin_data, model.in_request_type, model.in_Add_Param_One, model.in_Add_Param_Two);

					if (test.Body.dataInsertReturn.out_Status == "Y")
					{
						XmlDocument xmlDoc = new XmlDocument();
						xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

						XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
						XmlNodeList TRN_REF = xmlDoc.GetElementsByTagName("TRN_REF");
						XmlNodeList MESSAGE = xmlDoc.GetElementsByTagName("MESSAGE");

						string successResult = ISSUCCESS[0].InnerXml;
						string tranrefResult = TRN_REF[0].InnerXml;
						string messageResult = MESSAGE[0].InnerXml;

						if (type == "voucher")
						{
							await cBSPostingService.UpdateCBSVoucherStatusNew(voucherXml.uniqueKey, "POSTED", "voucher");
						}
						else if (type == "salary")
						{
							await cBSPostingService.UpdateCBSVoucherStatusNew(voucherXml.uniqueKey, "POSTED", "salary");
						}
						else if (type == "union")
						{
							await cBSPostingService.UpdateCBSVoucherStatusNew(voucherXml.uniqueKey, "POSTED", "union");
						}
						else
						{

						}

						if (voucherXml.type == "zone")
						{
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(voucherXml.periodId), null, voucherXml.BranchId, null, voucherXml.branchCode, bType, voucherXml.uniqueKey, successResult, tranrefResult, messageResult);
						}
						else
						{
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(voucherXml.periodId), voucherXml.BranchId, null, null, voucherXml.branchCode, bType, voucherXml.uniqueKey, successResult, tranrefResult, messageResult);
						}

						result.Add(new CBSVoucherPostingResponseVm
						{
							status = 1,
							trxcode = tranrefResult,
							message = messageResult,
							branchCode = voucherXml.branchCode,
							branchName = voucherXml.branchName
						});
					}
					else
					{
						XmlDocument xmlDoc = new XmlDocument();
						xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

						XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
						XmlNodeList REASON = xmlDoc.GetElementsByTagName("REASON");
						XmlNodeList WARNING = xmlDoc.GetElementsByTagName("WARNING");

						string successResult = ISSUCCESS[0].InnerXml;
						string reasonResult = REASON[0].InnerXml;
						string warningResult = WARNING[0].InnerXml;

						//<QRYRESULT><ISSUCCESS>Y</ISSUCCESS><TRN_REF>100MB000070</TRN_REF><MESSAGE>Transaction Completed</MESSAGE></QRYRESULT>
						if (voucherXml.type == "zone")
						{
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(voucherXml.periodId), null, voucherXml.BranchId, null, voucherXml.branchCode, bType, voucherXml.uniqueKey, successResult, reasonResult, warningResult);
						}
						else
						{
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(voucherXml.periodId), voucherXml.BranchId, null, null, voucherXml.branchCode, bType, voucherXml.uniqueKey, successResult, reasonResult, warningResult);
						}
						result.Add(new CBSVoucherPostingResponseVm
						{
							status = 0,
							trxcode = reasonResult,
							message = warningResult,
							branchCode = voucherXml.branchCode,
							branchName = voucherXml.branchName
						});
					}
				}
				catch (Exception ex)
				{
					if (voucherXml.type == "zone")
					{
						await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(voucherXml.periodId), null, null, voucherXml.BranchId, voucherXml.branchCode, bType, voucherXml.uniqueKey, null, null, ex.Message);
					}
					else
					{
						await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(voucherXml.periodId), voucherXml.BranchId, null, null, voucherXml.branchCode, bType, voucherXml.uniqueKey, null, null, ex.Message);
					}


					result.Add(new CBSVoucherPostingResponseVm
					{
						status = 0,
						trxcode = "",
						message = "Something Wrong",
						branchCode = voucherXml.branchCode,
						branchName = voucherXml.branchName
					});
				}
			}
			return Json("ok");
		}

		[HttpPost]
		public async Task<IActionResult> PostAllVoucherToCBS(CBSProcessViewModel vm)
		{
			var result = new List<CBSVoucherPostingResponseVm>();

			string userName = HttpContext.User.Identity.Name;
			var salaryPeriod = await salaryService.GetSalaryPeriodById(Convert.ToInt32(vm.periodId));


			var allVoucherXmls = await cBSPostingService.GetAllCBSVoucherXmls(Convert.ToInt32(vm.periodId));

			foreach (var item in allVoucherXmls)
			{
				var checkPostedLog = await cBSPostingService.checkSalaryPostedLog(Convert.ToInt32(item.BranchId), Convert.ToInt32(vm.periodId), "VOUCHER");
				if (checkPostedLog != null)
				{
					result.Add(new CBSVoucherPostingResponseVm
					{
						status = 1,
						trxcode = checkPostedLog,
						message = "Previous Posted",
						branchCode = item.branchCode,
						branchName = item.branchName
					});
				}
				else
				{
					try
					{
						var model = new CBSDataInsertModel
						{
							in_user_id = _configuration["CBS:user_id"],
							in_Auth_Key = _configuration["CBS:Password"],
							in_ChkSum = _configuration["CBS:ChkSum"],
							in_request_id = _configuration["CBS:request_id"],
							in_xml_data = item.XML_DATA,
							in_Bin_data = "",
							in_request_type = "req_transfer",
							in_Add_Param_One = "100",
							in_Add_Param_Two = ""
						};

						var test = await client.dataInsertAsync(model.in_user_id, model.in_Auth_Key, model.in_ChkSum, model.in_request_id, model.in_xml_data, model.in_Bin_data, model.in_request_type, model.in_Add_Param_One, model.in_Add_Param_Two);

						if (test.Body.dataInsertReturn.out_Status == "Y")
						{
							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

							XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
							XmlNodeList TRN_REF = xmlDoc.GetElementsByTagName("TRN_REF");
							XmlNodeList MESSAGE = xmlDoc.GetElementsByTagName("MESSAGE");

							string successResult = ISSUCCESS[0].InnerXml;
							string tranrefResult = TRN_REF[0].InnerXml;
							string messageResult = MESSAGE[0].InnerXml;

							//<QRYRESULT><ISSUCCESS>Y</ISSUCCESS><TRN_REF>100MB000070</TRN_REF><MESSAGE>Transaction Completed</MESSAGE></QRYRESULT>
							await cBSPostingService.UpdateCBSVoucherStatusNew(item.uniqueKey, "POSTED", "voucher");
							if (item.type == "zone")
							{
								await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), null, item.BranchId, null, item.branchCode, "VOUCHER", item.uniqueKey, successResult, tranrefResult, messageResult);
							}
							else
							{
								await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), item.BranchId, null, null, item.branchCode, "VOUCHER", item.uniqueKey, successResult, tranrefResult, messageResult);
							}

							result.Add(new CBSVoucherPostingResponseVm
							{
								status = 1,
								trxcode = tranrefResult,
								message = messageResult,
								branchCode = item.branchCode,
								branchName = item.branchName
							});
						}
						else
						{
							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

							XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
							XmlNodeList REASON = xmlDoc.GetElementsByTagName("REASON");
							XmlNodeList WARNING = xmlDoc.GetElementsByTagName("WARNING");

							string successResult = ISSUCCESS[0].InnerXml;
							string reasonResult = REASON[0].InnerXml;
							string warningResult = WARNING[0].InnerXml;

							//<QRYRESULT><ISSUCCESS>Y</ISSUCCESS><TRN_REF>100MB000070</TRN_REF><MESSAGE>Transaction Completed</MESSAGE></QRYRESULT>
							if (item.type == "zone")
							{
								await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), null, item.BranchId, null, item.branchCode, "VOUCHER", item.uniqueKey, successResult, reasonResult, warningResult);
							}
							else
							{
								await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), item.BranchId, null, null, item.branchCode, "VOUCHER", item.uniqueKey, successResult, reasonResult, warningResult);
							}
							result.Add(new CBSVoucherPostingResponseVm
							{
								status = 0,
								trxcode = reasonResult,
								message = warningResult,
								branchCode = item.branchCode,
								branchName = item.branchName
							});
						}
					}
					catch (Exception ex)
					{
						if (item.type == "zone")
						{
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), null, null, item.BranchId, item.branchCode, "VOUCHER", item.uniqueKey, null, null, ex.Message);
						}
						else
						{
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), item.BranchId, null, null, item.branchCode, "VOUCHER", item.uniqueKey, null, null, ex.Message);
						}


						result.Add(new CBSVoucherPostingResponseVm
						{
							status = 0,
							trxcode = "",
							message = "Something Wrong",
							branchCode = item.branchCode,
							branchName = item.branchName
						});
					}
				}




			}


			//var allHrBranches = await salaryService.GetAllHrBranchs();
			//foreach (var hrBranch in allHrBranches)
			//{
			//	var checkPostedLog = await cBSPostingService.checkSalaryPostedLog(Convert.ToInt32(hrBranch.Id), Convert.ToInt32(vm.periodId), "VOUCHER");
			//	if (checkPostedLog != null)
			//	{
			//		result.Add(new CBSVoucherPostingResponseVm
			//		{
			//			status = 1,
			//			trxcode = checkPostedLog,
			//			message = "Previous Posted",
			//			branchCode = hrBranch.branchCode,
			//			branchName = hrBranch.branchName
			//		});
			//	}
			//	else
			//	{
			//		IEnumerable<CBSXMLDataViewModel> xmlData = new List<CBSXMLDataViewModel>();
			//		if (vm.hrBranchId == 205)
			//		{
			//			xmlData = await cBSPostingService.GetCBSVoucherXMLData(salaryPeriod.salaryYear.yearName, salaryPeriod.monthName, hrBranch.branchCode);
			//		}
			//		else
			//		{
			//			xmlData = await cBSPostingService.GetCBSVoucherXMLData(salaryPeriod.salaryYear.yearName, salaryPeriod.monthName, hrBranch.branchCode);
			//		}
			//		//var in_xml_data = "<![CDATA[" + xmlData.FirstOrDefault().XML_DATA + "]]>";

			//		if (xmlData.Count() > 0)
			//		{
			//			try
			//			{
			//				var model = new CBSDataInsertModel
			//				{
			//					in_user_id = "EHR",
			//					in_Auth_Key = _configuration["CBS:Password"],
			//					in_ChkSum = "2731C9E8471B1B1F6E977E7A00ABEC4EE8376268",
			//					in_request_id = "HR1",
			//					in_xml_data = xmlData.FirstOrDefault().XML_DATA,
			//					in_Bin_data = "",
			//					in_request_type = "req_transfer",
			//					in_Add_Param_One = "100",
			//					in_Add_Param_Two = ""
			//				};

			//				var test = await client.dataInsertAsync(model.in_user_id, model.in_Auth_Key, model.in_ChkSum, model.in_request_id, model.in_xml_data, model.in_Bin_data, model.in_request_type, model.in_Add_Param_One, model.in_Add_Param_Two);

			//				if (test.Body.dataInsertReturn.out_Status == "Y")
			//				{
			//					XmlDocument xmlDoc = new XmlDocument();
			//					xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

			//					XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
			//					XmlNodeList TRN_REF = xmlDoc.GetElementsByTagName("TRN_REF");
			//					XmlNodeList MESSAGE = xmlDoc.GetElementsByTagName("MESSAGE");

			//					string successResult = ISSUCCESS[0].InnerXml;
			//					string tranrefResult = TRN_REF[0].InnerXml;
			//					string messageResult = MESSAGE[0].InnerXml;

			//					//<QRYRESULT><ISSUCCESS>Y</ISSUCCESS><TRN_REF>100MB000070</TRN_REF><MESSAGE>Transaction Completed</MESSAGE></QRYRESULT>
			//					await cBSPostingService.UpdateCBSVoucherStatus(xmlData.FirstOrDefault().uniqueKey, "POSTED");
			//					await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, hrBranch.branchCode, "VOUCHER", xmlData.FirstOrDefault().uniqueKey, successResult, tranrefResult, messageResult);
			//					result.Add(new CBSVoucherPostingResponseVm
			//					{
			//						status = 1,
			//						trxcode = tranrefResult,
			//						message = messageResult,
			//						branchCode = hrBranch.branchCode,
			//						branchName = hrBranch.branchName
			//					});
			//				}
			//				else
			//				{
			//					XmlDocument xmlDoc = new XmlDocument();
			//					xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

			//					XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
			//					XmlNodeList REASON = xmlDoc.GetElementsByTagName("REASON");
			//					XmlNodeList WARNING = xmlDoc.GetElementsByTagName("WARNING");

			//					string successResult = ISSUCCESS[0].InnerXml;
			//					string reasonResult = REASON[0].InnerXml;
			//					string warningResult = WARNING[0].InnerXml;

			//					//<QRYRESULT><ISSUCCESS>Y</ISSUCCESS><TRN_REF>100MB000070</TRN_REF><MESSAGE>Transaction Completed</MESSAGE></QRYRESULT>
			//					await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, hrBranch.branchCode, "VOUCHER", xmlData.FirstOrDefault().uniqueKey, successResult, reasonResult, warningResult);
			//					result.Add(new CBSVoucherPostingResponseVm
			//					{
			//						status = 0,
			//						trxcode = reasonResult,
			//						message = warningResult,
			//						branchCode = hrBranch.branchCode,
			//						branchName = hrBranch.branchName
			//					});
			//				}
			//			}
			//			catch (Exception ex)
			//			{
			//				await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, hrBranch.branchCode, "VOUCHER", xmlData.FirstOrDefault().uniqueKey, null, null, ex.Message);
			//				result.Add(new CBSVoucherPostingResponseVm
			//				{
			//					status = 0,
			//					trxcode = "",
			//					message = "Something Wrong",
			//					branchCode = hrBranch.branchCode,
			//					branchName = hrBranch.branchName
			//				});
			//			}
			//		}
			//		else
			//		{
			//			await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, hrBranch.branchCode, "VOUCHER", null, null, null, "Process Incomplete");
			//			result.Add(new CBSVoucherPostingResponseVm
			//			{
			//				status = 0,
			//				trxcode = "",
			//				message = "Process Not Done",
			//				branchCode = hrBranch.branchCode,
			//				branchName = hrBranch.branchName
			//			});
			//		}
			//	}
			//}

			return Json(result);
		}

		[HttpPost]
		public async Task<IActionResult> PostOfficerUninonVoucherToCBS(CBSProcessViewModel vm)
		{
			string userName = HttpContext.User.Identity.Name;
			var hrBranch = await salaryService.GetHrBranchById(Convert.ToInt32(vm.hrBranchId));
			var salaryPeriod = await salaryService.GetSalaryPeriodById(Convert.ToInt32(vm.periodId));

			var checkPostedLog = await cBSPostingService.checkSalaryPostedLog(Convert.ToInt32(vm.hrBranchId), Convert.ToInt32(vm.periodId), "UNION");
			if (checkPostedLog != null)
			{
				return Json("Already Posted");
			}
			else
			{
				IEnumerable<CBSXMLDataViewModel> xmlData = new List<CBSXMLDataViewModel>();
				if (vm.hrBranchId == 205)
				{
					xmlData = await cBSPostingService.GetCBSOfficerUninonVoucherXMLData(salaryPeriod.salaryYear.yearName, salaryPeriod.monthName, hrBranch.branchCode);
				}
				else
				{
					xmlData = await cBSPostingService.GetCBSOfficerUninonVoucherXMLData(salaryPeriod.salaryYear.yearName, salaryPeriod.monthName, hrBranch.branchCode);
				}

				if (xmlData.Count() > 0)
				{
					try
					{
						var model = new CBSDataInsertModel
						{
							in_user_id = _configuration["CBS:user_id"],
							in_Auth_Key = _configuration["CBS:Password"],
							in_ChkSum = _configuration["CBS:ChkSum"],
							in_request_id = _configuration["CBS:request_id"],
							in_xml_data = xmlData.FirstOrDefault().XML_DATA,
							in_Bin_data = "",
							in_request_type = "req_transfer",
							in_Add_Param_One = "100",
							in_Add_Param_Two = ""
						};
						var test = await client.dataInsertAsync(model.in_user_id, model.in_Auth_Key, model.in_ChkSum, model.in_request_id, model.in_xml_data, model.in_Bin_data, model.in_request_type, model.in_Add_Param_One, model.in_Add_Param_Two);
						if (test.Body.dataInsertReturn.out_Status == "Y")
						{
							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

							XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
							XmlNodeList TRN_REF = xmlDoc.GetElementsByTagName("TRN_REF");
							XmlNodeList MESSAGE = xmlDoc.GetElementsByTagName("MESSAGE");

							string successResult = ISSUCCESS[0].InnerXml;
							string tranrefResult = TRN_REF[0].InnerXml;
							string messageResult = MESSAGE[0].InnerXml;

							await cBSPostingService.UpdateCBSVoucherStatus(xmlData.FirstOrDefault().uniqueKey, "UNION");
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, null, hrBranch.branchCode, "UNION", xmlData.FirstOrDefault().uniqueKey, successResult, tranrefResult, messageResult);
							return Json("Success");
						}
						else
						{
							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

							XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
							XmlNodeList REASON = xmlDoc.GetElementsByTagName("REASON");
							XmlNodeList WARNING = xmlDoc.GetElementsByTagName("WARNING");

							string successResult = ISSUCCESS[0].InnerXml;
							string reasonResult = REASON[0].InnerXml;
							string warningResult = WARNING[0].InnerXml;

							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, null, hrBranch.branchCode, "UNION", xmlData.FirstOrDefault().uniqueKey, successResult, reasonResult, warningResult);
							return Json("Failed-" + warningResult);
						}
					}
					catch (Exception ex)
					{
						await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, null, hrBranch.branchCode, "UNION", xmlData.FirstOrDefault().uniqueKey, null, null, ex.Message);
						return Json("Something Wrong-" + ex.Message);
					}
				}
				else
				{
					await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), hrBranch.Id, null, null, hrBranch.branchCode, "UNION", null, null, null, "Process Incomplete");
					return Json("Process Not Done");
				}
			}
		}

		public async Task<IActionResult> PostPayOrderToCBS(CBSProcessViewModel vm)
		{
			var result = new List<CBSVoucherPostingResponseVm>();

			string userName = HttpContext.User.Identity.Name;
			var salaryPeriod = await salaryService.GetSalaryPeriodById(Convert.ToInt32(vm.periodId));

			await cBSPostingService.GenerateHeadOfficePayOrderXML(Convert.ToInt32(vm.periodId));

			var PayOrderXml = await cBSPostingService.GetCBSPayOrderXmls(Convert.ToInt32(vm.periodId));

			foreach (var item in PayOrderXml)
			{
				var checkPostedLog = await cBSPostingService.checkSalaryPostedLog(Convert.ToInt32(item.BranchId), Convert.ToInt32(vm.periodId), "PAYORDER");
				if (checkPostedLog != null)
				{
					result.Add(new CBSVoucherPostingResponseVm
					{
						status = 1,
						trxcode = checkPostedLog,
						message = "Previous Posted",
						branchCode = item.branchCode,
						branchName = item.branchName
					});
				}
				else
				{
					try
					{
						var model = new CBSDataInsertModel
						{
							in_user_id = _configuration["CBS:user_id"],
							in_Auth_Key = _configuration["CBS:Password"],
							in_ChkSum = _configuration["CBS:ChkSum"],
							in_request_id = _configuration["CBS:request_id"],
							in_xml_data = item.XML_DATA,
							in_Bin_data = "",
							in_request_type = "req_transfer",
							in_Add_Param_One = "100",
							in_Add_Param_Two = ""
						};

						var test = await client.dataInsertAsync(model.in_user_id, model.in_Auth_Key, model.in_ChkSum, model.in_request_id, model.in_xml_data, model.in_Bin_data, model.in_request_type, model.in_Add_Param_One, model.in_Add_Param_Two);

						if (test.Body.dataInsertReturn.out_Status == "Y")
						{
							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

							XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
							XmlNodeList TRN_REF = xmlDoc.GetElementsByTagName("TRN_REF");
							XmlNodeList MESSAGE = xmlDoc.GetElementsByTagName("MESSAGE");

							string successResult = ISSUCCESS[0].InnerXml;
							string tranrefResult = TRN_REF[0].InnerXml;
							string messageResult = MESSAGE[0].InnerXml;

							await cBSPostingService.UpdateCBSVoucherStatus(item.uniqueKey, "POSTED");

							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), item.BranchId, null, null, item.branchCode, "PAYORDER", item.uniqueKey, successResult, tranrefResult, messageResult);

							result.Add(new CBSVoucherPostingResponseVm
							{
								status = 1,
								trxcode = tranrefResult,
								message = messageResult,
								branchCode = item.branchCode,
								branchName = item.branchName
							});
						}
						else
						{
							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

							XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
							XmlNodeList REASON = xmlDoc.GetElementsByTagName("REASON");
							XmlNodeList WARNING = xmlDoc.GetElementsByTagName("WARNING");

							string successResult = ISSUCCESS[0].InnerXml;
							string reasonResult = REASON[0].InnerXml;
							string warningResult = WARNING[0].InnerXml;


							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), item.BranchId, null, null, item.branchCode, "PAYORDER", item.uniqueKey, successResult, reasonResult, warningResult);

							result.Add(new CBSVoucherPostingResponseVm
							{
								status = 0,
								trxcode = reasonResult,
								message = warningResult,
								branchCode = item.branchCode,
								branchName = item.branchName
							});
						}
					}
					catch (Exception ex)
					{
						await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), item.BranchId, null, null, item.branchCode, "PAYORDER", item.uniqueKey, null, null, ex.Message);

						result.Add(new CBSVoucherPostingResponseVm
						{
							status = 0,
							trxcode = "",
							message = "Something Wrong",
							branchCode = item.branchCode,
							branchName = item.branchName
						});
					}
				}




			}
			return Json(result);
		}

		[HttpPost]
		public async Task<IActionResult> PostAllUnionToCBS(CBSProcessViewModel vm)
		{
			var result = new List<CBSVoucherPostingResponseVm>();

			string userName = HttpContext.User.Identity.Name;
			var salaryPeriod = await salaryService.GetSalaryPeriodById(Convert.ToInt32(vm.periodId));


			var allUnionXmls = await cBSPostingService.GetAllCBSUnionXmls(Convert.ToInt32(vm.periodId));

			foreach (var item in allUnionXmls)
			{
				var checkPostedLog = await cBSPostingService.checkSalaryPostedLog(Convert.ToInt32(item.BranchId), Convert.ToInt32(vm.periodId), "UNION");
				if (checkPostedLog != null)
				{
					result.Add(new CBSVoucherPostingResponseVm
					{
						status = 1,
						trxcode = checkPostedLog,
						message = "Previous Posted",
						branchCode = item.branchCode,
						branchName = item.branchName
					});
				}
				else
				{
					try
					{
						var model = new CBSDataInsertModel
						{
							in_user_id = _configuration["CBS:user_id"],
							in_Auth_Key = _configuration["CBS:Password"],
							in_ChkSum = _configuration["CBS:ChkSum"],
							in_request_id = _configuration["CBS:request_id"],
							in_xml_data = item.XML_DATA,
							in_Bin_data = "",
							in_request_type = "req_transfer",
							in_Add_Param_One = "100",
							in_Add_Param_Two = ""
						};

						var test = await client.dataInsertAsync(model.in_user_id, model.in_Auth_Key, model.in_ChkSum, model.in_request_id, model.in_xml_data, model.in_Bin_data, model.in_request_type, model.in_Add_Param_One, model.in_Add_Param_Two);

						if (test.Body.dataInsertReturn.out_Status == "Y")
						{
							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

							XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
							XmlNodeList TRN_REF = xmlDoc.GetElementsByTagName("TRN_REF");
							XmlNodeList MESSAGE = xmlDoc.GetElementsByTagName("MESSAGE");

							string successResult = ISSUCCESS[0].InnerXml;
							string tranrefResult = TRN_REF[0].InnerXml;
							string messageResult = MESSAGE[0].InnerXml;

							//<QRYRESULT><ISSUCCESS>Y</ISSUCCESS><TRN_REF>100MB000070</TRN_REF><MESSAGE>Transaction Completed</MESSAGE></QRYRESULT>
							await cBSPostingService.UpdateCBSVoucherStatusNew(item.uniqueKey, "POSTED", "union");
							if (item.type == "zone")
							{
								await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), null, item.BranchId, null, item.branchCode, "UNION", item.uniqueKey, successResult, tranrefResult, messageResult);
							}
							else
							{
								await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), item.BranchId, null, null, item.branchCode, "UNION", item.uniqueKey, successResult, tranrefResult, messageResult);
							}
							result.Add(new CBSVoucherPostingResponseVm
							{
								status = 1,
								trxcode = tranrefResult,
								message = messageResult,
								branchCode = item.branchCode,
								branchName = item.branchName
							});
						}
						else
						{
							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

							XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
							XmlNodeList REASON = xmlDoc.GetElementsByTagName("REASON");
							XmlNodeList WARNING = xmlDoc.GetElementsByTagName("WARNING");

							string successResult = ISSUCCESS[0].InnerXml;
							string reasonResult = REASON[0].InnerXml;
							string warningResult = WARNING[0].InnerXml;

							//<QRYRESULT><ISSUCCESS>Y</ISSUCCESS><TRN_REF>100MB000070</TRN_REF><MESSAGE>Transaction Completed</MESSAGE></QRYRESULT>

							if (item.type == "zone")
							{
								await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), null, item.BranchId, null, item.branchCode, "UNION", item.uniqueKey, successResult, reasonResult, warningResult);
							}
							else
							{
								await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), item.BranchId, null, null, item.branchCode, "UNION", item.uniqueKey, successResult, reasonResult, warningResult);
							}

							result.Add(new CBSVoucherPostingResponseVm
							{
								status = 0,
								trxcode = reasonResult,
								message = warningResult,
								branchCode = item.branchCode,
								branchName = item.branchName
							});
						}
					}
					catch (Exception ex)
					{
						if (item.type == "zone")
						{
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), null, item.BranchId, null, item.branchCode, "UNION", item.uniqueKey, null, null, ex.Message);
						}
						else
						{
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), item.BranchId, null, null, item.branchCode, "UNION", item.uniqueKey, null, null, ex.Message);
						}

						result.Add(new CBSVoucherPostingResponseVm
						{
							status = 0,
							trxcode = "",
							message = "Something Wrong",
							branchCode = item.branchCode,
							branchName = item.branchName
						});
					}
				}




			}
			return Json(result);
		}

		public async Task<IActionResult> PostLoanToCBS(CBSProcessViewModel vm)
		{
			var hrBranch = await salaryService.GetHrBranchById(Convert.ToInt32(vm.hrBranchId));
			var salaryPeriod = await salaryService.GetSalaryPeriodById(Convert.ToInt32(vm.periodId));

			var xmlData = await cBSPostingService.GetCBSLoanXMLData(salaryPeriod.salaryYear.yearName, salaryPeriod.monthName, hrBranch.branchCode);

			var model = new CBSDataInsertModel
			{
				in_user_id = _configuration["CBS:user_id"],
				in_Auth_Key = _configuration["CBS:Password"],
				in_ChkSum = _configuration["CBS:ChkSum"],
				in_request_id = _configuration["CBS:request_id"],
				in_xml_data = xmlData.FirstOrDefault().XML_DATA,
				in_Bin_data = "",
				in_request_type = "req_transfer",
				in_Add_Param_One = "100",
				in_Add_Param_Two = ""
			};
			var test = await client.dataInsertAsync(model.in_user_id, model.in_Auth_Key, model.in_ChkSum, model.in_request_id, model.in_xml_data, model.in_Bin_data, model.in_request_type, model.in_Add_Param_One, model.in_Add_Param_Two);
			if (test.Body.dataInsertReturn.out_Status == "Y")
			{
				await cBSPostingService.UpdateCBSLoanStatus(xmlData.FirstOrDefault().uniqueKey, "POSTED");

				return Json(true);
			}
			else
			{
				return Json(false);
			}
		}

		public async Task<IActionResult> GetProcessedSalary(int periodId, int branchId, string status)
		{
			var data = await cBSPostingService.GetCBSProcessData(periodId, branchId, status);
			return Json(data);
		}

		public async Task<IActionResult> GetProcessedVoucher(int periodId, int branchId, string status)
		{
			var data = await cBSPostingService.GetCBSVoucherProcessData(periodId, branchId, status);
			return Json(data);
		}
		public async Task<IActionResult> GetProcessedLoan(int periodId, int branchId, string status)
		{
			var data = await cBSPostingService.GetCBSLoanProcessData(periodId, branchId, status);
			return Json(data);
		}



		[HttpPost]
		public async Task<IActionResult> PostAllSalaryToCBS(CBSProcessViewModel vm)
		{
			var result = new List<CBSVoucherPostingResponseVm>();

			string userName = HttpContext.User.Identity.Name;
			var salaryPeriod = await salaryService.GetSalaryPeriodById(Convert.ToInt32(vm.periodId));


			var allSalaryXmls = await cBSPostingService.GetAllCBSSalaryXmls(Convert.ToInt32(vm.periodId));

			foreach (var item in allSalaryXmls.Where(x => x.status == 1))
			{
				var checkPostedLog = await cBSPostingService.checkSalaryPostedLogByKey(Convert.ToInt32(item.BranchId), Convert.ToInt32(vm.periodId), "SALARY", item.uniqueKey);
				if (checkPostedLog != null)
				{
					result.Add(new CBSVoucherPostingResponseVm
					{
						status = 1,
						trxcode = checkPostedLog,
						message = "Previous Posted",
						branchCode = item.branchCode,
						branchName = item.branchName
					});
				}
				else
				{
					try
					{
						var model = new CBSDataInsertModel
						{
							in_user_id = _configuration["CBS:user_id"],
							in_Auth_Key = _configuration["CBS:Password"],
							in_ChkSum = _configuration["CBS:ChkSum"],
							in_request_id = _configuration["CBS:request_id"],
							in_xml_data = item.XML_DATA,
							in_Bin_data = "",
							in_request_type = "req_transfer",
							in_Add_Param_One = "100",
							in_Add_Param_Two = ""
						};

						var test = await client.dataInsertAsync(model.in_user_id, model.in_Auth_Key, model.in_ChkSum, model.in_request_id, model.in_xml_data, model.in_Bin_data, model.in_request_type, model.in_Add_Param_One, model.in_Add_Param_Two);

						if (test.Body.dataInsertReturn.out_Status == "Y")
						{
							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

							XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
							XmlNodeList TRN_REF = xmlDoc.GetElementsByTagName("TRN_REF");
							XmlNodeList MESSAGE = xmlDoc.GetElementsByTagName("MESSAGE");

							string successResult = ISSUCCESS[0].InnerXml;
							string tranrefResult = TRN_REF[0].InnerXml;
							string messageResult = MESSAGE[0].InnerXml;

							//<QRYRESULT><ISSUCCESS>Y</ISSUCCESS><TRN_REF>100MB000070</TRN_REF><MESSAGE>Transaction Completed</MESSAGE></QRYRESULT>
							await cBSPostingService.UpdateCBSVoucherStatusNew(item.uniqueKey, "POSTED", "salary");
							if (item.type == "zone")
							{
								await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), null, item.BranchId, null, item.branchCode, "SALARY", item.uniqueKey, successResult, tranrefResult, messageResult);
							}
							else
							{
								await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), item.BranchId, null, null, item.branchCode, "SALARY", item.uniqueKey, successResult, tranrefResult, messageResult);
							}
							result.Add(new CBSVoucherPostingResponseVm
							{
								status = 1,
								trxcode = tranrefResult,
								message = messageResult,
								branchCode = item.branchCode,
								branchName = item.branchName
							});
						}
						else
						{
							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.LoadXml(test.Body.dataInsertReturn.out_XML_Data);

							XmlNodeList ISSUCCESS = xmlDoc.GetElementsByTagName("ISSUCCESS");
							XmlNodeList REASON = xmlDoc.GetElementsByTagName("REASON");
							XmlNodeList WARNING = xmlDoc.GetElementsByTagName("WARNING");

							string successResult = ISSUCCESS[0].InnerXml;
							string reasonResult = REASON[0].InnerXml;
							string warningResult = WARNING[0].InnerXml;

							//<QRYRESULT><ISSUCCESS>Y</ISSUCCESS><TRN_REF>100MB000070</TRN_REF><MESSAGE>Transaction Completed</MESSAGE></QRYRESULT>
							if (item.type == "zone")
							{
								await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), null, item.BranchId, null, item.branchCode, "SALARY", item.uniqueKey, successResult, reasonResult, warningResult);
							}
							else
							{
								await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), item.BranchId, null, null, item.branchCode, "SALARY", item.uniqueKey, successResult, reasonResult, warningResult);
							}

							result.Add(new CBSVoucherPostingResponseVm
							{
								status = 0,
								trxcode = reasonResult,
								message = warningResult,
								branchCode = item.branchCode,
								branchName = item.branchName
							});
						}
					}
					catch (Exception ex)
					{
						if (item.type == "zone")
						{
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), null, item.BranchId, null, item.branchCode, "SALARY", item.uniqueKey, null, null, ex.Message);
						}
						else
						{
							await cBSPostingService.SaveCBSPostingLog(userName, Convert.ToInt32(vm.periodId), item.BranchId, null, null, item.branchCode, "SALARY", item.uniqueKey, null, null, ex.Message);
						}

						result.Add(new CBSVoucherPostingResponseVm
						{
							status = 0,
							trxcode = "",
							message = "Something Wrong",
							branchCode = item.branchCode,
							branchName = item.branchName
						});
					}
				}
			}			

			return Json(result);
		}

		public IActionResult PostingLogReport()
		{

			return View();
		}

		[AllowAnonymous]
		public async Task<IActionResult> GetPostingLogs(int periodId, string type, string successStatus)
		{
			//var data1 = await cBSPostingService.GetCBSLoanProcessData(55, 156, "Posted");
			var postingLog = await cBSPostingService.GetCBSPostingLogs(periodId, type, successStatus);
			var data = new CBSPostingLogReport
			{
				//companies = await eRPCompanyService.GetAllCompany(),
				cBSPostingLogViewModels = postingLog,
				salaryPeriod = await salaryService.GetSalaryPeriodById(periodId)
			};
			return View(data);
		}
		[AllowAnonymous]
		public IActionResult GetPostingLogsPdf(int periodId, string type, string successStatus)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema+"://" + host + "/CBSPosting/GetPostingLogs?periodId=" + periodId + "&type=" + type + "&successStatus=" + successStatus;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}
	}
}