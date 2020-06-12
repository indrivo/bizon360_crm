using System;
using GR.TaskManager.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.TaskManager.Abstractions.Models;
using GR.TaskManager.Abstractions.Models.ViewModels.TaskTypeViewModels;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace GR.TaskManager.Services
{
    public class TaskTypeService : ITaskTypeService
    {

        #region Injected

        /// <summary>
        /// Inject context
        /// </summary>
        private readonly ITaskManagerContext _context;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        #endregion


        public TaskTypeService(ITaskManagerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// get all task types
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetTaskTypeViewModel>>> GetAllTaskTypeAsync(
            bool includeDeleted = false)
        {

            var taskTypes = await _context.TaskTypes
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();


            return new SuccessResultModel<IEnumerable<GetTaskTypeViewModel>>(_mapper.Map<IEnumerable<GetTaskTypeViewModel>>(taskTypes));

        }

        /// <summary>
        /// Get all paginated task type
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetTaskTypeViewModel>>> GetAllPaginatedTaskTypeAsync(
           PageRequest request)
        {
            var pageResult = await _context.TaskTypes
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .GetPagedAsync(request);

            var map = pageResult.Map(_mapper.Map<IEnumerable<GetTaskTypeViewModel>>(pageResult.Result));

            return new SuccessResultModel<PagedResult<GetTaskTypeViewModel>>(map);

        }

        /// <summary>
        /// get task type by id
        /// </summary>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetTaskTypeViewModel>> GetTaskTypeByIdAsync(Guid? taskTypeId)
        {

            if (taskTypeId == null)
                return new InvalidParametersResultModel<GetTaskTypeViewModel>();

            var taskType = await _context.TaskTypes
                .FirstOrDefaultAsync(x => x.Id == taskTypeId);


            if (taskType == null)
                return new NotFoundResultModel<GetTaskTypeViewModel>();

            return new SuccessResultModel<GetTaskTypeViewModel>(_mapper.Map<GetTaskTypeViewModel>(taskType));

        }

        /// <summary>
        /// add task type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddTaskTypeAsync(TaskTypeViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var taskType = _mapper.Map<TaskType>(model);
            await _context.TaskTypes.AddAsync(taskType);
            var result = await _context.PushAsync();

            return result.Map(taskType.Id);
        }

        /// <summary>
        /// update task type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateTaskTypeAsync(TaskTypeViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var taskType = await _context.TaskTypes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (taskType == null)
                return new NotFoundResultModel();

            taskType = _mapper.Map<TaskType>(model);
            _context.TaskTypes.Update(taskType);


            return await _context.PushAsync();
        }

        /// <summary>
        /// disable task type
        /// </summary>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableTaskType(Guid? taskTypeId) =>
            await _context.DisableRecordAsync<TaskType>(taskTypeId);

        /// <summary>
        /// activate task type
        /// </summary>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateTaskType(Guid? taskTypeId) =>
            await _context.ActivateRecordAsync<TaskType>(taskTypeId);

        /// <summary>
        /// Delete task type
        /// </summary>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteTaskType(Guid? taskTypeId) =>
            await _context.RemovePermanentRecordAsync<TaskType>(taskTypeId);

    }
}
