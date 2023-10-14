using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.HRPMSTraining.Models;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.TrainingNew.Teacher;
using OPUSERP.HRPMS.Services.TrainingNew.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.TrainingNew
{
    public class IELTSTeacherService: IIELTSTeacherService
    {
        private readonly ERPDbContext _context;

        public IELTSTeacherService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeacherBasics>> GetAllTeacherBasics()
        {
            return await _context.teacherBasics.AsNoTracking().ToListAsync();
        }

        public async Task<TeacherBasics> GetTeacherBasic(int id)
        {
            return await _context.teacherBasics.Include(x => x.religion).Where(x => x.Id == id).FirstOrDefaultAsync(); 
        }

        public async Task<IEnumerable<TeacherCareer>> GetTeacherCareers(int id)
        {
            return await _context.teacherCareers.Where(x => x.teacherId == id).ToListAsync(); 
        }

        public async Task<IEnumerable<TeacherEducation>> GetTeacherEducations(int id)
        {
            return await _context.teacherEducations.Where(x => x.teacherId == id).ToListAsync(); 
        }

        public async Task<IEnumerable<TeacherExpertise>> GetTeacherExpertises(int id)
        {
            return await _context.teacherExpertises.Where(x => x.teacherId == id).ToListAsync();
        }

        public async Task<IEnumerable<TeacherSocialMeadia>> GetTeacherSocialMeadias(int id)
        {
            return await _context.teacherSocialMeadias.Where(x => x.teacherId == id).ToListAsync();
        }
        //public async Task<IELTSTeacherViewModel> GetTeacherById(int id)
        //{
        //    var data =new IELTSTeacherViewModel();
        //    data.teacher = await _context.teacherBasics.Include(x=>x.religion).Where(x => x.Id == id).FirstOrDefaultAsync();
        //    data.teacherCareers= await _context.teacherCareers.Where(x => x.teacherId == id).ToListAsync();
        //    data.teacherEducations= await _context.teacherEducations.Where(x => x.teacherId == id).ToListAsync();
        //    data.teacherExpertises= await _context.teacherExpertises.Where(x => x.teacherId == id).ToListAsync();
        //    data.teacherSocialMeadias= await _context.teacherSocialMeadias.Where(x => x.teacherId == id).ToListAsync();
        //    return data;
        //}

        public async Task<IEnumerable<TeacherBasics>> GetAllTeacherBasicsInfo()
        {
            var data= await _context.teacherBasics.Include(x=>x.religion).AsNoTracking().ToListAsync();
            var result = new List<TeacherBasics>();
            foreach (var item in data)
            {
                var degree = "";
                List<string> degList = await _context.teacherEducations.Where(x => x.teacherId == item.Id).Select(x => x.degree).ToListAsync();
                if(degList != null || degList.Count() > 0)
                {
                    degree = string.Join(",", degList.ToArray());
                }
                result.Add(new TeacherBasics
                {
                    Id=item.Id,
                    nameBangla=item.nameBangla,
                    nameEnglish=item.nameEnglish,
                    teacherCode=item.teacherCode,
                    maritalStatus=item.maritalStatus,
                    addressPresent=item.addressPresent,
                    addressPermanant=item.addressPermanant,
                    bloodGroup=item.bloodGroup,
                    mobilePersonal=item.mobilePersonal,
                    photoUrl=item.photoUrl,
                    mobileOffice=item.mobileOffice,
                    dateOfBirth=item.dateOfBirth,
                    eTin=item.eTin,
                    nid=item.nid,
                    heightCm=item.heightCm,
                    weightKg=item.weightKg,
                    religion=item.religion,
                    about=item.about,
                    status=item.status,
                    isActive=item.isActive,
                    remarks=item.remarks,
                    gender =item.gender,
                    emailPersonal=item.emailPersonal,
                    updatedBy= degree
                });
            }
            return result;
        }

        public async Task<TeacherBasics> GetTeacherBasicsById(int id)
        {
            return await _context.teacherBasics.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<TeacherCareer> GetTeacherCareerById(int teacherId)
        {
            return await _context.teacherCareers.Where(x => x.teacherId == teacherId).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<TeacherEducation> GetTeacherEducationById(int teacherId)
        {
            return await _context.teacherEducations.Where(x => x.teacherId == teacherId).AsNoTracking().FirstOrDefaultAsync();
        }
        
        public async Task<TeacherExpertise> GetTeacherExpertiseById(int teacherId)
        {
            return await _context.teacherExpertises.Where(x => x.teacherId == teacherId).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<TeacherBasics> SaveTeacher(int id)
        {
            return await _context.teacherBasics.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<int> SaveTeacher(TeacherBasics teacherBasics)
        {
            if (teacherBasics.Id != 0)
            {
                _context.teacherBasics.Update(teacherBasics);
            }
            else
            {
                await _context.teacherBasics.AddAsync(teacherBasics);
            }

            await _context.SaveChangesAsync();
            return teacherBasics.Id;
        }

        public async Task<int> SaveTeacherCareer(TeacherCareer teacherCareer)
        {
            if (teacherCareer.Id != 0)
            {
                _context.teacherCareers.Update(teacherCareer);
            }
            else
            {
                await _context.teacherCareers.AddAsync(teacherCareer);
            }

            await _context.SaveChangesAsync();
            return teacherCareer.Id;
        }

        public async Task<int> SaveTeacherEducation(TeacherEducation teacherEducation)
        {
            if (teacherEducation.Id != 0)
            {
                _context.teacherEducations.Update(teacherEducation);
            }
            else
            {
                await _context.teacherEducations.AddAsync(teacherEducation);
            }

            await _context.SaveChangesAsync();
            return teacherEducation.Id;
        }

        public async Task<int> SaveTeacherExpertise(TeacherExpertise teacherExpertise)
        {
            if (teacherExpertise.Id != 0)
            {
                _context.teacherExpertises.Update(teacherExpertise);
            }
            else
            {
                await _context.teacherExpertises.AddAsync(teacherExpertise);
            }

            await _context.SaveChangesAsync();
            return teacherExpertise.Id;
        }

        public async Task<int> SaveTeacherSocial(TeacherSocialMeadia teacherSocialMeadia)
        {
            if (teacherSocialMeadia.Id != 0)
            {
                _context.teacherSocialMeadias.Update(teacherSocialMeadia);
            }
            else
            {
                await _context.teacherSocialMeadias.AddAsync(teacherSocialMeadia);
            }

            await _context.SaveChangesAsync();
            return teacherSocialMeadia.Id;
        }

        public async Task<int> DeleteTeacherCareerById(int teacherId)
        {
            _context.teacherCareers.RemoveRange(_context.teacherCareers.Where(a => a.teacherId == teacherId));
            var result = await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteTeacherEducationById(int teacherId)
        {
            _context.teacherEducations.RemoveRange(_context.teacherEducations.Where(a => a.teacherId == teacherId));
            var result = await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteTeacherExpertiseById(int teacherId)
        {
            _context.teacherExpertises.RemoveRange(_context.teacherExpertises.Where(a => a.teacherId == teacherId));
            var result = await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteTeacherSocialeById(int teacherId)
        {
            _context.teacherSocialMeadias.RemoveRange(_context.teacherSocialMeadias.Where(a => a.teacherId == teacherId));
            var result = await _context.SaveChangesAsync();
            return 1;
        }
    }
}
