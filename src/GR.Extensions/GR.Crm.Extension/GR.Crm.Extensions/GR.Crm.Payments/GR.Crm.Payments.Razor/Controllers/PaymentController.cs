using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Payments.Abstractions;
using GR.Crm.Payments.Abstractions.ViewModels.PaymentsViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GR.Crm.Payments.Razor.Controllers
{
    public class PaymentController : BaseGearController
    {

        #region Injected

        /// <summary>
        /// Inject payment service
        /// </summary>
        private readonly IPaymentService _paymentService;
        #endregion

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var paymentRequest = await _paymentService.GetPaymentByIdAsync(id);
            if (!paymentRequest.IsSuccess) return NotFound();
            return View(paymentRequest.Result);
        }

        /// <summary>
        /// Get payment by id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<GetPaymentViewModel>))]
        public async Task<JsonResult> GetPaymentById([Required] Guid paymentId)
            => await JsonAsync(_paymentService.GetPaymentByIdAsync(paymentId));

        /// <summary>
        /// Get all payments
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<GetPaymentViewModel>>))]
        public async Task<JsonResult> GetAllPayments(bool includeDeleted = false)
            => await JsonAsync(_paymentService.GetAllPaymentsAsync(includeDeleted));

        /// <summary>
        /// Get all paginated payments
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<PagedResult<GetTablePaymentViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedPayments(PageRequest request)
            => await JsonAsync(_paymentService.GetAllPaginatedPaymentsAsync(request));



        /// <summary>
        /// Add payment
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddPayment(AddPaymentViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            
            return await JsonAsync(_paymentService.AddPaymentAsync(model));
        }


        /// <summary>
        /// Update payment
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdatePayment(UpdatePaymentViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();

            return await JsonAsync(_paymentService.UpdatePaymentAsync(model));
        }


        /// <summary>
        /// Disable payment
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DisablePayment([Required] Guid paymentId)
            => await JsonAsync(_paymentService.DisablePaymentAsync(paymentId));

        /// <summary>
        /// Activate payment
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivatePayment([Required] Guid paymentId)
            => await JsonAsync(_paymentService.ActivatePaymentAsync(paymentId));

        /// <summary>
        /// Delete payment
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DeletePayment([Required] Guid paymentId)
            => await JsonAsync(_paymentService.DeletePaymentAsync(paymentId));


        /// <summary>
        /// Import excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> ImportXml(IFormFile file)
            => await JsonAsync(_paymentService.ImportXmlAsync(file));
    }
}
