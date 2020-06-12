using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GR.Core;
using GR.Core.Attributes.Documentation;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Core.Helpers.Global;
using GR.Core.Helpers.Pagination;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Leads.Abstractions.ViewModels;
using GR.Identity.Abstractions.Helpers.Attributes;
using GR.Identity.Data.Permissions;
using GR.Identity.Permissions.Abstractions.Attributes;
using GR.TaskManager.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GR.Crm.Leads.Razor.Controllers
{
    [Authorize]
    [Author(Authors.LUPEI_NICOLAE, 1.1)]
    public sealed class LeadsController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// Inject lead service
        /// </summary>
        private readonly ILeadService<Lead> _leadService;

        /// <summary>
        /// Inject task service
        /// </summary>
        private readonly ITaskManager _taskService;

        /// <summary>
        /// Inject agreement service;
        /// </summary>
        private readonly IAgreementService _agreementService;

        #endregion

       
        public async Task<IActionResult> Details(Guid id)
        {
            var leadRequest = await _leadService.FindLeadByIdAsync(id);
            if (!leadRequest.IsSuccess) return NotFound();
            return View(leadRequest.Result);
        }

        public IActionResult LeadStates()
        {
            return View();
        }

        public async Task<IActionResult> LeadStatusDetails(Guid id)
        {
            var leadRequest = await _leadService.GetLeadStateByIdAsync(id);
            if (!leadRequest.IsSuccess) return NotFound();
            return View(leadRequest.Result);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="leadService"></param>
        /// <param name="taskService"></param>
        /// <param name="agreementService"></param>
        public LeadsController(ILeadService<Lead> leadService, ITaskManager taskService, IAgreementService agreementService)
        {
            _leadService = leadService;
            _taskService = taskService;
            _agreementService = agreementService;
        }

        #region Leads


        /// <summary>
        /// Get  lead by id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<GetLeadsViewModel>))]
        public async Task<JsonResult> GetLeadById([Required] Guid LeadId) => await JsonAsync(_leadService.GetLeadByIdAsync(LeadId));


        /// <summary>
        /// Get all leads
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<Lead>>))]
       
        public async Task<JsonResult> GetAllLeads(bool includeDeleted = false) => await JsonAsync(_leadService.GetAllLeadsAsync(includeDeleted));

        /// <summary>
        /// Get leads by pipeLine
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<Lead>>))]
      
        public async Task<JsonResult> GetLeadsByPipeLineId([Required]Guid? pipeLineId, bool includeDeleted = false)
            => await JsonAsync(_leadService.GetLeadsByPipeLineIdAsync(pipeLineId, includeDeleted));

        /// <summary>
        /// Get leads by organization id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<Lead>>))]
       
        public async Task<JsonResult> GetLeadsByOrganizationId([Required]Guid? organizationId, bool includeDeleted = false)
            => await JsonAsync(_leadService.GetLeadsByOrganizationIdAsync(organizationId, includeDeleted));

        /// <summary>
        /// Get leads by pipeline
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(PagedResult<GetLeadsViewModel>))]
       
        public async Task<JsonResult> GetPaginatedLeadsByPipeLineId([Required]PageRequest request, [Required]Guid? pipeLineId)
            => await JsonAsync(_leadService.GetPaginatedLeadsByPipeLineIdAsync(request, pipeLineId));

        /// <summary>
        /// Get paginated request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(PagedResult<Lead>))]
       
        public async Task<JsonResult> GetPaginatedLeads([Required] PageRequest request)
            => await JsonAsync(_leadService.GetPaginatedLeadsAsync(request));

        /// <summary>
        /// Get leads by organization id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(PagedResult<Lead>))]
        
        public async Task<JsonResult> GetPaginatedLeadsByOrganizationId([Required] PageRequest request, [Required]Guid? organizationId)
            => await JsonAsync(_leadService.GetPaginatedLeadsByOrganizationIdAsync(request, organizationId));

        /// <summary>
        /// Add a lead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadCreate)]
        public async Task<JsonResult> AddLead([Required]AddLeadViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_leadService.AddLeadAsync(model));
        }

        /// <summary>
        /// Update lead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadUpdate)]
        public async Task<JsonResult> UpdateLead([Required]UpdateLeadViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_leadService.UpdateLeadAsync(model));
        }

        /// <summary>
        /// Activate lead
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadDelete)]
        public async Task<JsonResult> ActivateLead(Guid? leadId)
            => await JsonAsync(_leadService.ActivateLeadAsync(leadId));

        /// <summary>
        /// Disable lead
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadDelete)]
        public async Task<JsonResult> DisableLead(Guid? leadId)
        {
            var taskRequest = await _taskService.GetAllTaskByLeadIdAsync(leadId);
            var agreementRequest = await _agreementService.GetAllAgreementsByLeadIdAsync(leadId);

            if (taskRequest.IsSuccess && taskRequest.Result.Any())
                return Json(new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Lead has task" } } });

            if (agreementRequest.IsSuccess && agreementRequest.Result.Any())
                return Json(new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Lead has agreement" } } });

            return await JsonAsync(_leadService.DisableLeadAsync(leadId));
        }

        /// <summary>
        /// Disable lead
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadDelete)]
        public async Task<JsonResult> DeleteLead(Guid? leadId)
            => await JsonAsync(_leadService.DeleteLeadAsync(leadId));

        /// <summary>
        /// Move lead to another stage
        /// </summary>
        /// <param name="leadId"></param>
        /// <param name="stageId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadUpdate)]
        public async Task<JsonResult> MoveLeadToStage([Required] Guid? leadId, [Required] Guid? stageId)
            => await JsonAsync(_leadService.MoveLeadToStageAsync(leadId, stageId));



        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadUpdate)]
        public async Task<JsonResult> ImportAF(IFormFile formFile)
            => await JsonAsync(_leadService.ImportAFAsync(formFile));
        
        #endregion

        #region Lead states

        /// <summary>
        /// Add lead state
        /// </summary>
        /// <param name="name"></param>
        /// <param name="styleClass"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        [HttpPut]
        //[Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadStateCreate)]
        public async Task<JsonResult> AddLeadState([Required] [MinLength(2)] string name, string styleClass, string description)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_leadService.AddLeadStateAsync(name, styleClass, description));
        }

        /// <summary>
        /// Update states order
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        // [Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadStateUpdate)]
        public async Task<JsonResult> OrderLeadStates([Required] IEnumerable<OrderLeadStatesViewModel> data)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_leadService.OrderLeadStateAsync(data));
        }

        /// <summary>
        /// Get lead state by id
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<LeadState>))]
        public async Task<JsonResult> GetLeadStateById([Required] Guid stateId)
            => await JsonAsync(_leadService.GetLeadStateByIdAsync(stateId));


        /// <summary>
        /// Get all lead states
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> GetAllLeadStates(bool includeDeleted = false)
            => await JsonAsync(_leadService.GetAllLeadStatesAsync(includeDeleted));

        /// <summary>
        /// Activate lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <returns></returns>
        [HttpDelete]
        //[Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadStateDelete)]
        public async Task<JsonResult> ActivateLeadState(Guid? leadStateId)
            => await JsonAsync(_leadService.ActivateLeadStateAsync(leadStateId));

        /// <summary>
        /// Disable lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <returns></returns>
        [HttpDelete]
        // [Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadStateDelete)]
        public async Task<JsonResult> DisableLeadState(Guid? leadStateId)
        {
            var leadRequest = await _leadService.GetAllLeadsAsync(true);

            if (leadRequest.IsSuccess && leadRequest.Result.FirstOrDefault(x => x.LeadState?.Id == leadStateId) != null)
                return Json(new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Has lead in this state" } } });

            return await JsonAsync(_leadService.DisableLeadStateAsync(leadStateId));
        }

        /// <summary>
        /// Remove lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <returns></returns>
        [HttpDelete]
        // [Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadStateDelete)]
        public async Task<JsonResult> RemoveLeadState(Guid? leadStateId)
            => await JsonAsync(_leadService.RemoveLeadStateAsync(leadStateId));

        /// <summary>
        /// Rename lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="styleClass"></param>
        /// <returns></returns>
        [HttpPost]
        //[Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadStateUpdate)]
        public async Task<JsonResult> RenameLeadState([Required]Guid? leadStateId, [Required][MinLength(2)]string name, string description, string styleClass)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_leadService.UpdateLeadStateAsync(leadStateId, name, description, styleClass));
        }

        /// <summary>
        /// Change lead state
        /// </summary>
        /// <param name="leadId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadUpdate)]
        public async Task<JsonResult> ChangeLeadState([Required] Guid? leadId, [Required] Guid? stateId)
            => await JsonAsync(_leadService.ChangeLeadStateAsync(leadId, stateId));


       
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> SeedSystemLeadState()
        {
            await _leadService.SeedSystemLeadState();

            return Json("");
        }

        #endregion



        #region Team

        /// <summary>
        /// Set Lead owner
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="leadId"></param>
        /// <param name="listMembersId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> SetLeadMembers([Required]Guid ownerId, [Required]Guid leadId, IEnumerable<Guid> listMembersId)
            => await JsonAsync(_leadService.SetLeadOwnerAsync(ownerId, leadId, listMembersId));

        #endregion



        #region Contract


        //[HttpGet]
        //[Route(DefaultApiRouteTemplate)]
        //[Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<MemoryStream>))]
        //public async Task<JsonResult> GenerateContractForLead([Required] Guid leadId, [Required]Guid? templateId)
        //    => await JsonAsync(_leadService.GenerateContractForLeadAsync(leadId, templateId));



        //[HttpGet]
        //[Route(DefaultApiRouteTemplate)]
        //[Produces(ContentType.ApplicationJson, Type = typeof(FileResult))]
        //public async Task<FileResult> GenerateFileContractForLead([Required] Guid leadId, [Required] Guid? templateId)
        //{
        //    var resultRequest = await _leadService.GenerateContractForLeadAsync(leadId, templateId);
        //    return resultRequest.IsSuccess ? File(resultRequest.Result.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "ABC.docx") : null;
        //}

        #endregion

    }
}
