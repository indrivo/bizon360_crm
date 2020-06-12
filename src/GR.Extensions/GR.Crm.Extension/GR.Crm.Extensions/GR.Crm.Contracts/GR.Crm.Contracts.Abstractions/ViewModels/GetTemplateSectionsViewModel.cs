using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Contracts.Abstractions.ViewModels
{
    public class GetTemplateSectionsViewModel : BaseModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Section order
        /// </summary>
       
        public virtual int Order { get; set; }

        /// <summary>
        /// Template value with tokens
        /// </summary>
        public virtual string TemplateValue { get; set; }
    }
}
