using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Filters.Enums;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Identity.Abstractions;
using GR.TaskManager.Abstractions;
using GR.TaskManager.Abstractions.Helpers;
using GR.TaskManager.Abstractions.Models;
using GR.TaskManager.Abstractions.Models.ViewModels;
using GR.TaskManager.Helpers;
using Task = GR.TaskManager.Abstractions.Models.Task;

namespace GR.TaskManager.Services
{
    public class TaskManager : TaskManagerHelper, ITaskManager
    {
        #region Injectable
        /// <summary>
        /// Inject db context
        /// </summary>
        private readonly ITaskManagerContext _context;

        /// <summary>
        /// Inject notification service
        /// </summary>
        private readonly TaskManagerNotificationService _notify;

        /// <summary>
        /// Inject user manager
        /// </summary>
        private readonly IUserManager<GearUser> _userManager;

        /// <summary>
        /// Lead service
        /// </summary>
        private readonly ILeadService<Lead> _leadService;
        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="identity"></param>
        /// <param name="userManager"></param>
        /// <param name="leadService"></param>
        public TaskManager(ITaskManagerContext context,
            IUserManager<GearUser> identity,
            IUserManager<GearUser> userManager,
            ILeadService<Lead> leadService)
        {
            _context = context;
            _userManager = userManager;
            _notify = new TaskManagerNotificationService(identity);
            _leadService = leadService;
        }

        #region Task GET

        public async Task<ResultModel<GetTaskViewModel>> GetTaskAsync(Guid taskId)
        {
            if (taskId == Guid.Empty) return ExceptionMessagesEnum.NullParameter.ToErrorModel<GetTaskViewModel>();

            var dbTaskResult = await _context.Tasks
                .Include(x => x.AssignedUsers)
                .Include(x => x.TaskItems)
                .Include(i=> i.TaskType)
                .FirstOrDefaultAsync(x => x.Id == taskId);
            if (dbTaskResult == null)
                return ExceptionMessagesEnum.TaskNotFound.ToErrorModel<GetTaskViewModel>();
            var currentUser = (await _userManager.GetCurrentUserAsync()).Result?.Id.ToGuid();
            var dto = GetTaskMapper(dbTaskResult, currentUser);

            return new ResultModel<GetTaskViewModel>
            {
                IsSuccess = true,
                Result = dto
            };
        }

        public async Task<ResultModel<List<GetTaskItemViewModel>>> GetTaskItemsAsync(Guid taskId)
        {
            if (taskId == Guid.Empty) return ExceptionMessagesEnum.NullParameter.ToErrorModel<List<GetTaskItemViewModel>>();

            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);
            if (task == null) return ExceptionMessagesEnum.TaskNotFound.ToErrorModel<List<GetTaskItemViewModel>>();

            var dbTaskItemsResult = await _context.TaskItems.Where(x => x.Task == task).ToListAsync();
            var dto = new List<GetTaskItemViewModel>();
            if (dbTaskItemsResult.Any())
                dto.AddRange(TaskItemsMapper(new Task { TaskItems = dbTaskItemsResult }));
            else
                return ExceptionMessagesEnum.TaskItemsNotFound.ToErrorModel<List<GetTaskItemViewModel>>();

            return new ResultModel<List<GetTaskItemViewModel>>
            {
                IsSuccess = true,
                Result = dto
            };
        }

        public async Task<ResultModel<PagedResult<GetTaskViewModel>>> GetUserTasksAsync(string userName, PageRequest request)
        {
            if (string.IsNullOrEmpty(userName))
                return ExceptionMessagesEnum.NullParameter.ToErrorModel<PagedResult<GetTaskViewModel>>();

            var query = _context.Tasks
                .Include(x => x.AssignedUsers)
                .Include(x => x.TaskItems)
                .Include(i=> i.TaskType)
                .Where(x => (x.Author == userName.Trim()) & (!x.IsDeleted || request.IncludeDeleted));

            if (request.PageRequestFilters.Select(s => s.Propriety).Contains("AssignedUsers"))
            {
                query = await CustomFilterTask(query, request);
                request.PageRequestFilters = request.PageRequestFilters.Where(s => s.Propriety != "AssignedUsers");
            }

            var pageResult = await query.GetPagedAsync(request);
            var currentUser = (await _userManager.GetCurrentUserAsync()).Result?.Id.ToGuid();

            return GetTasksAsync(pageResult, currentUser);
        }

