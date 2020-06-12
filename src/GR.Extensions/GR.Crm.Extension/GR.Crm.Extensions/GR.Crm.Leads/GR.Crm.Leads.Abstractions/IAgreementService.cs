using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Leads.Abstractions.ViewModels.AgreementsViewModels;

namespace GR.Crm.Leads.Abstractions
{
    public interface IAgreementService
    {

        /// <summary>
        /// Add agreement
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddAgreementAsync(AddAgreementViewModel model);

        /// <summary>
        /// Update agreement
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateAgreementAsync(UpdateAgreementViewModel model);

        /// <summary>
        /// Get agreement by id
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        Task<ResultModel<GetAgreementViewModel>> GetAgreementByIdAsync(Guid? agreementId);

        /// <summary>
        /// Get all paginated agreements for table
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetTableAgreementViewModel>>> GetAllTablePaginatedAgreementsAsync(PageRequest request);


        /// <summary>
        /// Get all paginated agreements
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetAgreementViewModel>>> GetAllPaginatedAgreementsAsync(PageRequest request);

        /// <summary>
        /// Get all agreements
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetAgreementViewModel>>> GetAllAgreementsAsync(bool includeDeleted);

        /// <summary>
        /// Get all agreement by lead id
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetAgreementViewModel>>> GetAllAgreementsByLeadIdAsync(Guid? leadId);

        /// <summary>
        /// Get all agreements by organization id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetAgreementViewModel>>> GetAllAgreementsByOrganizationIdIdAsync(Guid? organizationId);

        /// <summary>
        /// Disable agreements
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableAgreementAsync(Guid? agreementId);

        /// <summary>
        /// Activate agreement
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateAgreementAsync(Guid? agreementId);

        /// <summary>
        /// Delete permanently 
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteAgreementAsync(Guid? agreementId);


        /// <summary>
        /// Generate contract
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        Task<ResultModel<MemoryStream>> GenerateContractForAgreementIdAsync(Guid? agreementId);
    }
}
