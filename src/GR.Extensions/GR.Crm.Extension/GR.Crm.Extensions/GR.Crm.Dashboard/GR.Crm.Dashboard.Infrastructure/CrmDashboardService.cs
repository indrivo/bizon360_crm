using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Dashboard.Abstractions;
using GR.Crm.Dashboard.Abstractions.ViewModel;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.PipeLines.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Dashboard.Infrastructure
{
    public class CrmDashboardService : ICrmDashboardService
    {

        #region Injected

        /// <summary>
        /// inject lead context
        /// </summary>
        private readonly ILeadContext<Lead> _leadContext;

        /// <summary>
        /// Inject PipeLine
        /// </summary>
        private readonly IPipeLineContext _pipeLineContext;

        #endregion


        public CrmDashboardService(ILeadContext<Lead> leadContext, IPipeLineContext pipeLineContext)
        {
            _leadContext = leadContext;
            _pipeLineContext = pipeLineContext;
        }


        /// <summary>
        /// Get list lead indices
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<LeadDashboardViewModel>>> GetLeadDashboardIndicesAsync(IEnumerable<PageRequestFilter> filters)
        {
            var selectPeriod = filters.FirstOrDefault(x => x.Propriety == "Period");
            var days = 30;

            if (selectPeriod != null)
            {
                if (!int.TryParse(selectPeriod.Value.ToStringIgnoreNull(), out days))
                    days = 30;
            }
            var period = DateTime.Now.AddDays(-days).Date;
            var lastPeriod = DateTime.Now.AddDays(-days * 2).Date;

            var leadQuery = _leadContext.Leads
                .Include(i => i.LeadState)
                .Include(i => i.PipeLine)
                .NonDeleted();

            var pipeLineQuery = _pipeLineContext.PipeLines.NonDeleted();
            var listIndices = new List<LeadDashboardViewModel>();



            #region FilterByPipeLine

            var selectPipeLine = filters.Where(x => x.Propriety == "PipeLine").Select(s=> s.Value);

            if (selectPipeLine.Any())
                pipeLineQuery = pipeLineQuery.Where(x => selectPipeLine.Contains(x.Id.ToString()));

            #endregion


            foreach (var pipeLine in pipeLineQuery)
            {
                var leads = leadQuery.Where(x => x.PipeLineId == pipeLine.Id);
                var currentLeads = leadQuery.Where(x => x.PipeLineId == pipeLine.Id && x.Created >= period);
                var lastLeads = leadQuery.Where(x => x.PipeLineId == pipeLine.Id && x.Created <= period && x.Created >= lastPeriod);

                var leadInfo = new LeadDashboardViewModel
                {
                    PipeLine = pipeLine.Name,
                    LeadCount = leads.Count(x => x.LeadState.Name != "Won" && x.LeadState.Name != "Lost")
                };

                //lead New
                leadInfo.NewLead = await currentLeads.CountAsync(x => string.Equals(x.LeadState.Name, "New", StringComparison.CurrentCultureIgnoreCase));
                var lastNewLead = await lastLeads.CountAsync(x => string.Equals(x.LeadState.Name, "New", StringComparison.CurrentCultureIgnoreCase));
                leadInfo.NewLeadProgress = CalculationPercentageIncrease(leadInfo.NewLead, lastNewLead);
                leadInfo.NewLeadEvolution = currentLeads.Where(x => string.Equals(x.LeadState.Name, "New", StringComparison.CurrentCultureIgnoreCase))
                    .GroupBy(g => new { Data = g.Created })
                                .Select(s => new DashboardEvolutionValues()
                                { GroupKey = s.Key, Value = s.Count() });

                //lead Won
                leadInfo.WonLead = await currentLeads.CountAsync(x => string.Equals(x.LeadState.Name, "Won", StringComparison.CurrentCultureIgnoreCase));
                var lastWonLead = await lastLeads.CountAsync(x => string.Equals(x.LeadState.Name, "Won", StringComparison.CurrentCultureIgnoreCase));
                leadInfo.WonLeadProgress = CalculationPercentageIncrease(leadInfo.WonLead, lastWonLead);
                leadInfo.WonLeadEvolution = currentLeads.Where(x => string.Equals(x.LeadState.Name, "Won", StringComparison.CurrentCultureIgnoreCase))
                    .GroupBy(g => new { Data = g.Created })
                    .Select(s => new DashboardEvolutionValues()
                        { GroupKey = s.Key, Value = s.Count() });

                //lead lost
                leadInfo.LostLead = await currentLeads.CountAsync(x => string.Equals(x.LeadState.Name, "Lost", StringComparison.CurrentCultureIgnoreCase));
                var lastLostLead = await lastLeads.CountAsync(x => string.Equals(x.LeadState.Name, "Lost", StringComparison.CurrentCultureIgnoreCase));
                leadInfo.LostLeadProgress = CalculationPercentageIncrease(leadInfo.LostLead, lastLostLead);
                leadInfo.LostLeadEvolution = currentLeads.Where(x => string.Equals(x.LeadState.Name, "Lost", StringComparison.CurrentCultureIgnoreCase))
                    .GroupBy(g => new { Data = g.Created })
                    .Select(s => new DashboardEvolutionValues()
                        { GroupKey = s.Key, Value = s.Count() });


                //lead sum
                leadInfo.LeadSum = await currentLeads.SumAsync(s=> s.Value);
                var lastLeadSum = await lastLeads.SumAsync(s => s.Value);
                leadInfo.LeadSumProgress = CalculationPercentageIncrease(leadInfo.LostLead, lastLostLead);

                listIndices.Add(leadInfo);

            }


            return new SuccessResultModel<IEnumerable<LeadDashboardViewModel>>(listIndices);
        }


        #region Helper

        private static decimal CalculationPercentageIncrease(decimal newValue, decimal oldValue)
        {
            if (newValue == 0 && oldValue == 0)
                return 0;

            if (newValue > 0 && oldValue == 0)
                return 100;

            if (newValue == 0 && oldValue > 0)
                return -100;

            return newValue > oldValue ?
                Math.Round(Math.Abs((oldValue - newValue) / newValue * 100), 2)
                : Math.Round((newValue - oldValue) / oldValue * 100, 2);
        }

        #endregion

    }
}