        public async Task<ResultModel<PagedResult<GetTaskViewModel>>> GetAssignedTasksAsync(Guid userId, string userName, PageRequest request)
        {
            if (userId == Guid.Empty) return ExceptionMessagesEnum.NullParameter.ToErrorModel<PagedResult<GetTaskViewModel>>();

            var query = _context.Tasks
                .Include(x => x.AssignedUsers)
                .Include(i=> i.TaskType)
                .Where(x => (x.UserId == userId || x.AssignedUsers.Any(c => c.UserId.Equals(userId)))
                            & (!x.IsDeleted || request.IncludeDeleted)
                            & (x.Author != userName));

            if (request.PageRequestFilters.Select(s => s.Propriety).Contains("AssignedUsers"))
            {
                query = await CustomFilterTask(query, request);
                request.PageRequestFilters = request.PageRequestFilters.Where(s => s.Propriety != "AssignedUsers");
            }

            var pageResult = await query.GetPagedAsync(request);


            return GetTasksAsync(pageResult, userId);
        }

        /// <summary>
        /// Get all user task by name and assigned task
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResultModel<PagedResult<GetTaskViewModel>>> GetAllUserTasksAsync(Guid userId, string userName, PageRequest request)
        {
            if (userId == Guid.Empty) return ExceptionMessagesEnum.NullParameter.ToErrorModel<PagedResult<GetTaskViewModel>>();


            var allLeadsRequest = (await _leadService.GetAllLeadsAsync(false)).Result.ToList();


            var query = _context.Tasks
                .Include(x => x.AssignedUsers)
                .Include(i=> i.TaskType)
                .Where(x => (x.UserId == userId || x.AssignedUsers.Any(c => c.UserId.Equals(userId)))
                            || (x.Author == userName));


            if (request.PageRequestFilters.Select(s => s.Propriety).Contains("AssignedUsers"))
            {
                query = await CustomFilterTask(query, request);
                request.PageRequestFilters = request.PageRequestFilters.Where(s => s.Propriety != "AssignedUsers");
            }

            var queryTest = query
                .Select(s => new GetTaskViewModel
                {
                    Id = s.Id,
                    TaskNumber = s.TaskNumber,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    Description = s.Description,
                    Name = s.Name,
                    Status = s.Status,
                    TaskPriority = s.TaskPriority,
                    IsDeleted = s.IsDeleted,
                    UserId = s.UserId,
                    Author = s.Author,
                    LeadId = s.LeadId,
                    TaskTypeId = s.TaskTypeId,
                    TaskType = s.TaskType,
                    LeadName = s.LeadId.HasValue ?
                        allLeadsRequest.FirstOrDefault(i => i.Id == s.LeadId.Value) == null ? "" : allLeadsRequest.FirstOrDefault(i => i.Id == s.LeadId.Value).Name
                        : "",
                });

            var paginatedResult = await queryTest.GetPagedAsync(request);


            var test = paginatedResult.Result;

            foreach (var item in test)
            {
                var task = await query.FirstOrDefaultAsync(x => x.Id == item.Id);

                if (task == null)
                    continue;

                item.ModifiedBy = task.ModifiedBy.IsNullOrEmpty()
                    ? task.Author
                    : task.ModifiedBy;
                item.UserTeam = task.AssignedUsers?.Select(x => x.UserId);
                item.AccessLevel = GetTaskAccessLevel(task, userId).ToString();
                item.StatusItem = new TaskEnumItem { Value = (int)task.Status, Text = task.Status.ToString() };
                item.TaskPriorityItem = new TaskEnumItem { Value = (int)task.TaskPriority, Text = task.TaskPriority.ToString() };
                item.AssignedUsers = GetUsersByListId(task.AssignedUsers?.Select(x => x.UserId));
            }


            paginatedResult.Result = test;

            //var pageResult = await query.GetPagedAsync(request);

            return new SuccessResultModel<PagedResult<GetTaskViewModel>>(paginatedResult);
        }

        public async Task<ResultModel<PagedResult<GetTaskViewModel>>> GetTaskByLeadIdAsync(Guid? leadId, PageRequest request)
        {
            if (leadId == null)
                return ExceptionMessagesEnum.NullParameter.ToErrorModel<PagedResult<GetTaskViewModel>>();

            var query = _context.Tasks
                .Include(x => x.AssignedUsers)
                .Include(x => x.TaskItems)
                .Include(i=> i.TaskType)
                .Where(x => (x.LeadId == leadId) & (!x.IsDeleted || request.IncludeDeleted));


            if (request.PageRequestFilters.Select(s => s.Propriety).Contains("AssignedUsers"))
            {
                query = await CustomFilterTask(query, request);
                request.PageRequestFilters = request.PageRequestFilters.Where(s => s.Propriety != "AssignedUsers");
            }


            var pageResult = await query.GetPagedAsync(request);
            var currentUser = (await _userManager.GetCurrentUserAsync()).Result?.Id.ToGuid();

            return GetTasksAsync(pageResult, currentUser);
        }


