using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Abstractions.Models
{
    public class ProductType: BaseModel
    {
        /// <summary>
        /// Product type name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }
    }
}
