using OPUSERP.Areas.HRPMSTraining.Models;
using OPUSERP.HRPMS.Data.Entity.TrainingNew.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.TrainingNew.Interfaces
{
    public interface IIELTSTeacherService
    {
        Task<IEnumerable<TeacherBasics>> GetAllTeacherBasics();
        Task<IEnumerable<TeacherBasics>> GetAllTeacherBasicsInfo();
        Task<TeacherBasics> GetTeacherBasicsById(int id);
        //Task<IELTSTeacherViewModel> GetTeacherById(int id);
        Task<TeacherCareer> GetTeacherCareerById(int teacherId);
        Task<TeacherEducation> GetTeacherEducationById(int teacherId);
        Task<TeacherExpertise> GetTeacherExpertiseById(int teacherId);
        Task<TeacherBasics> GetTeacherBasic(int id);
        Task<IEnumerable<TeacherCareer>> GetTeacherCareers(int id);
        Task<IEnumerable<TeacherEducation>> GetTeacherEducations(int id);
        Task<IEnumerable<TeacherExpertise>> GetTeacherExpertises(int id);
        Task<IEnumerable<TeacherSocialMeadia>> GetTeacherSocialMeadias(int id);
        Task<int> DeleteTeacherCareerById(int teacherId);
        Task<int> DeleteTeacherEducationById(int teacherId);
        Task<int> DeleteTeacherExpertiseById(int teacherId);
        Task<int> DeleteTeacherSocialeById(int teacherId);
        Task<int> SaveTeacher(TeacherBasics teacherBasics);
        Task<int> SaveTeacherCareer(TeacherCareer teacherCareer);
        Task<int> SaveTeacherEducation(TeacherEducation teacherEducation);
        Task<int> SaveTeacherExpertise(TeacherExpertise teacherExpertise);
        Task<int> SaveTeacherSocial(TeacherSocialMeadia teacherSocialMeadia);
    }
}
