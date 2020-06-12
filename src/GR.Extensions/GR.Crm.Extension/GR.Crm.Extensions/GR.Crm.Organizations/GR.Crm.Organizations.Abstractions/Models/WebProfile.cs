using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Organizations.Abstractions.Models
{
    public class WebProfile : BaseModel
    {
        /// <summary>
        /// Provider name
        /// </summary>
        [Required]
        [MaxLength(50)]
        public virtual string ProviderName { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [MaxLength(50)]
        public virtual string Url { get; set; }
        
        /// <summary>
        /// Icon
        /// </summary>
        
        public virtual byte[] Icon { get; set; }


        /// <summary>
        /// Icon name
        /// </summary>
        public virtual string IconName { get; set; }
    }
}
