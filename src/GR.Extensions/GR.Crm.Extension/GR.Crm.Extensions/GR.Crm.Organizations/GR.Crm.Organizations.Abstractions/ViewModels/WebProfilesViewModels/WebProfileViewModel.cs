using System.ComponentModel.DataAnnotations;
using GR.Core;
using Microsoft.AspNetCore.Http;

namespace GR.Crm.Organizations.Abstractions.ViewModels.WebProfilesViewModels
{
    public class WebProfileViewModel : BaseModel
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
        /// Icon
        /// </summary>
        [Required]
        public virtual IFormFile Icon { get; set; }
    }
}
