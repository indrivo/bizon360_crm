using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.ViewModels.AgreementsViewModels;
using GR.Identity.Data.Permissions;
using GR.Identity.Permissions.Abstractions.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GR.Crm.Leads.Razor.Controllers
{
    [Authorize]
    public class AgreementController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// Inject lead service
        /// </summary>
        private readonly IAgreementService _agreementService;

        #endregion

        public IActionResult Index()
        {
            return View();
        }


        
        public async Task<IActionResult> Details(Guid id)
        {
            var agreement = await _agreementService.GetAgreementByIdAsync(id);
            if (!agreement.IsSuccess) return NotFound();
            return View(agreement.Result);
        }


        public AgreementController(IAgreementService agreementService)
        {
            _agreementService = agreementService;
        }


        /// <summary>
        /// Get all agreements
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<GetAgreementViewModel>>))]
       
        public async Task<JsonResult> GetAllAgreements(bool includeDeleted = false)
            => await JsonAsync(_agreementService.GetAllAgreementsAsync(includeDeleted), SerializerSettings);


        /// <summary>
        /// Get all paginated agreements for table
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<PagedResult<GetTableAgreementViewModel>>))]
        
        public async Task<JsonResult> GetAllTablePaginatedAgreements(PageRequest request)
            => await JsonAsync(_agreementService.GetAllTablePaginatedAgreementsAsync(request), SerializerSettings);


        /// <summary>
        /// Get all paginated agreements
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<PagedResult<GetAgreementViewModel>>))]
      
        public async Task<JsonResult> GetAllPaginatedAgreements(PageRequest request)
           => await JsonAsync(_agreementService.GetAllPaginatedAgreementsAsync(request), SerializerSettings);


        /// <summary>
        /// Get all agreements by lead id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<GetAgreementViewModel>>))]
       
        public async Task<JsonResult> GetAllAgreementsByLeadId([Required] Guid leadId)
            => await JsonAsync(_agreementService.GetAllAgreementsByLeadIdAsync(leadId), SerializerSettings);


        /// <summary>
        /// Get all agreements by organization id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<GetAgreementViewModel>>))]
       
        public async Task<JsonResult> GetAllAgreementsByOrganizationIdId([Required] Guid organizationId)
            => await JsonAsync(_agreementService.GetAllAgreementsByOrganizationIdIdAsync(organizationId), SerializerSettings);

        /// <summary>
        /// Get  agreements by id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<GetAgreementViewModel>))]
       
        public async Task<JsonResult> GetAgreementById([Required] Guid agreementId)
            => await JsonAsync(_agreementService.GetAgreementByIdAsync(agreementId), SerializerSettings);

        /// <summary>
        /// Add agreement
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmAgreementCreate)]
        public async Task<JsonResult> AddAgreement([Required] AddAgreementViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_agreementService.AddAgreementAsync(model));
        }


        /// <summary>
        /// Update agreement
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmAgreementUpdate)]
        public async Task<JsonResult> UpdateAgreement([Required] UpdateAgreementViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_agreementService.UpdateAgreementAsync(model));
        }


        /// <summary>
        /// Delete agreement
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmAgreementDelete)]
        public async Task<JsonResult> DeleteAgreement([Required] Guid agreementId)
            => await JsonAsync(_agreementService.DeleteAgreementAsync(agreementId));

        /// <summary>
        /// Disable agreement
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmAgreementDelete)]
        public async Task<JsonResult> DisableAgreement([Required] Guid agreementId)
            => await JsonAsync(_agreementService.DisableAgreementAsync(agreementId));

        /// <summary>
        /// Activate agreement
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmAgreementDelete)]
        public async Task<JsonResult> ActivateAgreement([Required] Guid agreementId)
            => await JsonAsync(_agreementService.ActivateAgreementAsync(agreementId));


        /// <summary>
        /// Generate contract for agreement by agreement id
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(FileResult))]
        public async Task<FileResult> GenerateFileContractForAgreement([Required] Guid agreementId)
        {
            var agreementRequest = await _agreementService.GetAgreementByIdAsync(agreementId);

            if (!agreementRequest.IsSuccess)
                return null;

            var agreementName = agreementRequest.Result.Name;

            var resultRequest = await _agreementService.GenerateContractForAgreementIdAsync(agreementId);
            return resultRequest.IsSuccess ? File(resultRequest.Result.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "" + agreementName + ".docx") : null;
        }

    }
}
