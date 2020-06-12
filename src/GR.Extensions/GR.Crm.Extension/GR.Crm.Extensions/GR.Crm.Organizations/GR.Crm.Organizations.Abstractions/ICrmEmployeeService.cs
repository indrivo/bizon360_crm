using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Organizations.Abstractions.ViewModels.EmployeesViewModels;

namespace GR.Crm.Organizations.Abstractions
{
    public interface ICrmEmployeeService
    {

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetEmployeeViewModel>>> GetAllEmployeesAsync();

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetEmployeeViewModel>>> GetAllPaginatedEmployeesAsync(PageRequest request);

        /// <summary>
        /// Get Employee by id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Task<ResultModel<GetEmployeeViewModel>> GetEmployeeByIdAsync(Guid? employeeId);

        /// <summary>
        /// Disable Employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableEmployeeAsync(Guid? employeeId);

        /// <summary>
        /// Activate Employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateEmployeeAsync(Guid? employeeId);

        /// <summary>
        /// Delete Employee by Id
        /// </summary>
        /// <param name="employeId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteEmployeeAsync(Guid? employeId);

        /// <summary>
        /// Add new employee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
       Task<ResultModel<Guid>> AddNewEmployeeAsync(EmployeeViewModel model);

        /// <summary>
        /// Update employee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateEmployeeAsync(EmployeeViewModel model);
    }
}
