using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using DinkToPdf.Contracts;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPServices.EmailService.interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.Payroll.Services.Salary.Interfaces;

namespace OPUSERP.Areas.Payroll.Controllers
{
	[Area("HR")]
	[Authorize]
	public class EmailingController : Controller
	{
		private readonly ISalaryService salaryService;
		private readonly IUserInfoes userInfoes;
		private readonly IEmailSenderService emailSenderService;
		private readonly IPersonalInfoService employeeInfoService;

		private readonly IHostingEnvironment _hostingEnvironment;

		private readonly string rootPath;
		private readonly MyPDF myPDF;
		public string FileName;

		public EmailingController(ISalaryService salaryService, IUserInfoes userInfoes, IEmailSenderService emailSenderService, IPersonalInfoService employeeInfoService, IHostingEnvironment _hostingEnvironment, IConverter converter)
		{
			this.salaryService = salaryService;
			this.employeeInfoService = employeeInfoService;
			this.emailSenderService = emailSenderService;
			this.userInfoes = userInfoes;

			this._hostingEnvironment = _hostingEnvironment;
			myPDF = new MyPDF(_hostingEnvironment, converter);
			rootPath = _hostingEnvironment.ContentRootPath;
		}
		//option + '&employeeId=' + employeeId + '&designationId=' + designationId + '&branchid=' + branchid
		public async Task<IActionResult> GenerateAllPdf(int periodId, int option, int? employeeId, int? designationId, int? branchid)
		{
			
			string status = "";
			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";

			//Asad Added On 08.08.2023
			List<EmployeeInfo> empList = new List<EmployeeInfo>();
			if (option == 0) //Individual
			{
				await emailSenderService.DeleteEmailLogByEmployeeId(periodId, employeeId);
				empList = (List<EmployeeInfo>) await employeeInfoService.GetSalaryPaidEmployeeByEmployeeId(periodId, employeeId);
			}
			if (option == 3) //Designation
			{
				await emailSenderService.DeleteEmailLogByDesignationId(periodId, designationId);
				empList = (List<EmployeeInfo>)await employeeInfoService.GetSalaryPaidEmployeeByDegignationId(periodId, designationId);
			}
			if (option == 2) //Branch
			{
				await emailSenderService.DeleteEmailLogByBranchId(periodId, branchid);
				empList = (List<EmployeeInfo>)await employeeInfoService.GetSalaryPaidEmployeeByBranchId(periodId, branchid);
			}
			if (option == 1) //All
			{
				await emailSenderService.DeleteAllEmailLogByPeriodId(periodId);
				empList = (List<EmployeeInfo>)await employeeInfoService.GetAllSalaryActiveEmployeesByPeriodId(periodId);
			}

			//Asad Commented On 08.08.2023
			//var empList = await employeeInfoService.GetAllSalaryActiveEmployeesByPeriodId(periodId);

			
			var salaryperiod = await salaryService.GetSalaryPeriodById(periodId);

			string folder = _hostingEnvironment.WebRootPath + "/pdf/" + salaryperiod.periodName + "/Incomplete";
			//await salaryService.DeleteSendEmailLogStatus(salaryperiod.periodName);
			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}
			else
			{
				Directory.Delete(folder, true);
				Directory.CreateDirectory(folder);
			}

			//Asad Commented On 08.08.2023
			//await emailSenderService.DeleteAllEmailLogByPeriodId(periodId);


			foreach (var item in empList)
			{
				url = scheme + "://" + host + "/Payroll/PayrollReport/SalaryCertificateReportView?employeeInfoId=" + item.Id + "&salaryPeriodId=" + periodId;

				string fName = salaryperiod.periodName + "/Incomplete/Payslip_" + salaryperiod.periodName + "_" + item.employeeCode + ".pdf";
				string fileurl = myPDF.GeneratePDFForMailNew(out status, url, fName);

				if (!string.IsNullOrWhiteSpace(item.emailAddress))
				{
					var data = new SendEmailLogStatus
					{
						//sender = "notun01@gmail.com",
						sender = "pmis@bdbl.com.bd",
						receiver = item.emailAddress,
						date = DateTime.Now,
						employeeId = item.Id,
						type = null,
						module = fName,
						itemName = "PaySlip",
						isDelete = salaryperiod.Id,
						PeriodId = periodId,
						DesignationId = item.lastDesignationId,
						BranchId = item.hrBranchId
					};
					await salaryService.SaveSendEmailLogStatus(data);
				}
			}

