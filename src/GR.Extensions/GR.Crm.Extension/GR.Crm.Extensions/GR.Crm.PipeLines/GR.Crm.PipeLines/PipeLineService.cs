using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using GR.Core.Attributes.Documentation;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Global;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.PipeLines.Abstractions;
using GR.Crm.PipeLines.Abstractions.Models;
using GR.Crm.PipeLines.Abstractions.ViewModels;
using GR.Crm.PipeLines.Helpers;
using GR.Identity.Abstractions;
using GR.Notifications.Abstractions;
using GR.Notifications.Abstractions.Models.Notifications;
using GR.WorkFlows.Abstractions;
using GR.WorkFlows.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.PipeLines
{
    [Author(Authors.LUPEI_NICOLAE, 1.1)]
    public class PipeLineService : ICrmPipeLineService
    {
        #region Injectable

        /// <summary>
        /// Inject PipeLine context
        /// </summary>
        private readonly IPipeLineContext _context;

        /// <summary>
        /// Inject workflow 
        /// </summary>
        private readonly IWorkFlowCreatorService<WorkFlow> _workFlowCreatorService;

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
        /// <param name="workFlowCreatorService"></param>
        /// <param name="mapper"></param>
        /// <param name="notify"></param>
        public PipeLineService(IPipeLineContext context,
            IWorkFlowCreatorService<WorkFlow> workFlowCreatorService,
            IMapper mapper,
            INotify<GearRole> notify)
        {
            _context = context;
            _workFlowCreatorService = workFlowCreatorService;
            _mapper = mapper;
            _notify = notify;
        }



        /// <summary>
        /// Get PipeLine by id
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PipeLine>> GetPipeLineByIdAsync(Guid? pipeLineId)
        {

            if (pipeLineId == null)
                return new InvalidParametersResultModel<PipeLine>();

            var pipeLine = await _context.PipeLines
                .FirstOrDefaultAsync(x => x.Id == pipeLineId);

            if (pipeLine == null)
                return new NotFoundResultModel<PipeLine>();

            return new SuccessResultModel<PipeLine>(pipeLine);

        }


        /// <summary>
        /// Get all pipelines
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<PipeLine>>> GetAllPipeLinesAsync(bool includeDeleted = false)
        {
            var pipeLines = await _context.PipeLines
                .AsNoTracking()
                .Include(x => x.Stages)
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();
            return new SuccessResultModel<IEnumerable<PipeLine>>(pipeLines);
        }


        /// <summary>
        /// Get all paginated pipeLines
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<PipeLine>>> GetAllPaginatedPipeLinesAsync(PageRequest request)
        {
            var query = _context.PipeLines
                .AsNoTracking()
                .Include(x => x.Stages)
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .OrderByWithDirection(x => x.GetPropertyValue(request.Attribute), request.Descending);

            var pagedResult = await query.GetPagedAsync(request.Page, request.PageSize);
            var mappedCollection = _mapper.Map<IEnumerable<PipeLine>>(pagedResult.Result);

            var result = new ResultModel<PagedResult<PipeLine>>
            {
                IsSuccess = true,
                Result = pagedResult.Map(mappedCollection)
            };

            return result;
        }

        /// <summary>
        /// Add pipeline
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddPipeLineAsync(CreatePipeLineViewModel model)
        {
            if (model == null) return InvalidParametersResultModel<Guid>.Instance;
            WorkFlow workflow = null;
            var pipeLine = _mapper.Map<PipeLine>(model);

            if (pipeLine.WorkFlowId.HasValue)
            {
                var workFlowRequest = await _workFlowCreatorService.GetWorkFlowByIdAsync(pipeLine.WorkFlowId);
                if (!workFlowRequest.IsSuccess) return new InvalidParametersResultModel<Guid>(PipeLineMessageResult.WORKFLOW_NOT_FOUND);
                workflow = workFlowRequest.Result;
            }

            var pipeLineBd = await _context.PipeLines.
            FirstOrDefaultAsync(x => string.Equals(x.Name.Trim(), pipeLine.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (pipeLineBd != null)
                return new ResultModel<Guid> { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "PipeLine [" + pipeLineBd.Name + "] exist" } } };

            await _context.PipeLines.AddAsync(pipeLine);

            var dbResult = await _context.PushAsync();
            if (!dbResult.IsSuccess) return dbResult.Map<Guid>();

            await AddStageToPipeLineAsync(new AddStageViewModel
            {
                PipeLineId = pipeLine.Id,
                Name = "Start",
                Term = 1,
                IsSystem = true
            });
            await AddStageToPipeLineAsync(new AddStageViewModel
            {
                PipeLineId = pipeLine.Id,
                Name = "End",
                IsSystem = true
            });
            
            await _notify.SendNotificationAsync(new Notification
            {
                Content = $"Add PipeLine {pipeLine?.Name}",
                Subject = "Info",
                NotificationTypeId = NotificationType.Info
            });

            if (workflow != null)
            {
                await GeneratePipeLineStagesAsync(pipeLine.Id, workflow.States.ToList());
            }

            return new SuccessResultModel<Guid>(pipeLine.Id);
        }

        /// <summary>
        /// Remove pipeline permanently
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> RemovePipeLinePermanently(Guid? pipeLineId)
            => await _context.RemovePermanentRecordAsync<PipeLine>(pipeLineId);

        /// <summary>
        /// Update pipeline metadata
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel> UpdatePipeLineAsync([Required]UpdatePipeLineViewModel model)
        {
            if (model == null) return new InvalidParametersResultModel();
            var pipeLineRequest = await FindPipeLineByIdAsync(model.Id);
            if (!pipeLineRequest.IsSuccess) return pipeLineRequest.ToBase();
            var pipeLine = pipeLineRequest.Result;
            pipeLine.Name = model.Name;
            pipeLine.Description = model.Description;
            _context.PipeLines.Update(pipeLine);
            var result = await _context.PushAsync();

            if (result.IsSuccess)
            {
                await _notify.SendNotificationAsync(new Notification
                {
                    Content = $"Add PipeLine {pipeLine?.Name}",
                    Subject = "Info",
                    NotificationTypeId = NotificationType.Info
                });
            }

            return result;
        }

        /// <summary>
        /// Generate pipeline stages from workflow states
        /// </summary>
        /// <param name="pipeLine"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> GeneratePipeLineStagesAsync(Guid? pipeLine, ICollection<State> states)
        {
            if (pipeLine == null) return InvalidParametersResultModel.Instance.ToBase();
            var fails = 0;
            foreach (var state in states)
            {
                var addResult = await AddStageToPipeLineAsync(new AddStageViewModel
                {
                    PipeLineId = pipeLine.Value,
                    Name = state.Name,
                });
                if (!addResult.IsSuccess) fails++;
            }

            return fails > 0 ? new ResultModel() : new SuccessResultModel<object>().ToBase();
        }

        /// <summary>
        /// Add stage to pipeline
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddStageToPipeLineAsync(AddStageViewModel model)
        {
            if (model == null) return InvalidParametersResultModel<Guid>.Instance;


            var stageBd = await _context.Stages.FirstOrDefaultAsync(x =>
                x.PipeLineId == model.PipeLineId && string.Equals(x.Name.Trim(), model.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (stageBd != null)
                return new ResultModel<Guid> { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Stage [" + model.Name + "] exist" } } };


            var pipeLineRequest = await GetPipeLineByIdAsync(model.PipeLineId);

            if(!pipeLineRequest.IsSuccess)
                return new ResultModel<Guid> { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = " select pipeLine does not exist" } } };

            var stage = new Stage
            {
                Name = model.Name,
                Term = model.Term,
                PipeLineId = model.PipeLineId,
                Order = await GetPipeLineStagesCountAsync(model.PipeLineId),
                WorkFlowStateId = model.WorkFlowStateId,
                IsSystem = model.IsSystem

            };
            await _context.Stages.AddAsync(stage);
            var result = await _context.PushAsync();

            if (!result.IsSuccess) return result.Map(stage.Id);

            await SystemOrderStage(stage.PipeLineId);

            if (result.IsSuccess)
                await _notify.SendNotificationAsync(new Notification
                {
                    Content = $"Add stage {stage?.Name} in the {pipeLineRequest.Result?.Name}",
                    Subject = "Info",
                    NotificationTypeId = NotificationType.Info
                });

            return result.Map(stage.Id);
        }

        /// <summary>
        /// Disable pipeline by id
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisablePipeLineAsync(Guid? pipeLineId) =>
            await _context.DisableRecordAsync<PipeLine>(pipeLineId);

        /// <summary>
        /// Activate pipeline
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivatePipeLineAsync(Guid? pipeLineId) =>
            await _context.ActivateRecordAsync<PipeLine>(pipeLineId);

        /// <summary>
        /// Remove permanently stage
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> RemoveStagePermanentlyAsync(Guid? stageId) => await _context.RemovePermanentRecordAsync<Stage>(stageId);


        /// <summary>
        /// Find stage by id
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Stage>> FindStageByIdAsync(Guid? stageId)
        {
            if (stageId == null) return new NotFoundResultModel<Stage>();
            var stage = await _context.Stages.Include(i => i.PipeLine).AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(stageId));
            if (stage == null) return new NotFoundResultModel<Stage>();
            return new SuccessResultModel<Stage>(stage);
        }

        /// <summary>
        /// Update stage
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateStageAsync(UpdateStageViewModel model)
        {
            if (model == null) return new InvalidParametersResultModel();
            var stageRequest = await FindStageByIdAsync(model.Id);
            if (!stageRequest.IsSuccess) return stageRequest.ToBase();
            var stage = stageRequest.Result;
            stage.Name = model.Name;
            stage.Term = model.Term;
            
            _context.Stages.Update(stage);
            var result = await _context.PushAsync();

            if (result.IsSuccess)
                await _notify.SendNotificationAsync(new Notification
                {
                    Content = $"Update stage {stage?.Name} in the {stage?.PipeLine?.Name}",
                    Subject = "Info",
                    NotificationTypeId = NotificationType.Info
                });

            return result;
        }

        /// <summary>
        /// Order stages
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<ResultModel> OrderStagesAsync(IEnumerable<OrderStagesViewModel> data)
        {
            Guid? pipeLineId = null;

            foreach (var stageOrder in data)
            {
                var stage = await _context.Stages.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id.Equals(stageOrder.StageId));
                if (stage == null) continue;
                pipeLineId = stage.PipeLineId;
                stage.Order = stageOrder.Order;
                _context.Stages.Update(stage);
            }

            var result = await _context.PushAsync();

            if (result.IsSuccess)
                await SystemOrderStage(pipeLineId);

            return result;
        }

        /// <summary>
        /// Order stage by system Start/End
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        private async Task SystemOrderStage(Guid? pipeLineId)
        {

            if (pipeLineId == null)
            {
                new InvalidParametersResultModel();
                return;
            }

            var listStages = _context.Stages.Where(x => x.PipeLineId == pipeLineId);
            var leadStageCount = await listStages.CountAsync();

            var stageStar = listStages.FirstOrDefault(x =>
                string.Equals(x.Name, "Start", StringComparison.InvariantCultureIgnoreCase));

            var stageEnd = listStages.FirstOrDefault(x =>
                string.Equals(x.Name, "End", StringComparison.InvariantCultureIgnoreCase));

            if (stageStar != null)
            {
                listStages = listStages.Where(x => x.Id != stageStar.Id);
                stageStar.Order = 0;
                _context.Stages.Update(stageStar);
            }

            if (stageEnd != null)
            {
                listStages = listStages.Where(x => x.Id != stageEnd.Id);
                stageEnd.Order = leadStageCount;
                _context.Stages.Update(stageEnd);
            }


            if (!listStages.Any())
            {
                await _context.PushAsync();
                return;
            }

            var index = 1;

            foreach (var stage in listStages.OrderBy(o => o.Order))
            {
                stage.Order = index;
                index++;
                _context.Stages.Update(stage);
            }

            await _context.PushAsync();
        }



        /// <summary>
        /// Find pipeline by id
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PipeLine>> FindPipeLineByIdAsync(Guid? pipeLineId,
            Expression<Func<PipeLine, bool>> filter = null,
            params Expression<Func<PipeLine, object>>[] includes)
        {
            if (pipeLineId == null) return new NotFoundResultModel<PipeLine>();
            var query = _context.PipeLines;
            if (query == null) return new NotFoundResultModel<PipeLine>();
            foreach (var include in includes) query.Include(include);

            var pipeLine = await query
                .AsNoTracking()
                .WhereIf(filter != null, filter)
                .FirstOrDefaultAsync(x => x.Id.Equals(pipeLineId));
            if (pipeLine == null) return new NotFoundResultModel<PipeLine>();
            return new SuccessResultModel<PipeLine>(pipeLine);
        }

        /// <summary>
        /// Disable stage
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableStageAsync(Guid? stageId) => await _context.DisableRecordAsync<Stage>(stageId);



        /// <summary>
        /// Activate stage
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateStageAsync(Guid? stageId)
            => await _context.ActivateRecordAsync<Stage>(stageId);

        /// <summary>
        /// Get pipeline 
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<Stage>>> GetPipeLineStagesAsync(Guid? pipeLineId, bool includeDeleted = false)
        {
            if (pipeLineId == null) return new NotFoundResultModel<IEnumerable<Stage>>();
            var stages = await _context.Stages
                .Where(x => x.PipeLineId.Equals(pipeLineId) && (!x.IsDeleted || includeDeleted))
                .OrderBy(x => x.Order)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<Stage>>(stages);
        }

        #region Helpers

        /// <summary>
        /// Get pipeline stages count
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        protected virtual async Task<int> GetPipeLineStagesCountAsync(Guid? pipeLineId)
        {
            if (pipeLineId == null) return default;
            var count = await _context.Stages
                .AsNoTracking()
                .NonDeleted()
                .CountAsync(x => x.PipeLineId.Equals(pipeLineId));
            return count;
        }

        #endregion
    }
}
