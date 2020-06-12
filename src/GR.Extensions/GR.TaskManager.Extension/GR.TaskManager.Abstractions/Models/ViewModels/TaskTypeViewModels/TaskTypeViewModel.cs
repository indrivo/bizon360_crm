using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.TaskManager.Abstractions.Models.ViewModels.TaskTypeViewModels
{
    public class TaskTypeViewModel
    {
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Task type name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }
    }
}
