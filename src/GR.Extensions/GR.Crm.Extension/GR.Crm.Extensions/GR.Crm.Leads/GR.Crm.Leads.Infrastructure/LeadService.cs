using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Leads.Abstractions.ViewModels;
using GR.Crm.Leads.Infrastructure.Extensions;
using GR.Crm.PipeLines.Abstractions;
using GR.Crm.Teams.Abstractions;
using GR.Crm.Teams.Abstractions.Helpers;
using GR.Crm.Teams.Abstractions.ViewModels;
using GR.Identity.Abstractions;
using GR.Notifications.Abstractions;
using GR.Notifications.Abstractions.Models.Notifications;
using GR.WorkFlows.Abstractions;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GR.Crm.Leads.Infrastructure
{
    public class CrmLeadService : ILeadService<Lead>
    {
        #region Injectable

        /// <summary>
        /// Inject context
        /// </summary>
        private readonly ILeadContext<Lead> _context;

        /// <summary>
        /// Inject workflow executor
        /// </summary>
        private readonly IWorkFlowExecutorService _workFlowExecutorService;

        /// <summary>
        /// Inject pipeLine service
        /// </summary>
        private readonly ICrmPipeLineService _crmPipeLineService;


        /// <summary>
        /// Inject team service
        /// </summary>
        private readonly ICrmTeamService _teamService;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Inject notification
        /// </summary>
        private readonly INotify<GearRole> _notify;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="workFlowExecutorService"></param>
        /// <param name="crmPipeLineService"></param>
        /// <param name="teamService"></param>
        /// <param name="mapper"></param>
        /// <param name="notify"></param>
        public CrmLeadService(ILeadContext<Lead> context,
            IWorkFlowExecutorService workFlowExecutorService,
            ICrmPipeLineService crmPipeLineService,
            ICrmTeamService teamService, IMapper mapper, INotify<GearRole> notify)
        {
            _context = context;
            _workFlowExecutorService = workFlowExecutorService;
            _crmPipeLineService = crmPipeLineService;
            _teamService = teamService;
            _mapper = mapper;
            _notify = notify;
        }



        #region  Lead

        /// <summary>
        /// Add new lead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddLeadAsync([Required] AddLeadViewModel model)
        {
            if (model == null) return new InvalidParametersResultModel<Guid>();
            var lead = _mapper.Map<Lead>(model);

            var leadBd = await _context.Leads.FirstOrDefaultAsync(x =>
                x.PipeLineId == model.PipeLineId && x.Name.Trim().ToLower().Equals(model.Name.Trim().ToLower()));

            if (leadBd != null)
                return new ResultModel<Guid>
                {
                    IsSuccess = false,
                    Errors = new List<IErrorModel> { new ErrorModel { Message = "Lead [" + leadBd.Name + "] exist" } }
                };

            await _context.Leads.AddAsync(lead);
            var addLeadResult = await _context.PushAsync();

            return addLeadResult.Map<Guid>(lead.Id);
        }

        /// <summary>
        /// Update lead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateLeadAsync([Required] UpdateLeadViewModel model)
        {
            if (model == null) return new InvalidParametersResultModel();

            var lead = await _context.Leads
                .Include(i => i.Team)
                .ThenInclude(i => i.TeamMembers)
                .FirstOrDefaultAsync(x => x.Id.Equals(model.Id));

            if (lead == null) return new NotFoundResultModel();

            var lastStageId = lead.StageId;
            var lastStateId = lead.LeadStateId;

            lead.Name = model.Name;
            lead.CurrencyCode = model.CurrencyCode;
            lead.Value = model.Value;
            if (model.OrganizationId != null) lead.OrganizationId = model.OrganizationId.Value;
            lead.DeadLine = model.DeadLine;
            if (model.LeadStateId != null) lead.LeadStateId = model.LeadStateId.Value;
            lead.StageId = model.StageId;
            lead.ContactId = model.ContactId;
            lead.SolutionTypeId = model.SolutionTypeId;
            lead.SourceId = model.SourceId;
            lead.TechnologyTypeId = model.TechnologyTypeId;
            lead.ProductTypeId = model.ProductTypeId;
            lead.ServiceTypeId = model.ServiceTypeId;
            lead.Description = model.Description;
            _context.Leads.Update(lead);


            var result = await _context.PushAsync();

            if (result.IsSuccess)
            {
                var listNotificationUserId = lead.Team.TeamMembers.Select(s => s.UserId);

                if (lastStageId != lead.StageId)
                {
                    var newStage = await _context.Stages.FindByIdAsync(lead.StageId);
                    var lastStage = await _context.Stages.FindByIdAsync(lastStageId);

                    var notification = new Notification
                    {
                        Content = $"Lead '{lead?.Name}' change stage '{lastStage?.Name}' to '{newStage?.Name}'",
                        Subject = "Info",
                        NotificationTypeId = NotificationType.Info
                    };
                    await _notify.SendNotificationAsync(listNotificationUserId, notification);
                    await _notify.SendNotificationToSystemAdminsAsync(notification);
                }

                if (lastStateId != lead.LeadStateId)
                {
                    var newState = await _context.States.FindByIdAsync(lead.LeadStateId);
                    var lastState = await _context.States.FindByIdAsync(lastStateId);


                    var notification = new Notification
                    {
                        Content = $"Lead '{lead?.Name}' change state '{lastState?.Name}' to '{newState?.Name}'",
                        Subject = "Info",
                        NotificationTypeId = NotificationType.Info
                    };
                    await _notify.SendNotificationAsync(listNotificationUserId, notification);
                    await _notify.SendNotificationToSystemAdminsAsync(notification);

                }
            }

            return result;
        }

        /// <summary>
        /// Get initial lead state
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<LeadState>> GetInitialLeadStateAsync()
        {
            var leadState = await _context.States
                .AsNoTracking()
                .OrderBy(x => x.Order)
                .FirstOrDefaultAsync();
            if (leadState == null) return new NotFoundResultModel<LeadState>();
            return new SuccessResultModel<LeadState>(leadState);
        }


        /// <summary>
        /// Get lead by Id
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetLeadsViewModel>> GetLeadByIdAsync(Guid? leadId)
        {
            if (leadId == null)
                return new InvalidParametersResultModel<GetLeadsViewModel>();

            var lead = await _context
                .BuildLeadsQuery()
                .FirstOrDefaultAsync(x => x.Id == leadId);

            if (lead == null)
                return new NotFoundResultModel<GetLeadsViewModel>();


            var leadToReturn = _mapper.Map<GetLeadsViewModel>(lead);


            var teamRequest = await _teamService.GetTeamMembersByTeamIdAsync(lead.TeamId);
            if (teamRequest.IsSuccess)
            {
                leadToReturn.LeadMembers = teamRequest.Result;
            }


            return new SuccessResultModel<GetLeadsViewModel>(leadToReturn);

        }

        /// <summary>
        /// Get paginated leads
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<PagedResult<GetLeadsViewModel>> GetPaginatedLeadsAsync(PageRequest request)
        {
            if (request == null)
            {
                var response = new PagedResult<GetLeadsViewModel>();
                response.Errors.Add(new ErrorModel(string.Empty, "Invalid parameters"));
                return response;
            }

            var query = _context
                .BuildLeadsQuery()
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .OrderByWithDirection(x => x.GetPropertyValue(request.Attribute), request.Descending);

            var pagedResult = await query.GetPagedAsync(request.Page, request.PageSize);
            var mappedCollection = _mapper.Map<IEnumerable<GetLeadsViewModel>>(pagedResult.Result).ToList();

            foreach (var lead in mappedCollection)
            {
                var teamRequest = await _teamService.GetTeamMembersByTeamIdAsync(lead.TeamId);
                if (teamRequest.IsSuccess)
                {
                    lead.LeadMembers = teamRequest.Result;
                }
            }

            return pagedResult.Map(mappedCollection);
        }

        /// <summary>
        /// Get all leads
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<Lead>>> GetAllLeadsAsync(bool includeDeleted = false)
        {
            var data = await _context
                .BuildLeadsQuery()
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<Lead>>(data);
        }

        /// <summary>
        /// Get leads by pipeLine
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<Lead>>> GetLeadsByPipeLineIdAsync(Guid? pipeLineId, bool includeDeleted = false)
        {
            if (pipeLineId == null) return new InvalidParametersResultModel<IEnumerable<Lead>>();
            var data = await _context
                .BuildLeadsQuery()
                .Where(x => x.PipeLineId.Equals(pipeLineId) && (!x.IsDeleted || includeDeleted))
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<Lead>>(data);
        }

        /// <summary>
        /// Get leads by organization id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<Lead>>> GetLeadsByOrganizationIdAsync(Guid? organizationId, bool includeDeleted = false)
        {
            if (organizationId == null) return new InvalidParametersResultModel<IEnumerable<Lead>>();
            var data = await _context
                .BuildLeadsQuery()
                .Where(x => x.OrganizationId.Equals(organizationId) && (!x.IsDeleted || includeDeleted))
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<Lead>>(data);
        }

        /// <summary>
        /// Get paginated result of leads by organization id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public virtual async Task<PagedResult<GetLeadsViewModel>> GetPaginatedLeadsByOrganizationIdAsync(PageRequest request, Guid? organizationId)
        {
            if (request == null)
            {
                var response = new PagedResult<GetLeadsViewModel>();
                response.Errors.Add(new ErrorModel(string.Empty, "Invalid parameters"));
                return response;
            }

            var query = _context
                .BuildLeadsQuery()
                .Where(x => x.OrganizationId.Equals(organizationId) && (!x.IsDeleted || request.IncludeDeleted));

            if (request.PageRequestFilters.Select(s => s.Propriety).Contains("Owner"))
            {
                var listOwnerId = request.PageRequestFilters
                    .Where(x => string.Equals(x.Propriety.Trim(), "Owner".Trim(), StringComparison.CurrentCultureIgnoreCase)).Select(s => s.Value.ToStringIgnoreNull()).ToList();

                var listLeadId = new List<Guid>();

                foreach (var ownerId in listOwnerId)
                {
                    var lead = await query.Where(x => x.Team.TeamMembers.FirstOrDefault(i => i.UserId.ToString() == ownerId) != null).ToListAsync();
                    listLeadId.AddRange(lead.Select(s => s.Id));
                }

                query = query.Where(x => listLeadId.Contains(x.Id));

                request.PageRequestFilters = request.PageRequestFilters.Where(s => s.Propriety != "Owner");
            }

            var pagedResult = await query.GetPagedAsync(request);

            var mappedCollection = _mapper.Map<IEnumerable<GetLeadsViewModel>>(pagedResult.Result).ToList();

            foreach (var lead in mappedCollection)
            {
                var teamRequest = await _teamService.GetTeamMembersByTeamIdAsync(lead.TeamId);
                if (teamRequest.IsSuccess)
                {
                    lead.LeadMembers = teamRequest.Result;
                }
            }

            //var pagedResult = await query.GetPagedAsync(request.Page, request.PageSize);
            //var mappedCollection = _mapper.Map<IEnumerable<GetLeadsViewModel>>(pagedResult.Result).ToList();


            //foreach (var lead in mappedCollection)
            //{
            //    var teamRequest = await _teamService.GetTeamMembersByTeamIdAsync(lead.TeamId);
            //    if (teamRequest.IsSuccess)
            //    {
            //        lead.LeadMembers = teamRequest.Result;
            //    }
            //}

            return pagedResult.Map(mappedCollection);
        }

        /// <summary>
        /// Get leads by pipeline with pagination
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetLeadsViewModel>>> GetPaginatedLeadsByPipeLineIdAsync(PageRequest request, Guid? pipeLineId)
        {
            if (request == null)
            {
               return new InvalidParametersResultModel<PagedResult<GetLeadsViewModel>>();
            }

            var query = _context
                .BuildLeadsQuery()
                .Where(x => x.PipeLineId.Equals(pipeLineId) && (!x.IsDeleted || request.IncludeDeleted))
                .Select(s => new GetLeadsViewModel
                {
                    Organization = s.Organization,
                    Name = s.Name,

                    Id = s.Id,
                    Value = s.Value,
                    LeadStateId = s.LeadStateId,
                    Created = s.Created,
                    OrganizationId = s.OrganizationId,
                    PipeLineId = s.PipeLineId,
                    Stage = s.Stage,
                    IsDeleted = s.IsDeleted,
                    LeadState = s.LeadState,
                    ProductId = s.ProductId,
                    Product = s.Product,
                    StartDate = s.StartDate,
                    Team = s.Team,
                    Author = s.Author,
                    Changed = s.Changed,
                    StageDeadLine = s.StageDeadLine,
                    CurrencyCode = s.CurrencyCode,
                    DeadLine = s.DeadLine,
                    PipeLine = s.PipeLine,
                    TeamId = s.TeamId,
                    OrganizationName = s.Organization.Name,
                    Currency = s.Currency,
                    ModifiedBy = s.ModifiedBy,
                    Version = s.Version,
                    StageId = s.StageId,
                    TenantId = s.TeamId
                });

            if (request.PageRequestFilters.Select(s => s.Propriety).Contains("Owner"))
            {
                var listOwnerId = request.PageRequestFilters
                    .Where(x => string.Equals(x.Propriety.Trim(), "Owner".Trim(), StringComparison.CurrentCultureIgnoreCase)).Select(s => s.Value.ToStringIgnoreNull()).ToList();

                var listLeadId = new List<Guid>();

                foreach (var ownerId in listOwnerId)
                {
                    var lead = await query.Where(x => x.Team.TeamMembers.Where(i => i.TeamRoleId == TeamResources.Owner).FirstOrDefault(i => i.UserId.ToString() == ownerId) != null).ToListAsync();
                    listLeadId.AddRange(lead.Select(s => s.Id));
                }

                query = query.Where(x => listLeadId.Contains(x.Id));

                request.PageRequestFilters = request.PageRequestFilters.Where(s => s.Propriety != "Owner");
            }

            var pagedResult = await query.GetPagedAsync(request);

            var mappedCollection = pagedResult.Result;

            foreach (var lead in mappedCollection)
            {
                var teamRequest = await _teamService.GetTeamMembersByTeamIdAsync(lead.TeamId);
                if (teamRequest.IsSuccess)
                {
                    lead.LeadMembers = teamRequest.Result;
                }
            }


            return new SuccessResultModel<PagedResult<GetLeadsViewModel>>(pagedResult.Map(mappedCollection));
        }

        /// <summary>
        /// Get leads by stage id
        /// </summary>
        /// <param name="stageId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetLeadsViewModel>>> GetLeadsByStageIdAsync(Guid? stageId, bool includeDeleted = false)
        {
            if (stageId == null)
                return new InvalidParametersResultModel<IEnumerable<GetLeadsViewModel>>();

            var leads = await _context.Leads
                .Where(x => x.StageId == stageId && (!x.IsDeleted || includeDeleted))
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetLeadsViewModel>>(leads.Adapt<IEnumerable<GetLeadsViewModel>>());
        }

        /// <summary>
        /// Find lead by id
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Lead>> FindLeadByIdAsync(Guid? leadId)
        {
            if (leadId == null) return new InvalidParametersResultModel<Lead>();
            var lead = await _context
                .BuildLeadsQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == leadId);
            if (lead == null) return new NotFoundResultModel<Lead>();
            return new SuccessResultModel<Lead>(lead);
        }

        /// <summary>
        /// Move lead to another stage
        /// </summary>
        /// <param name="leadId"></param>
        /// <param name="stageId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> MoveLeadToStageAsync(Guid? leadId, Guid? stageId)
        {
            if (stageId == null) return new InvalidParametersResultModel();
            var lead = await _context.Leads.Include(i => i.LeadState)
                .Include(i => i.Team)
                .ThenInclude(i => i.TeamMembers)
                .FirstOrDefaultAsync(x => x.Id == leadId);
            if (lead == null) return new NotFoundResultModel();
            if (lead.StageId == stageId) return new SuccessResultModel<object>().ToBase();

            var stageRequest = await _crmPipeLineService.FindStageByIdAsync(stageId);
            if (!stageRequest.IsSuccess) return stageRequest.ToBase();
            var stage = stageRequest.Result;

            if (stage.Term != null)
                lead.StageDeadLine = DateTime.Now.AddDays(stage.Term.Value);
            lead.StageId = stageId.Value;
            _context.Leads.Update(lead);
            var result = await _context.PushAsync();

            if (!result.IsSuccess) return result;

            var newStage = await _context.Stages.FindByIdAsync(stageId);

            var listNotificationUserId = lead.Team.TeamMembers.Select(s => s.UserId);
            var notification = new Notification
            {
                Content = $"Lead {lead?.Name} change stage {stage?.Name} to {newStage?.Name}",
                Subject = "Info",
                NotificationTypeId = NotificationType.Info
            };

            await _notify.SendNotificationAsync(listNotificationUserId, notification);
            await _notify.SendNotificationToSystemAdminsAsync(notification);


            return result;
        }

        /// <summary>
        /// Change lead state
        /// </summary>
        /// <param name="leadId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ChangeLeadStateAsync(Guid? leadId, Guid? stateId)
        {
            if (leadId == null || stateId == null) return new InvalidParametersResultModel();
            var lead = await _context.Leads
                .Include(i => i.LeadState)
                .Include(i => i.Team)
                .ThenInclude(i => i.TeamMembers)
                .FirstOrDefaultAsync(x => x.Id == leadId);
            if (lead == null) return new NotFoundResultModel();
            var initialStateName = lead.LeadState?.Name;
            if (lead.LeadStateId == stateId) return new SuccessResultModel<object>().ToBase();

            lead.LeadStateId = stateId.Value;
            _context.Leads.Update(lead);

            var result = await _context.PushAsync();

            if (!result.IsSuccess) return result;

            var newState = await _context.States.FindByIdAsync(stateId);

            var listNotificationUserId = lead.Team.TeamMembers.Select(s => s.UserId);
            var notification = new Notification
            {
                Content = $"Lead {lead?.Name} change state {initialStateName} to {newState?.Name}",
                Subject = "Info",
                NotificationTypeId = NotificationType.Info
            };

            await _notify.SendNotificationAsync(listNotificationUserId, notification);
            await _notify.SendNotificationToSystemAdminsAsync(notification);

            return result;
        }


        /// <summary>
        /// Disable lead
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        public async Task<ResultModel> DisableLeadAsync(Guid? leadId)
            => await _context.DisableRecordAsync<Lead>(leadId);

        /// <summary>
        /// Activate lead
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        public async Task<ResultModel> ActivateLeadAsync(Guid? leadId)
            => await _context.ActivateRecordAsync<Lead>(leadId);


        /// <summary>
        /// Delete lead
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        public async Task<ResultModel> DeleteLeadAsync(Guid? leadId)
            => await _context.RemovePermanentRecordAsync<Lead>(leadId);




        #endregion

        #region LeadState

        /// <summary>
        /// Add lead state
        /// </summary>
        /// <param name="name"></param>
        /// <param name="styleClass"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public async Task<ResultModel<Guid>> AddLeadStateAsync([Required] string name, string styleClass, string description)
        {
            if (name.IsNullOrEmpty()) return new InvalidParametersResultModel<Guid>("Name must be not null");
            var order = await _context.States.CountAsync();
            var leadState = new LeadState
            {
                Name = name,
                Order = order + 1,
                StateStyleClass = styleClass,
                Description = description
            };

            await _context.States.AddAsync(leadState);
            var dbResult = await _context.PushAsync();
            return dbResult.IsSuccess ? new SuccessResultModel<Guid>(leadState.Id)
                : dbResult.Map(Guid.Empty);
        }

        /// <summary>
        /// Order lead states
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<ResultModel> OrderLeadStateAsync(IEnumerable<OrderLeadStatesViewModel> data)
        {
            foreach (var stateOrder in data)
            {
                var state = await _context.States.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id.Equals(stateOrder.StateId));
                if (state == null) continue;
                state.Order = stateOrder.Order;
                _context.States.Update(state);
            }

            return await _context.PushAsync();
        }

        /// <summary>
        /// Get all lead states
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public async Task<ResultModel<IEnumerable<LeadState>>> GetAllLeadStatesAsync(bool includeDeleted = false)
        {
            var data = await _context.States
                .Where(x => !x.IsDeleted || includeDeleted)
                .OrderBy(x => x.Order)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<LeadState>>(data);
        }

        /// <summary>
        /// Get lead state by id
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public async Task<ResultModel<LeadState>> GetLeadStateByIdAsync(Guid? stateId)
        {
            if (stateId == null)
                return new InvalidParametersResultModel<LeadState>();

            var data = await _context.States
                .FirstOrDefaultAsync(x => x.Id.Equals(stateId));

            if (data == null)
                return new NotFoundResultModel<LeadState>();

            return new SuccessResultModel<LeadState>(data);
        }

        /// <summary>
        /// Rename lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="styleClass"></param>
        /// <returns></returns>
        public async Task<ResultModel> UpdateLeadStateAsync(Guid? leadStateId, string name, string description, string styleClass)
        {
            if (leadStateId == null || name.IsNullOrEmpty())
                return new InvalidParametersResultModel();

            var leadState = await _context.States.FindByIdAsync(leadStateId);
            if (leadState == null) return new NotFoundResultModel();

            if (leadState.IsSystem)
                return new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Is system state" } } };

            leadState.Name = name;
            leadState.Description = description;
            leadState.StateStyleClass = styleClass;
            _context.States.Update(leadState);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Disable lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <returns></returns>
        public async Task<ResultModel> DisableLeadStateAsync(Guid? leadStateId)
        {
            var leadState = await _context.States.FindByIdAsync(leadStateId);
            if (leadState == null) return new NotFoundResultModel();

            if (leadState.IsSystem)
                return new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Is system state" } } };

            leadState.IsDeleted = true;
            _context.States.Update(leadState);
            return await _context.PushAsync();
        }


        /// <summary>
        /// Activate lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <returns></returns>
        public async Task<ResultModel> ActivateLeadStateAsync(Guid? leadStateId)
            => await _context.DisableRecordAsync<LeadState>(leadStateId);

        /// <summary>
        /// Remove lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <returns></returns>
        public async Task<ResultModel> RemoveLeadStateAsync(Guid? leadStateId)
        {
            var leadState = await _context.States.FindByIdAsync(leadStateId);
            if (leadState == null) return new NotFoundResultModel();

            if (leadState.IsSystem)
                return new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Is system state" } } };

            _context.States.Remove(leadState);
            return await _context.PushAsync();
        }



        /// <summary>
        /// Seed lead state 
        /// </summary>
        /// <returns></returns>
        public virtual async Task SeedSystemLeadState()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Configuration/SystemLeadState.json");
            var items = JsonParser.ReadArrayDataFromJsonFile<ICollection<LeadState>>(path);

            if (items == null)
                return;

            if (items.Any())
            {
                await _context.States.AddRangeAsync(items);
                await _context.PushAsync();
            }
        }

        #endregion

        #region Teams

        /// <summary>
        /// Set team for lead
        /// </summary>
        /// <param name="lead"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> SetTeamForLeadAsync(Lead lead, Guid? teamId)
        {
            if (lead == null || teamId == null) return new InvalidParametersResultModel();
            lead.TeamId = teamId;
            _context.Leads.Update(lead);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Get leads count by organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<ResultModel<int>> GetLeadsCountByOrganizationAsync(Guid? organizationId)
        {
            if (organizationId == null) return new InvalidParametersResultModel<int>();
            var count = await _context.Leads
                .CountAsync(x => x.OrganizationId.Equals(organizationId));

            return new SuccessResultModel<int>(count);
        }

        /// <summary>
        /// Set owner for lead
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="leadId"></param>
        /// <param name="listMembersId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> SetLeadOwnerAsync(Guid? userId, Guid? leadId, IEnumerable<Guid> listMembersId)
        {
            if (userId == null || leadId == null) return new InvalidParametersResultModel<Guid>();
            var leadRequest = await FindLeadByIdAsync(leadId);

            if (!leadRequest.IsSuccess)
                return new ResultModel<Guid> { IsSuccess = leadRequest.IsSuccess, Errors = leadRequest.Errors };


            var listUsersIdAssignedToLead = listMembersId.ToList();
            listUsersIdAssignedToLead.Add(userId.Value);

            var lead = leadRequest.Result;
            if (lead.HasTeam())
            {
                var currentOwner = lead.Team.TeamMembers
                    .FirstOrDefault(x => x.TeamRoleId.Equals(TeamResources.Owner));

                if (currentOwner != null)
                {
                    await _teamService.DeleteMemberToTeamAsync(currentOwner.Id);
                }

                var members = lead.Team.TeamMembers.Where(x => x.TeamRoleId.Equals(TeamResources.Member)).ToList();

                if (members.Any())
                    foreach (var member in members)
                    {
                        await _teamService.DeleteMemberToTeamAsync(member.Id);
                    }

                var addResult = await _teamService.AddNewMemberToTeamAsync(new TeamMemberViewModel
                {
                    TeamRoleId = TeamResources.Owner,
                    TeamId = lead.TeamId.GetValueOrDefault(),
                    UserId = userId.Value
                });

                if (listMembersId == null || !listMembersId.Any()) return addResult;

                foreach (var memberId in listMembersId)
                {
                    await _teamService.AddNewMemberToTeamAsync(new TeamMemberViewModel
                    {
                        TeamRoleId = TeamResources.Member,
                        TeamId = lead.TeamId.GetValueOrDefault(),
                        UserId = memberId
                    });
                }



                if (addResult.IsSuccess)
                {
                    var notification = new Notification
                    {
                        Content = $"Update lead {lead?.Name} ",
                        Subject = "Info",
                        NotificationTypeId = NotificationType.Info
                    };

                    await _notify.SendNotificationAsync(listUsersIdAssignedToLead, notification);
                    await _notify.SendNotificationToSystemAdminsAsync(notification);
                }

                return addResult;
            }

            var addTeamRequest = await _teamService.AddTeamAsync(new AddTeamViewModel
            {
                Name = $"{lead.Name} Team"
            });

            if (!addTeamRequest.IsSuccess) return addTeamRequest.Map<Guid>();
            var teamId = (Guid)addTeamRequest.Result;
            var mapTeamToLead = await SetTeamForLeadAsync(lead, teamId);
            if (!mapTeamToLead.IsSuccess)
            {
                //remove team
                return mapTeamToLead.Map<Guid>();
            }

            var addMemberResult = await _teamService.AddNewMemberToTeamAsync(new TeamMemberViewModel
            {
                TeamRoleId = TeamResources.Owner,
                TeamId = teamId,
                UserId = userId.Value
            });


            if (listMembersId == null || !listMembersId.Any()) return !addMemberResult.IsSuccess ? addMemberResult.Map<Guid>()
                : addMemberResult.Map(addMemberResult.Result);

            foreach (var memberId in listMembersId)
            {
                await _teamService.AddNewMemberToTeamAsync(new TeamMemberViewModel
                {
                    TeamRoleId = TeamResources.Member,
                    TeamId = lead.TeamId.GetValueOrDefault(),
                    UserId = memberId
                });
            }


            if (addMemberResult.IsSuccess)
            {
                var notification = new Notification
                {
                    Content = $"Add lead {lead?.Name} ",
                    Subject = "Info",
                    NotificationTypeId = NotificationType.Info
                };

                await _notify.SendNotificationAsync(listUsersIdAssignedToLead, notification);
                await _notify.SendNotificationToSystemAdminsAsync(notification);
            }



            return !addMemberResult.IsSuccess ? addMemberResult.Map<Guid>()
                : addMemberResult.Map(addMemberResult.Result);
        }

        #endregion



        /// <summary>
        /// Import af from excel
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ImportAFAsync(IFormFile formFile)
        {


         
            //var listExcel = new List<Lead>();

            //using (var stream = new MemoryStream())
            //{
            //    await formFile.CopyToAsync(stream);

            //    IWorkbook workbook;

            //    if (formFile.FileName.IndexOf(".xlsx", StringComparison.Ordinal) > 0)
            //        workbook = WorkbookFactory.Create(stream);
            //    else if (formFile.FileName.IndexOf(".xls", StringComparison.Ordinal) > 0)
            //        workbook = new HSSFWorkbook(stream);
            //    else
            //        return new ResultModel
            //        {
            //            IsSuccess = false,
            //            Errors = new List<IErrorModel> { new ErrorModel { Message = "File is not in excel format" } }
            //        };

            //    var sheet = workbook.GetSheetAt(0);


            //    for (var item = sheet.FirstRowNum + 2; item <= sheet.LastRowNum; item++)
            //    {
            //        var row = sheet.GetRow(item);
            //        if (row == null)
            //            continue;

            //        try
            //        {


            //            listExcel.Add(new Lead
            //            {
            //                Id = Guid.Parse(row.GetCell(1).CellStringValue()),
            //                IsDeleted = row.GetCell(3).CellStringValue() != "False",
            //                Name = row.GetCell(5).CellStringValue(),
            //                Value = row.GetCell(7).CellDecimalValue().Value,
            //                LeadStateId = Guid.Parse(row.GetCell(9).CellStringValue()),
            //                PipeLineId = Guid.Parse(row.GetCell(11).CellStringValue()),
            //                StageId = Guid.Parse(row.GetCell(13).CellStringValue()),
            //                OrganizationId = Guid.Parse(row.GetCell(15).CellStringValue()),
            //                TeamId = Guid.Parse(row.GetCell(17).CellStringValue()),
            //                CurrencyCode = row.GetCell(19).CellStringValue(),
            //                ProductTypeId = row.GetCell(21).CellStringValue() != "NULL"
            //                    ? Guid.Parse(row.GetCell(21).CellStringValue())
            //                    : (Guid?)null,
            //                ServiceTypeId = row.GetCell(23).CellStringValue() != "NULL"
            //                    ? Guid.Parse(row.GetCell(23).CellStringValue())
            //                    : (Guid?)null,
            //                SolutionTypeId = row.GetCell(25).CellStringValue() != "NULL"
            //                    ? Guid.Parse(row.GetCell(25).CellStringValue())
            //                    : (Guid?)null,
            //                TechnologyTypeId = row.GetCell(27).CellStringValue() != "NULL"
            //                    ? Guid.Parse(row.GetCell(27).CellStringValue())
            //                    : (Guid?)null,
            //                SourceId = row.GetCell(29).CellStringValue() != "NULL"
            //                    ? Guid.Parse(row.GetCell(29).CellStringValue())
            //                    : (Guid?)null,
            //                ClarificationDeadline = row.GetCell(31).CellStringValue() != "NULL"
            //                    ? row.GetCell(31).CellStringValue().StringToDateTime().Value
            //                    : (DateTime?)null,
            //                CountryId = row.GetCell(33).CellStringValue() != "NULL"
            //                    ? Guid.Parse(row.GetCell(33).CellStringValue())
            //                    : (Guid?)null,
            //                DeadLine = row.GetCell(35).CellStringValue() != "NULL"
            //                    ? row.GetCell(35).CellStringValue().StringToDateTime().Value
            //                    : (DateTime?)null,
            //                StartDate = row.GetCell(36).CellStringValue() != "NULL"
            //                    ? row.GetCell(36).CellStringValue().StringToDateTime().Value
            //                    : DateTime.Now,
            //                Created = row.GetCell(36).CellStringValue() != "NULL"
            //                    ? row.GetCell(36).CellStringValue().StringToDateTime().Value
            //                    : DateTime.Now,
            //                Changed = row.GetCell(37).CellStringValue() != "NULL"
            //                    ? row.GetCell(37).CellStringValue().StringToDateTime().Value
            //                    : DateTime.Now,
            //            });
            //        }
            //        catch (Exception ex)
            //        {

            //        }
            //    }
            //}



            //// var l = listExcel.FirstOrDefault();

            //await _context.Leads.AddRangeAsync(listExcel);

            //var result = await _context.PushAsync();


            return new ResultModel();
        }







    }




}


