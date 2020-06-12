using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Teams.Abstractions;
using GR.Crm.Teams.Abstractions.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GR.Crm.Teams.Razor.Controllers
{
    [Authorize]
    public class TeamController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// Inject crm team
        /// </summary>
        private readonly ICrmTeamService _teamService;

        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public TeamController(ICrmTeamService teamService)
        {
            _teamService = teamService;
        }

        /// <summary>
        /// Get team by id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<TeamViewModel>))]
        public async Task<JsonResult> GetTeamById([Required] Guid teamId)
            => await JsonAsync(_teamService.GetTeamByIdAsync(teamId));

       /// <summary>
       /// Get paginated team
       /// </summary>
       /// <param name="request"></param>
       /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(Task<ResultModel<PagedResult<TeamViewModel>>>))]
        public async Task<JsonResult> GetPaginatedTeam(PageRequest request)
            => await JsonAsync(_teamService.GetPaginatedTeamAsync(request));


        /// <summary>
        /// Get all team 
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<TeamViewModel>>))]
        public async Task<JsonResult> GetAllTeams(bool includeDeleted = false)
            => await JsonAsync(_teamService.GetAllTeamsAsync(includeDeleted));

        /// <summary>
        /// Get team by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<TeamViewModel>>))]
        public async Task<JsonResult> GetTeamsByUserId([Required] Guid userId, bool includeDeleted = false)
            => await JsonAsync(_teamService.GetTeamsByUserIdAsync(userId, includeDeleted));

        /// <summary>
        /// Delete team by id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteTeamById([Required] Guid teamId)
            => await JsonAsync(_teamService.DeleteTeamByIdAsync(teamId));


        /// <summary>
        /// Disable team by id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableTeam([Required] Guid teamId)
            => await JsonAsync(_teamService.DisableTeamAsync(teamId));

        /// <summary>
        /// Activate team by id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateTeam([Required] Guid teamId)
            => await JsonAsync(_teamService.ActivateTeamAsync(teamId));


        /// <summary>
        /// Add new team 
        /// </summary>
        /// <param name="teamModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> AddTeam([Required] AddTeamViewModel teamModel)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_teamService.AddTeamAsync(teamModel));
        }

        /// <summary>
        /// Update new team 
        /// </summary>
        /// <param name="teamModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateTeam([Required] AddTeamViewModel teamModel)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_teamService.UpdateTeamAsync(teamModel));
        }



        /// <summary>
        /// Get team role by id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<TeamRoleViewModel>))]
        public async Task<JsonResult> GetTeamRoleById([Required] Guid roleId)
            => await JsonAsync(_teamService.GetTeamRoleByIdAsync(roleId));

        /// <summary>
        /// Get all team role 
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<TeamRoleViewModel>>))]
        public async Task<JsonResult> GetAllTeamRoles(bool includeDeleted = false)
            => await JsonAsync(_teamService.GetAllTeamRolesAsync(includeDeleted));


        /// <summary>
        /// Get user team role by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<TeamRoleViewModel>))]
        public async Task<JsonResult> GetTeamUserRole([Required] Guid userId)
            => await JsonAsync(_teamService.GetTeamUserRoleAsync(userId));

        /// <summary>
        /// Delete team role
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<TeamRoleViewModel>))]
        public async Task<JsonResult> DeleteTeamRole([Required] Guid roleId)
            => await JsonAsync(_teamService.DeleteTeamRoleAsync(roleId));

        /// <summary>
        /// Add team role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> AddTeamRole([Required] TeamRoleViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_teamService.AddTeamRoleAsync(model));
        }

        /// <summary>
        /// update team role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateTeamRole([Required] TeamRoleViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_teamService.UpdateTeamRoleAsync(model));
        }


        /// <summary>
        /// Get team member by id
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetTeamMemberViewModel>))]
        public async Task<JsonResult> GetTeamMemberById([Required] Guid memberId)
            => await JsonAsync(_teamService.GetTeamMemberByIdAsync(memberId));

        /// <summary>
        /// Get team member by id
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetTeamMemberViewModel>>))]
        public async Task<JsonResult> GetAllTeamMember()
            => await JsonAsync(_teamService.GetAllTeamMemberAsync());


        /// <summary>
        /// Get team members by team id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetTeamMemberViewModel>>))]
        public async Task<JsonResult> GetTeamMembersByTeamId([Required] Guid teamId)
            => await JsonAsync(_teamService.GetTeamMembersByTeamIdAsync(teamId));

        /// <summary>
        /// add new member to team
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> AddNewMemberToTeam([Required] TeamMemberViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_teamService.AddNewMemberToTeamAsync(model));
        }


        /// <summary>
        /// Delete team member
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteMemberToTeam([Required] Guid memberId) =>
            await JsonAsync(_teamService.DeleteMemberToTeamAsync(memberId));
    }
}