			//if (empList?.Count() > 0)
			//{
			//	await SendMail(empList, option, periodId, mailText);
			//}
			
			return RedirectToAction("EmailPaySlip");
		}

		//Asad Added on 09.08.2023
		//private async Task<int> SendMail(List<EmployeeInfo> empList, int option, int periodId, string mailText)
		//{
		//	try
		//	{
		//		string fileName;

		//		if (option == 0)
		//		{
		//			var employeeId = empList.FirstOrDefault().Id;
		//			var emailAddress = empList.FirstOrDefault().emailAddress;
		//			var employeeName = empList.FirstOrDefault().nameEnglish;
					

		//			//var emp = await employeeInfoService.GetEmployeeInfoById((int)employeeId);

		//			//var email = emp.emailAddress;

		//			string host = HttpContext.Request.Host.ToString();
		//			string scheme = Request.Scheme;
		//			string baseUrl = $"" + scheme + "://" + host + "/Payroll/PayrollReport/SalaryCertificateReport?rptType=SaCertificate&employeeInfoId=" + employeeId + "&salaryPeriodId=" + periodId;

		//			string html = "<div><strong>Your PaySlip.</strong></div>"
		//					+ " <br/>"
		//					+ "<p>Dear Sir,</p>"
		//					+ " <br/>"
		//					+ mailText + " <br/>"
		//					+ " This is your payslip given by accounts department please download it by clicking below"
		//					+ "<br/>"
		//					+ "<div><a href='" + baseUrl + "' download><button>PaySlip</button></a></div>"
		//					+ " <br/>"
		//					+ "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Accounts Department.</p></div>"
		//					+ " <br/>";

		//			string scheme1 = Request.Scheme;
		//			var host1 = Request.Host;
		//			string url = "";

		//			url = scheme + "://" + host + "/Payroll/PayrollReport/SalaryCertificateReportView?employeeInfoId=" + employeeId + "&salaryPeriodId=" + periodId;

		//			string fileurl = myPDF.GeneratePDFForMail(out fileName, url);


		//			if (email != null)
		//			{

		//				await emailSenderService.SendEmailWithAttatchment(email, "Accounts Department", mailText, fileurl);
		//				SendEmailLogStatus data = new SendEmailLogStatus
		//				{
		//					sender = HttpContext.User.Identity.Name,
		//					receiver = employeeName,
		//					receiverEmail = emailAddress,
		//					employeeId = employeeId,
		//					date = DateTime.Now,
		//					type = "Send",
		//					module = "HR",
		//					itemName = "PaySlip"
		//				};
		//				await salaryService.SaveSendEmailLogStatus(data);
		//			}
		//		}
		//		else if (model.All == 2)    //Branch
		//		{
		//			//Asad Added On 09.08.2023
		//			var empList = await employeeInfoService.GetSalaryPaidEmployeeByBranchId((int)model.salaryPeriodId, model.hrBranchid);

		//			//Asad Commented On 09.08.2023
		//			//var emps = await employeeInfoService.GetAllSalaryActiveEmployees();
		//			//var empList = emps.Where(x => x.hrBranchId == model.hrBranchid && x.Id == 2037).ToList();

		//			foreach (var data in empList)
		//			{
		//				var email = data.emailAddress;

		//				string host = HttpContext.Request.Host.ToString();
		//				string scheme = Request.Scheme;
		//				string baseUrl = $"" + scheme + "://" + host + "/Payroll/Emailing/GetSalaryPaySlipSendEmailLogStatus?employeeInfoId=" + data.Id + "&salaryPeriodId=" + model.salaryPeriodId;

