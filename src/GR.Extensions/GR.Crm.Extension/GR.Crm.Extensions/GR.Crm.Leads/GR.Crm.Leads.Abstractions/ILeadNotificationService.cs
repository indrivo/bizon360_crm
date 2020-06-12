using System.Threading.Tasks;

namespace GR.Crm.Leads.Abstractions
{
    public interface ILeadNotificationService
    {
        /// <summary>
        /// Sent notification expired lead
        /// </summary>
        /// <returns></returns>
        Task NotifyUsersOnLeadExpirationByStageAsync();
    }
}