        public async Task<ResultModel<IEnumerable<GetTaskViewModel>>> GetAllTaskByLeadIdAsync(Guid? leadId)
        {
            if (leadId == null)
                return new InvalidParametersResultModel<IEnumerable<GetTaskViewModel>>();

            var dbTasksResult = await _context.Tasks
                .Include(x => x.AssignedUsers)
                .Include(x => x.TaskItems)
                .Include(i=> i.TaskType)
                .Where(x => (x.LeadId == leadId))
                .ToListAsync();

            var list = dbTasksResult.Select(task => GetTaskMapper(task)).ToList();

            return new SuccessResultModel<IEnumerable<GetTaskViewModel>>(list);
        }

        #endregion

        #region Task

        public virtual async Task<ResultModel<Guid>> CreateTaskAsync(CreateTaskViewModel task)
        {
            var taskModel = TaskMapper(task);
            taskModel.TaskNumber = await GenerateTaskNumberAsync();
            foreach (var user in task.UserTeam)
            {
                var checkUser = await _userManager
                        .UserManager
                        .Users
                        .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id.ToGuid().Equals(user));
                if (checkUser == null) continue;
                taskModel.AssignedUsers.Add(new TaskAssignedUser
                {
                    UserId = user
                });
            }
            _context.Tasks.Add(taskModel);
            var result = await _context.PushAsync();