		//				string html = "<div><strong>Your PaySlip.</strong></div>"
		//						+ " <br/>"
		//						+ "<p>Dear Sir,</p>"
		//						+ model.mailText + " <br/>"
		//						+ " This is your payslip given by accounts department please download it by clicking below button"
		//						+ "<br/>"
		//						+ "<div><a href='" + baseUrl + "' download><button style='font-size: 25px; margin-top: 10px; background: lightblue;'>Pay Slip</button></a></div>"
		//						+ " <br/>"
		//						+ "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Accounts Department.</p></div>"
		//						+ " <br/>";

		//				if (email != null)
		//				{
		//					await emailSenderService.SendEmailWithFrom(email, "Accounts Department", model.mailSub, html);
		//					SendEmailLogStatus data1 = new SendEmailLogStatus
		//					{
		//						sender = HttpContext.User.Identity.Name,
		//						receiver = data.nameEnglish,
		//						receiverEmail = email,
		//						employeeId = data.Id,
		//						date = DateTime.Now,
		//						type = "Send",
		//						module = "HR",
		//						itemName = "PaySlip"
		//					};
		//					await salaryService.SaveSendEmailLogStatus(data1);
		//				}
		//			}
		//		}
		//		else if (model.All == 3) //Designation
		//		{
		//			var salaryPeriodId = model.salaryPeriodId == null ? default(int) : model.salaryPeriodId.Value;

		//			var empList = await employeeInfoService.GetSalaryPaidEmployeeByDegignationId((int)model.salaryPeriodId, (int)model.designationId);

		//			//Asad Commented On 09.08.2023
		//			//var empList = await employeeInfoService.GetAllSalaryActiveEmployeesByPeriodAndDesig((int)model.salaryPeriodId, (int)model.designationId);


		//			foreach (var data in empList)
		//			{
		//				var email = data.emailAddress;

		//				string host = HttpContext.Request.Host.ToString();
		//				string scheme = Request.Scheme;

		//				if (email != null)
		//				{
		//					var payslipLog = await emailSenderService.getAttatchmentUrlByEmpId(data.Id, (int)model.salaryPeriodId);
		//					if (payslipLog != null)
		//					{
		//						if (payslipLog.type != "Send")
		//						{
		//							await emailSenderService.SendEmailWithAttatchment(email, "Accounts Department", model.mailText, _hostingEnvironment.WebRootPath + "/pdf/" + payslipLog.module);

		//							Thread.Sleep(1000);

		//							payslipLog.type = "Send";
		//							await salaryService.SaveSendEmailLogStatus(payslipLog);
		//						}
		//					}
		//				}
		//				Thread.Sleep(3000);
		//			}
		//		}
		//		else
		//		{
		//			var empList = await employeeInfoService.GetAllSalaryActiveEmployeesByPeriodId((int)model.salaryPeriodId);
		//			foreach (var data in empList)
		//			{
		//				var email = data.emailAddress;

		//				string host = HttpContext.Request.Host.ToString();
		//				string scheme = Request.Scheme;
		//				string baseUrl = $"" + scheme + "://" + host + "/Payroll/Emailing/GetSalaryPaySlipSendEmailLogStatus?employeeInfoId=" + data.Id + "&salaryPeriodId=" + model.salaryPeriodId;
		//				string fileurl = myPDF.GeneratePDFForMail(out fileName, baseUrl);

		//				string html = "<div><strong>Your PaySlip.</strong></div>"
		//						+ " <br/>"
		//						+ "<p>Dear Sir,</p>"
		//						+ model.mailText + " <br/>"
		//						+ " This is your payslip given by accounts department please download it by clicking below button"
		//						+ "<br/>"
		//						+ "<div><a href='" + baseUrl + "' download><button>PaySlip</button></a></div>"
		//						+ " <br/>"
		//						+ "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Accounts Department.</p></div>"
		//						+ " <br/>";

		//				if (email != null)
		//				{
		//					//await emailSenderService.SendEmailWithFrom(email, "Accounts Department", model.mailSub, html);
		//					await emailSenderService.SendEmailWithAttatchment(email, "Accounts Department", model.mailText, fileurl);

