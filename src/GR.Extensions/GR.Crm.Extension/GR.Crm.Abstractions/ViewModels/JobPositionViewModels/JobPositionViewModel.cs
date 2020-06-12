using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Abstractions.ViewModels.JobPositionViewModels
{
    public class JobPositionViewModel : BaseModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }
    }
}
