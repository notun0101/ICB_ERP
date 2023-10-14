using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSTraining.Models;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.TrainingNew.Teacher;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.HRPMS.Services.TrainingNew;
using OPUSERP.HRPMS.Services.TrainingNew.Interfaces;

namespace OPUSERP.Areas.HRPMSTraining.Controllers
{
    [Area("HRPMSTraining")]
    public class TrainingTeacherController : Controller
    {
        private readonly IIELTSTeacherService IELTSTeacherService;
        private readonly IReligionMunicipalityService religionService;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly string rootPath;

        public TrainingTeacherController(IHostingEnvironment _hostingEnvironment, IIELTSTeacherService _IELTSTeacherService, IReligionMunicipalityService _religionService)
        {
            this.IELTSTeacherService = _IELTSTeacherService;
            this.religionService = _religionService;
            this.hostingEnvironment = _hostingEnvironment;
            rootPath = hostingEnvironment.ContentRootPath;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AllTeacher()
        {
            var model = new IELTSTeacherViewModel
            {
                teacherBasics = await IELTSTeacherService.GetAllTeacherBasicsInfo()
            };
            return View(model);
        }

        public async Task<IActionResult> NewTeacher(int? id)
        {
            var model = new IELTSTeacherViewModel
            {
                religions = await religionService.GetReligions(),
                teacher = await IELTSTeacherService.GetTeacherBasic(Convert.ToInt32(id)),
                teacherCareers= await IELTSTeacherService.GetTeacherCareers(Convert.ToInt32(id)),
                teacherEducations= await IELTSTeacherService.GetTeacherEducations(Convert.ToInt32(id)),
                teacherExpertises= await IELTSTeacherService.GetTeacherExpertises(Convert.ToInt32(id)),
                teacherSocialMeadias= await IELTSTeacherService.GetTeacherSocialMeadias(Convert.ToInt32(id))
                //teacherVM = await IELTSTeacherService.GetTeacherById(Convert.ToInt32(id))
            };
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewTeacher([FromForm] IELTSTeacherViewModel model)
        {
            
            try
            {            
                var teacherBasic = new TeacherBasics();
                var teacherCareer = new TeacherCareer();
                var teacherEducation = new TeacherEducation();
                var teacherExpertise = new TeacherExpertise();
                var teacherSocial = new TeacherSocialMeadia();
                if (model.teacherId > 0)
                {
                    //Basic
                    teacherBasic = await IELTSTeacherService.GetTeacherBasicsById(Convert.ToInt32( model.teacherId));
                    teacherBasic.teacherCode = model.teacherCode;
                    teacherBasic.nameEnglish = model.nameEnglish;
                    teacherBasic.nameBangla = model.nameBangla;
                    teacherBasic.maritalStatus = model.maritalStatus;
                    teacherBasic.addressPresent = model.addressPresent;
                    teacherBasic.addressPermanant = model.addressPermanant;
                    teacherBasic.bloodGroup = model.bloodGroup;
                    teacherBasic.mobilePersonal = model.mobilePersonal;
                    teacherBasic.mobileOffice = model.mobileOffice;
                    teacherBasic.emailPersonal = model.emailPersonal;
                    teacherBasic.dateOfBirth = model.dateOfBirth;
                    teacherBasic.eTin = model.eTin;
                    teacherBasic.nid = model.nid;
                    teacherBasic.heightCm =model.heightCm;
                    teacherBasic.weightKg = model.weightKg;
                    teacherBasic.gender = model.gender;
                    teacherBasic.religionId = model.religion;
                    teacherBasic.about = model.about;
                    teacherBasic.updatedBy = User.Identity.Name;
                    teacherBasic.updatedAt = DateTime.Now;                
                }
                else
                {
                    teacherBasic = new TeacherBasics
                    {
                        teacherCode = model.teacherCode,
                        nameEnglish = model.nameEnglish,
                        nameBangla = model.nameBangla,
                        maritalStatus = model.maritalStatus,
                        addressPresent = model.addressPresent,
                        addressPermanant = model.addressPermanant,
                        bloodGroup = model.bloodGroup,
                        mobilePersonal = model.mobilePersonal,
                        mobileOffice = model.mobileOffice,
                        emailPersonal = model.emailPersonal,
                        dateOfBirth = model.dateOfBirth,
                        eTin = model.eTin,
                        nid = model.nid,
                        heightCm = model.heightCm,
                        weightKg = model.weightKg,
                        gender = model.gender,
                        religionId = model.religion,
                        about = model.about,
                        status = 0,
                        createdAt=DateTime.Now,
                        createdBy=User.Identity.Name
                    };
                }
                if (model.teacherPhotoUrl != null)
                {                
                    string fileName = "";
                    string message1 = FileSave.SaveImage(out fileName, model.teacherPhotoUrl);
                    teacherBasic.photoUrl = fileName;
                }
                var teacherId = await IELTSTeacherService.SaveTeacher(teacherBasic);
                if (model.teacherId > 0)
                {
                    //Career
                    var del1 = await IELTSTeacherService.DeleteTeacherCareerById(Convert.ToInt32(model.teacherId));
                    //Education
                    var del2 = await IELTSTeacherService.DeleteTeacherEducationById(Convert.ToInt32(model.teacherId));
                    //Expertise
                    var del3 = await IELTSTeacherService.DeleteTeacherExpertiseById(Convert.ToInt32(model.teacherId));
                    //Social
                    var del4 = await IELTSTeacherService.DeleteTeacherSocialeById(Convert.ToInt32(model.teacherId));
                }
                if (model.socialNameArr !=null)
                {
                    for (int i = 0; i < model.socialNameArr.Length; i++)
                    {
                        teacherSocial = new TeacherSocialMeadia
                        {
                            name = model.socialNameArr[i],
                            username = model.socialUserNameArr[i],
                            teacherId = teacherId,
                            createdBy = User.Identity.Name,
                            createdAt = DateTime.Now
                        };
                        var carrerId = await IELTSTeacherService.SaveTeacherSocial(teacherSocial);
                    }
                }
                if (model.companyNameArr != null)
                {
                    for (int i = 0; i < model.companyNameArr.Length; i++)
                    {
                        teacherCareer = new TeacherCareer
                        {
                            companyName = model.companyNameArr[i],
                            role = model.roleArr[i],
                            startYear = model.startYearArr[i],
                            endYear = model.endYearArr[i],
                            description = model.descriptionArr[i],
                            teacherId = teacherId,
                            createdBy = User.Identity.Name,
                            createdAt = DateTime.Now
                        };
                        var carrerId = await IELTSTeacherService.SaveTeacherCareer(teacherCareer);
                    }
                }
                if (model.degreeArr != null)
                {
                    for (int i = 0; i < model.degreeArr.Length; i++)
                    {
                        teacherEducation = new TeacherEducation
                        {
                            universityName = model.universityArr[i],
                            degree = model.degreeArr[i],
                            session = model.sessionArr[i],
                            description = model.desEduArr[i],
                            teacherId = teacherId,
                            createdBy = User.Identity.Name,
                            createdAt = DateTime.Now
                        };
                        var eduId = await IELTSTeacherService.SaveTeacherEducation(teacherEducation);
                    }
                }
                if (model.titleArr != null)
                {
                    for (int i = 0; i < model.titleArr.Length; i++)
                    {
                        teacherExpertise = new TeacherExpertise
                        {
                            title=model.titleArr[i],
                            subtitle=model.subtitleArr[i],
                            teacherId=teacherId,
                            createdBy=User.Identity.Name,
                            createdAt=DateTime.Now
                        };
                        var eId=await IELTSTeacherService.SaveTeacherExpertise(teacherExpertise);
                    }
                }
                return RedirectToAction("AllTeacher");
                }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}