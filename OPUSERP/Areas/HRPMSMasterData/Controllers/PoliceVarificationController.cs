using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.HRPMS.Services.Employee.Interfaces;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class PoliceVarificationController : Controller
    {
        private readonly ILisenceService lisenceService;
        private readonly IPersonalInfoService personalInfoService;

        private object myPDF;

        public PoliceVarificationController(IHostingEnvironment hostingEnvironment, ILisenceService lisenceService, IPersonalInfoService personalInfoService)
        {
            this.personalInfoService = personalInfoService;

            this.lisenceService = lisenceService;
        }
       
        //public IActionResult PoliceVarificationPdf()
        //{

            //string scheme = Request.Scheme;
            //var host = Request.Host;

            //string url = scheme + "://" + host + "/HRPMSEmployee/Info/ExperienceLetterView";

            //string fileName;
            //string status = myPDF.GeneratePDF(out fileName, url);
            //if (status != "done")
            //{
            //    return Content("<h1>" + status + "</h1>");
            //}

            //var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            //return new FileStreamResult(stream, "application/pdf");

        //}
    }
}