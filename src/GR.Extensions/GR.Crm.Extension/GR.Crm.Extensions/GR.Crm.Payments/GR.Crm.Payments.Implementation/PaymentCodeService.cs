using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Payments.Abstractions;
using GR.Crm.Payments.Abstractions.Models;
using GR.Crm.Payments.Abstractions.ViewModels.PaymentsViewModels;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Payments.Implementation
{
    public class PaymentCodeService : IPaymentCodeService
    {


        #region Inject

        /// <summary>
        /// Inject context
        /// </summary>
        private readonly IPaymentContext _context;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        #endregion
        public PaymentCodeService(IPaymentContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        /// <summary>
        /// Bet payment code by id
        /// </summary>
        /// <param name="paymentCodeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetPaymentCodeViewModel>> GetPaymentCodeByIdAsync(Guid? paymentCodeId)
        {
            if (paymentCodeId == null)
                return new InvalidParametersResultModel<GetPaymentCodeViewModel>();

            var paymentCode = await _context.PaymentCodes
                .FirstOrDefaultAsync(x => x.Id == paymentCodeId);

            if (paymentCode == null)
                return new NotFoundResultModel<GetPaymentCodeViewModel>();

            return new SuccessResultModel<GetPaymentCodeViewModel>(_mapper.Map<GetPaymentCodeViewModel>(paymentCode));

        }

        /// <summary>
        /// Bet payment code by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetPaymentCodeViewModel>> GetPaymentCodeByCodeAsync(string code)
        {
            if (string.IsNullOrEmpty(code))
                return new InvalidParametersResultModel<GetPaymentCodeViewModel>();

            var paymentCode = await _context.PaymentCodes
                .FirstOrDefaultAsync(x => string.Equals(x.Code, code, StringComparison.InvariantCultureIgnoreCase));

            if (paymentCode == null)
                return new NotFoundResultModel<GetPaymentCodeViewModel>();

            return new SuccessResultModel<GetPaymentCodeViewModel>(_mapper.Map<GetPaymentCodeViewModel>(paymentCode));

        }


        /// <summary>
        /// Get all paginated payment code
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetPaymentCodeViewModel>>> GetAllPaginatedPaymentCodeAsync(
            PageRequest request)
        {

            if (request == null)
                return new InvalidParametersResultModel<PagedResult<GetPaymentCodeViewModel>>();

            var listPaymentCodes = await _context.PaymentCodes
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .GetPagedAsync(request);

            var map = listPaymentCodes.Map(_mapper.Map<IEnumerable<GetPaymentCodeViewModel>>(listPaymentCodes.Result));


            return new SuccessResultModel<PagedResult<GetPaymentCodeViewModel>>(map);

        }

        /// <summary>
        /// Get all  payment code
        /// </summary>
        public virtual async Task<ResultModel<IEnumerable<GetPaymentCodeViewModel>>> GetAllPaymentCodeAsync(
            bool includeDeleted = false)
        {
            var listPaymentCodes = await _context.PaymentCodes
               .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetPaymentCodeViewModel>>(_mapper.Map<IEnumerable<GetPaymentCodeViewModel>>(listPaymentCodes));
        }

        /// <summary>
        /// Add payment code
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddPaymentCodeAsync(PaymentCodeViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();


            var paymentCode = _mapper.Map<PaymentCode>(model);

            await _context.PaymentCodes.AddAsync(paymentCode);
            var result = await _context.PushAsync();

            return result.Map(paymentCode.Id);
        }

        /// <summary>
        /// Update payment code
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdatePaymentCodeAsync(PaymentCodeViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var paymentCode = await _context.PaymentCodes.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (paymentCode == null)
                return new NotFoundResultModel();

            paymentCode.Code = model.Code;
            paymentCode.Name = model.Name;

            _context.PaymentCodes.Update(paymentCode);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Disable payment code
        /// </summary>
        /// <param name="paymentCodeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisablePaymentCodeAsync(Guid? paymentCodeId) =>
            await _context.DisableRecordAsync<PaymentCode>(paymentCodeId);

        /// <summary>
        /// Activate payment code
        /// </summary>
        /// <param name="paymentCodeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivatePaymentCodeAsync(Guid? paymentCodeId) =>
            await _context.ActivateRecordAsync<PaymentCode>(paymentCodeId);

        /// <summary>
        /// Remove payment code
        /// </summary>
        /// <param name="paymentCodeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> RemovePaymentCodeAsync(Guid? paymentCodeId) =>
            await _context.RemovePermanentRecordAsync<PaymentCode>(paymentCodeId);

    }
}
