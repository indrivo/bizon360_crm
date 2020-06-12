using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.TaskManager.Abstractions.Models
{
    public class TaskType : BaseModel
    {

        /// <summary>
        /// Task type name 
        /// </summary>
        [Required]
        public virtual string Name { get; set; }
    }
}
