using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GR.Core.BaseControllers;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Filters.Enums;
using GR.Core.Helpers.Pagination;
using GR.Crm.Abstractions;
using GR.Crm.Abstractions.Models;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.Enums;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GR.Crm.Razor.Controllers
{
    [Authorize]
    public class CrmCommonController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// Inject crm service
        /// </summary>
        private readonly ICrmService _crmService;

        /// <summary>
        /// Inject crm organization
        /// </summary>
        private readonly ICrmOrganizationService _organizationService;

        /// <summary>
        /// Inject crm lead service
        /// </summary>
        private readonly ILeadService<Lead> _leadService;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        #endregion

        public CrmCommonController(ICrmService crmService,
            ICrmOrganizationService organizationService,
            ILeadService<Lead> leadService,
            IMapper mapper)
        {
            _crmService = crmService;
            _organizationService = organizationService;
            _leadService = leadService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<Currency>>))]
        public async Task<JsonResult> GetAllCurrencies() => await JsonAsync(_crmService.GetAllCurrenciesAsync());


        /// <summary>
        /// Get organization by filter
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<GetTableOrganizationViewModel>>))]
        public async Task<JsonResult> GetAllOrganizationPaginated(PageRequest request)
        {

            //request.PageRequestFilters = new List<PageRequestFilter>
            //{
            //    new PageRequestFilter{Propriety = "WorkCategory", Value = "7a368249-8958-48e9-b27d-f4b0e8f35b0f", NextOperator = FilterNextOperator.And},
            //    new PageRequestFilter{Propriety = "ClientType", Value = OrganizationType.Prospect, NextOperator = FilterNextOperator.Or},
            //    new PageRequestFilter{Propriety = "WorkCategory", Value = "7a368249-8958-48e9-b27d-f4b0e8f35b0f", NextOperator = FilterNextOperator.Or},
            //    new PageRequestFilter{Propriety = "ClientType", Value = OrganizationType.Lead, NextOperator = FilterNextOperator.And},
            //};

            var organizationRequest = await _organizationService.GetPaginatedOrganizationAsync(request);
            var organization = organizationRequest.Result.Result.ToList();

            if (!organization.Any())
            { 
                return Json(new ResultModel<PagedResult<GetTableOrganizationViewModel>>
                {
                    IsSuccess = false,
                });
            }

            var listOrganization = organization
                .Select(async s => new GetTableOrganizationViewModel
                {
                    Contacts = s.Contacts,
                    Id = s.Id,
                    Created = s.Created,
                    Name = s.Name,
                    ClientType = s.ClientType,
                    IsDeleted = s.IsDeleted,
                    Email = s.Email,
                    LeadCount = (await _leadService.GetLeadsCountByOrganizationAsync(s.Id)).Result
                }).Select(s => s.Result).ToList()
                .OrderByWithDirection(x => x.GetPropertyValue(request.Attribute), request.Descending).ToList();
           

            var result = new ResultModel<PagedResult<GetTableOrganizationViewModel>>
            {
                IsSuccess = true,
                Result = new PagedResult<GetTableOrganizationViewModel>
                {
                    Result = listOrganization,
                    IsSuccess = organizationRequest.Result.IsSuccess,
                    CurrentPage = organizationRequest.Result.CurrentPage,
                    PageCount = organizationRequest.Result.PageCount,
                    PageSize = organizationRequest.Result.PageSize,
                    Errors = organizationRequest.Result.Errors,
                    RowCount = organizationRequest.Result.RowCount,
                    KeyEntity = organizationRequest.Result.KeyEntity
                },
            };

            return Json(result, SerializerSettings);
        }
    }
}