using System;
using System.Collections.Generic;

namespace GR.TaskManager.Abstractions.Models.ViewModels
{
    public sealed class CreateTaskViewModel : TaskBaseModel
    {
        /// <summary>
        /// Task items
        /// </summary>
        public List<TaskItemViewModel> TaskItems { get; set; }

        /// <summary>
        /// Lead id
        /// </summary>
        public Guid? LeadId { get; set; }

        /// <summary>
        /// Task type id
        /// </summary>
        public  Guid? TaskTypeId { get; set; }

        
    }
}
