using GR.Core;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Core.Helpers.Responses;
using GR.Entities.Data;
using GR.Identity.Abstractions;
using GR.Identity.Abstractions.Helpers.Attributes;
using GR.Identity.Abstractions.Models;
using GR.Identity.Abstractions.Models.MultiTenants;
using GR.Identity.Abstractions.Models.UserProfiles;
using GR.Identity.Abstractions.ViewModels.RoleViewModels;
using GR.Identity.Abstractions.ViewModels.UserViewModels;
using GR.Identity.Data;
using GR.Identity.Data.Permissions;
using GR.Identity.Permissions.Abstractions;
using GR.Identity.Permissions.Abstractions.Attributes;
using GR.Identity.Roles.Razor.ViewModels.RoleViewModels;
using GR.Notifications.Abstractions;
using GR.Notifications.Abstractions.Models.Notifications;
using IdentityServer4.EntityFramework.DbContexts;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GR.Core.Extensions;
using GR.Core.Helpers.Pagination;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GR.Identity.Roles.Razor.Controllers
{
    public class RolesController : BaseIdentityController<ApplicationDbContext, EntitiesDbContext, GearUser, GearRole, Tenant, INotify<GearRole>>
    {
        #region Injectable

        /// <summary>
        /// Inject configuration db context
        /// </summary>
        private ConfigurationDbContext ConfigurationDbContext { get; }

        /// <summary>
        /// Inject sign in manager
        /// </summary>
        private readonly SignInManager<GearUser> _signInManager;

        /// <summary>
        /// Inject logger
        /// </summary>
        private readonly ILogger<RolesController> _logger;

        /// <summary>
        /// Inject permission dataService
        /// </summary>
        private readonly IPermissionService _permissionService;

        /// <summary>
        /// Inject user manager
        /// </summary>
        private readonly IUserManager<GearUser> _userManager;

        #endregion Injectable

        public RolesController(UserManager<GearUser> userManager, RoleManager<GearRole> roleManager, ApplicationDbContext applicationDbContext, EntitiesDbContext context, INotify<GearRole> notify, SignInManager<GearUser> signInManager, ILogger<RolesController> logger, IPermissionService permissionService, ConfigurationDbContext configurationDbContext, IUserManager<GearUser> userManager1) : base(userManager, roleManager, applicationDbContext, context, notify)
        {
            _signInManager = signInManager;
            _logger = logger;
            _permissionService = permissionService;
            ConfigurationDbContext = configurationDbContext;
            _userManager = userManager1;
        }

        /// <summary>
        /// RoleProfile / Add
        /// </summary>
        /// <returns></returns>
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmCreateRole)]
        public async Task<IActionResult> Create()
        {
            var model = new CreateRoleViewModel
            {
                Profiles = await ApplicationDbContext.Profiles.Where(x => x.IsDeleted == false).AsNoTracking().ToListAsync(),
                Clients = await ConfigurationDbContext.Clients.AsNoTracking().ToListAsync()
            };

            return View(model);
        }

        /// <summary>
        ///     Create role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmCreateRole)]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Profiles = await ApplicationDbContext.Profiles.AsNoTracking().Where(x => x.IsDeleted == false).ToListAsync();
                model.Clients = await ConfigurationDbContext.Clients.AsNoTracking().ToListAsync();
                return View(model);
            }

            if (ApplicationRoleExists(model.Name))
            {
                model.Profiles = await ApplicationDbContext.Profiles.AsNoTracking().Where(x => x.IsDeleted == false).ToListAsync();
                model.Clients = await ConfigurationDbContext.Clients.AsNoTracking().ToListAsync();
                ModelState.AddModelError("", "Role with same name exist!");
                return View(model);
            }

            var applicationRole = new GearRole
            {
                Name = model.Name,
                Title = model.Title,
                IsDeleted = model.IsDeleted,
                Created = DateTime.Now,
                Changed = DateTime.Now,
                Author = User.Identity.Name,
                ClientId = model.ClientId,
                TenantId = CurrentUserTenantId
            };
            var result = await RoleManager.CreateAsync(applicationRole);
            var user = await _signInManager.UserManager.GetUserAsync(User);
            var client = ConfigurationDbContext.Clients.AsNoTracking().FirstOrDefault(x => x.Id.Equals(model.ClientId))
                ?.ClientName;
            await Notify.SendNotificationAsync(new Notification
            {
                Content = $"{user?.UserName} created the role {applicationRole.Name} for {client}",
                Subject = "Info",
                NotificationTypeId = NotificationType.Info
            });
            if (!result.Succeeded)
            {
                model.Profiles = await ApplicationDbContext.Profiles.AsNoTracking().Where(x => x.IsDeleted == false).ToListAsync();
                model.Clients = await ConfigurationDbContext.Clients.AsNoTracking().ToListAsync();
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            else
            {
                var role = await ApplicationDbContext.Roles.AsNoTracking().SingleOrDefaultAsync(m => m.Name == model.Name);
                if (role == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (model.SelectedProfileId != null && model.SelectedProfileId.Any())
                {
                    var listOfRoles = new List<RoleProfile>();
                    foreach (var _ in model.SelectedProfileId)
                    {
                        var newRoleProfile = new RoleProfile
                        {
                            ApplicationRoleId = role.Id,
                            ProfileId = Guid.Parse(_)
                        };
                        listOfRoles.Add(newRoleProfile);
                    }

                    await ApplicationDbContext.RoleProfiles.AddRangeAsync(listOfRoles);
                }
                else
                {
                    //Todo: Modify later !
                    var profile = ApplicationDbContext.Profiles.FirstOrDefault();
                    if (profile != null)
                    {
                        var newRoleProfile = new RoleProfile
                        {
                            ApplicationRoleId = role.Id,
                            ProfileId = profile.Id
                        };
                        await ApplicationDbContext.RoleProfiles.AddAsync(newRoleProfile);
                    }
                }

                if (model.SelectedPermissionId.Any())
                {
                    var listOfRolePermission = new List<RolePermission>();
                    foreach (var _ in model.SelectedPermissionId)
                    {
                        var permission = await ApplicationDbContext.Permissions.AsNoTracking()
                            .SingleOrDefaultAsync(x => x.Id == Guid.Parse(_));
                        if (permission != null)
                        {
                            listOfRolePermission.Add(new RolePermission
                            {
                                Author = User.Identity.Name,
                                PermissionCode = permission.PermissionKey,
                                PermissionId = permission.Id,
                                RoleId = role.Id
                            });
                        }
                    }

                    await ApplicationDbContext.RolePermissions.AddRangeAsync(listOfRolePermission);
                }

                try
                {
                    await ApplicationDbContext.SaveChangesAsync();
                    await _permissionService.RefreshCacheByRoleAsync(applicationRole.Name);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        /// <summary>
        /// POST: Roles/Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmDeleteRole)]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return Json(new { message = "Not found!", success = false });
            }

            var applicationRole = await RoleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return Json(new { message = "Role not found!", success = false });
            }

            if (applicationRole.IsNoEditable)
            {
                return Json(new { message = "Is system role!", success = false });
            }

            if (ApplicationDbContext.UserRoles.Any(x => x.RoleId == id))
            {
                return Json(new { message = "Role is used!", success = false });
            }

            var roleProfilesList = ApplicationDbContext.RoleProfiles.AsNoTracking()
                .Where(x => x.ApplicationRoleId.Equals(applicationRole.Id));
            var rolePermissionsList =
                ApplicationDbContext.RolePermissions.AsNoTracking().Where(x => x.RoleId.Equals(applicationRole.Id));
            if (await rolePermissionsList.AnyAsync())
            {
                try
                {
                    ApplicationDbContext.RolePermissions.RemoveRange(rolePermissionsList);
                    await ApplicationDbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    return Json(new { message = "Error on delete role permissions!", success = false });
                }
            }

            if (await roleProfilesList.AnyAsync())
            {
                try
                {
                    ApplicationDbContext.RoleProfiles.RemoveRange(roleProfilesList);
                    await ApplicationDbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    return Json(new { message = "Error on delete role profiles!", success = false });
                }
            }

            try
            {
                await RoleManager.DeleteAsync(applicationRole);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Json(new { message = "Error on delete role !", success = false });
            }

            await Notify.SendNotificationToSystemAdminsAsync(new Notification
            {
                Content = $"{User.Identity.Name} deleted the role {applicationRole.Name}",
                Subject = "Info",
                NotificationTypeId = NotificationType.Info
            });
            await _permissionService.RefreshCacheByRoleAsync(applicationRole.Name, true);
            return Json(new { message = "Role was delete with success!", success = true });
        }

        /// <summary>
        /// GET: Roles/Edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmEditRole)]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationRole = await RoleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            var roleProfilesId = await ApplicationDbContext.Set<RoleProfile>().Where(x => x.ApplicationRoleId == applicationRole.Id)
                .Select(x => x.ProfileId.ToString()).ToListAsync();
            var rolePermissionId = await ApplicationDbContext.Set<RolePermission>().Where(x => x.RoleId == id)
                .Select(x => x.PermissionId.ToString()).ToListAsync();

            var model = new UpdateRoleViewModel
            {
                Profiles = ApplicationDbContext.Profiles.Where(x => !x.IsDeleted).ToList(),
                Id = applicationRole.Id,
                ClientName = ConfigurationDbContext.Clients.FirstOrDefault(x => x.Id.Equals(applicationRole.ClientId))
                    ?.ClientName,
                Name = applicationRole.Name,
                SelectedProfileId = roleProfilesId,
                Title = applicationRole.Title,
                IsDeleted = applicationRole.IsDeleted,
                IsNoEditable = applicationRole.IsNoEditable,
                PermissionsList = await ApplicationDbContext.Permissions.AsNoTracking()
                    .Where(x => x.ClientId == applicationRole.ClientId).ToListAsync(),
                SelectedPermissionId = rolePermissionId,
                TenantId = applicationRole.TenantId
            };
            return View(model);
        }

        /// <summary>
        /// POST: Roles/Edit
        /// </summary>
        /// <param name="hub"></param>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmEditRole)]
        public async Task<IActionResult> Edit([FromServices] INotificationHub hub, string id, UpdateRoleViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var applicationRole = await RoleManager.FindByIdAsync(id);
            applicationRole.Name = model.Name;
            applicationRole.Title = model.Title;
            applicationRole.IsNoEditable = model.IsNoEditable;
            applicationRole.IsDeleted = model.IsDeleted;
            applicationRole.ModifiedBy = User.Identity.Name;
            applicationRole.Changed = DateTime.Now;
            applicationRole.TenantId = model.TenantId;

            model.Profiles = ApplicationDbContext.Profiles.Where(x => x.IsDeleted == false);
            model.PermissionsList =
                await ApplicationDbContext.Permissions.Where(x => x.ClientId == applicationRole.ClientId).ToListAsync();
            var result = await RoleManager.UpdateAsync(applicationRole);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                var roleProfilesId = ApplicationDbContext.RoleProfiles.Where(x => x.ApplicationRoleId == applicationRole.Id);
                try
                {
                    ApplicationDbContext.RoleProfiles.RemoveRange(roleProfilesId);
                    await ApplicationDbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    ModelState.AddModelError("", "Error on delete role profiles!");
                    return View(model);
                }

                var role = await ApplicationDbContext.Roles.SingleOrDefaultAsync(m => m.Name == model.Name);
                if (role == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (model.SelectedProfileId != null)
                {
                    var roleProfileList = new List<RoleProfile>();
                    foreach (var _ in model.SelectedProfileId)
                    {
                        roleProfileList.Add(new RoleProfile
                        {
                            ApplicationRoleId = role.Id,
                            ProfileId = Guid.Parse(_)
                        });
                    }

                    try
                    {
                        await ApplicationDbContext.AddRangeAsync(roleProfileList);
                        await ApplicationDbContext.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                        ModelState.AddModelError("", "Error on add role profile!");
                        return View(model);
                    }
                }

                //Delete previous permissions
                var rolePermissionId =
                    ApplicationDbContext.RolePermissions.Where(x => x.RoleId == applicationRole.Id);
                if (await rolePermissionId.AnyAsync())
                {
                    ApplicationDbContext.RolePermissions.RemoveRange(rolePermissionId);
                    await ApplicationDbContext.SaveChangesAsync();
                }

                var rolePermissionList = new List<RolePermission>();
                foreach (var _ in model.SelectedPermissionId)
                {
                    var permission = await ApplicationDbContext.Permissions.SingleOrDefaultAsync(x => x.Id == Guid.Parse(_));
                    if (permission == null) continue;
                    rolePermissionList.Add(new RolePermission
                    {
                        PermissionCode = permission.PermissionKey,
                        RoleId = id,
                        PermissionId = permission.Id
                    });
                }

                var user = await _signInManager.UserManager.GetUserAsync(User);

                try
                {
                    await ApplicationDbContext.RolePermissions.AddRangeAsync(rolePermissionList);
                    await ApplicationDbContext.SaveChangesAsync();
                    await _permissionService.RefreshCacheByRoleAsync(applicationRole.Name);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    ModelState.AddModelError("", "Error on add role permission!");
                    return View(model);
                }

                //var onlineUsers = hub.GetOnlineUsers();
                //await User.RefreshOnlineUsersClaims(Context, _signInManager, onlineUsers);
                await Notify.SendNotificationToSystemAdminsAsync(new Notification
                {
                    Content = $"{user.UserName} edited the role {applicationRole.Name}",
                    Subject = "Info",
                    NotificationTypeId = NotificationType.Info
                });
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        /// <summary>
        /// Get list of roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmReadRole)]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get application roles list
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ApplicationRolesList(DTParameters param)
        {
            var filtered = GetApplicationRoleFiltered(param.Search.Value, param.SortOrder, param.Start, param.Length,
                out var totalCount);
            var list = filtered.Adapt<List<RoleListViewModel>>();
            foreach (var role in list)
            {
                role.ClientName = ConfigurationDbContext.Clients.FirstOrDefault(x => x.Id.Equals(role.ClientId))
                    ?.ClientName;
            }

            var finalResult = new DTResult<RoleListViewModel>
            {
                Draw = param.Draw,
                Data = list,
                RecordsFiltered = totalCount,
                RecordsTotal = filtered.Count
            };

            return Json(finalResult);
        }

        /// <summary>
        /// Get application role filtered
        /// </summary>
        /// <param name="search"></param>
        /// <param name="sortOrder"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        private List<GearRole> GetApplicationRoleFiltered(string search, string sortOrder, int start, int length,
            out int totalCount)
        {
            var result = ApplicationDbContext.Roles.Where(p =>
                search == null || p.Name != null &&
                p.Name.ToLower().Contains(search.ToLower()) || p.Author != null &&
                p.Author.ToLower().Contains(search.ToLower()) || p.ModifiedBy != null &&
                p.ModifiedBy.ToString().ToLower().Contains(search.ToLower()) || p.Created != null &&
                p.Created.ToString(CultureInfo.InvariantCulture).ToLower().Contains(search.ToLower())).ToList();
            totalCount = result.Count;

            result = result.Skip(start).Take(length).ToList();
            switch (sortOrder)
            {
                case "name":
                    result = result.OrderBy(a => a.Name).ToList();
                    break;

                case "created":
                    result = result.OrderBy(a => a.Created).ToList();
                    break;

                case "author":
                    result = result.OrderBy(a => a.Author).ToList();
                    break;

                case "modifiedBy":
                    result = result.OrderBy(a => a.ModifiedBy).ToList();
                    break;

                case "changed":
                    result = result.OrderBy(a => a.Changed).ToList();
                    break;

                case "isDeleted":
                    result = result.OrderBy(a => a.IsDeleted).ToList();
                    break;

                case "name DESC":
                    result = result.OrderByDescending(a => a.Name).ToList();
                    break;

                case "created DESC":
                    result = result.OrderByDescending(a => a.Created).ToList();
                    break;

                case "author DESC":
                    result = result.OrderByDescending(a => a.Author).ToList();
                    break;

                case "modifiedBy DESC":
                    result = result.OrderByDescending(a => a.ModifiedBy).ToList();
                    break;

                case "changed DESC":
                    result = result.OrderByDescending(a => a.Changed).ToList();
                    break;

                case "isDeleted DESC":
                    result = result.OrderByDescending(a => a.IsDeleted).ToList();
                    break;

                default:
                    result = result.AsQueryable().ToList();
                    break;
            }

            return result.ToList();
        }

        /// <summary>
        /// Check if Application Role exists
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool ApplicationRoleExists(string name)
        {
            return ApplicationDbContext.Roles.Any(e => e.Name == name);
        }

        /// <summary>
        /// Check role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> CheckRoleName(string roleName)
        {
            if (roleName == null)
            {
                return Json(null);
            }

            var result = await ApplicationDbContext.Roles.FirstOrDefaultAsync(x => x.Name == roleName);
            return Json(result != null);
        }

        /// <summary>
        /// Get permissions by client
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetPermissionsByClient(int? id)
        {
            if (!id.HasValue)
            {
                return Json(true);
            }

            return Json(ApplicationDbContext.Permissions.Where(x => x.ClientId == id).Select(x => new
            {
                x.Id,
                x.PermissionName
            }).ToList());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [Route("api/[controller]/[action]")]
        [HttpGet]
        [Authorize(Roles = GlobalResources.Roles.ADMINISTRATOR)]
        public async Task<IActionResult> RefreshCachedPermissionsForEachRole()
        {
            await _permissionService.SetOrResetPermissionsOnCacheAsync();
            return StatusCode(200);
        }

        /// <summary>
        /// Get users in role for current company
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<SampleGetUserViewModel>>))]
        [Route("api/[controller]/[action]")]
        public async Task<JsonResult> GetUsersInRoleForCurrentCompany([Required] string roleName) => Json(await _userManager.GetUsersInRoleForCurrentCompanyAsync(roleName));

        /// <summary>
        /// Change user roles
        /// </summary>
        /// <returns></returns>
        [Roles(GlobalResources.Roles.ADMINISTRATOR, "Company Administrator")]
        [HttpPost]
        [Produces("application/json", Type = typeof(ResultModel))]
        [Route("api/[controller]/[action]")]
        public async Task<JsonResult> ChangeUserRoles(Guid? userId, IEnumerable<Guid> roles)
            => await JsonAsync(_userManager.ChangeUserRolesAsync(userId, roles));

        /// <summary>
        /// Get current user roles
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<SampleGetUserViewModel>>))]
        [Route("api/[controller]/[action]")]
        public async Task<JsonResult> GetUserRoles([Required]string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null) return Json(new NotFoundResultModel());
            var roles = (await _userManager.GetUserRolesAsync(user)).Select(x => new BaseRoleViewModel
            {
                Id = x.Id,
                Name = x.Name
            });

            return Json(new SuccessResultModel<IEnumerable<BaseRoleViewModel>>(roles));
        }

        /// <summary>
        /// Get  user roles
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json", Type = typeof(ResultModel<Dictionary<string, object>>))]
        [Route("api/[controller]/[action]")]
        public async Task<JsonResult> GetUserRolesWithAllRoles([Required]string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null) return Json(new NotFoundResultModel());
            var roles = (await _userManager.GetUserRolesAsync(user)).Select(x => new BaseRoleViewModel
            {
                Id = x.Id,
                Name = x.Name
            });

            var dict = new Dictionary<string, object>
            {
                { "UserRoles", roles },
                { "AllRoles",  RoleManager.Roles.Select(x => new BaseRoleViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })}
            };

            return Json(new SuccessResultModel<Dictionary<string, object>>(dict));
        }


        /// <summary>
        /// Get role by id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<UpdateRoleViewModel>))]
        [Route(DefaultApiRouteTemplate)]
        public async Task<JsonResult> GetRolesById(string  roleId)
        {

            if (string.IsNullOrEmpty(roleId))
                return Json(new InvalidParametersResultModel());

            //var role = await ApplicationDbContext.Roles
            //    .FirstOrDefaultAsync(i => i.Id.Equals(roleId));

            var applicationRole = await RoleManager.FindByIdAsync(roleId);
            if (applicationRole == null)
            {
                return Json(new NotFoundResult());
            }

            var roleProfilesId = await ApplicationDbContext.Set<RoleProfile>().Where(x => x.ApplicationRoleId == applicationRole.Id)
                .Select(x => x.ProfileId.ToString()).ToListAsync();
            var rolePermissionId = await ApplicationDbContext.Set<RolePermission>().Where(x => x.RoleId == roleId)
                .Select(x => x.PermissionId.ToString()).ToListAsync();

            var model = new UpdateRoleViewModel
            {
                Profiles = ApplicationDbContext.Profiles.Where(x => !x.IsDeleted).ToList(),
                Id = applicationRole.Id,
                ClientName = ConfigurationDbContext.Clients.FirstOrDefault(x => x.Id.Equals(applicationRole.ClientId))
                    ?.ClientName,
                Name = applicationRole.Name,
                SelectedProfileId = roleProfilesId,
                Title = applicationRole.Title,
                IsDeleted = applicationRole.IsDeleted,
                IsNoEditable = applicationRole.IsNoEditable,
                PermissionsList = await ApplicationDbContext.Permissions.AsNoTracking()
                    .Where(x => x.ClientId == applicationRole.ClientId &&  rolePermissionId.Contains(x.Id.ToString())).ToListAsync(),
                SelectedPermissionId = rolePermissionId,
                TenantId = applicationRole.TenantId
            };



            return model == null ? Json(new NotFoundResult()) : Json(new SuccessResultModel<UpdateRoleViewModel>(model));
        }


        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<SampleGetUserViewModel>>))]
        [Route("api/[controller]/[action]")]
        public JsonResult GetAllRolesAsync()
            => Json(new SuccessResultModel<IEnumerable<BaseRoleViewModel>>(RoleManager.Roles.Select(x => new BaseRoleViewModel
            {
                Id = x.Id,
                Name = x.Name
            })));

        /// <summary>
        /// Get all paginated roles
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<PagedResult<GearRole>>))]
        [Route(DefaultApiRouteTemplate)]
        public async Task<JsonResult> GetAllPaginatedRoles(PageRequest request)
        {
            var roles = await ApplicationDbContext.Roles
                .Where(i => !i.IsDeleted || request.IncludeDeleted)
                .GetPagedAsync(request);
            return Json(new SuccessResultModel<PagedResult<GearRole>>(roles));
        }

        /// <summary>
        /// Add role 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<string>))]
        [Route(DefaultApiRouteTemplate)]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmCreateRole)]
        public async Task<JsonResult> AddRole(CreateRoleViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return JsonModelStateErrors();
            }

            if (ApplicationRoleExists(model.Name))
            {
                ModelState.AddModelError("", "Role with same name exist!");
                return JsonModelStateErrors();
            }

            var applicationRole = new GearRole
            {
                Name = model.Name,
                Title = model.Title,
                IsDeleted = model.IsDeleted,
                Created = DateTime.Now,
                Changed = DateTime.Now,
                Author = User.Identity.Name,
                ClientId = model.ClientId,
                TenantId = CurrentUserTenantId
            };



            var result = await RoleManager.CreateAsync(applicationRole);
            var user = await _signInManager.UserManager.GetUserAsync(User);
            var client = ConfigurationDbContext.Clients.AsNoTracking().FirstOrDefault(x => x.Id.Equals(model.ClientId))
                ?.ClientName;
            await Notify.SendNotificationAsync(new Notification
            {
                Content = $"{user?.UserName} created the role {applicationRole.Name} for {client}",
                Subject = "Info",
                NotificationTypeId = NotificationType.Info
            });

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return JsonModelStateErrors();
            }



            var role = await ApplicationDbContext.Roles.AsNoTracking().SingleOrDefaultAsync(m => m.Name == model.Name);
            if (role == null)
            {
                return Json(new NotFoundResultModel<string>());
            }

            if (model.SelectedProfileId != null && model.SelectedProfileId.Any())
            {
                var listOfRoles = new List<RoleProfile>();
                foreach (var _ in model.SelectedProfileId)
                {
                    var newRoleProfile = new RoleProfile
                    {
                        ApplicationRoleId = role.Id,
                        ProfileId = Guid.Parse(_)
                    };
                    listOfRoles.Add(newRoleProfile);
                }

                await ApplicationDbContext.RoleProfiles.AddRangeAsync(listOfRoles);
            }
            else
            {
                //Todo: Modify later !
                var profile = ApplicationDbContext.Profiles.FirstOrDefault();
                if (profile != null)
                {
                    var newRoleProfile = new RoleProfile
                    {
                        ApplicationRoleId = role.Id,
                        ProfileId = profile.Id
                    };
                    await ApplicationDbContext.RoleProfiles.AddAsync(newRoleProfile);
                }
            }

            if (model.SelectedProfileId != null && model.SelectedPermissionId.Any())
            {
                var listOfRolePermission = new List<RolePermission>();
                foreach (var _ in model.SelectedPermissionId)
                {
                    var permission = await ApplicationDbContext.Permissions.AsNoTracking()
                        .SingleOrDefaultAsync(x => x.Id == Guid.Parse(_));
                    if (permission != null)
                    {
                        listOfRolePermission.Add(new RolePermission
                        {
                            Author = User.Identity.Name,
                            PermissionCode = permission.PermissionKey,
                            PermissionId = permission.Id,
                            RoleId = role.Id
                        });
                    }
                }

                await ApplicationDbContext.RolePermissions.AddRangeAsync(listOfRolePermission);
            }

            try
            {
                await ApplicationDbContext.SaveChangesAsync();
                await _permissionService.RefreshCacheByRoleAsync(applicationRole.Name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


            return Json(new SuccessResultModel<string>(applicationRole.Id));
        }

        /// <summary>
        /// Get permission by client id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<SelectListItem>>))]
        [Route(DefaultApiRouteTemplate)]
        public async Task<JsonResult> GetPermissionsByClientId(int? id)
        {
            if (!id.HasValue)
            {
                return Json(new InvalidParametersResultModel());
            }
            var result = await ApplicationDbContext.Permissions
                .Where(x => x.ClientId == id)
                .Select(s => new SelectListItem
                {
                    Text = s.PermissionName,
                    Value = s.Id.ToString(),
                }).ToListAsync();

            return Json(new SuccessResultModel<IEnumerable<SelectListItem>>(result));
        }

        /// <summary>
        /// Get all clients
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<SelectListItem>>))]
        [Route(DefaultApiRouteTemplate)]
        public async Task<JsonResult> GetClients()
        {
            var result = await ConfigurationDbContext.Clients.AsNoTracking()
               .Select(s => new SelectListItem
               {
                   Text = s.ClientName,
                   Value = s.Id.ToString(),
               }).ToListAsync();

            return Json(new SuccessResultModel<IEnumerable<SelectListItem>>(result));
        }


        /// <summary>
        /// Edit role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [Route(DefaultApiRouteTemplate)]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmEditRole)]
        public async Task<JsonResult> EditRole(ApiUpdateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonModelStateErrors();
            }

            var applicationRole = await RoleManager.FindByIdAsync(model.Id);
            applicationRole.Name = model.Name;
            applicationRole.Title = model.Title;
            applicationRole.Changed = DateTime.Now;

            model.PermissionsList =
                await ApplicationDbContext.Permissions.Where(x => x.ClientId == applicationRole.ClientId).ToListAsync();

            var result = await RoleManager.UpdateAsync(applicationRole);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
              
                //Delete previous permissions
                var rolePermissionId =
                    ApplicationDbContext.RolePermissions.Where(x => x.RoleId == applicationRole.Id);
                if (await rolePermissionId.AnyAsync())
                {
                    ApplicationDbContext.RolePermissions.RemoveRange(rolePermissionId);
                    await ApplicationDbContext.SaveChangesAsync();
                }

                var rolePermissionList = new List<RolePermission>();
                if (model.permissions != null )
                {
                    foreach (var _ in model.permissions)
                    {
                        var permission =
                            await ApplicationDbContext.Permissions.SingleOrDefaultAsync(x => x.Id == Guid.Parse(_));
                        if (permission == null) continue;
                        rolePermissionList.Add(new RolePermission
                        {
                            PermissionCode = permission.PermissionKey,
                            RoleId = model.Id,
                            PermissionId = permission.Id
                        });
                    }
                }

                var user = await _signInManager.UserManager.GetUserAsync(User);

                try
                {
                    await ApplicationDbContext.RolePermissions.AddRangeAsync(rolePermissionList);
                    await ApplicationDbContext.SaveChangesAsync();
                    await _permissionService.RefreshCacheByRoleAsync(applicationRole.Name);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    ModelState.AddModelError("", "Error on add role permission!");
                    return JsonModelStateErrors();
                }

                //var onlineUsers = hub.GetOnlineUsers();
                //await User.RefreshOnlineUsersClaims(Context, _signInManager, onlineUsers);
                await Notify.SendNotificationToSystemAdminsAsync(new Notification
                {
                    Content = $"{user.UserName} edited the role {applicationRole.Name}",
                    Subject = "Info",
                    NotificationTypeId = NotificationType.Info
                });
                return Json(new ResultModel { IsSuccess = true });
            }

            return Json(new ResultModel { IsSuccess = true });
        }

        /// <summary>
        /// Deactivate role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [Route(DefaultApiRouteTemplate)]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmDeleteRole)]
        public async Task<JsonResult> DeactivateRole(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Json(new InvalidParametersResultModel());


            var role = await ApplicationDbContext.Roles
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (role == null)
                return Json(new NotFoundResultModel());

            role.IsDeleted = true;

            var usersInRole = await UserManager.GetUsersInRoleAsync(role.Name);

            if (usersInRole.Any() || role.IsNoEditable)
                return Json(new ResultModel
                {
                    IsSuccess = false,
                    Errors = new List<IErrorModel> { new ErrorModel { Message = "Exist user in this role" } }
                });


            ApplicationDbContext.Roles.Update(role);


            return await JsonAsync(ApplicationDbContext.PushAsync());
        }

        /// <summary>
        /// Activate role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [Route(DefaultApiRouteTemplate)]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmDeleteRole)]
        public async Task<JsonResult> ActivateRole(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Json(new InvalidParametersResultModel());


            var role = await ApplicationDbContext.Roles
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (role == null)
                return Json(new NotFoundResultModel());

            role.IsDeleted = false;
            ApplicationDbContext.Roles.Update(role);

            return await JsonAsync(ApplicationDbContext.PushAsync());
        }

        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [Route(DefaultApiRouteTemplate)]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmDeleteRole)]
        public async Task<JsonResult> DeleteRole(string id)
        {


            if (id == null)
            {
                return Json(new InvalidParametersResultModel());
            }

            var applicationRole = await RoleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return Json(new NotFoundResultModel());
            }

            if (applicationRole.IsNoEditable)
            {
                return Json(new ResultModel() { Errors = new List<IErrorModel>{new ErrorModel{Message = "Is system role!" } } , IsSuccess = false });
            }

            if (ApplicationDbContext.UserRoles.Any(x => x.RoleId == id))
            {
                return Json(new ResultModel() { Errors = new List<IErrorModel> { new ErrorModel { Message = "Role is used!" } }, IsSuccess = false });
            }

            var roleProfilesList = ApplicationDbContext.RoleProfiles.AsNoTracking()
                .Where(x => x.ApplicationRoleId.Equals(applicationRole.Id));
            var rolePermissionsList =
                ApplicationDbContext.RolePermissions.AsNoTracking().Where(x => x.RoleId.Equals(applicationRole.Id));
            if (await rolePermissionsList.AnyAsync())
            {
                try
                {
                    ApplicationDbContext.RolePermissions.RemoveRange(rolePermissionsList);
                    await ApplicationDbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                     return Json(new ResultModel() { Errors = new List<IErrorModel> { new ErrorModel { Message = "Error on delete role permissions!" } }, IsSuccess = false });
                }
            }

            if (await roleProfilesList.AnyAsync())
            {
                try
                {
                    ApplicationDbContext.RoleProfiles.RemoveRange(roleProfilesList);
                    await ApplicationDbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    return Json(new ResultModel() { Errors = new List<IErrorModel> { new ErrorModel { Message = "Error on delete role profiles!" } }, IsSuccess = false });
                }
            }

            try
            {
                await RoleManager.DeleteAsync(applicationRole);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Json(new ResultModel() { Errors = new List<IErrorModel> { new ErrorModel { Message = "Error on delete role !" } }, IsSuccess = false });
            }

            await Notify.SendNotificationToSystemAdminsAsync(new Notification
            {
                Content = $"{User.Identity.Name} deleted the role {applicationRole.Name}",
                Subject = "Info",
                NotificationTypeId = NotificationType.Info
            });
            await _permissionService.RefreshCacheByRoleAsync(applicationRole.Name, true);
            return Json(new  ResultModel{ IsSuccess = true });


            //if (string.IsNullOrEmpty(id))
            //    return Json(new InvalidParametersResultModel());


            //var role = await ApplicationDbContext.Roles
            //    .FirstOrDefaultAsync(x => x.Id.Equals(id));

            //if (role == null)
            //    return Json(new NotFoundResultModel());


            //var usersInRole = await UserManager.GetUsersInRoleAsync(role.Name);

            //if (usersInRole.Any() || role.IsNoEditable)
            //    return Json(new ResultModel
            //    {
            //        IsSuccess = false,
            //        Errors = new List<IErrorModel> { new ErrorModel { Message = "Exist user in this role" } }
            //    });


            //ApplicationDbContext.Roles.Remove(role);


            //return await JsonAsync(ApplicationDbContext.PushAsync());
        }
    }
}