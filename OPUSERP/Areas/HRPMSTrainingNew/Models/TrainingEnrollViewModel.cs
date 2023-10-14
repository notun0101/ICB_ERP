using OPUSERP.Areas.HRPMSTrainingNew.Models.Lang;
using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using OPUSERP.Models.HRPMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSTrainingNew.Models
{
    public class TrainingEnrollViewModel
    {
        public int id { get; set; }

        public string course { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? planeStartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? planeEndDate { get; set; }

        public string year { get; set; }

        public string participant { get; set; }

        public string employeeType { get; set; }

        public string budget { get; set; }

        public int noOfParticipantsActual { get; set; }

        public int employeeId { get; set; }

        public string organization { get; set; }

        public string designation { get; set; }

        public string employeeName { get; set; }

        public string nid { get; set; }

        public string address { get; set; }

        public string mobile { get; set; }

        public string email { get; set; }

        public string resourcePersonId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? startDateActual { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? endDateActual { get; set; }

        public ApplicationUser applicationUser { get; set; }

        public TrainingImplementationLn fLang { get; set; }

        public IEnumerable<EmployeeType> employeeTypes { get; set; }

        public IEnumerable<TrainingInfoNew> trainingInfoNews { get; set; }
        public TrainingInfoNew trainingInfoNew { get; set; }

        public IEnumerable<EnrolledTrainee> enrolledTrainees { get; set; }
        public EnrolledTrainee enrolledTraineeForTraining { get; set; }

        public IEnumerable<ResourcePerson> resourcePeople { get; set; }

        public IEnumerable<TrainingResourcePerson> trainingResourcePersons { get; set; }


        public IEnumerable<TrainingScheduleAll> trainingSchedules { get; set; }
        public IEnumerable<EnrolledTrainee> myTraining { get; set; }
        public IEnumerable<TraningAndPerticipantsVm> trainingWithPerticipants { get; set; }
        public IEnumerable<TraningAndPerticipantsVmUpdated> trainingWithPerticipantsUpdated { get; set; }
        public IEnumerable<YearlyTraining> yearlyTrainings { get; set; }
        public IEnumerable<InstituteTrainingReport> instituteTrainings { get; set; }
        public IEnumerable<TrainingofonlyBDViewModel> TrainingofonlyBD { get; set; }
        public IEnumerable<CourseWiseTraining> courseWiseTrainings { get; set; }
		public IEnumerable<TrainingHonorariumViewModel> trainingHonorariumViewModel { get; set; }
		public IEnumerable<IndTrainingReportSPViewModel> indTrainingReportSPs { get; set; }

        public IEnumerable<CourseCoordinator> CourseCoordinators { get; set; }



        public IEnumerable<CourseWiseParticipents> courseWiseParticipents { get; set; }
		public IEnumerable<TrainingPerticipantsSpVm> enrolledTraineees { get; set; }
		public EnrolledTrainee EnrolledTrainee { get; set; }
		public IEnumerable<Company> companies { get; set; }
        public IEnumerable<BangladeshBankManpowerReportVM> bangladeshBankManpowerReportVMs { get; set; }
        public IEnumerable<InstituteTrainingReportViewModel> instituteTrainingReportViewModels { get; set; }
		public IEnumerable<ScheduleResourcePersonVm> ScheduleResourcePersons { get; set; } 
	}
    public class YearlyTraining
    {
        public string course { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int? durationDays { get; set; }
        public int? totalPerticipants { get; set; }
    }
    public class CourseWiseTraining
    {
        public int? trainingId { get; set; }
        public string course { get; set; }
        public int? trainingType { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int? perticipants { get; set; }
        public int? durationDays { get; set; }
    }
    public class TrainingScheduleVm
    {
        public int? trainingId { get; set; }
        public int? trainerId { get; set; }
        public string trainers { get; set; }
        public DateTime? effectiveDate { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string topic { get; set; }
        public string isBreak { get; set; }
        public int? sortOrder { get; set; }
    }
	public class TrainingScheduleTrainersVm
	{
		public int? trainingId { get; set; }
		public string trainers { get; set; }
	}
	public class InstituteTrainingReport
    {
        public string participantName { get; set; }
        public string designation { get; set; }
        public string employeeCode { get; set; }
        public string countryName { get; set; }
        public string placeOfPosting { get; set; }
        public string trainingInstituteName { get; set; }
        public string course { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int? durationDays { get; set; }
        public int? totalPerticipants { get; set; }
    }
    public class TrainingofonlyBDViewModel
    {
        public string IdNo { get; set; }

        public string PerticipantName { get; set; }
        public string countryName { get; set; }
        public string designation { get; set; }
        public string Posting { get; set; }
        public int? TrainingId { get; set; }

        public int? trainingInstituteId { get; set; }
        public string trainingInstituteName { get; set; }
        public string trainingInstituteShortName { get; set; }
        public string course { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int? Duration { get; set; }
        public string Type { get; set; }
    }
    public class ScheduleResourcePersonVm 
	{
		public string name { get; set; }

		public string designation { get; set; }

		public string workPlace { get; set; }

		public string specialization { get; set; }
		public string type { get; set; } //In House/External

		public string contactNumber { get; set; }

		public string performance { get; set; }

		public string email { get; set; }

		public string address { get; set; }

		public string remarks { get; set; }

		public string url { get; set; }


		//Other For Future
		public string orgType { get; set; }
	}
	public class InstituteTrainingReportViewModel
    {
        public string participantName { get; set; }
        public string designation { get; set; }
        public string employeeCode { get; set; }
        public string placeOfPosting { get; set; }
        public int? trainingInstituteId { get; set; }
        public string trainingInstituteName { get; set; }
        public string course { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int? durationDays { get; set; }
        public int? totalPerticipants { get; set; }
    }

    public class TraningAndPerticipantsVm
    {
        public int? designationId { get; set; }
        public string designationName { get; set; }
        public int? employeeId { get; set; }
        public string employeeCode { get; set; }
        public string employeeName { get; set; }
        public string courseTitle { get; set; }
        public int? trainingId { get; set; }
        public int? trainerId { get; set; }
		public string trainerName { get; set; }
		public string trainerDesignation { get; set; }
		public int? perticipantsId { get; set; }
        public string Qualification { get; set; }
        public string placeOfPosting { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string subjectName { get; set; }
        public string duration { get; set; }
        public string mobileNumberOffice { get; set; }
        public string emailAddress { get; set; }
        public string Signature { get; set; }
		public string courseCoordinator { get; set; }
		public string courseCoordinatorDesignation { get; set; }
		public string courseTypeNameEN { get; set; }
		public string courseTypeNameBN { get; set; }
		public int? courseTypeId { get; set; }
		public int? isBreak { get; set; }
		public string topic { get; set; }
		public DateTime? effectiveDate { get; set; }
		public int? sortOrder { get; set; }

	}
    public class TraningAndPerticipantsVmUpdated
    {
        public int? designationId { get; set; }
        public string designationName { get; set; }
        public int? employeeId { get; set; }
        public string employeeCode { get; set; }
        public string employeeName { get; set; }
        public string courseTitle { get; set; }
        public int? trainingId { get; set; }
        public string trainerName { get; set; }
        public string trainerDesignation { get; set; }
        public int? perticipantsId { get; set; }
        public string Qualification { get; set; }
        public string placeOfPosting { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string subjectName { get; set; }
        public string duration { get; set; }
        public string mobileNumberOffice { get; set; }
        public string emailAddress { get; set; }
        public string Signature { get; set; }
        public string courseTypeNameEN { get; set; }
        public string courseTypeNameBN { get; set; }
        public int? courseTypeId { get; set; }
        public int? isBreak { get; set; }
        public string topic { get; set; }
        public DateTime? effectiveDate { get; set; }
        public int? sortOrder { get; set; }

    }





    public class TrainingScheduleViewModel
    {
        public int? trainingScheduleId { get; set; }
        public int? trainingId { get; set; }
        public int? trainerSId { get; set; }
        public DateTime? effectiveDate { get; set; }
        public string topic { get; set; }
        public string[] trainerId { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public int? isBreak { get; set; }
        public int? sortOrder { get; set; }
        public TrainingInfoNew trainingInfoNew { get; set; }

        public IEnumerable<TrainingInfoNew> trainingInfoNews { get; set; }
        public IEnumerable<TrainingScheduleAll> trainingSchedules { get; set; }
        public IEnumerable<CourseCoordinator> courseCoordinators { get; set; }
    }
    

    public class TrainingScheduleAll
    {
        public DateTime? effectiveDate { get; set; }
        public IEnumerable<TrainingScheduleVm> trainingScheduleVms { get; set; }
        public IEnumerable<TrainingInfoNew> trainingInfoNews { get; set; }
       
    }
    public class ManpowerSummary 
	{
		public string grade { get; set; }
		public int? totalEmployee { get; set; }
		public int? male { get; set; }
		public int? feMale { get; set; }
		public int? upTo34Years { get; set; }
		public int? above34Years { get; set; }
		public int? islam { get; set; }
		public int? Hinduism { get; set; }
		public int? buddhism { get; set; }
		public int? christian { get; set; }
		public int? Others { get; set; }
		public int? disabled { get; set; }
		public int? minorEthnicSect { get; set; }
		public int? sl { get; set; }
	 }
	public class ManpowerSummaryViewModel
	{
		public IEnumerable<ManpowerSummary> manpowerSummaryList { get; set; } 
	}

	public class CourseWiseParticipents
    {
        public int? trainingId { get; set; }
        public int? employeeId { get; set; }
        public string course { get; set; }
        public string name { get; set; }
        public string designation { get; set; }
        public string workingPlace { get; set; }
        public int? trainingType { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
       
        public int? durationDays { get; set; }
    }


    public class BangladeshBankManpowerReportVM
    {
        ////public int year { get; set; }
        public string trainingInstituteName { get; set; }
        public string course { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int? trainingType { get; set; }
        public int? Officer { get; set; }
        public int? Staff { get; set; }
        public int? Total { get; set; }
    }



    public class IndTrainingReportSPViewModel
    {
        public int Id { get; set; }
        public int? trainingCategoryId { get; set; }
        public int? trainingInstituteId { get; set; }
        public int? employeeId { get; set; }
        public string sponsoringAgency { get; set; }
        public string remarks { get; set; }
        public string year { get; set; }
        public string course { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string tType { get; set; }
    }


}
