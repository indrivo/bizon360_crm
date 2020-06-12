using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Payments.Abstractions.ViewModels.PaymentsViewModels;

namespace GR.Crm.Payments.Abstractions
{
    public interface IPaymentCodeService
    {

        /// <summary>
        /// Bet payment code by id
        /// </summary>
        /// <param name="paymentCodeId"></param>
        /// <returns></returns>
        Task<ResultModel<GetPaymentCodeViewModel>> GetPaymentCodeByIdAsync(Guid? paymentCodeId);

        /// <summary>
        /// Bet payment code by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<ResultModel<GetPaymentCodeViewModel>> GetPaymentCodeByCodeAsync(string code);

        /// <summary>
        /// Get all paginated payment code
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetPaymentCodeViewModel>>> GetAllPaginatedPaymentCodeAsync(PageRequest request);

        /// <summary>
        /// Get all  payment code
        /// </summary>
        Task<ResultModel<IEnumerable<GetPaymentCodeViewModel>>> GetAllPaymentCodeAsync(bool includeDeleted = false);

        /// <summary>
        /// Add payment code
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddPaymentCodeAsync(PaymentCodeViewModel model);


        /// <summary>
        /// Update payment code
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdatePaymentCodeAsync(PaymentCodeViewModel model);


        /// <summary>
        /// Disable payment code
        /// </summary>
        /// <param name="paymentCodeId"></param>
        /// <returns></returns>
        Task<ResultModel> DisablePaymentCodeAsync(Guid? paymentCodeId);

        /// <summary>
        /// Activate payment code
        /// </summary>
        /// <param name="paymentCodeId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivatePaymentCodeAsync(Guid? paymentCodeId);


        /// <summary>
        /// Remove payment code
        /// </summary>
        /// <param name="paymentCodeId"></param>
        /// <returns></returns>
        Task<ResultModel> RemovePaymentCodeAsync(Guid? paymentCodeId);
    }
}
