using Microsoft.AspNetCore.Http;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.TrainingNew.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSTraining.Models
{
    public class IELTSTeacherViewModel
    {
        public string teacherCode { get; set; }
        public string nameEnglish { get; set; }
        public string nameBangla { get; set; }
        public string maritalStatus { get; set; }
        public string addressPresent { get; set; }
        public string addressPermanant { get; set; }
        public string bloodGroup { get; set; }
        public string mobilePersonal { get; set; }
        public string mobileOffice { get; set; }
        public string emailPersonal { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string eTin { get; set; }
        public string nid { get; set; }
        public int heightCm { get; set; }
        public int weightKg { get; set; }
        public string gender { get; set; }
        public int religion { get; set; }
        public string about { get; set; }
        public int startYear { get; set; }
        public int endYear { get; set; }
        public string role { get; set; }
        public string companyName { get; set; }
        public string description { get; set; }
        public string descriptionEdu { get; set; }
        public int? teacherId { get; set; }
        public int status { get; set; }
        public string[] titleArr { get; set; }
        public string[] subtitleArr { get; set; }
        public string[] universityArr { get; set; }
        public string[] degreeArr { get; set; }
        public string[] sessionArr { get; set; }
        public string[] desEduArr { get; set; }
        public string[] companyNameArr { get; set; }
        public string[] roleArr { get; set; }
        public int[] startYearArr { get; set; }
        public int[] endYearArr { get; set; }
        public string[] descriptionArr { get; set; }
        public string[] socialNameArr { get; set; }
        public string[] socialUserNameArr { get; set; }
        public string[] socialLinkArr { get; set; }
        public IFormFile teacherPhotoUrl { get; set; }

        public IEnumerable<Religion> religions { get; set; }
        public IEnumerable<TeacherBasics> teacherBasics { get; set; }
        public TeacherBasics teacher { get; set; }
        public IEnumerable<TeacherCareer> teacherCareers { get; set; }
        public IEnumerable<TeacherEducation> teacherEducations { get; set; }
        public IEnumerable<TeacherExpertise> teacherExpertises { get; set; }
        public IEnumerable<TeacherSocialMeadia> teacherSocialMeadias { get; set; }
        //public IELTSTeacherViewModel teacherVM { get; set; }
    }
}
