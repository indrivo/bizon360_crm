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
using GR.Crm.Organizations.Abstractions.ViewModels.IndustriesViewModels;
using GR.Identity.Data.Permissions;
using GR.Identity.Permissions.Abstractions.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GR.Crm.Organizations.Razor.Controllers
{
    [Authorize]
    public class IndustryController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// contact service
        /// </summary>
        private ICrmIndustryService _industryService;

        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public IndustryController(ICrmIndustryService industryService)
        {
            _industryService = industryService;
        }


        /// <summary>
        /// Get all paginated industries 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<GetIndustryViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedIndustries(PageRequest request)
            => await JsonAsync(_industryService.GetAllPaginatedIndustriesAsync(request));


        /// <summary>
        /// Get all industries 
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetIndustryViewModel>>))]
        public async Task<JsonResult> GetAllIndustries(bool includeDeleted = false)
            => await JsonAsync(_industryService.GetAllIndustriesAsync(includeDeleted));

        /// <summary>
        /// Get industry by id
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetIndustryViewModel>))]
        public async Task<JsonResult> GetIndustryById(Guid? industryId)
            => await JsonAsync(_industryService.GetIndustryByIdAsync(industryId));

        /// <summary>
        /// Disable industry by id
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmEntityDelete)]
        public async Task<JsonResult> DisableIndustryById(Guid? industryId)
            => await JsonAsync(_industryService.DisableIndustryAsync(industryId));

        /// <summary>
        /// Activate industry by id
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmEntityDelete)]
        public async Task<JsonResult> ActivateIndustryById(Guid? industryId)
            => await JsonAsync(_industryService.ActivateIndustryAsync(industryId));

        /// <summary>
        /// Activate industry by id
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmEntityDelete)]
        public async Task<JsonResult> DeleteIndustryById(Guid? industryId)
            => await JsonAsync(_industryService.DeleteIndustryAsync(industryId));

        /// <summary>
        /// Add new contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmEntityCreate)]
        public async Task<JsonResult> AddNewIndustry([Required] IndustryViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_industryService.AddNewIndustryAsync(model));
        }

        /// <summary>
        /// Update industry
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmEntityUpdate)]
        public async Task<JsonResult> UpdateIndustry([Required] IndustryViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_industryService.UpdateIndustryAsync(model));
        }
    }
}
