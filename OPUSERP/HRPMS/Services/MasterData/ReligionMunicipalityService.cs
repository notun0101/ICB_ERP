﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.EntityFrameworkCore;
using OPUSERP.Data.Entity.MasterData;

namespace OPUSERP.HRPMS.Services.MasterData
{
    public class ReligionMunicipalityService : IReligionMunicipalityService
    {
        private readonly ERPDbContext _context;

        public ReligionMunicipalityService(ERPDbContext context)
        {
            _context = context;
        }

        //Religion
        public async Task<bool> DeleteReligionById(int id)
        {
            _context.religions.Remove(_context.religions.Find(id));
            return 1 == await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Religion>> GetReligions()
        {
            return await _context.religions.AsNoTracking().ToListAsync();
        }
        //Expertise
        public async Task<IEnumerable<EmpExpertise>> GetExpertise()
        {
            return await _context.empExpertises.AsNoTracking().ToListAsync();
        }

        public async Task<Religion> GetReligionById(int id)
        {
            return await _context.religions.FindAsync(id);
        }

        public async Task<bool> SaveReligion(Religion religion)
        {
            if(religion.Id != 0)
                _context.religions.Update(religion);
            else
                _context.religions.Add(religion);
            return 1 == await _context.SaveChangesAsync();
        }

        //MunicipilityLocation
        public async Task<bool> DeleteMunicipilityLocationById(int id)
        {
            _context.municipilityLocations.Remove(_context.municipilityLocations.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MunicipilityLocation>> GetMunicipilityLocation()
        {
            return await _context.municipilityLocations.AsNoTracking().ToListAsync();
        }

        public async Task<MunicipilityLocation> GetMunicipilityLocationById(int id)
        {
            return await _context.municipilityLocations.FindAsync(id);
        }

        public async Task<bool> SaveMunicipilityLocation(MunicipilityLocation municipilityLocation)
        {
            if(municipilityLocation.Id != 0)
                _context.municipilityLocations.Update(municipilityLocation);
            else
                _context.municipilityLocations.Add(municipilityLocation);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveMedicalDisease(MedicalDisease medicalDisease)
        {
            if (medicalDisease.Id != 0)
                _context.medicalDiseases.Update(medicalDisease);
            else
                _context.medicalDiseases.Add(medicalDisease);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MedicalDisease>> GetMedicalDiseases()
        {
            return await _context.medicalDiseases.AsNoTracking().ToListAsync();
        }

        public async Task<MedicalDisease> GetMedicalDiseaseById(int id)
        {
            return await _context.medicalDiseases.FindAsync(id);
        }

        public async Task<bool> DeleteMedicalDiseaseById(int id)
        {
            _context.medicalDiseases.Remove(_context.medicalDiseases.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
    }
}
