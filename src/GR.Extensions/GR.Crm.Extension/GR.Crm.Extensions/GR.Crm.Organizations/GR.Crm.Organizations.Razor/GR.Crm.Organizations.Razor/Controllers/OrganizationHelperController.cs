using System.Linq;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.ViewModels.HelpersViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GR.Crm.Organizations.Razor.Controllers
{
    [Authorize]
    public class OrganizationHelperController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// Industry service
        /// </summary>
        private readonly ICrmIndustryService _crmIndustryService;

        /// <summary>
        /// Employee service
        /// </summary>
        private readonly ICrmEmployeeService _crmEmployeeService;

        #endregion

        public OrganizationHelperController(ICrmIndustryService crmIndustryService, 
            ICrmEmployeeService crmEmployeeService)
        {
            _crmIndustryService = crmIndustryService;
            _crmEmployeeService = crmEmployeeService;
        }

        /// <summary>
        /// Get selectors Employees and Industries
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<ListSelectorsForOrganization>))]
        public virtual async Task<JsonResult> GetSelectorsForOrganization(bool includeDeleted = false)
        {
            var listSelectors = new ListSelectorsForOrganization();

            var listIndustriesRequest = await _crmIndustryService.GetAllIndustriesAsync(includeDeleted);
            var lisEmployeesRequest = await _crmEmployeeService.GetAllEmployeesAsync();
           

            if (listIndustriesRequest.IsSuccess)
            {
                listSelectors.ListIndustry = listIndustriesRequest.Result.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                });
            }

            if (lisEmployeesRequest.IsSuccess)
            {
                listSelectors.ListEmployees = lisEmployeesRequest.Result.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Interval
                });
            }

            var result = new ResultModel<ListSelectorsForOrganization>
            {
                IsSuccess = true,
                Result = listSelectors
            };

            return Json(result, SerializerSettings);

        }

    }
}
