using System;
using System.Collections.Generic;
using GR.Crm.Leads.Abstractions.Models;
using GR.TaskManager.Abstractions.Enums;
using Microsoft.AspNetCore.Routing;

namespace GR.TaskManager.Abstractions.Models.ViewModels
{
    public sealed class GetTaskViewModel : TaskBaseModel
    {
        /// <summary>
        /// Record id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Record id
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Task number
        /// </summary>
        public string TaskNumber { get; set; }

        /// <summary>
        /// Task items counted [completed/total]
        /// </summary>
        public int[] TaskItemsCount { get; set; }

        /// <summary>
        /// Author
        /// </summary>
        public string Author { get; set; } = "Undefined";

        /// <summary>
        /// Modified by
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Access level
        /// </summary>
        public string AccessLevel { get; set; } = TaskAccess.Undefined.ToString();


        /// <summary>
        /// Lead 
        /// </summary>
        public Guid? LeadId { get; set; }

        /// <summary>
        /// Lead 
        /// </summary>
        public Lead Lead { get; set; }

        public  string LeadName { get; set; }

        public TaskEnumItem StatusItem { get; set; }

        public TaskEnumItem TaskPriorityItem { get; set; }

        public IEnumerable<AssignedUser> AssignedUsers { get; set; } = new List<AssignedUser>();

        public ICollection<TaskAssignedUser> Assigned { get; set; }


        /// <summary>
        /// Task type id
        /// </summary>
        public  Guid? TaskTypeId { get; set; }

        /// <summary>
        /// Task type
        /// </summary>
        public  TaskType TaskType { get; set; }

    }


    public class TaskEnumItem
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }


    public class AssignedUser
    {
        public virtual Guid Id { get; set; }
        public virtual string LastName { get; set; }
        public virtual string FirstName { get; set; }
    }
}
