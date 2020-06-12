using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Contracts.Abstractions.Models;
using GR.Crm.Contracts.Abstractions.ViewModels;

namespace GR.Crm.Contracts.Abstractions
{
    public interface ICrmContractsService
    {
        /// <summary>
        /// Add new contract template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddContractTemplateAsync([Required] AddTemplateViewModel model);


        /// <summary>
        /// Update new contract template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
       Task<ResultModel> UpdateContractTemplateAsync([Required] UpdateTemplateViewModel model);

        /// <summary>
        /// Delete contract template
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteContractTemplateAsync(Guid? contractId);

        /// <summary>
        /// Disable contract template
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableContractTemplateAsync(Guid? contractId);


        /// <summary>
        /// Activate contract template
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateContractTemplateAsync(Guid? contractId);

        /// <summary>
        /// Get contract sections
        /// </summary>
        /// <param name="contractTemplateId"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetTemplateSectionsViewModel>>> GetContractTemplateSectionsAsync(Guid? contractTemplateId);

        /// <summary>
        /// Find template by id with includes
        /// </summary>
        /// <param name="contractTemplateId"></param>
        /// <returns></returns>
        Task<ResultModel<ContractTemplate>> FindContractTemplateByIdWithIncludesAsync(Guid? contractTemplateId);

        /// <summary>
        /// Get all contract template
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<ContractTemplate>>> GetAllContractTemplateAsync(bool includeDeleted);

        /// <summary>
        /// Get all paginated contract template
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResult<ContractTemplate>> GetAllPaginatedContractTemplateAsync(PageRequest request);

        /// <summary>
        /// Generate contract document from templates
        /// Token format: Some {Name} was pushed
        /// </summary>
        /// <param name="contractTemplateId"></param>
        /// <param name="dataTokens"></param>
        /// <returns></returns>
        Task<ResultModel<MemoryStream>> GenerateDocumentFromTemplateAsync(Guid? contractTemplateId,
            IDictionary<string, string> dataTokens);

        /// <summary>
        /// Find section by id
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        Task<ResultModel<ContractSection>> FindSectionByIdAsync(Guid? sectionId);

        /// <summary>
        /// Update contract section
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateContractSectionAsync([Required] UpdateTemplateSectionViewModel model);


        /// <summary>
        /// Delete contractSection
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteContractSectionAsync(Guid? sectionId);

        /// <summary>
        /// Add section to contract template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddSectionToContractTemplateAsync([Required] AddSectionViewModel model);


        /// <summary>
        /// Order stages
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<ResultModel> OrderSectionAsync(IEnumerable<OrderSectionViewModel> data);
    }
}