using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GR.Core.Attributes.Documentation;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Core.Helpers.Global;
using GR.Core.Helpers.Pagination;
using GR.Crm.Contracts.Abstractions;
using GR.Crm.Contracts.Abstractions.Models;
using GR.Crm.Contracts.Abstractions.ViewModels;
using GR.Crm.Leads.Abstractions;
using GR.Identity.Data.Permissions;
using GR.Identity.Permissions.Abstractions.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GR.Crm.Contracts.Razor.Controllers
{
    /// <summary>
    /// Rest API for contracts
    /// </summary>
    [Authorize]
    [Author(Authors.LUPEI_NICOLAE, 1.1)]
    public sealed class ContractsController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// Inject service
        /// </summary>
        private readonly ICrmContractsService _service;

        /// <summary>
        /// Inject agreement service
        /// </summary>
        private readonly IAgreementService _agreementService;

        #endregion

        public IActionResult ContractTemplates()
        {
            return View();
        }



       
        public async Task<IActionResult> ContractTemplateSections(Guid id)
        {
            var template = await _service.FindContractTemplateByIdWithIncludesAsync(id);
            if (!template.IsSuccess) return NotFound();
            return View(template.Result);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        public ContractsController(ICrmContractsService service, IAgreementService agreementService)
        {
            _service = service;
            _agreementService = agreementService;
        }


        /// <summary>
        /// Add contract template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmDocumentTemplateCreate)]
        public async Task<JsonResult> AddContractTemplate([Required] AddTemplateViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_service.AddContractTemplateAsync(model));
        }


        /// <summary>
        /// Update contract template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmDocumentTemplateUpdate)]
        public async Task<JsonResult> UpdateContractTemplate([Required] UpdateTemplateViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_service.UpdateContractTemplateAsync(model));
        }

        /// <summary>
        /// Disable contract 
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmDocumentTemplateDelete)]
        public async Task<JsonResult> DisableContractTemplate([Required] Guid? contractId)
        {
            var agreementRequest = await _agreementService.GetAllAgreementsAsync(true);

            if(agreementRequest.IsSuccess && agreementRequest.Result.FirstOrDefault(x=> x.ContractTemplateId == contractId) != null)
                return Json(new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "There is agreement that contains this template" } } });

            return await JsonAsync(_service.DisableContractTemplateAsync(contractId));
        }
            

        /// <summary>
        /// Activate contract 
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmDocumentTemplateDelete)]
        public async Task<JsonResult> ActivateContractTemplate([Required] Guid? contractId) =>
            await JsonAsync(_service.ActivateContractTemplateAsync(contractId));

        /// <summary>
        /// Delete contract 
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmDocumentTemplateDelete)]
        public async Task<JsonResult> DeleteContractTemplate([Required] Guid? contractId) =>
            await JsonAsync(_service.DeleteContractTemplateAsync(contractId));


        /// <summary>
        /// Get contract sections
        /// </summary>
        /// <param name="contractTemplateId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetTemplateSectionsViewModel>>))]
       
        public async Task<JsonResult> GetContractTemplateSections([Required] Guid? contractTemplateId) =>
            await JsonAsync(_service.GetContractTemplateSectionsAsync(contractTemplateId));

        /// <summary>
        /// Find contract template by id
        /// </summary>
        /// <param name="contractTemplateId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<ContractTemplate>))]
      
        public async Task<JsonResult> FindContractTemplateById([Required]Guid? contractTemplateId)
            => await JsonAsync(_service.FindContractTemplateByIdWithIncludesAsync(contractTemplateId));


        /// <summary>
        /// Get all contract template
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<ContractTemplate>))]
        
        public async Task<JsonResult> GetAllContractTemplate(bool includeDeleted = false)
            => await JsonAsync(_service.GetAllContractTemplateAsync(includeDeleted));


        /// <summary>
        /// Get all paginated contract template
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(PagedResult<ContractTemplate>))]
       
        public async Task<JsonResult> GetAllPaginatedContractTemplate(PageRequest request)
            => await JsonAsync(_service.GetAllPaginatedContractTemplateAsync(request));

        /// <summary>
        /// Find section by id
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<ContractSection>))]
        
        public async Task<JsonResult> FindSectionById([Required]Guid? sectionId)
            => await JsonAsync(_service.FindSectionByIdAsync(sectionId));

        /// <summary>
        /// Add section to contract
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmDocumentTemplateCreate)]
        public async Task<JsonResult> AddSectionToContractTemplate([Required] AddSectionViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_service.AddSectionToContractTemplateAsync(model));
        }


        /// <summary>
        /// Delete contract 
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmDocumentTemplateDelete)]
        public async Task<JsonResult> DeleteContractSection([Required] Guid? sectionId) =>
            await JsonAsync(_service.DeleteContractSectionAsync(sectionId));


        /// <summary>
        /// Update contract section
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<ContractSection>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmDocumentTemplateUpdate)]
        public async Task<JsonResult> UpdateContractSection([Required] UpdateTemplateSectionViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_service.UpdateContractSectionAsync(model));
        }


        /// <summary>
        /// Order section 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmDocumentTemplateUpdate)]
        public async Task<JsonResult> OrderSection([Required] IEnumerable<OrderSectionViewModel> data) =>
            await JsonAsync(_service.OrderSectionAsync(data));
    }
}
