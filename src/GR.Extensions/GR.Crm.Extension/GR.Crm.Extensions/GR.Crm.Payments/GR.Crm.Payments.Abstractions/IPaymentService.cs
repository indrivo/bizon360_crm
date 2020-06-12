using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Payments.Abstractions.ViewModels.PaymentDashboardViewModel;
using GR.Crm.Payments.Abstractions.ViewModels.PaymentsViewModels;
using Microsoft.AspNetCore.Http;

namespace GR.Crm.Payments.Abstractions
{
    public interface IPaymentService
    {
        /// <summary>
        /// Get payment by id
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        Task<ResultModel<GetPaymentViewModel>> GetPaymentByIdAsync(Guid? paymentId);

        /// <summary>
        /// Get all payments
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
       Task<ResultModel<IEnumerable<GetPaymentViewModel>>> GetAllPaymentsAsync(bool includeDeleted = false);

        /// <summary>
        /// Get all paginated payments
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetTablePaymentViewModel>>> GetAllPaginatedPaymentsAsync(PageRequest request);

        /// <summary>
        /// Add payment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddPaymentAsync(AddPaymentViewModel model);

        /// <summary>
        /// update payment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
       Task<ResultModel> UpdatePaymentAsync(UpdatePaymentViewModel model);

        /// <summary>
        /// Delete payment
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
       Task<ResultModel> DeletePaymentAsync(Guid? paymentId);

        /// <summary>
        /// Disable payment
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
       Task<ResultModel> DisablePaymentAsync(Guid? paymentId);

        /// <summary>
        /// Activate payment
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivatePaymentAsync(Guid? paymentId);


        /// <summary>
        /// ExtractData to excel
        /// </summary>
        /// <param name="excel"></param>
        /// <returns></returns>
        Task<ResultModel> ImportXmlAsync(IFormFile excel);

    }
}
