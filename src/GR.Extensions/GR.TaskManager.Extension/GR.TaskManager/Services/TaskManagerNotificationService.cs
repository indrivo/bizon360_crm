using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Identity.Abstractions;
using GR.Notifications.Abstractions;
using GR.Notifications.Abstractions.Models.Notifications;
using GR.TaskManager.Abstractions;
using GR.TaskManager.Abstractions.Enums;
using TaskStatus = GR.TaskManager.Abstractions.Enums.TaskStatus;

namespace GR.TaskManager.Services
{
    public sealed class TaskManagerNotificationService : ITaskManagerNotificationService
    {
        private readonly INotify<GearRole> _notify;
        private readonly IUserManager<GearUser> _identity;
        private readonly ITaskManagerContext _context;

        private const string TaskCreated = "Task #{0} has been assigned to you.";
        private const string TaskUpdated = "Task #{0} has been updated by {1}.";
        private const string TaskCompleted = "Task #{0} has been completed by {1}.";
        private const string TaskChangeStatus = "Task #{0} change state {1} to {2} by {3}.";
        private const string TaskChangePriority = "Task #{0} change priority {1} to {2} by {3}.";
        private const string TaskRemoved = "Task #{0} has been removed.";
        private const string TaskTitle = "Task Notification";
        private const string TaskExpires = "Task #{0} expires tomorrow.";

        public TaskManagerNotificationService(IUserManager<GearUser> identity)
        {
            _notify = IoC.Resolve<INotify<GearRole>>();
            _identity = identity;
            _context = IoC.Resolve<ITaskManagerContext>();
        }

        internal async Task AddTaskNotificationAsync(Abstractions.Models.Task task)
        {
            var listAssignedUserId = task.AssignedUsers.Select(s => s.UserId).ToList();

            await _notify.SendNotificationAsync(listAssignedUserId,
                new Notification
                {
                    Content = string.Format(TaskCreated, task.TaskNumber),
                    Subject = TaskTitle,
                    NotificationTypeId = NotificationType.Info
                });
        }

        internal async Task UpdateTaskNotificationAsync(Abstractions.Models.Task task)
        {
            var recipients = new List<Guid>();
            var taskUserRequest = await _identity.UserManager.FindByNameAsync(task.Author);
            if (taskUserRequest != null)
                recipients.Add(taskUserRequest.Id.ToGuid());
            recipients.AddRange(task.AssignedUsers.Select(s => s.UserId));
            recipients = recipients.Distinct().ToList();
            await _notify.SendNotificationAsync(recipients,
                new Notification
                {
                    Content = string.Format(TaskUpdated, task.TaskNumber, task.Author),
                    Subject = TaskTitle,
                    NotificationTypeId = NotificationType.Info
                });
        }

        internal async Task ChangeStatusTaskNotificationAsync(Abstractions.Models.Task task, TaskStatus lastStatus)
        {
            var recipients = task.AssignedUsers.Select(s => s.UserId).ToList();
            var taskUserRequest = await _identity.UserManager.FindByNameAsync(task.Author);
            if (taskUserRequest != null)
                recipients.Add(taskUserRequest.Id.ToGuid());
            recipients = recipients.Distinct().ToList();

            await _notify.SendNotificationAsync(recipients,
                new Notification
                {
                    Content = string.Format(TaskChangeStatus, task.TaskNumber, lastStatus.ToString(), task.Status.ToString(), task.Author),
                    Subject = TaskTitle,
                    NotificationTypeId = NotificationType.Info
                });
        }
        internal async Task ChangePriorityTaskNotificationAsync(Abstractions.Models.Task task, TaskPriority lastPriority)
        {
            var recipients = task.AssignedUsers.Select(s => s.UserId).ToList();
            var taskUserRequest = await _identity.UserManager.FindByNameAsync(task.Author);
            if (taskUserRequest != null)
                recipients.Add(taskUserRequest.Id.ToGuid());
            recipients = recipients.Distinct().ToList();

            await _notify.SendNotificationAsync(recipients,
                new Notification
                {
                    Content = string.Format(TaskChangePriority, task.TaskNumber, lastPriority.ToString(), task.TaskPriority.ToString(), task.Author),
                    Subject = TaskTitle,
                    NotificationTypeId = NotificationType.Info
                });
        }

        internal async Task DeleteTaskNotificationAsync(Abstractions.Models.Task task)
        {
            var listAssignedUserId = task.AssignedUsers.Select(s => s.UserId).ToList();
            var userRequest = await _identity.UserManager.FindByNameAsync(task.Author);
            if (userRequest != null)
                listAssignedUserId.Add(userRequest.Id.ToGuid());

            listAssignedUserId = listAssignedUserId.Distinct().ToList();

            await _notify.SendNotificationAsync(listAssignedUserId,
                new Notification
                {
                    Content = string.Format(TaskRemoved, task.TaskNumber),
                    Subject = TaskTitle,
                    NotificationTypeId = NotificationType.Info
                });
        }

        public async Task TaskExpirationNotificationAsync()
        {
            var notificationItems = await _context.Tasks
                .Include(i => i.AssignedUsers)
                .Where(x => x.EndDate.Date == DateTime.Now.Date.AddDays(1) && x.Status != TaskStatus.Completed).ToListAsync();


            foreach (var item in notificationItems)
            {
                var listAssignedUsersId = item.AssignedUsers.Select(s => s.UserId).ToList();
                var userRequest = await _identity.UserManager.FindByNameAsync(item.Author);
                if (userRequest != null)
                    listAssignedUsersId.Add(userRequest.Id.ToGuid());

                listAssignedUsersId = listAssignedUsersId.Distinct().ToList();

                await _notify.SendNotificationAsync(listAssignedUsersId, new Notification
                {
                    Content = string.Format(TaskExpires, item.TaskNumber),
                    Subject = TaskTitle,
                    NotificationTypeId = NotificationType.Info
                });
            }
        }
    }
}
