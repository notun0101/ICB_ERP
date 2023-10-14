using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.SCMRequisition.Models;
using OPUSERP.Helpers;

namespace OPUSERP.Areas.SCMReport.Controllers
{
    [Area("SCMReport")]
    public class RequisitionNewReportController : Controller
    {
      
        private readonly MyPDF myPDF;
        private readonly string rootPath;

        public RequisitionNewReportController(IHostingEnvironment hostingEnvironment, IConverter converter)
        {
           
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> EquipmentDistributionView()
        {
            var model = new RequisitionViewModel
            {
            };
            return View();
        }
        [AllowAnonymous]

        public IActionResult EquipmentDistributionPdf()
        {

            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/SCMReport/RequisitionNewReport/EquipmentDistributionView";

            string fileName;
            string status = myPDF.GeneratePDF1(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }
    }

   
}