		//					SendEmailLogStatus data1 = new SendEmailLogStatus
		//					{
		//						sender = HttpContext.User.Identity.Name,
		//						receiver = data.nameEnglish,
		//						receiverEmail = email,
		//						employeeId = data.Id,
		//						date = DateTime.Now,
		//						type = "Send",
		//						module = "HR",
		//						itemName = "PaySlip"
		//					};
		//					await salaryService.SaveSendEmailLogStatus(data1);
		//				}
		//			}
		//		}
		//		return RedirectToAction(nameof(EmailPaySlip));
		//	}
		//	catch (Exception ex)
		//	{
		//		return RedirectToAction(nameof(EmailPaySlip));
		//	}

		//}


		public IActionResult CountPayslipGenerate(string period)
		{
			string path = _hostingEnvironment.WebRootPath + "/pdf/" + period + "/Incomplete/";
			int fCount = 0;
			if (Directory.Exists(path))
			{
				fCount = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length;
			}
			return Json(fCount);
		}

		public IActionResult CountFixationGenerate(string type, int year)
		{
			string path = _hostingEnvironment.WebRootPath + "/pdf/" + type + "_" + year.ToString() + "/Incomplete/";
			int fCount = 0;
			if (Directory.Exists(path))
			{
				fCount = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length;
			}
			return Json(fCount);
		}

		public async Task<ActionResult> EmailPaySlip()
		{
			EmailingViewModel model = new EmailingViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriod(),
				hrBranches = await salaryService.GetAllHrBranchs(),
				designations = await employeeInfoService.GetAllDesignation()
			};

