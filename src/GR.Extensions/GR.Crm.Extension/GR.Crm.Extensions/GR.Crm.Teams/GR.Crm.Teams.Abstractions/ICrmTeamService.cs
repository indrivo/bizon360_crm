using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Teams.Abstractions.ViewModels;

namespace GR.Crm.Teams.Abstractions
{
    public interface ICrmTeamService
    {
        /// <summary>
        /// get paginated team 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<TeamViewModel>>> GetPaginatedTeamAsync(PageRequest request);

        /// <summary>
        /// Get team by id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        Task<ResultModel<TeamViewModel>> GetTeamByIdAsync(Guid? teamId);

        /// <summary>
        /// Get all teams
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<TeamViewModel>>> GetAllTeamsAsync(bool includeDeleted);

        /// <summary>
        /// Get team by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<TeamViewModel>>> GetTeamsByUserIdAsync(Guid? userId, bool includeDeleted);

        /// <summary>
        /// Delete team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteTeamByIdAsync(Guid? teamId);

        /// <summary>
        /// Add team
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> AddTeamAsync(AddTeamViewModel model);

        /// <summary>
        /// update team
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateTeamAsync(AddTeamViewModel model);


        /// <summary>
        /// Get team rol by id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<ResultModel<TeamRoleViewModel>> GetTeamRoleByIdAsync(Guid? roleId);

        /// <summary>
        /// Get all team roles
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<TeamRoleViewModel>>> GetAllTeamRolesAsync(bool includeDeleted);

        /// <summary>
        /// Get user team role by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResultModel<TeamRoleViewModel>> GetTeamUserRoleAsync(Guid? userId);

        /// <summary>
        /// Delete team role
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
       Task<ResultModel> DeleteTeamRoleAsync(Guid? roleId);


        /// <summary>
        /// Disable team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableTeamAsync(Guid? teamId);

        /// <summary>
        /// Activate team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateTeamAsync(Guid? teamId);


        /// <summary>
        /// Add team role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> AddTeamRoleAsync(TeamRoleViewModel model);


        /// <summary>
        /// update team role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateTeamRoleAsync(TeamRoleViewModel model);

        /// <summary>
        /// Get team member by id
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        Task<ResultModel<GetTeamMemberViewModel>> GetTeamMemberByIdAsync(Guid? memberId);

        /// <summary>
        /// Get all members
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetTeamMemberViewModel>>> GetAllTeamMemberAsync();

        /// <summary>
        /// Get team members by team id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
       Task<ResultModel<IEnumerable<GetTeamMemberViewModel>>> GetTeamMembersByTeamIdAsync(Guid? teamId);


        /// <summary>
        /// add new member to team
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddNewMemberToTeamAsync(TeamMemberViewModel model);


        /// <summary>
        /// Update team member
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateTeamMemberAsync(TeamMemberViewModel model);


        /// <summary>
        /// delete team member
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteMemberToTeamAsync(Guid? memberId);


        /// <summary>
        /// Seed team role
        /// </summary>
        /// <returns></returns>
        Task SeedTeamRole();
    }
}
