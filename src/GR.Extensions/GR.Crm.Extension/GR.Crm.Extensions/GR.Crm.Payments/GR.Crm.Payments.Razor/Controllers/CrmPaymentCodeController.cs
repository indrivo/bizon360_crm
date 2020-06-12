using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Payments.Abstractions;
using GR.Crm.Payments.Abstractions.ViewModels.PaymentsViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GR.Crm.Payments.Razor.Controllers
{
    public class CrmPaymentCodeController : BaseGearController
    {

        #region Inject

        /// <summary>
        /// Inject payment code service
        /// </summary>
        private readonly IPaymentCodeService _paymentCodeService;

        #endregion


        public CrmPaymentCodeController(IPaymentCodeService paymentCodeService)
        {
            _paymentCodeService = paymentCodeService;
        }

        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Get payment code by id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<GetPaymentCodeViewModel>))]
        public async Task<JsonResult> GetPaymentCodeById([Required] Guid paymentCodeId)
            => await JsonAsync(_paymentCodeService.GetPaymentCodeByIdAsync(paymentCodeId));

        /// <summary>
        /// Get payment code by code
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<GetPaymentCodeViewModel>))]
        public async Task<JsonResult> GetPaymentCodeByCode([Required] string code)
            => await JsonAsync(_paymentCodeService.GetPaymentCodeByCodeAsync(code));


        /// <summary>
        /// Get all payment code 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<GetPaymentCodeViewModel>>))]
        public async Task<JsonResult> GetAllPaymentCode()
            => await JsonAsync(_paymentCodeService.GetAllPaymentCodeAsync());

        /// <summary>
        /// Get all paginated payment code 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<PagedResult<GetPaymentCodeViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedPaymentCode(PageRequest request)
            => await JsonAsync(_paymentCodeService.GetAllPaginatedPaymentCodeAsync(request));



        /// <summary>
        /// Add payment code 
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddPaymentCode(PaymentCodeViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_paymentCodeService.AddPaymentCodeAsync(model));
        }

        /// <summary>
        /// Update payment code 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdatePaymentCode(PaymentCodeViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_paymentCodeService.UpdatePaymentCodeAsync(model));
        }



        /// <summary>
        /// Disable payment code by id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DisablePaymentCode([Required] Guid paymentCodeId)
            => await JsonAsync(_paymentCodeService.DisablePaymentCodeAsync(paymentCodeId));

        /// <summary>
        /// Activate payment code by id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivatePaymentCode([Required] Guid paymentCodeId)
            => await JsonAsync(_paymentCodeService.ActivatePaymentCodeAsync(paymentCodeId));


        /// <summary>
        /// Delete payment code by id
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> RemovePaymentCode([Required] Guid paymentCodeId)
            => await JsonAsync(_paymentCodeService.RemovePaymentCodeAsync(paymentCodeId));
    }
}