			return View(model);
		}

		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EmailPaySlip([FromForm] EmailingViewModel model)
		{
			//return Json(model);
			try
			{
				string fileName;
				var logFileName = DateTime.Now.ToString("yyyy-dd-M") + ".txt";
				var path = Path.Combine(_hostingEnvironment.WebRootPath, "Logger\\Logs\\" + logFileName);
				

				if (model.All == 0)
				{
					
					var emp = await employeeInfoService.GetEmployeeInfoById((int)model.employeeInfoId);
										
					string host = HttpContext.Request.Host.ToString();
					string scheme = Request.Scheme;
					string baseUrl = $"" + scheme + "://" + host + "/Payroll/PayrollReport/SalaryCertificateReport?rptType=SaCertificate&employeeInfoId=" + model.employeeInfoId + "&salaryPeriodId=" + model.salaryPeriodId;

					string html = "<div><strong>Your PaySlip.</strong></div>"
							+ " <br/>"
							+ "<p>Dear Sir,</p>"
							+ " <br/>"
							+ model.mailText + " <br/>"
							+ " This is your payslip given by accounts department please download it by clicking below"
							+ "<br/>"
							+ "<div><a href='" + baseUrl + "' download><button>PaySlip</button></a></div>"
							+ " <br/>"
							+ "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Accounts Department.</p></div>"
							+ " <br/>";


					string scheme1 = Request.Scheme;
					var host1 = Request.Host;
					string url = "";

					url = scheme + "://" + host + "/Payroll/PayrollReport/SalaryCertificateReportView?employeeInfoId=" + model.employeeInfoId + "&salaryPeriodId=" + model.salaryPeriodId;
                    string prefix = "SalaryCertificate";

                    if (emp?.emailAddress != null)
					{
						var info = "ID: " + emp.Id.ToString() + "   Code: " + emp.employeeCode + "   Name: " + emp.nameEnglish + "   Email: " + emp.emailAddress;

						try
						{
							//Added By Asad On 10.08.2023
							var emailLog = await emailSenderService.GetEmailLogStatusByEmpId((int)emp.Id, (int)model.salaryPeriodId);
							if (emailLog != null)
							{
								if (emailLog.type != "Send")
								{
									string fileurl = myPDF.GeneratePDFForMail(out fileName, prefix, url);
									await emailSenderService.SendEmailWithAttatchment(emp?.emailAddress, "Accounts Department", model.mailText, fileurl);

									emailLog.type = "Send";
									await salaryService.SaveSendEmailLogStatus(emailLog);
								}
							}
														
							info = "Succeeded: " + info;
							Utility.Helper.Logger.Write("Emailing Pay Slip", info, null, path);
						}
						catch(Exception ex)
						{
							info = "Failed: " + info;
							Utility.Helper.Logger.Write("Emailing Pay Slip", info, ex, path);
						}
                        
                    }
				}
				else if (model.All == 2)	//Branch
				{
					//Asad Added On 09.08.2023
					var empList = await employeeInfoService.GetSalaryPaidEmployeeByBranchId((int)model.salaryPeriodId, model.hrBranchid);
										
					foreach (var data in empList)
					{
						
						string host = HttpContext.Request.Host.ToString();
						string scheme = Request.Scheme;
						string baseUrl = $"" + scheme + "://" + host + "/Payroll/Emailing/GetSalaryPaySlipSendEmailLogStatus?employeeInfoId=" + data.Id + "&salaryPeriodId=" + model.salaryPeriodId;

						string html = "<div><strong>Your PaySlip.</strong></div>"
								+ " <br/>"
								+ "<p>Dear Sir,</p>"
								+ model.mailText + " <br/>"
								+ " This is your payslip given by accounts department please download it by clicking below button"
								+ "<br/>"
								+ "<div><a href='" + baseUrl + "' download><button style='font-size: 25px; margin-top: 10px; background: lightblue;'>Pay Slip</button></a></div>"
								+ " <br/>"
								+ "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Accounts Department.</p></div>"
								+ " <br/>";

						if (data.emailAddress != null)
						{
						var info = "ID: " + data.Id.ToString() + "   Code: " +  data.employeeCode + "   Name: " + data.nameEnglish + "   Email: " + data.emailAddress;

							try
							{
								//Asad Added On 10.08.2023
								var emailLog = await emailSenderService.GetEmailLogStatusByEmpId((int)data.Id, (int)model.salaryPeriodId);
								if (emailLog != null)
								{
									if (emailLog.type != "Send")
									{
										await emailSenderService.SendEmailWithFrom(data.emailAddress, "Accounts	Department", model.mailSub, html);

										emailLog.type = "Send";
										await salaryService.SaveSendEmailLogStatus(emailLog);
									}
								}

								info = "Succeeded: " + info;
								Utility.Helper.Logger.Write("Emailing Pay Slip", info, null, path);
							}
							catch(Exception ex) 
							{
								info = "Failed: " + info;
								Utility.Helper.Logger.Write("Emailing Pay Slip", info, ex, path);
							}

                        }
                    }
				}
				else if (model.All == 3) //Designation
				{
					var salaryPeriodId = model.salaryPeriodId == null ? default(int) : model.salaryPeriodId.Value;

					var empList = await employeeInfoService.GetSalaryPaidEmployeeByDegignationId((int)model.salaryPeriodId, (int)model.designationId);
										
					foreach (var data in empList)
					{

						string host = HttpContext.Request.Host.ToString();
						string scheme = Request.Scheme;

						if (data.emailAddress != null)
						{
							var info = "ID: " + data.Id.ToString() + "   Code: " + data.employeeCode + "   Name: " + data.nameEnglish + "   Email: " + 
					
							data.emailAddress;

							var emailLog = await emailSenderService.GetEmailLogStatusByEmpId(data.Id, (int)model.salaryPeriodId);
							if (emailLog != null)
							{
								if (emailLog.type != "Send")
								{
									try
									{
										await emailSenderService.SendEmailWithAttatchment(data.emailAddress, "Accounts Department", model.mailText, _hostingEnvironment.WebRootPath + "/pdf/" + emailLog.module);

										Thread.Sleep(1000);

										emailLog.type = "Send";
										await salaryService.SaveSendEmailLogStatus(emailLog);
																				
										info = "Succeeded: " + info;
										Utility.Helper.Logger.Write("Emailing Pay Slip", info, null, path);
									}
									catch (Exception ex)
									{
										info = "Failed: " + info;
										Utility.Helper.Logger.Write("Emailing Pay Slip", info, ex, path);
									}
								}
							}
						}
						Thread.Sleep(3000);
					}
				}
				else
				{
					var empList = await employeeInfoService.GetAllSalaryActiveEmployeesByPeriodId((int)model.salaryPeriodId);
					foreach (var data in empList)
					{						
						string host = HttpContext.Request.Host.ToString();
						string scheme = Request.Scheme;
						string baseUrl = $"" + scheme + "://" + host + "/Payroll/Emailing/GetSalaryPaySlipSendEmailLogStatus?employeeInfoId=" + data.Id + "&salaryPeriodId=" + model.salaryPeriodId;
                        string prefix = "SalaryCertificate";

                        string html = "<div><strong>Your PaySlip.</strong></div>"
								+ " <br/>"
								+ "<p>Dear Sir,</p>"
								+ model.mailText + " <br/>"
								+ " This is your payslip given by accounts department please download it by clicking below button"
								+ "<br/>"
								+ "<div><a href='" + baseUrl + "' download><button>PaySlip</button></a></div>"
								+ " <br/>"
								+ "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Accounts Department.</p></div>"
								+ " <br/>";

						if (data.emailAddress != null)
						{
							//Asad Added on 10.08.2023
							var info = "ID: " + data.Id.ToString() + "   Emp. Code: " + data.employeeCode + "   Name: " + data.nameEnglish + "   Email: " + data.emailAddress;

							try
							{
								var emailLog = await emailSenderService.GetEmailLogStatusByEmpId((int)data.Id, (int)model.salaryPeriodId);
								if (emailLog != null)
								{
									if (emailLog.type != "Send")
									{
										string fileurl = myPDF.GeneratePDFForMail(out fileName, prefix, baseUrl);
										await emailSenderService.SendEmailWithAttatchment(data.emailAddress, "Accounts Department", model.mailText, fileurl);

										emailLog.type = "Send";
										await salaryService.SaveSendEmailLogStatus(emailLog);
									}
								}

								info = "Succeeded: " + info;
								Utility.Helper.Logger.Write("Emailing Pay Slip", info, null, path);
							}
							catch(Exception ex)
							{
								info = "Failed: " + info;
								Utility.Helper.Logger.Write("Emailing Pay Slip", info, ex, path);
							}

                        }
                    }
				}
				return RedirectToAction(nameof(EmailPaySlip));
			}
			catch (Exception ex)
			{
				return RedirectToAction(nameof(EmailPaySlip));
			}

		}

		#region Fixation Mail
		public async Task<IActionResult> GenerateAllFixationPdf(string type, int year)
		{
			string status = "";
			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";

			var empList = await employeeInfoService.GetAllFixationEmployee(type, year);

			string folder = _hostingEnvironment.WebRootPath + "/pdf/" + type + "_" + year.ToString() + "/Incomplete";

			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}
			else
			{
				Directory.Delete(folder, true);
				Directory.CreateDirectory(folder);
			}

			foreach (var item in empList)
			{
				url = scheme + "://" + host + "/Payroll/Fixation/IncrementReportViewMail?id=" + item.Id;

				string fName = type + "_" + year.ToString() + "/Incomplete/" + type + "_" + year.ToString() + "_" + item.empCode + ".pdf";
				string fileurl = myPDF.GeneratePDFForMailNew(out status, url, fName);

				var data = new SendEmailLogStatus
				{
					sender = "pmis@bdbl.com.bd",
					receiver = item.employee?.emailAddress,
					date = DateTime.Now,
					employeeId = item.employee?.Id,
					type = null,
					module = fName,
					itemName = type,
					//isDelete = salaryperiod.Id
				};

				await salaryService.SaveSendEmailLogStatus(data);
			}

			return RedirectToAction("EmailFixationIncrement");
		}

		public async Task<ActionResult> EmailFixationIncrement()
		{
			EmailingViewModel model = new EmailingViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriod(),
				hrBranches = await salaryService.GetAllHrBranchs(),
				designations = await employeeInfoService.GetAllDesignation()
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EmailFixationIncrement([FromForm] EmailingViewModel model)
		{
			//return Json(model);
			try
			{
				string fileName;

				if (model.All == 0)
				{
					var emp = await employeeInfoService.GetEmployeeInfoById((int)model.employeeInfoId);
					var email = emp.emailAddress;

					string host = HttpContext.Request.Host.ToString();
					string scheme = Request.Scheme;
					string baseUrl = $"" + scheme + "://" + host + "/Payroll/PayrollReport/SalaryCertificateReport?rptType=SaCertificate&employeeInfoId=" + model.employeeInfoId + "&salaryPeriodId=" + model.salaryPeriodId;
                    string prefix = "Increment";

                    string html = "<div><strong>Your PaySlip.</strong></div>"
							+ " <br/>"
							+ "<p>Dear Sir,</p>"
							+ " <br/>"
							+ model.mailText + " <br/>"
							+ " This is your payslip given by accounts department please download it by clicking below"
							+ "<br/>"
							+ "<div><a href='" + baseUrl + "' download><button>PaySlip</button></a></div>"
							+ " <br/>"
							+ "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Accounts Department.</p></div>"
							+ " <br/>";

					string scheme1 = Request.Scheme;
					var host1 = Request.Host;
					string url = "";

					url = scheme + "://" + host + "/Payroll/PayrollReport/SalaryCertificateReportView?employeeInfoId=" + model.employeeInfoId + "&salaryPeriodId=" + model.salaryPeriodId;

					string fileurl = myPDF.GeneratePDFForMail(out fileName, prefix, url);


					if (email != null)
					{
						//await emailSenderService.SendEmailWithFrom(email, "Accounts Department", model.mailSub, html);
						await emailSenderService.SendEmailWithAttatchment(email, "Accounts Department", model.mailText, fileurl);
						SendEmailLogStatus data = new SendEmailLogStatus
						{
							sender = HttpContext.User.Identity.Name,
							receiver = emp.nameEnglish,
							receiverEmail = email,
							employeeId = emp.Id,
							date = DateTime.Now,
							type = "Send",
							module = "HR",
							itemName = "PaySlip"
						};
						await salaryService.SaveSendEmailLogStatus(data);
					}
				}
				else if (model.All == 2)
				{
					var emps = await employeeInfoService.GetAllSalaryActiveEmployees();
					var empList = emps.Where(x => x.hrBranchId == model.hrBranchid && x.Id == 2037).ToList();
					foreach (var data in empList)
					{
						var email = data.emailAddress;

						string host = HttpContext.Request.Host.ToString();
						string scheme = Request.Scheme;
						string baseUrl = $"" + scheme + "://" + host + "/Payroll/Emailing/GetSalaryPaySlipSendEmailLogStatus?employeeInfoId=" + data.Id + "&salaryPeriodId=" + model.salaryPeriodId;

						string html = "<div><strong>Your PaySlip.</strong></div>"
								+ " <br/>"
								+ "<p>Dear Sir,</p>"
								+ model.mailText + " <br/>"
								+ " This is your payslip given by accounts department please download it by clicking below button"
								+ "<br/>"
								+ "<div><a href='" + baseUrl + "' download><button style='font-size: 25px; margin-top: 10px; background: lightblue;'>Pay Slip</button></a></div>"
								+ " <br/>"
								+ "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Accounts Department.</p></div>"
								+ " <br/>";

						if (email != null)
						{
							await emailSenderService.SendEmailWithFrom(email, "Accounts Department", model.mailSub, html);
							SendEmailLogStatus data1 = new SendEmailLogStatus
							{
								sender = HttpContext.User.Identity.Name,
								receiver = data.nameEnglish,
								receiverEmail = email,
								employeeId = data.Id,
								date = DateTime.Now,
								type = "Send",
								module = "HR",
								itemName = "PaySlip"
							};
							await salaryService.SaveSendEmailLogStatus(data1);
						}
					}
				}
				else if (model.All == 3)
				{
					var empList = await employeeInfoService.GetAllSalaryActiveEmployeesByPeriodAndDesig((int)model.salaryPeriodId, (int)model.designationId);
					foreach (var data in empList)
					{
						var email = data.emailAddress;

						string host = HttpContext.Request.Host.ToString();
						string scheme = Request.Scheme;

						if (email != null)
						{
							var payslipLog = await emailSenderService.getAttatchmentUrlByEmpId(data.Id, (int)model.salaryPeriodId);
							if (payslipLog.type != "Send")
							{
								await emailSenderService.SendEmailWithAttatchment(email, "Accounts Department", model.mailText, _hostingEnvironment.WebRootPath + "/pdf/" + payslipLog.module);

								payslipLog.type = "Send";
								await salaryService.SaveSendEmailLogStatus(payslipLog);
							}
						}
					}
				}
				else
				{
					var empList = await employeeInfoService.GetAllSalaryActiveEmployeesByPeriodId((int)model.salaryPeriodId);
					foreach (var data in empList)
					{
						var email = data.emailAddress;

						string host = HttpContext.Request.Host.ToString();
						string scheme = Request.Scheme;
						string baseUrl = $"" + scheme + "://" + host + "/Payroll/Emailing/GetSalaryPaySlipSendEmailLogStatus?employeeInfoId=" + data.Id + "&salaryPeriodId=" + model.salaryPeriodId;
						string prefix = "SalaryCertificate";

						string fileurl = myPDF.GeneratePDFForMail(out fileName, prefix, baseUrl);

						string html = "<div><strong>Your PaySlip.</strong></div>"
								+ " <br/>"
								+ "<p>Dear Sir,</p>"
								+ model.mailText + " <br/>"
								+ " This is your payslip given by accounts department please download it by clicking below button"
								+ "<br/>"
								+ "<div><a href='" + baseUrl + "' download><button>PaySlip</button></a></div>"
								+ " <br/>"
								+ "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Accounts Department.</p></div>"
								+ " <br/>";

						if (email != null)
						{
							//await emailSenderService.SendEmailWithFrom(email, "Accounts Department", model.mailSub, html);
							await emailSenderService.SendEmailWithAttatchment(email, "Accounts Department", model.mailText, fileurl);

							SendEmailLogStatus data1 = new SendEmailLogStatus
							{
								sender = HttpContext.User.Identity.Name,
								receiver = data.nameEnglish,
								receiverEmail = email,
								employeeId = data.Id,
								date = DateTime.Now,
								type = "Send",
								module = "HR",
								itemName = "PaySlip"
							};
							await salaryService.SaveSendEmailLogStatus(data1);
						}
					}
				}
				return RedirectToAction(nameof(EmailPaySlip));
			}
			catch (Exception ex)
			{
				return RedirectToAction(nameof(EmailPaySlip));
			}

		}

		#endregion
		public async Task<IActionResult> CheckEmailSendingStatusByperiodId(int periodId)
		{
			var data = new EmailingViewModel
			{
				emailStatusViewModels = await emailSenderService.GetEmailSendingStatusByPeriodId(periodId)
			};
			return View(data);
		}

		[HttpPost]
		public async Task<IActionResult> SendIndividualSalaryCertificate(int emailLogId)
		{
			var data = await emailSenderService.GetEmailSendingLogById(emailLogId);

			var email = data.employee?.emailAddress;

			string host = HttpContext.Request.Host.ToString();
			string scheme = Request.Scheme;

			if (email != null)
			{
				await emailSenderService.SendEmailWithAttatchment(email, "Accounts Department", "", _hostingEnvironment.WebRootPath + "/pdf/" + data.module);

				data.receiver = email;
				data.type = "Send";
				await salaryService.SaveSendEmailLogStatus(data);
			}

			return Json("Send");
		}

		[HttpGet]
		public async Task<IActionResult> GetSalaryPeriodById(int periodId)
		{
			return Json(await salaryService.GetSalaryPeriodNameById(periodId));
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetSalaryPaySlipSendEmailLogStatus(int employeeInfoId, int salaryPeriodId)
		{
			SendEmailLogStatus data1 = new SendEmailLogStatus
			{
				employeeId = employeeInfoId,
				date = DateTime.Now,
				type = "Download",
				module = "HR",
				itemName = "PaySlip"
			};
			await salaryService.SaveSendEmailLogStatus(data1);

			return RedirectToAction("SalaryReport", "PayrollReport", new
			{
				rptType = "PSLIP",
				employeeInfoId = employeeInfoId,
				salaryPeriodId = salaryPeriodId
			});
		}

	}
}