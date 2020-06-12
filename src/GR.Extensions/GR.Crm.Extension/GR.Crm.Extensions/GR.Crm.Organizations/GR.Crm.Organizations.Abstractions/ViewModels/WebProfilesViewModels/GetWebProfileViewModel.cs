using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Organizations.Abstractions.ViewModels.WebProfilesViewModels
{
    public class GetWebProfileViewModel : BaseModel
    {
        /// <summary>
        /// Provider name
        /// </summary>
        [Required]
        public virtual string ProviderName { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// Icon url
        /// </summary>
        [Required]
        public virtual string IconUrl { get; set; }
    }
}
