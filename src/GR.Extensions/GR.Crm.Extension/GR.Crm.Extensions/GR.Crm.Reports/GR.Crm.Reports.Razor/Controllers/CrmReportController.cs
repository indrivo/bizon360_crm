using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Reports.Abstraction;
using GR.Crm.Reports.Abstraction.ViewModels.LeadReportViewModels;
using GR.Crm.Reports.Abstraction.ViewModels.PaymentReportViewModel;
using GR.Identity.Data.Permissions;
using GR.Identity.Permissions.Abstractions.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GR.Crm.Reports.Razor.Controllers
{
    [Authorize]
    public class CrmReportController : BaseGearController
    {

        #region Inject

        /// <summary>
        /// Inject Crm report service
        /// </summary>
        private readonly ICrmReportService _crmReportService;

        #endregion


        public CrmReportController(ICrmReportService crmReportService)
        {
            _crmReportService = crmReportService;
        }

        public IActionResult Index(string type)
        {
            ViewBag.Type = type;
            return View();
        }

        /// <summary>
        /// Get Lead report 
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<ReportLeadViewModel>>))]
        public async Task<JsonResult> LeadReport(IEnumerable<PageRequestFilter> filters, IEnumerable<string> listGroupProperties)
            => await JsonAsync(_crmReportService.LeadReportAsync(filters, listGroupProperties));


        /// <summary>
        /// Get payment report
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<ReportPaymentViewModel>>))]
        public async Task<JsonResult> PaymentsReport(IEnumerable<PageRequestFilter> filters, IEnumerable<string> listGroupProperties)
            => await JsonAsync(_crmReportService.PaymentsReportAsync(filters, listGroupProperties));



        /// <summary>
        /// Get agreement report
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<ReportPaymentViewModel>>))]
        public async Task<JsonResult> AgreementsReport(IEnumerable<PageRequestFilter> filters, IEnumerable<string> listGroupProperties)
            => await JsonAsync(_crmReportService.AgreementsReportAsync(filters, listGroupProperties));


        /// <summary>
        /// Get agreement report
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<ReportLeadViewModel>>))]
        public async Task<JsonResult> TaskReport(IEnumerable<PageRequestFilter> filters, IEnumerable<string> listGroupProperties)
            => await JsonAsync(_crmReportService.TaskReportAsync(filters, listGroupProperties), SerializerSettings);

    }
}
