using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Organizations.Abstractions.ViewModels.IndustriesViewModels;

namespace GR.Crm.Organizations.Abstractions
{
    public interface ICrmIndustryService
    {

        /// <summary>
        /// Get All paginated Industries
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetIndustryViewModel>>> GetAllPaginatedIndustriesAsync(PageRequest request);

        /// <summary>
        /// Get All Industries
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetIndustryViewModel>>> GetAllIndustriesAsync(bool includeDeleted);

        /// <summary>
        /// Get industry by id
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        Task<ResultModel<GetIndustryViewModel>> GetIndustryByIdAsync(Guid? industryId);

        /// <summary>
        /// Deactivate industry by id
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableIndustryAsync(Guid? industryId);

        /// <summary>
        /// Activate Industry by id
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateIndustryAsync(Guid? industryId);

        /// <summary>
        /// Delete industry
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteIndustryAsync(Guid? industryId);


        /// <summary>
        /// Add new industry
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddNewIndustryAsync(IndustryViewModel model);

        /// <summary>
        /// Update industry
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateIndustryAsync(IndustryViewModel model);
    }
}
