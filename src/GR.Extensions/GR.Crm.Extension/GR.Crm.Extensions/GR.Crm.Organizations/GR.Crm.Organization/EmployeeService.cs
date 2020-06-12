using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.EmployeesViewModels;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Organizations
{
    public class EmployeeService : ICrmEmployeeService
    {
        #region Injectable

        /// <summary>
        /// organization context
        /// </summary>
        private readonly ICrmOrganizationContext _context;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        #endregion

        public EmployeeService(ICrmOrganizationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetEmployeeViewModel>>> GetAllEmployeesAsync()
        {
            var listEmployees = await _context.Employees
                .ToListAsync();

            var map = _mapper.Map<IEnumerable<GetEmployeeViewModel>>(listEmployees);

            return new SuccessResultModel<IEnumerable<GetEmployeeViewModel>>(map);
        }




        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetEmployeeViewModel>>> GetAllPaginatedEmployeesAsync(PageRequest request)
        {
            var pagedResult = await _context.Employees
                .GetPagedAsync(request);


            var map = pagedResult.Map(_mapper.Map<IEnumerable<GetEmployeeViewModel>>(pagedResult.Result));

            return new SuccessResultModel<PagedResult<GetEmployeeViewModel>>(map);
        }


        /// <summary>
        /// Get Employee by id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetEmployeeViewModel>> GetEmployeeByIdAsync(Guid? employeeId)
        {
            if(employeeId == null)
                return new InvalidParametersResultModel<GetEmployeeViewModel>();

            var employee = await _context.Employees
                .FirstOrDefaultAsync(x => x.Id == employeeId);

            if(employee == null)
                return  new NotFoundResultModel<GetEmployeeViewModel>();



            return new SuccessResultModel<GetEmployeeViewModel>(_mapper.Map<GetEmployeeViewModel>(employee));
        }

        /// <summary>
        /// Disable Employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableEmployeeAsync(Guid? employeeId) =>
            await _context.DisableRecordAsync<Employee>(employeeId);

        /// <summary>
        /// Activate Employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateEmployeeAsync(Guid? employeeId) =>
            await _context.ActivateRecordAsync<Employee>(employeeId);

        /// <summary>
        /// Delete Employee by Id
        /// </summary>
        /// <param name="employeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteEmployeeAsync(Guid? employeId)
        {
            if(employeId == null)
                return new InvalidParametersResultModel();

            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == employeId);

            if(employee == null)
                return new NotFoundResultModel();

            _context.Employees.Remove(employee);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Add new employee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddNewEmployeeAsync(EmployeeViewModel model)
        {
            if(model == null)
                return new InvalidParametersResultModel<Guid>();

            var newEmployee = _mapper.Map<Employee>(model);

           await _context.Employees.AddAsync(newEmployee);
           var result = await _context.PushAsync();

           return result.Map(newEmployee.Id);
        }

        /// <summary>
        /// Update employee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateEmployeeAsync(EmployeeViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var employee = await _context.Employees
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if(employee == null)
                return  new NotFoundResultModel();

            employee.Interval = model.Interval;

            _context.Employees.Update(employee);
            return await _context.PushAsync();
        }
 

    }
}
