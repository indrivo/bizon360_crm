using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Identity.Abstractions;
using GR.Notifications.Abstractions;
using GR.Notifications.Abstractions.Models.Notifications;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Leads.Infrastructure
{
    public class LeadNotificationService : ILeadNotificationService
    {
        private readonly INotify<GearRole> _notify;
        /// <summary>
        /// Inject context
        /// </summary>
        private readonly ILeadContext<Lead> _context;
        private const string LeadTitle = "Lead expire";
        private const string LeadExpires = "Lead #{0} expire tomorrow.";

        public LeadNotificationService(
            INotify<GearRole> notify,
            ILeadContext<Lead>  context)
        {
            _notify = notify;
            _context = context;
        }

        public async Task NotifyUsersOnLeadExpirationByStageAsync()
        {
            var notificationItems = await _context.Leads
                .Include(i=> i.Team)
                .ThenInclude(i=>i.TeamMembers)
                .Where(x => (x.StageDeadLine.HasValue &&  x.StageDeadLine.Value.Date == DateTime.Now.AddDays(1).Date) 
                            ||  x.DeadLine.HasValue && x.DeadLine.Value.Date == DateTime.Now.AddDays(1).Date).ToListAsync();

            foreach (var item in notificationItems)
            {
                var assignedUsersId = notificationItems.SelectMany(s => s.Team.TeamMembers).Select(s => s.UserId);

                var notification = new Notification
                {
                    Content = string.Format(LeadExpires, item.Name),
                    Subject = LeadTitle,
                    NotificationTypeId = NotificationType.Info
                };

                await _notify.SendNotificationToSystemAdminsAsync(notification);
                await _notify.SendNotificationAsync(assignedUsersId, notification);
            }

        }
    }
}
