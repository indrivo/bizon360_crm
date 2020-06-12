using System;
using System.Collections.Generic;
using NPOI.SS.Formula.Functions;

namespace GR.Crm.Dashboard.Abstractions.ViewModel
{
    public class LeadDashboardViewModel
    {
        /// <summary>
        /// PipeLine name
        /// </summary>
        public virtual string PipeLine { get; set; }

        /// <summary>
        /// Lead count
        /// </summary>
        public virtual int LeadCount { get; set; } = 0;

        /// <summary>
        /// Lead value sum
        /// </summary>
        public virtual decimal LeadSum { get; set; } = 0;

        /// <summary>
        /// Lead sum Progress
        /// </summary>
        public virtual decimal LeadSumProgress { get; set; }


        /// <summary>
        /// Lead state Won
        /// </summary>
        public virtual int WonLead { get; set; } = 0;

        /// <summary>
        /// Lead WonLead Progress
        /// </summary>
        public virtual decimal WonLeadProgress { get; set; }

        /// <summary>
        /// Lead WonLead evolution
        /// </summary>
        public virtual IEnumerable<DashboardEvolutionValues> WonLeadEvolution { get; set; }


        /// <summary>
        /// Lead state New
        /// </summary>
        public virtual int NewLead { get; set; } = 0;

        /// <summary>
        /// Lead NewLead Progress
        /// </summary>
        public virtual decimal NewLeadProgress { get; set; }

        /// <summary>
        /// Lead NewLead Evolution
        /// </summary>
        public virtual IEnumerable<DashboardEvolutionValues> NewLeadEvolution { get; set; }



        /// <summary>
        /// Lead state New
        /// </summary>
        public virtual int LostLead { get; set; } = 0;

        /// <summary>
        /// Lead NewLead Progress
        /// </summary>
        public virtual decimal LostLeadProgress { get; set; }

        /// <summary>
        /// Lead LostLead Evolution
        /// </summary>
        public virtual IEnumerable<DashboardEvolutionValues> LostLeadEvolution { get; set; }

    }



    public class DashboardEvolutionValues
    {
        public virtual object GroupKey { get; set; }

        public virtual int Value { get; set; }
    }
}
