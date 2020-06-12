using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Dashboard.Abstractions.ViewModel;

namespace GR.Crm.Dashboard.Abstractions
{
    public interface ICrmDashboardService
    {

        /// <summary>
        /// Get list lead indices
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<LeadDashboardViewModel>>> GetLeadDashboardIndicesAsync(IEnumerable<PageRequestFilter> filters);

        
    }
}
