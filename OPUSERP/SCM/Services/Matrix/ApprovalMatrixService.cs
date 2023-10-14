using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.Auth.Models;
using OPUSERP.Areas.SCMMatrix.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity.Matrix;
using OPUSERP.SCM.Services.Matrix.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Services.Matrix
{
    public class ApprovalMatrixService : IApprovalMatrixService
    {
        private readonly ERPDbContext _context;

        public ApprovalMatrixService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveApprovalMatrix(ApprovalMatrix approvalMatrix)
        {
            try
            {
                IfExistDelete(approvalMatrix);
                if (approvalMatrix.Id != 0)
                {
                    approvalMatrix.updatedBy = approvalMatrix.createdBy;
                    approvalMatrix.updatedAt = DateTime.Now;
                    _context.ApprovalMatrices.Update(approvalMatrix);
                }
                else
                {
                    approvalMatrix.createdAt = DateTime.Now;
                    _context.ApprovalMatrices.Add(approvalMatrix);
                }

                await _context.SaveChangesAsync();
                return approvalMatrix.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<bool> UpdateApprovalMatrix(int projectId, int matrixTypeId, int newUserId, int oldUserId)
        {
            var data = await _context.ApprovalMatrices.Where(x => x.projectId == projectId && x.matrixTypeId == matrixTypeId && x.userId == oldUserId).ToListAsync();
            if (data.Count() > 0)
            {
                foreach (var d in data)
                {
                    var VoucherMasters = _context.ApprovalMatrices.Find(d.Id);
                    VoucherMasters.userId = newUserId;


                    _context.Entry(VoucherMasters).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

            }

            return true;
        }
		public async Task<int> DeleteApprovalMatrixByProjectRaiserMTypeId(int projectId, int raiser, int mTypeId)
		{
			var data = await _context.ApprovalMatrices.Where(x => x.projectId == projectId && x.userId == raiser && x.matrixTypeId == mTypeId).AsNoTracking().ToListAsync();
			_context.ApprovalMatrices.RemoveRange(data);
			return await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<ApproverPanelViewModel>> GetPRApproverPanelByUserId(int userId, int reqId)
        {
            try
            {
                var result = _context.panelViewModels.FromSql($"SP_GET_PRApproverPanelByUserId {userId},{reqId}").ToListAsync();

                return await result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<ApprovalMatrix>> GetApprovalMatrixList()
        {
            return await _context.ApprovalMatrices.Include(x => x.project)
                .Include(x => x.matrixType)
                .Include(x => x.approverType)
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<System.Object>> GetApprovalMatrixByRaiserId(int projectId, int matrixTypeId, int raiserId)
        {
            try
            {
                var result = (from A in _context.ApprovalMatrices
                              join MT in _context.MatrixTypes on A.matrixTypeId equals MT.Id
                              join AT in _context.ApproverTypes on A.approverTypeId equals AT.Id
                              join U in _context.Users on A.nextApproverId equals U.userId
                              join E in _context.employeeInfos on U.Id equals E.ApplicationUserId
                              where A.userId == raiserId && A.projectId == projectId && A.matrixTypeId == matrixTypeId
                              select new
                              {
                                  A.matrixTypeId,
                                  MT.matrixTypeName,
                                  A.approverTypeId,
                                  AT.approverTypeName,
                                  A.userId,
                                  A.nextApproverId,
                                  A.projectId,
                                  A.sequenceNo,
                                  A.isActive,
                                  E.nameEnglish
                              }).OrderBy(x => x.sequenceNo)
                       .AsNoTracking().ToListAsync();
                return await result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<ApprovalMatrixViewModel>> GetAllTypeApprovalMatrixByRaiserIdAndTypeId(int projectId, int matrixTypeId, int raiserId)
        {
            try
            {
                var result = await (from A in _context.ApprovalMatrices
                                    join MT in _context.MatrixTypes on A.matrixTypeId equals MT.Id
                                    where A.userId == raiserId && A.projectId == projectId && A.matrixTypeId == matrixTypeId && A.isActive == 1
                                    select new ApprovalMatrixViewModel
                                    {
                                        matrixTypeId = A.matrixTypeId,
                                        approverTypeId = A.approverTypeId,
                                        userId = A.userId,
                                        nextApproverId = A.nextApproverId,
                                        projectId = A.projectId,
                                        sequenceNo = A.sequenceNo,
                                        isActive = A.isActive
                                    }).OrderBy(x => x.sequenceNo)
                       .AsNoTracking().ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public async Task<IEnumerable<MatrixInformationVM>> GetAllMatrixInformation(string userName)
        {
            try
            {
                var result = await _context.matrixInformationVMs.FromSql($"sp_GetAllMatrixInformation {userName}").ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //public async Task<IEnumerable<ApprovalMatrix>> GetAllMatrix(int id)
        //{

        //    return await _context.ApprovalMatrices.Include(e=>e.project).Include(e=>e.matrixType).Include(e=>e.approverType).Where(e=>e.project.specialBranchUnitId == id).ToListAsync();

        //}

        public async Task<IEnumerable<ApprovalMatrixsVM>> GetApprovalMatrixs()
        {
            try
            {
                var result = await _context.approvalMatrixsVMs.FromSql($"sp_GetApprovalMatrixs").ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IEnumerable<ProjectWithUserVm>> GetAllProjectsByMatrixType(string mTypeName)
        {
            try
            {
                var result = await _context.projectWithUserVms.FromSql($"sp_ProjectWithRaiser {mTypeName}").ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<ProjectWithUserVm>> GetAllProjectsByMatrixTypeByRaisar(string mTypeName)
        {
            try
            {
                var result = await _context.projectWithUserVms.FromSql($"sp_ProjectWithRaiser {mTypeName}").ToListAsync();

                var list = new List<ProjectWithUserVm>();
                foreach (var item in result)
                {
                    var raiser = new ProjectWithUserVm
                    {
                        projectId = item.projectId,
                        projectName = item.projectName,
                        matrixTypeName = item.matrixTypeName,
                        Raiser = item.Raiser,
                        Photo = item.Photo,
                        userid = item.userid,

                        approvalList = (from A in _context.ApprovalMatrices
                                        join MT in _context.MatrixTypes on A.matrixTypeId equals MT.Id
                                        join AT in _context.ApproverTypes on A.approverTypeId equals AT.Id
                                        join U in _context.Users on A.nextApproverId equals U.userId
                                        join E in _context.employeeInfos on U.Id equals E.ApplicationUserId
                                        join p in _context.photographs on E.Id equals p.employeeId into pp
                                        from ph in pp.DefaultIfEmpty()
                                        where A.userId == item.userid && A.projectId == item.projectId && MT.matrixTypeName == mTypeName && A.isActive == 1
                                        select new ApprovalMatrixViewModel
                                        {
                                            matrixTypeId = A.matrixTypeId,
                                            approverTypeId = A.approverTypeId,
                                            userId = A.userId,
                                            approvalName = E.nameEnglish,
                                            nextApproverId = A.nextApproverId,
                                            projectId = A.projectId,
                                            sequenceNo = A.sequenceNo,
                                            isActive = A.isActive,
                                            photo = ph.url
                                        }).OrderBy(x => x.sequenceNo)
                       .AsNoTracking().ToList()
                    };

                    list.Add(raiser);

                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IEnumerable<ApprovarsByProject>> GetApprovarsByUserIdAndProjectId(int userid, int projectid, string matrixTypeName)
        {
            try
            {
                var result = await _context.approvarsByProjects.FromSql($"sp_ApprovarsByUserAndProject {userid}, {projectid}, {matrixTypeName}").ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

		public async Task<IEnumerable<StatusLog>> GetStatusLogByReqId(int reqId)
        {
            try
            {
				var result = await _context.StatusLogs.Include(x => x.cSMaster).Include(x => x.matrixType).Include(x => x.requisition).Include(x => x.requisition.employeeinfo).Include(x => x.requisition.employeeinfo.ApplicationUser).Include(x => x.statusInfo).Where(x => x.requisitionId == reqId).AsNoTracking().ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public EmployeeInfoViewModel GetEmployeeInfobyUser(string userName)
        {
            return _context.employeeInfoViewModels.FromSql($"sp_GetAspnetuserInfoByuser {userName}").AsNoTracking().FirstOrDefault();
        }

        public async Task<IEnumerable<System.Object>> GetPRApproverPanel(string userName)
        {

            var result = _context.panelViewModels.FromSql($"SP_GET_PRApproverPanel {userName}").ToListAsync();
            return await result;
        }

        public async Task<IEnumerable<ApproverPanelViewModel>> GetPRApproverPanelList(string userName, int matrixTypeId, int projectId)
        {
            try
            {
                var result = _context.panelViewModels.FromSql($"SP_GET_PRApproverPanel {userName},{matrixTypeId},{projectId}").ToListAsync();

                return await result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
		public async Task<IEnumerable<ApproverPanelViewModel>> GetPRApproverPanelList1(int csId, int matrixTypeId, int projectId)
        {
            try
            {
				var userid = _context.CSMasters.Where(x => x.Id == csId).Select(x => x.userId).FirstOrDefault();
				var username = _context.Users.Where(x => x.userId == userid).Select(x => x.UserName).FirstOrDefault();
                var result = _context.panelViewModels.FromSql($"SP_GET_PRApproverPanel {username},{matrixTypeId},{projectId}").ToListAsync();

                return await result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<ApproverPanelViewModel>> GetPRApproverPanelListFromApprovalLogs(string userName, int matrixTypeId, int reqMasterId)
        {
            try
            {
                //var result = _context.panelViewModels.FromSql($"SP_GET_PRApproverPanel_FromLog {userName},{matrixTypeId},{reqMasterId}").ToListAsync();
                var result = _context.panelViewModels.FromSql($"SP_GET_PRApproverPanel_ByMasterId {userName},{matrixTypeId},{reqMasterId}").ToListAsync();
                return await result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<ApproverPanelFViewModel>> GetPRApproverPanelFListFromApprovalLogs(string userName, int matrixTypeId, int reqMasterId)
        {
            try
            {
                var result = await _context.panelViewFModels.FromSql($"SP_GET_PRApproverPanel_FromLogF {userName},{matrixTypeId},{reqMasterId}").ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<ApprovalMatrixVMR>> GetApprovalMatrixVMR(int reqMasterId)
        {
            try
            {
                var result = _context.approvalMatrixVMRs.FromSql($"spgetaproverbyreq {reqMasterId}").ToListAsync();
                return await result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApprovalMatrix> GetApprovalMatrixById(int id)
        {
            return await _context.ApprovalMatrices.FindAsync(id);
        }
        public async Task<IEnumerable<ApprovalMatrix>> GetApprovalMatrixByUserId(int userid)
        {
            return await _context.ApprovalMatrices.Where(x => x.userId == userid).ToListAsync();
        }

        internal void IfExistDelete(ApprovalMatrix matrix)
        {
			var data = _context.ApprovalMatrices.Where(x => x.projectId == matrix.projectId && x.matrixTypeId == matrix.matrixTypeId && x.userId == matrix.userId && x.nextApproverId == matrix.nextApproverId).ToList();

			_context.ApprovalMatrices.RemoveRange(data);
        }

        public async Task<bool> DeleteApprovalMatrixById(int id)
        {
            _context.ApprovalMatrices.Remove(_context.ApprovalMatrices.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteApprovalMatrixByprojectuserId(int projectId, int userId)
        {
            _context.ApprovalMatrices.RemoveRange(_context.ApprovalMatrices.Where(x => x.projectId == projectId && x.userId == userId).ToList());
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<ApprovalMatrixForEditViewModel> GetApprovalMatrixByProjectAndRaiserId(int? projectId, int? raiserId,int? matrixTypeId)
        {
            var raiser = await _context.ApprovalMatrices.Include(x => x.approverType).Include(x => x.matrixType).Include(x => x.project).Where(x => x.projectId == projectId).Where(x => x.userId == raiserId).Where(x => x.approverTypeId != 4 && x.matrixTypeId==matrixTypeId).FirstOrDefaultAsync();
            var fApprovar = await _context.ApprovalMatrices.Include(x => x.approverType).Include(x => x.matrixType).Include(x => x.project).Where(x => x.projectId == projectId).Where(x => x.userId == raiserId).Where(x => x.approverTypeId == 4 && x.matrixTypeId==matrixTypeId).FirstOrDefaultAsync();
            
            RaiserViewModel raiserData = new RaiserViewModel();
            if(raiser != null)
            {
                raiserData.id = raiser.userId;
                raiserData.Name = await _context.employeeInfos.Where(x => x.ApplicationUserId == (_context.Users.Where(y => y.userId == raiser.userId).Select(y => y.Id).FirstOrDefault())).Select(x => x.nameEnglish).FirstOrDefaultAsync();
                raiserData.Photo = await _context.photographs.Where(x => x.employeeId == _context.employeeInfos.Where(y => y.ApplicationUserId == _context.Users.Where(z => z.userId == raiser.userId).Select(z => z.Id).FirstOrDefault()).Select(y => y.Id).FirstOrDefault()).Select(x => x.url).FirstOrDefaultAsync();
            };

			FinalApprovarViewModel finalApprovarData = new FinalApprovarViewModel();
			if (fApprovar != null)
			{

				finalApprovarData.id = fApprovar.nextApproverId;
				finalApprovarData.Name = await _context.employeeInfos.Where(x => x.ApplicationUserId == (_context.Users.Where(y => y.userId == fApprovar.nextApproverId).Select(y => y.Id).FirstOrDefault())).Select(x => x.nameEnglish).FirstOrDefaultAsync();
				finalApprovarData.Photo = await _context.photographs.Where(x => x.employeeId == _context.employeeInfos.Where(y => y.ApplicationUserId == _context.Users.Where(z => z.userId == fApprovar.nextApproverId).Select(z => z.Id).FirstOrDefault()).Select(y => y.Id).FirstOrDefault()).Select(x => x.url).FirstOrDefaultAsync();
			};

            List<ApprovarViewModel> approvars = new List<ApprovarViewModel>();
            foreach (var item in _context.ApprovalMatrices.Where(x => x.projectId == projectId).Where(x => x.userId == raiserId).Where(x => x.approverTypeId != 4 && x.matrixTypeId== matrixTypeId && x.nextApproverId>0).ToList())
            {
                var approvar = new ApprovarViewModel
                {
                    id = item.nextApproverId,
                    Name = await _context.employeeInfos.Where(x => x.ApplicationUserId == (_context.Users.Where(y => y.userId == item.nextApproverId).Select(y => y.Id).FirstOrDefault())).Select(x => x.nameEnglish).FirstOrDefaultAsync(),
                    Photo = await _context.photographs.Where(x => x.employeeId == _context.employeeInfos.Where(y => y.ApplicationUserId == _context.Users.Where(z => z.userId == item.nextApproverId).Select(z => z.Id).FirstOrDefault()).Select(y => y.Id).FirstOrDefault()).Select(x => x.url).FirstOrDefaultAsync(),
                    sequence = item.sequenceNo
                };
                approvars.Add(approvar);
            }
            var data = new ApprovalMatrixForEditViewModel
            {
                projectId = raiser?.projectId,
                projectName = raiser?.project.projectName,
                typeId = raiser?.matrixTypeId,
                typeName = raiser?.matrixType.matrixTypeName,
                Raiser = raiserData,
                Approvars = approvars,
                FinalApprovar = finalApprovarData
            };
            return data;
        }
    }
}
