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
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.Enums;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels;
using GR.Identity.Data.Permissions;
using GR.Identity.Permissions.Abstractions.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GR.Crm.Organizations.Razor.Controllers
{
    [Authorize]
    public class OrganizationsController : BaseGearController
    {

        #region Injectable

        /// <summary>
        /// Inject crm organization
        /// </summary>
        private readonly ICrmOrganizationService _organizationService;

        /// <summary>
        /// Inject lead service
        /// </summary>
        private readonly ILeadService<Lead> _leaService;
        #endregion

        
        public OrganizationsController(ICrmOrganizationService organizationService, ILeadService<Lead> leaService)
        {
            _organizationService = organizationService;
            _leaService = leaService;
        }

        public IActionResult Index()
        {
            return View();
        }

       
        public async Task<IActionResult> Details(Guid id)
        {
            var organizationRequest = await _organizationService.FindOrganizationByIdAsync(id);
            if (!organizationRequest.IsSuccess) return NotFound();
            return View(organizationRequest.Result);
        }

        /// <summary>
        /// Get paginated organization 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<GetOrganizationViewModel>>))]
        
        public async Task<JsonResult> GetPaginatedOrganization(PageRequest request)
            => await JsonAsync(_organizationService.GetPaginatedOrganizationAsync(request), SerializerSettings);

        /// <summary>
        /// Get organization by id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetOrganizationViewModel>))]
       
        public async Task<JsonResult> GetOrganizationById([Required] Guid organizationId)
            => await JsonAsync(_organizationService.FindOrganizationByIdAsync(organizationId), SerializerSettings);


        /// <summary>
        /// Get all organizations 
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetOrganizationViewModel>>))]
        
        public async Task<JsonResult> GetAllOrganization(bool includeDeleted = false)
            => await JsonAsync(_organizationService.GetAllActiveOrganizationsAsync(includeDeleted), SerializerSettings);

        /// <summary>
        /// Get all organizations  clients
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<OrganizationViewModel>>))]
       
        public async Task<JsonResult> GetAllClientOrganization(bool includeDeleted = false)
            => await JsonAsync(_organizationService.GetAllActiveOrganizationsByTypeAsync(OrganizationType.Client, includeDeleted), SerializerSettings);

        /// <summary>
        /// Get all organizations prospect
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<OrganizationViewModel>>))]
       
        public async Task<JsonResult> GetAllProspectOrganization(bool includeDeleted = false)
            => await JsonAsync(_organizationService.GetAllActiveOrganizationsByTypeAsync(OrganizationType.Prospect, includeDeleted), SerializerSettings);

        /// <summary>
        /// Get all organizations lead
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<OrganizationViewModel>>))]
        
        public async Task<JsonResult> GetAllLeadOrganization(bool includeDeleted = false)
            => await JsonAsync(_organizationService.GetAllActiveOrganizationsByTypeAsync(OrganizationType.Lead, includeDeleted), SerializerSettings);


        /// <summary>
        /// migration organization prospect to client
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientUpdate)]
        public async Task<JsonResult> MigrationProspectOrganizationToClientById([Required] Guid organizationId)
            => await JsonAsync(_organizationService.MigrationOrganizationToTypeAsync(organizationId, OrganizationType.Client), SerializerSettings);

        /// <summary>
        /// migration organization prospect to Lead
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientUpdate)]
        public async Task<JsonResult> MigrationProspectToLeadById([Required] Guid organizationId)
            => await JsonAsync(_organizationService.MigrationOrganizationToTypeAsync(organizationId, OrganizationType.Lead), SerializerSettings);
       


        /// <summary>
        /// delete organization by id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientDelete)]
        public async Task<JsonResult> DeleteOrganizationPermanentlyById([Required] Guid organizationId)
            => await JsonAsync(_organizationService.DeleteOrganizationAsync(organizationId), SerializerSettings);

        /// <summary>
        /// Deactivate organization by id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientDelete)]
        public async Task<JsonResult> DeactivateOrganizationById([Required] Guid organizationId)
        {
            var leadRequest = await _leaService.GetLeadsByOrganizationIdAsync(organizationId, false);

            if(leadRequest.IsSuccess && leadRequest.Result.ToList().Any())
                return Json(new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Organization has active lead" } } });

            return await JsonAsync(_organizationService.DeactivateOrganizationAsync(organizationId), SerializerSettings);
        }

        /// <summary>
        /// Activate organization by id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientDelete)]
        public async Task<JsonResult> ActivateOrganizationById([Required] Guid organizationId)
            => await JsonAsync(_organizationService.ActivateOrganizationAsync(organizationId), SerializerSettings);

        /// <summary>
        /// Add new organization
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<OrganizationViewModel>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientCreate)]
        public async Task<JsonResult> AddNewOrganization([Required] OrganizationViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_organizationService.AddNewOrganizationAsync(model), SerializerSettings);
        }

        /// <summary>
        /// Update organization
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientUpdate)]
        public async Task<JsonResult> UpdateOrganization([Required] OrganizationViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_organizationService.UpdateOrganizationAsync(model), SerializerSettings);
        }


        /// <summary>
        /// Import organization
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientCreate)]
        public async Task<JsonResult> ImportOrganization([Required] IFormFile file)
        {
            return await JsonAsync(_organizationService.ImportOrganizationAsync(file), SerializerSettings);
        }
    }
}
