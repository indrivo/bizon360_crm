using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Reports.Abstraction.ViewModels.AgreementViewModels;
using GR.Crm.Reports.Abstraction.ViewModels.LeadReportViewModels;
using GR.Crm.Reports.Abstraction.ViewModels.PaymentReportViewModel;

namespace GR.Crm.Reports.Abstraction
{
    public interface ICrmReportService
    {
        /// <summary>
        /// Lead report
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<ReportLeadViewModel>>> LeadReportAsync(IEnumerable<PageRequestFilter> filters, IEnumerable<string> listGroupProperties);

        /// <summary>
        /// Payment report 
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<ReportPaymentViewModel>>> PaymentsReportAsync(IEnumerable<PageRequestFilter> filters, IEnumerable<string> listGroupProperties);


        /// <summary>
        /// Agreement report 
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<ReportAgreementViewModel>>> AgreementsReportAsync(IEnumerable<PageRequestFilter> filters, IEnumerable<string> listGroupProperties);

        /// <summary>
        /// Task report
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="listGroupProperties"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<ReportLeadViewModel>>> TaskReportAsync(IEnumerable<PageRequestFilter> filters, IEnumerable<string> listGroupProperties);
    }
}
