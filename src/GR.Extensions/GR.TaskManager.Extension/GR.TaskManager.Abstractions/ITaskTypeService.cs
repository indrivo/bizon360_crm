using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.TaskManager.Abstractions.Models;
using GR.TaskManager.Abstractions.Models.ViewModels.TaskTypeViewModels;

namespace GR.TaskManager.Abstractions
{
    public interface ITaskTypeService
    {

        /// <summary>
        /// get all task types
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetTaskTypeViewModel>>> GetAllTaskTypeAsync(bool includeDeleted);

        /// <summary>
        /// Get all paginated task type
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetTaskTypeViewModel>>> GetAllPaginatedTaskTypeAsync(PageRequest request);

        /// <summary>
        /// get task type by id
        /// </summary>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
        Task<ResultModel<GetTaskTypeViewModel>> GetTaskTypeByIdAsync(Guid? taskTypeId);

        /// <summary>
        /// add task type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddTaskTypeAsync(TaskTypeViewModel model);

        /// <summary>
        /// update task type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateTaskTypeAsync(TaskTypeViewModel model);
    

        /// <summary>
        /// disable task type
        /// </summary>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
       Task<ResultModel> DisableTaskType(Guid? taskTypeId) ;

        /// <summary>
        /// activate task type
        /// </summary>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateTaskType(Guid? taskTypeId);

        /// <summary>
        /// Delete task type
        /// </summary>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteTaskType(Guid? taskTypeId);

    }
}
