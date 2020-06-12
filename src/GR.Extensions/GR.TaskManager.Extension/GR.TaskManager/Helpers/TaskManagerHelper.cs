using System;
using System.Collections.Generic;
using System.Linq;
using Mapster;
using Newtonsoft.Json;
using GR.Core.Helpers;
using GR.Identity.Abstractions;
using GR.TaskManager.Abstractions.Enums;
using GR.TaskManager.Abstractions.Models;
using GR.TaskManager.Abstractions.Models.ViewModels;
using GR.Core.Extensions;
using GR.Core.Helpers.Pagination;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;

namespace GR.TaskManager.Helpers
{
    public class TaskManagerHelper
    {

        internal static Task TaskMapper(UpdateTaskViewModel taskViewModel, Task dbTaskResult)
        {
            if (dbTaskResult == null) return null;

            dbTaskResult.Name = taskViewModel.Name;
            dbTaskResult.Description = taskViewModel.Description;
            dbTaskResult.StartDate = taskViewModel.StartDate;
            dbTaskResult.EndDate = taskViewModel.EndDate;
            dbTaskResult.Status = taskViewModel.Status;
            dbTaskResult.TaskPriority = taskViewModel.TaskPriority;
            dbTaskResult.UserId = taskViewModel.UserId;
            dbTaskResult.LeadId = taskViewModel.LeadId;
            dbTaskResult.TaskTypeId = taskViewModel.TaskTypeId;
            if (!string.IsNullOrWhiteSpace(dbTaskResult.Files)) dbTaskResult.Files = JsonConvert.SerializeObject(taskViewModel.Files);

            return dbTaskResult;
        }

        internal static Task TaskMapper(CreateTaskViewModel taskViewModel)
        {
            var dto = taskViewModel.Adapt<Task>();
            if (!string.IsNullOrWhiteSpace(dto.Files)) dto.Files = JsonConvert.SerializeObject(taskViewModel.Files);
            if (taskViewModel.TaskItems == null) return dto;

            foreach (var item in taskViewModel.TaskItems)
                dto.TaskItems.Add(
                    new TaskItem
                    {
                        IsDone = false,
                        Name = item.Name
                    });

            return dto;
        }

        internal static GetTaskViewModel GetTaskMapper(Task dbTaskResult, Guid? currentUserId = null)
        {
            var dto = new GetTaskViewModel
            {
                Id = dbTaskResult.Id,
                TaskNumber = dbTaskResult.TaskNumber,
                StartDate = dbTaskResult.StartDate,
                EndDate = dbTaskResult.EndDate,
                Description = dbTaskResult.Description,
                Name = dbTaskResult.Name,
                Status = dbTaskResult.Status,
                TaskPriority = dbTaskResult.TaskPriority,
                IsDeleted = dbTaskResult.IsDeleted,
                UserId = dbTaskResult.UserId,
                Author = dbTaskResult.Author,
                LeadId = dbTaskResult.LeadId,
                ModifiedBy = dbTaskResult.ModifiedBy.IsNullOrEmpty()
                    ? dbTaskResult.Author
                    : dbTaskResult.ModifiedBy,
                UserTeam = dbTaskResult.AssignedUsers?.Select(x => x.UserId),
                AccessLevel = GetTaskAccessLevel(dbTaskResult, currentUserId).ToString(),
                StatusItem = new TaskEnumItem { Value = (int)dbTaskResult.Status, Text = dbTaskResult.Status.ToString() },
                TaskPriorityItem = new TaskEnumItem { Value = (int)dbTaskResult.TaskPriority, Text = dbTaskResult.TaskPriority.ToString() },
                AssignedUsers = GetUsersByListId(dbTaskResult.AssignedUsers?.Select(x => x.UserId)),
                TaskType = dbTaskResult.TaskType,
                TaskTypeId = dbTaskResult.TaskTypeId
            };

            var leadService = IoC.Resolve<ILeadService<Lead>>();
            var leadRequest = leadService.FindLeadByIdAsync(dto.LeadId).Result;
            if (leadRequest.IsSuccess)
                dto.Lead = leadRequest.Result;


            if (!string.IsNullOrWhiteSpace(dbTaskResult.Files)) dto.Files = JsonConvert.DeserializeObject<List<Guid>>(dbTaskResult.Files);
            dto.TaskItemsCount = CountTaskItems(dbTaskResult);
            return dto;
        }

        /// <summary>
        /// Get task permission
        /// </summary>
        /// <param name="dbTaskResult"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public static TaskAccess GetTaskAccessLevel(Task dbTaskResult, Guid? currentUserId)
        {
            if (dbTaskResult == null) return TaskAccess.Undefined;
            var userManager = IoC.Resolve<IUserManager<GearUser>>();
            if (currentUserId == null) return TaskAccess.Undefined;
            var taskAuthor = userManager.UserManager.Users.FirstOrDefault(x => x.UserName.Equals(dbTaskResult.Author.Trim()));
            if (taskAuthor != null && taskAuthor.Id.ToGuid().Equals(currentUserId.Value)) return TaskAccess.Author;

            return dbTaskResult.UserId.Equals(currentUserId)
                    ? TaskAccess.Owner
                    : dbTaskResult.AssignedUsers?.FirstOrDefault(x => x.UserId.Equals(currentUserId)) != null
                        ? TaskAccess.PartOfTeam
                        : TaskAccess.Undefined;
        }

        internal static IEnumerable<GetTaskItemViewModel> TaskItemsMapper(Task dbTaskResult)
        {
            return dbTaskResult.TaskItems.Select(item => new GetTaskItemViewModel
            {
                Id = item.Id,
                IsDone = item.IsDone,
                Name = item.Name,
            }).AsEnumerable();
        }

        internal static ResultModel<PagedResult<GetTaskViewModel>> GetTasksAsync(PagedResult<Task> dbTasksResult, Guid? currentUserId = null)
        {
            var taskPage = new PagedResult<GetTaskViewModel>
            {
                CurrentPage = dbTasksResult.CurrentPage,
                PageCount = dbTasksResult.PageCount,
                RowCount = dbTasksResult.RowCount,
                PageSize = dbTasksResult.PageSize,
                Result = new List<GetTaskViewModel>()
            };


            if (dbTasksResult.Result.Count > 0)
                for (var index = 0; index < dbTasksResult.Result.Count; index++)
                {
                    var item = dbTasksResult.Result[index];
                    taskPage.Result.Add(GetTaskMapper(item, currentUserId));
                }

            return new ResultModel<PagedResult<GetTaskViewModel>>
            {
                IsSuccess = true,
                Result = taskPage
            };
        }

        private static int[] CountTaskItems(Task dbTasksResult)
        {
            if (dbTasksResult.TaskItems == null || dbTasksResult.TaskItems.Count == 0) return new[] { 0, 0 };

            var total = dbTasksResult.TaskItems.Count;
            var completed = dbTasksResult.TaskItems.Count(x => x.IsDone);

            return new[] { completed, total };
        }

        public static IEnumerable<AssignedUser> GetUsersByListId(IEnumerable<Guid> listId)
        {
            var userService = IoC.Resolve<IUserManager<GearUser>>();
            var listUsers = new List<AssignedUser>();
            if (listId == null || !listId.Any()) return listUsers;

            foreach (var userId in listId)
            {
                var user = userService.UserManager.FindByIdAsync(userId.ToString()).Result;
                if (user != null)
                    listUsers.Add(new AssignedUser { Id = userId, LastName = user.UserLastName, FirstName = user.UserFirstName });
            }

            return listUsers;
        }
    }
}
