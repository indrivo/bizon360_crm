using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Abstractions.ViewModels.JobPositionViewModels;
using GR.TaskManager.Abstractions;
using GR.TaskManager.Abstractions.Models.ViewModels.TaskTypeViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GR.TaskManager.Razor.Controllers
{
    public class TaskTypeController : BaseGearController
    {

        #region Inject

        /// <summary>
        /// Inject task type service
        /// </summary>
        private readonly ITaskTypeService _taskTypeService;



        #endregion

        public TaskTypeController(ITaskTypeService taskTypeService)
        {
            _taskTypeService = taskTypeService;
        }

        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Get all paginated task type
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<GetTaskTypeViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedTaskType(PageRequest request)
        {
            var result = await _taskTypeService.GetAllPaginatedTaskTypeAsync(request);
            return  Json(result);
        }

        /// <summary>
        /// Get all task type
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetTaskTypeViewModel>>))]
        public async Task<JsonResult> GetAllTaskType(bool includeDeleted = false)
            => await JsonAsync(_taskTypeService.GetAllTaskTypeAsync(includeDeleted));


        /// <summary>
        /// Get task type by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetTaskTypeViewModel>))]
        public async Task<JsonResult> GetTaskTypeById([Required] Guid id)
            => await JsonAsync(_taskTypeService.GetTaskTypeByIdAsync(id));



        /// <summary>
        /// Add new task type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddTaskTypeAsync([Required] TaskTypeViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_taskTypeService.AddTaskTypeAsync(model));
        }

        /// <summary>
        /// Update task type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateTaskType([Required] TaskTypeViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_taskTypeService.UpdateTaskTypeAsync(model));
        }


        /// <summary>
        /// Delete task type by id
        /// </summary>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteTaskType([Required] Guid id)
            => await JsonAsync(_taskTypeService.DeleteTaskType(id));

        /// <summary>
        /// Activate task type
        /// </summary>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateTaskType([Required] Guid id)
            => await JsonAsync(_taskTypeService.ActivateTaskType(id));

        /// <summary>
        /// Disable task type
        /// </summary>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableTaskType([Required] Guid id)
            => await JsonAsync(_taskTypeService.DisableTaskType(id));



    }
}
