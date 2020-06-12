using System;
using System.Collections.Generic;
using GR.TaskManager.Abstractions.Enums;
using GR.TaskManager.Abstractions.Extensions;

namespace GR.TaskManager.Abstractions.Models.ViewModels
{
    public class UpdateTaskViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public List<Guid> Files { get; set; }

        public DateTime StartDate { get; set; }
        
        [ValidationUpdateTaskDateTimeAttributeExtensions("End date is less Start date")]
        public DateTime EndDate { get; set; }

        public Guid UserId { get; set; }

        public TaskPriority TaskPriority { get; set; } = TaskPriority.Low;

        public TaskStatus Status { get; set; } = TaskStatus.NotStarted;

        /// <summary>
        /// User team
        /// </summary>
        public virtual ICollection<Guid> UserTeam { get; set; } = new List<Guid>();

        /// <summary>
        /// Lead id
        /// </summary>
        public virtual Guid? LeadId { get; set; }

        /// <summary>
        /// Task type id
        /// </summary>
        public virtual Guid? TaskTypeId { get; set; }
    }
}
