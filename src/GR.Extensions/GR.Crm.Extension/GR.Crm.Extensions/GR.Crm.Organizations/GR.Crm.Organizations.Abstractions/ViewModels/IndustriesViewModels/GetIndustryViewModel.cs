using GR.Core;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Organizations.Abstractions.ViewModels.IndustriesViewModels
{
    public class GetIndustryViewModel : BaseModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
