using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
    [Table("AcrEmployeeInfo", Schema = "ACR")]
    public class AcrEmployeeInfo : Base
    {
        public int? assessmentId { get; set; }
        public Assessment assessment { get; set; }
        [MaxLength(10)]
        public string EmpCode { get; set; }
        [MaxLength(150)]
        public string EmpName { get; set; }
        [MaxLength(150)]
        public string OfficEmailID { get; set; }
        [MaxLength(10)]
        public string Status { get; set; }
        [MaxLength(15)]
        public string MaritalStatus { get; set; }
        [MaxLength(200)]
        public string FatherName { get; set; }
        [MaxLength(200)]
        public string MotherName { get; set; }
        [MaxLength(200)]
        public string SpouseName { get; set; }
        [MaxLength(100)]
        public string PerDistrict { get; set; }
        [MaxLength(100)]
        public string PreDistrict { get; set; }
        [MaxLength(20)]
        public string OfficMobile { get; set; }
        [MaxLength(20)]
        public string Mobile { get; set; }
        [MaxLength(8)]
        public string BirthDate { get; set; }
        [MaxLength(8)]
        public string JoiningDate { get; set; }
        [MaxLength(8)]
        public string PrlDate { get; set; }
        [MaxLength(20)]
        public string SSNo { get; set; }
        [MaxLength(15)]
        public string FileNo { get; set; }
        [MaxLength(20)]
        public string PFAC { get; set; }

        [MaxLength(20)]
        public string NationalID { get; set; }
        [MaxLength(150)]
        public string DesignationName { get; set; }
        [MaxLength(150)]
        public string JoiningDesignationName { get; set; }
        [MaxLength(10)]
        public string BranchCode { get; set; }
        [MaxLength(200)]
        public string BranchName { get; set; }
        [MaxLength(200)]
        public string DivisionName { get; set; }
        [MaxLength(40)]
        public string SalaryScale { get; set; }
        public decimal? BasicAmount { get; set; }
        public int? NoOfChild { get; set; }
        [MaxLength(20)]
        public string JoinDatePresentPosition { get; set; }
        [MaxLength(40)]
        public string JobUnderSupervisor { get; set; }
        [MaxLength(200)]
        public string EducationalQual { get; set; }
        [MaxLength(50)]
        public string LastEducation { get; set; }
        [MaxLength(150)]
        public string DiplomaName { get; set; }
        [MaxLength(4)]
        public string DiplomaYearOne { get; set; }
        [MaxLength(4)]
        public string DiplomaYearTwo { get; set; }
        [MaxLength(150)]
        public string ProfQName { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string RewardsList { get; set; }
        public string PunishmentList { get; set; }
        public string PunishmentLetterNo { get; set; }
        public string PunishmentDate { get; set; }
        public string LastPromotionDate { get; set; }

        [MaxLength(4)]
        public string DesiCode { get; set; }
        [MaxLength(4)]
        public string JoiningDesignation { get; set; }
        [MaxLength(4)]
        public string DivisionCode { get; set; }
    }
}
