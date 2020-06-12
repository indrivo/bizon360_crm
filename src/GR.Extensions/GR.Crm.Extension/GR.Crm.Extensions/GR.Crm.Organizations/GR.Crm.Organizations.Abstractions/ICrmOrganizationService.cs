using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Organizations.Abstractions.Enums;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels;
using Microsoft.AspNetCore.Http;

namespace GR.Crm.Organizations.Abstractions
{
    public interface ICrmOrganizationService
    {

        /// <summary>
        /// Get all paginated  organization
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetOrganizationViewModel>>> GetPaginatedOrganizationAsync(PageRequest request);

        /// <summary>
        /// find organization by id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        Task<ResultModel<GetOrganizationViewModel>> FindOrganizationByIdAsync(Guid? organizationId);

        /// <summary>
        /// Find organization by fiscal code
        /// </summary>
        /// <param name="fiscalCode"></param>
        /// <returns></returns>
       Task<ResultModel<GetOrganizationViewModel>> GetOrganizationByFiscalCodeAsync(string fiscalCode);

        /// <summary>
        /// Get all active organizations
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetOrganizationViewModel>>> GetAllActiveOrganizationsAsync(bool includeDeleted);

        /// <summary>
        /// Delete organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteOrganizationAsync(Guid? organizationId);

        /// <summary>
        /// Deactivate organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        Task<ResultModel> DeactivateOrganizationAsync(Guid? organizationId);

        /// <summary>
        /// Activate organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateOrganizationAsync(Guid? organizationId);

        /// <summary>
        /// Add new organization
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddNewOrganizationAsync(OrganizationViewModel model);

        /// <summary>
        /// Update organization
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> UpdateOrganizationAsync(OrganizationViewModel model);


        /// <summary>
        /// Migration prospect to client
        /// </summary>
        /// <param name="prospectId"></param>
        /// /// <param name="type"></param>
        /// <returns></returns>
        Task<ResultModel> MigrationOrganizationToTypeAsync(Guid? prospectId, OrganizationType type);

       
        //Task<ResultModel> MigrationLeadToClientAsync(Guid? leadId);

        /// <summary>
        /// Get all active organizations by type 
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetOrganizationViewModel>>> GetAllActiveOrganizationsByTypeAsync(OrganizationType type, bool includeDeleted);


        /// <summary>
        /// Import organization 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        Task<ResultModel> ImportOrganizationAsync(IFormFile formFile);

    }
}
