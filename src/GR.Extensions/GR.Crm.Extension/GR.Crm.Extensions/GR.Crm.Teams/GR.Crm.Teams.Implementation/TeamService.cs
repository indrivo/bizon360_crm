using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Teams.Abstractions;
using GR.Crm.Teams.Abstractions.Helpers;
using GR.Crm.Teams.Abstractions.Models;
using GR.Crm.Teams.Abstractions.ViewModels;
using GR.Identity.Abstractions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Teams.Implementation
{
    public class TeamService : ICrmTeamService
    {

        #region Injectable
        /// <summary>
        /// Team context
        /// </summary>
        private readonly ICrmTeamContext _teamContext;

        /// <summary>
        /// Inject user manager
        /// </summary>
        private readonly IUserManager<GearUser> _userManager;

        /// <summary>
        /// Inject 
        /// </summary>
        private readonly IMapper _mapper;

        #endregion


        public TeamService(ICrmTeamContext teamContext,
            IMapper mapper, 
            IUserManager<GearUser> userManager)
        {
            _teamContext = teamContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// get paginated team 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<TeamViewModel>>> GetPaginatedTeamAsync(PageRequest request)
        {
            if(request == null)
                return new InvalidParametersResultModel<PagedResult<TeamViewModel>>();

            var query = await _teamContext.Teams
                .Include(i => i.TeamMembers)
                .ThenInclude(i => i.TeamRole)
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .GetPagedAsync(request);
                
            var mappedCollection = query.Result.Adapt<IEnumerable<TeamViewModel>>().ToList(); 


            foreach (var team in mappedCollection)
            {
                var membersRequest = await GetTeamMembersByTeamIdAsync(team.Id);
                if (membersRequest.IsSuccess)
                    team.TeamMembers = membersRequest.Result;
            }

            var result = new ResultModel<PagedResult<TeamViewModel>>
            {
                IsSuccess = true,
                Result = query.Map(mappedCollection)
            };

            return result;
        }

        /// <summary>
        /// Get team by id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<TeamViewModel>> GetTeamByIdAsync(Guid? teamId)
        {
            if (teamId == null)
                return new InvalidParametersResultModel<TeamViewModel>();

            var teamRequest = await _teamContext.Teams
                .Include(x => x.TeamMembers)
                .ThenInclude(x => x.TeamRole)
                .FirstOrDefaultAsync(x => x.Id == teamId);

            if (teamRequest == null)
                return new NotFoundResultModel<TeamViewModel>();

            var team = teamRequest.Adapt<TeamViewModel>();
            var memberRequest = await GetTeamMembersByTeamIdAsync(team.Id);
            if (memberRequest.IsSuccess)
                team.TeamMembers = memberRequest.Result;

            return new SuccessResultModel<TeamViewModel>(team);

        }

        /// <summary>
        /// Get all teams
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<TeamViewModel>>> GetAllTeamsAsync(bool includeDeleted = false)
        {
            var listTeamsRequest = await _teamContext.Teams
                .Include(i => i.TeamMembers)
                .ThenInclude(i => i.TeamRole)
                .Where(x=> !x.IsDeleted || includeDeleted)
                .ToListAsync();

            var listTeams = listTeamsRequest.Adapt<IEnumerable<TeamViewModel>>().ToList();

            foreach (var team in listTeams)
            {
                var membersRequest = await GetTeamMembersByTeamIdAsync(team.Id);
                if (membersRequest.IsSuccess)
                    team.TeamMembers = membersRequest.Result;
            }

            return new SuccessResultModel<IEnumerable<TeamViewModel>>(listTeams);
        }

        /// <summary>
        /// Get team by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<TeamViewModel>>> GetTeamsByUserIdAsync(Guid? userId, bool includeDeleted = false)
        {
            if (userId == null)
                return new InvalidParametersResultModel<IEnumerable<TeamViewModel>>();

            var user = await _userManager.UserManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return new NotFoundResultModel<IEnumerable<TeamViewModel>>();

            var listTeamsRequest = await _teamContext.TeamMembers
                .Include(i => i.TeamRole)
                .Include(i => i.Team)
                .Where(x => x.UserId == userId && (!x.IsDeleted || includeDeleted))
                .Select(s => s.Team)
                .ToListAsync();

            var listTeams = listTeamsRequest.Adapt<IEnumerable<TeamViewModel>>().ToList();
            foreach (var team in listTeams)
            {
                var membersRequest = await GetTeamMembersByTeamIdAsync(team.Id);
                if (membersRequest.IsSuccess)
                    team.TeamMembers = membersRequest.Result;
            }

            return new SuccessResultModel<IEnumerable<TeamViewModel>>(listTeams);
        }

        /// <summary>
        /// Delete team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteTeamByIdAsync(Guid? teamId)
        {
            if (teamId == null)
                return new InvalidParametersResultModel();

            var team = await _teamContext.Teams
                .FirstOrDefaultAsync(x => x.Id == teamId);

            if (team == null)
                return new NotFoundResultModel();

            _teamContext.Teams.Remove(team);
            return await _teamContext.PushAsync();
        }

        /// <summary>
        /// Add team
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> AddTeamAsync(AddTeamViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var team = new Team
            {
                Name = model.Name
            };

            await _teamContext.Teams.AddAsync(team);
            var result = await _teamContext.PushAsync();
            result.Result = team.Id;

            if (!result.IsSuccess || !model.ListMembers.Any()) return result;
            
            foreach (var member in model.ListMembers)
            {
                await AddNewMemberToTeamAsync(new TeamMemberViewModel
                    {TeamId = team.Id, TeamRoleId = member.RoleId, UserId = member.UserId});
            }

            return result;
        }

        /// <summary>
        /// update team
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateTeamAsync(AddTeamViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var team = await _teamContext.Teams
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (team == null)
                return new NotFoundResultModel();

            team.Name = model.Name;
            _teamContext.Teams.Update(team);

            var listMembers = await _teamContext.TeamMembers.Where(x => x.TeamId == model.Id).ToListAsync();
            _teamContext.TeamMembers.RemoveRange(listMembers);

            var result = await _teamContext.PushAsync();
            result.Result = team.Id;

            if (!result.IsSuccess || !model.ListMembers.Any()) return result;

            foreach (var member in model.ListMembers)
            {
                await AddNewMemberToTeamAsync(new TeamMemberViewModel
                    { TeamId = team.Id, TeamRoleId = member.RoleId, UserId = member.UserId });
            }
            return result;
        }

        /// <summary>
        /// Disable team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableTeamAsync(Guid? teamId) =>
            await _teamContext.DisableRecordAsync<Team>(teamId);

        /// <summary>
        /// Activate team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateTeamAsync(Guid? teamId) =>
            await _teamContext.ActivateRecordAsync<Team>(teamId);



        /// <summary>
        /// Get team rol by id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<TeamRoleViewModel>> GetTeamRoleByIdAsync(Guid? roleId)
        {
            if (roleId == null)
                return new InvalidParametersResultModel<TeamRoleViewModel>();

            var role = await _teamContext.TeamRoles
                .FirstOrDefaultAsync(x => x.Id == roleId);

            if (role == null)
                return new NotFoundResultModel<TeamRoleViewModel>();

            return new SuccessResultModel<TeamRoleViewModel>(role.Adapt<TeamRoleViewModel>());
        }

        /// <summary>
        /// Get all team roles
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<TeamRoleViewModel>>> GetAllTeamRolesAsync(bool includeDeleted = false)
        {
            var listTeamRoles = await _teamContext.TeamRoles
                .Where(x=> !x.IsDeleted || includeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<TeamRoleViewModel>>(listTeamRoles.Adapt<IEnumerable<TeamRoleViewModel>>());
        }

        /// <summary>
        /// Get user team role by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<TeamRoleViewModel>> GetTeamUserRoleAsync(Guid? userId)
        {
            if (userId == null)
                return new InvalidParametersResultModel<TeamRoleViewModel>();

            var user = await _userManager.UserManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return new NotFoundResultModel<TeamRoleViewModel>();

            var role = (await _teamContext.TeamMembers
                .Include(x => x.TeamRole)
                .FirstOrDefaultAsync(x => x.UserId == userId))?.TeamRole;

            if (role == null)
                return new NotFoundResultModel<TeamRoleViewModel>();

            return new SuccessResultModel<TeamRoleViewModel>(role.Adapt<TeamRoleViewModel>());
        }

        /// <summary>
        /// Delete team role
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteTeamRoleAsync(Guid? roleId)
        {
            if (roleId == null)
                return new InvalidParametersResultModel();

            var role = await _teamContext.TeamRoles
                .FirstOrDefaultAsync(x => x.Id == roleId);

            if (role == null)
                return new NotFoundResultModel();

            _teamContext.TeamRoles.Remove(role);
            return await _teamContext.PushAsync();
        }

        /// <summary>
        /// Add team role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> AddTeamRoleAsync(TeamRoleViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var newRole = new TeamRole
            {
                Name = model.Name
            };

            await _teamContext.TeamRoles.AddAsync(newRole);
            var result = await _teamContext.PushAsync();
            result.Result = newRole.Id;

            return result;
        }

        /// <summary>
        /// update team role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateTeamRoleAsync(TeamRoleViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();


            var role = await _teamContext.TeamRoles
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (role == null)
                return new NotFoundResultModel();

            role.Name = model.Name;
            _teamContext.TeamRoles.Update(role);

            return await _teamContext.PushAsync();
        }


        /// <summary>
        /// Get team member by id
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetTeamMemberViewModel>> GetTeamMemberByIdAsync(Guid? memberId)
        {
            if (memberId == null)
                return new InvalidParametersResultModel<GetTeamMemberViewModel>();

            var memberRequest = await _teamContext.TeamMembers
                .Include(i => i.TeamRole)
                .Include(i => i.Team)
                .FirstOrDefaultAsync(x => x.Id == memberId);

            if (memberRequest == null)
                return new NotFoundResultModel<GetTeamMemberViewModel>();

            var member = memberRequest.Adapt<GetTeamMemberViewModel>();

            member = await GetUserInfo(member);

            return new SuccessResultModel<GetTeamMemberViewModel>(member);

        }

        /// <summary>
        /// Get all members
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetTeamMemberViewModel>>> GetAllTeamMemberAsync()
        {
            var listMembersRequest  = await _teamContext.TeamMembers
                .Include(i => i.TeamRole)
                .Include(i => i.Team)
                .ToListAsync();

            var listMembers = listMembersRequest.Adapt<IEnumerable<GetTeamMemberViewModel>>();
            var listToReturn = new List<GetTeamMemberViewModel>();

            foreach (var member in listMembers)
            {
                listToReturn.Add(await GetUserInfo(member));
            }

            return new SuccessResultModel<IEnumerable<GetTeamMemberViewModel>>(listToReturn);
        }

        /// <summary>
        /// Get team members by team id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetTeamMemberViewModel>>> GetTeamMembersByTeamIdAsync(Guid? teamId)
        {
            if (teamId == null)
                return new InvalidParametersResultModel<IEnumerable<GetTeamMemberViewModel>>();

            var listMembersRequest = await _teamContext.TeamMembers
                .Include(i => i.TeamRole)
                .Include(i => i.Team)
                .Where(x => x.TeamId == teamId)
                .ToListAsync();

            var listMembers = listMembersRequest.Adapt<IEnumerable<GetTeamMemberViewModel>>();
            var listToReturn = new List<GetTeamMemberViewModel>();

            foreach (var member in listMembers)
            {
                listToReturn.Add(await GetUserInfo(member));
            }


            return new SuccessResultModel<IEnumerable<GetTeamMemberViewModel>>(listToReturn);
        }


        /// <summary>
        /// add new member to team
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddNewMemberToTeamAsync(TeamMemberViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var teamRequest = await GetTeamByIdAsync(model.TeamId);

            if (!teamRequest.IsSuccess)
                return new ResultModel<Guid> { IsSuccess = false, Errors = teamRequest.Errors };

            var team = teamRequest.Result;
            var userRequest = await _userManager.GetCurrentUserAsync();

            if (!userRequest.IsSuccess)
                return new ResultModel<Guid> { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "User not found" } } };

            var currentUser = userRequest.Result;
            var teamAuthor = await _userManager.UserManager.FindByNameAsync(team.Author);

            var existMember = await _teamContext.TeamMembers
                .FirstOrDefaultAsync(x => x.UserId == model.UserId && x.TeamId == model.TeamId);


            if (currentUser.Id != teamAuthor.Id && existMember.TeamRoleId != TeamResources.Owner)
                return new ResultModel<Guid>
                {
                    IsSuccess = false,
                    Errors = new List<IErrorModel> {new ErrorModel {Message = "User has no permissions to add member"}}
                };


            var existTeamMember = await _teamContext.TeamMembers.FirstOrDefaultAsync(x =>
                    x.TeamId == model.TeamId && x.UserId == model.UserId);

            if(existTeamMember != null)
                return new ResultModel<Guid>
                {
                    IsSuccess = false,
                    Errors = new List<IErrorModel> { new ErrorModel { Message = "In the team is this member" } }
                };

            var newMember = new TeamMember
            {
                UserId = model.UserId,
                TeamId = model.TeamId,
                TeamRoleId = model.TeamRoleId
            };

            await _teamContext.TeamMembers.AddAsync(newMember);

            var result = await _teamContext.PushAsync();
            return new ResultModel<Guid> {Result = newMember.Id, IsSuccess = result.IsSuccess, Errors = result.Errors};
        }


        /// <summary>
        /// Update team member
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateTeamMemberAsync(TeamMemberViewModel model)
        {

            var teamMember = await _teamContext.TeamMembers.FirstOrDefaultAsync(x => x.Id == model.Id);

            if(teamMember == null)
                return new NotFoundResultModel();

            teamMember.TeamRoleId = model.TeamRoleId;
            teamMember.TeamId = model.TeamId;
            teamMember.UserId = model.UserId;

            _teamContext.TeamMembers.Update(teamMember);
            return _teamContext.Push();

        }


        /// <summary>
        /// delete team member
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteMemberToTeamAsync(Guid? memberId)
        {
            if (memberId == null)
                return new InvalidParametersResultModel();

            var teamMember = await _teamContext.TeamMembers.FirstOrDefaultAsync(x => x.Id == memberId);
            _teamContext.TeamMembers.Remove(teamMember);

            return await _teamContext.PushAsync();
        }

       

        /// <summary>
        /// Seed team role 
        /// </summary>
        /// <returns></returns>
        public virtual async Task SeedTeamRole()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Configuration/TeamRole.json");
            var items = JsonParser.ReadArrayDataFromJsonFile<ICollection<TeamRole>>(path);

            if (items == null)
                return;

            if (items.Any())
            {
                await  _teamContext.TeamRoles.AddRangeAsync(items);
                await _teamContext.PushAsync();
            }
        }


        /// <summary>
        /// Get user data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<GetTeamMemberViewModel> GetUserInfo(GetTeamMemberViewModel model)
        {
            var user = await _userManager.UserManager.FindByIdAsync(model.UserId.ToString());

            if (user == null)
                return model;

            model.FirstName = user.UserFirstName;
            model.LastName = user.UserLastName;

            return model;
        }
    }
}
