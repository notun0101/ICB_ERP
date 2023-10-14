using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using OPUSERP.Data.Entity;
using OPUSERP.Helpers.Errors;
using OPUSERP.Areas.Auth.Models;
using OPUSERP.Areas.API.Models;
using OPUSERP.ERPServices.AuthService.Interfaces;
using OPUSERP.HRPMS.Services.Leave.Interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System.Linq;
using OPUSERP.Data.Entity.Auth;

namespace OPUSERP.Areas.API.Controllers
{
    [Route("apibdbl/[controller]/[Action]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDbChangeService dbChangeService;
        private readonly IJwtFactoryService _jwtFactory;
        private readonly IPersonalInfoService personalInfoService;
        private readonly ILeaveRouteService leaveRouteService;

        public AuthController(UserManager<ApplicationUser> userManager, ILeaveRouteService leaveRouteService, IJwtFactoryService jwtFactory, IPersonalInfoService personalInfoService, IDbChangeService dbChangeService)
        {
            _userManager = userManager;
            this.leaveRouteService = leaveRouteService;
            _jwtFactory = jwtFactory;
            this.personalInfoService = personalInfoService;
            this.dbChangeService = dbChangeService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(_userManager);
        }

        private IActionResult Json(UserManager<ApplicationUser> userManager)
        {
            throw new NotImplementedException();
        }

        [HttpPost("loginCLUB")]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody]loginViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(model.ID);

            if (user != null && (await _userManager.CheckPasswordAsync(user, model.Password)))
            {
                var roles = await _userManager.GetRolesAsync(user);
                string id = await personalInfoService.GetEmployeeIDByAuthID(user.Id);
                var response = new
                {
                    id = id,
                    auth_token = await _jwtFactory.GenerateToken(user.UserName, id, roles)
                };

                var jwt = JsonConvert.SerializeObject(response);
                return new OkObjectResult(jwt);

            }

            return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
        }

     //   [Route("apibdbl/Auth/loginBDBL")]
        [HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> loginBDBL([FromBody]loginViewModel model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var user = await _userManager.FindByNameAsync(model.ID);

			if (user != null && (await _userManager.CheckPasswordAsync(user, model.Password)))
			{
				var roles = await _userManager.GetRolesAsync(user);
                //string id = await personalInfoService.GetEmployeeIDByAuthID(user.Id);
                var employee = await personalInfoService.GetEmployeeInfoByCode(user.EmpCode);
                var approvalLeaveCount = await leaveRouteService.GetLeaveRouteCountByEmpId(employee.Id);
                var posting = personalInfoService.GetPostingByEmpId(employee.Id);
                var profilePhoto = await personalInfoService.GetProfilePhotoByEmpId(employee.Id);
                var leaveApprover = await personalInfoService.CheckLeaveApprover(employee.Id);
                var natures = await personalInfoService.GetRoleNaturesByUserName(user.UserName);


                try
                {
                    UserLogHistory userLog = new UserLogHistory
                    {
                        userId = model.ID,
                        logTime = DateTime.Now,
                        status = 1,
                        deviceId = model.fcmToken,
                    };
                    await dbChangeService.SaveUserLogHistory(userLog);
                }
                catch (Exception)
                {

                }
                var response = new
				{
					id = user.Id,
					username = user.UserName,
                    empId = employee.Id,
                    empCode = user.EmpCode,
                    email = user.Email,
                    mobile = employee.mobileNumberOffice,
                    joiningDate = employee.joiningDateGovtService,
                    jobNature = employee.employeeType?.empType,
                    activityStatus = employee.activityStatus == 1 ? "Active" : "Inactive",
                    postingPlace = posting,
                    profilePhotoUrl = profilePhoto,
                    userid = user.userId,
                    pendingLeaveForApproval = approvalLeaveCount,
					status = user.isActive,
					sbuid = user.specialBranchUnitId,
					userRoles = roles.ToList(),
                    roleNatures = natures,
                    isLeaveApprover = leaveApprover > 0 ? 1 : 0,
                    auth_token = await _jwtFactory.GenerateToken(user.UserName, user.Id, roles)
				};

				var jwt = JsonConvert.SerializeObject(response);
				return new OkObjectResult(jwt);

			}

			return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
		}

		[HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePsswordViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var data = await _userManager.ChangePasswordAsync(await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value), model.OldPassword, model.Password);
            return new OkObjectResult(new {Message =  data.ToString()});
        }
    }
}