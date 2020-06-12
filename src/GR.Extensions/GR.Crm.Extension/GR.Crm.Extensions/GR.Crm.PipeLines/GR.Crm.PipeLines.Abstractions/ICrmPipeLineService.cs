using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.PipeLines.Abstractions.Models;
using GR.Crm.PipeLines.Abstractions.ViewModels;
using GR.WorkFlows.Abstractions.Models;

namespace GR.Crm.PipeLines.Abstractions
{
    public interface ICrmPipeLineService
    {
        /// <summary>
        /// Add pipeline
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddPipeLineAsync(CreatePipeLineViewModel model);

        /// <summary>
        /// Get PipeLine by id
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        Task<ResultModel<PipeLine>> GetPipeLineByIdAsync(Guid? pipeLineId);


        /// <summary>
        /// Get all paginated pipeLines
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<PipeLine>>> GetAllPaginatedPipeLinesAsync(PageRequest request);

        /// <summary>
        /// Generate pipeline stages from workflow states
        /// </summary>
        /// <param name="pipeLine"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        Task<ResultModel> GeneratePipeLineStagesAsync(Guid? pipeLine, ICollection<State> states);

        /// <summary>
        /// Add stage to pipeline
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddStageToPipeLineAsync(AddStageViewModel model);

        /// <summary>
        /// Disable pipeline
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        Task<ResultModel> DisablePipeLineAsync(Guid? pipeLineId);

        /// <summary>
        /// Activate pipeline
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivatePipeLineAsync(Guid? pipeLineId);

        /// <summary>
        /// Disable stage
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableStageAsync(Guid? stageId);

        /// <summary>
        /// Activate stage
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateStageAsync(Guid? stageId);

        /// <summary>
        /// Remove an state permanently
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        Task<ResultModel> RemoveStagePermanentlyAsync(Guid? stageId);

        /// <summary>
        /// Find stage by id
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        Task<ResultModel<Stage>> FindStageByIdAsync(Guid? stageId);

        /// <summary>
        /// Get Pipeline stages
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<Stage>>> GetPipeLineStagesAsync(Guid? pipeLineId, bool includeDeleted);

        /// <summary>
        /// Get all pipelines
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<PipeLine>>> GetAllPipeLinesAsync(bool includeDeleted = false);

        /// <summary>
        /// Find pipeline by id
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<ResultModel<PipeLine>> FindPipeLineByIdAsync(Guid? pipeLineId,
            Expression<Func<PipeLine, bool>> filter = null,
            params Expression<Func<PipeLine, object>>[] includes);

        /// <summary>
        /// Remove pipeline permanently
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        Task<ResultModel> RemovePipeLinePermanently(Guid? pipeLineId);

        /// <summary>
        /// Update pipeline
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdatePipeLineAsync([Required] UpdatePipeLineViewModel model);

        /// <summary>
        /// Update stage
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateStageAsync(UpdateStageViewModel model);

        /// <summary>
        /// Order stages
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<ResultModel> OrderStagesAsync(IEnumerable<OrderStagesViewModel> data);
    }
}