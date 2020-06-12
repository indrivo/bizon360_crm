using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Dashboard.Abstractions;
using GR.Crm.Dashboard.Abstractions.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GR.Crm.Dashboard.Razor.Controller
{
    public class CrmDashboardController : BaseGearController
    {

        #region Injected

        /// <summary>
        /// Inject dashboard service
        /// </summary>
        private readonly ICrmDashboardService _dashboardService;

        #endregion

        public IActionResult Index()
        {
            return View();
        }


        public CrmDashboardController(ICrmDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }


        /// <summary>
        /// Get list lead indices
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<LeadDashboardViewModel>>))]
        public async Task<JsonResult> GetLeadDashboardIndices(IEnumerable<PageRequestFilter> filters)
             => await JsonAsync(_dashboardService.GetLeadDashboardIndicesAsync(filters), SerializerSettings);
    }

    
}
