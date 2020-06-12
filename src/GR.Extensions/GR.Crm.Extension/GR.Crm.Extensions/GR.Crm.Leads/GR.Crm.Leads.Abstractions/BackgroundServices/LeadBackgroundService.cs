using System;
using System.Threading.Tasks;
using GR.Core;
using GR.Core.Services;
using Microsoft.Extensions.Logging;

namespace GR.Crm.Leads.Abstractions.BackgroundServices
{
    public class LeadBackgroundService : BaseBackgroundService<LeadBackgroundService>
    {

        #region Injectable

        /// <summary>
        /// Inject service
        /// </summary>
        private readonly ILeadNotificationService _leadBackgroundService;

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="managerNotificationService"></param>
        public LeadBackgroundService(ILogger<LeadBackgroundService> logger, ILeadNotificationService managerNotificationService)
            : base("Lead manager", logger)
        {
            _leadBackgroundService = managerNotificationService;
            Interval = TimeSpan.FromHours(24);
        }

        /// <summary>
        /// Send logs
        /// </summary>
        /// <param name="state"></param>
        public override async Task Execute(object state)
        {
            if (!GearApplication.Configured) return;
            await _leadBackgroundService.NotifyUsersOnLeadExpirationByStageAsync();
        }
    }
}
