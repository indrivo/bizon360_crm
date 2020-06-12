using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.ViewModels.EmployeesViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GR.Crm.Organizations.Razor.Controllers
{
    [Authorize]
    public class EmployeeController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// contact service
        /// </summary>
        private readonly ICrmEmployeeService _employeeService;

        #endregion

        public EmployeeController(ICrmEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Get all employees 
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetEmployeeViewModel>>))]
        public async Task<JsonResult> GetAllEmployees()
            => await JsonAsync(_employeeService.GetAllEmployeesAsync());


        /// <summary>
        /// Get all paginated employees 
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<GetEmployeeViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedEmployees(PageRequest request)
            => await JsonAsync(_employeeService.GetAllPaginatedEmployeesAsync(request));


        /// <summary>
        /// Get employee by id
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetEmployeeViewModel>))]
        public async Task<JsonResult> GetEmployeeById(Guid? employeeId)
            => await JsonAsync(_employeeService.GetEmployeeByIdAsync(employeeId));


        /// <summary>
        /// Disable employee by id
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableEmployeeById(Guid? industryId)
            => await JsonAsync(_employeeService.DisableEmployeeAsync(industryId));

        /// <summary>
        /// Activate employee by id
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateEmployeeById(Guid? industryId)
            => await JsonAsync(_employeeService.ActivateEmployeeAsync(industryId));

        /// <summary>
        /// Delete employee by id
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteEmployeeById(Guid? industryId)
            => await JsonAsync(_employeeService.DeleteEmployeeAsync(industryId));

        /// <summary>
        /// Add new  employee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddNewEmployee([Required] EmployeeViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_employeeService.AddNewEmployeeAsync(model));
        }

        /// <summary>
        /// Update employee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> UpdateEmployee([Required] EmployeeViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_employeeService.UpdateEmployeeAsync(model));
        }
    }
}