            if (result.IsSuccess) await _notify.AddTaskNotificationAsync(taskModel);
            return new ResultModel<Guid>
            {
                IsSuccess = result.IsSuccess,
                Result = taskModel.Id,
                Errors = result.Errors
            };
        }

        public virtual async Task<ResultModel> UpdateTaskAsync(UpdateTaskViewModel task)
        {
            var dbTaskResult = _context.Tasks
                .Include(x => x.AssignedUsers)
                .FirstOrDefault(x => (x.Id == task.Id) & (x.IsDeleted == false));

            if (dbTaskResult == null)
                return ExceptionMessagesEnum.TaskNotFound.ToErrorModel();

            var lastStatus = dbTaskResult.Status;
            var lastPriority = dbTaskResult.TaskPriority;

            var taskModel = TaskMapper(task, dbTaskResult);
            _context.Tasks.Update(taskModel);
            var result = await _context.PushAsync();

            if (!result.IsSuccess)
                return result;

            await AddOrUpdateUsersToTaskGroupAsync(dbTaskResult, task.UserTeam);

            if (task.Status != lastStatus)
                await _notify.ChangeStatusTaskNotificationAsync(taskModel, lastStatus);
            else if (task.TaskPriority != lastPriority)
                await _notify.ChangePriorityTaskNotificationAsync(taskModel, lastPriority);
            else
                await _notify.UpdateTaskNotificationAsync(taskModel);

            return result;
        }

        /// <inheritdoc />
        /// <summary>
        /// Add or remove user to task team
        /// </summary>
        /// <param name="task"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> AddOrUpdateUsersToTaskGroupAsync(Task task, IEnumerable<Guid> users)
        {
            var response = new ResultModel();
            if (task == null)
            {
                response.Errors.Add(new ErrorModel(string.Empty, nameof(NullReferenceException)));
                return response;
            }

            var current = task.AssignedUsers?.ToList() ?? new List<TaskAssignedUser>();
            _context.TaskAssignedUsers.RemoveRange(current);
            var newUsers = users.Select(x => new TaskAssignedUser
            {
                UserId = x,
                TaskId = task.Id
            });

            await _context.TaskAssignedUsers.AddRangeAsync(newUsers);

            return await _context.PushAsync();
        }

        public async Task<ResultModel> DeleteTaskAsync(Guid taskId)
        {
            if (taskId == Guid.Empty) return ExceptionMessagesEnum.NullParameter.ToErrorModel();

            var dbTaskResult = _context.Tasks.FirstOrDefault(x => x.Id == taskId);
            if (dbTaskResult == null) return ExceptionMessagesEnum.TaskNotFound.ToErrorModel();

            var currentUser = await _userManager.GetCurrentUserAsync();

            if (dbTaskResult.Author != currentUser.Result.UserName)
                return new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Not have permission to deactivate task" } } };

            dbTaskResult.IsDeleted = true;
            _context.Tasks.Update(dbTaskResult);
            var result = await _context.PushAsync();

            if (result.IsSuccess) await _notify.DeleteTaskNotificationAsync(dbTaskResult);
            return new ResultModel
            {
                IsSuccess = result.IsSuccess,
                Errors = result.Errors
            };
        }

        public async Task<ResultModel> DeletePermanentTaskAsync(Guid taskId)
        {
            if (taskId == Guid.Empty) return ExceptionMessagesEnum.NullParameter.ToErrorModel();

            var task = _context.Tasks.FirstOrDefault(x => x.Id == taskId);
            if (task == null) return ExceptionMessagesEnum.TaskNotFound.ToErrorModel();

            _context.Tasks.Remove(task);
            return await _context.PushAsync();
        }

        public async Task<ResultModel> RestoreTaskAsync(Guid taskId)
        {
            if (taskId == Guid.Empty) return ExceptionMessagesEnum.NullParameter.ToErrorModel();

            var dbTaskResult = _context.Tasks.FirstOrDefault(x => x.Id == taskId);
            if (dbTaskResult == null) return ExceptionMessagesEnum.TaskNotFound.ToErrorModel();

            dbTaskResult.IsDeleted = false;
            _context.Tasks.Update(dbTaskResult);
            var result = await _context.PushAsync();

            if (result.IsSuccess) await _notify.DeleteTaskNotificationAsync(dbTaskResult);
            return new ResultModel
            {
                IsSuccess = result.IsSuccess,
                Errors = result.Errors
            };
        }

        #endregion

        #region Task Items

        public async Task<ResultModel<Guid>> CreateTaskItemAsync(CreateTaskItemViewModel taskItem)
        {
            var dbTaskResult = _context.Tasks.FirstOrDefault(x => x.Id == taskItem.TaskId);
            if (dbTaskResult == null) return ExceptionMessagesEnum.TaskNotFound.ToErrorModel<Guid>();

            var taskModel = new TaskItem { Name = taskItem.Name, Task = dbTaskResult };

            _context.TaskItems.Add(taskModel);
            var result = await _context.PushAsync();

            return new ResultModel<Guid>
            {
                IsSuccess = result.IsSuccess,
                Result = taskModel.Id,
                Errors = result.Errors
            };
        }

        public async Task<ResultModel<Guid>> UpdateTaskItemAsync(UpdateTaskItemViewModel taskItem)
        {
            var dbTaskResult = _context.TaskItems.FirstOrDefault(x => x.Id == taskItem.Id);
            if (dbTaskResult == null) return ExceptionMessagesEnum.TaskNotFound.ToErrorModel<Guid>();

            dbTaskResult.Name = taskItem.Name;
            dbTaskResult.IsDone = taskItem.IsDone;
            _context.TaskItems.Update(dbTaskResult);

            var result = await _context.PushAsync();

            return new ResultModel<Guid>
            {
                IsSuccess = result.IsSuccess,
                Result = dbTaskResult.Id,
                Errors = result.Errors
            };
        }

        public async Task<ResultModel> DeleteTaskItemAsync(Guid taskItemId)
        {
            if (taskItemId == Guid.Empty) return ExceptionMessagesEnum.NullParameter.ToErrorModel();

            var task = _context.TaskItems.FirstOrDefault(x => x.Id == taskItemId);
            if (task == null) return ExceptionMessagesEnum.TaskNotFound.ToErrorModel();

            _context.TaskItems.Remove(task);

            var result = await _context.PushAsync();

            return new ResultModel
            {
                IsSuccess = result.IsSuccess,
                Errors = result.Errors
            };
        }

        private async Task<string> GenerateTaskNumberAsync()
        {
            const string number = "00001";
            var taskNumber = await _context.Tasks.MaxAsync(x => x.TaskNumber);
            if (taskNumber.IsNullOrEmpty()) return number;
            var lastNumber = taskNumber.IsNumeric() ? int.Parse(taskNumber) : int.Parse(number);
            return $"{++lastNumber:00000}";
        }

        #endregion



        #region Helper

        private static async Task<IQueryable<Task>> CustomFilterTask(IQueryable<Task> source, PageRequest request)
        {
            if (!request.PageRequestFilters.Select(s => s.Propriety).Contains("AssignedUsers")) return source;


            var listUsersId = request.PageRequestFilters
                .Where(x => string.Equals(x.Propriety.Trim(), "AssignedUsers".Trim(), StringComparison.CurrentCultureIgnoreCase)).Select(s => s.Value.ToStringIgnoreNull()).ToList();

            var listTaskId = new List<Guid>();

            foreach (var user in listUsersId)
            {
                var task = await source.Where(x => x.AssignedUsers.FirstOrDefault(i => i.UserId.ToString() == user) != null)
                    .ToListAsync();
                listTaskId.AddRange(task.Select(s => s.Id));
            }

            source = source.Where(x => listTaskId.Contains(x.Id));
            request.PageRequestFilters = request.PageRequestFilters.Where(s => s.Propriety != "AssignedUsers");


            return source;
        }


        #endregion
    }
}
