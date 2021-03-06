using GR.Core;
using GR.Entities.Data;
using GR.Identity.Abstractions.Models.UserProfiles;
using GR.Identity.Data;
using GR.Identity.Razor.ViewModels.UserProfileViewModels;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GR.Core.BaseControllers;

namespace GR.Identity.Razor.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Profile Controller
    /// </summary>
    public class ProfileController : BaseGearController
    {
        private readonly EntitiesDbContext _contextEntities;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(ApplicationDbContext context,
            ILogger<ProfileController> logger,
            EntitiesDbContext contextEntities)
        {
            _contextEntities = contextEntities;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get list of profiles
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult OrderList(DTParameters param)
        {
            var filtered = GetOrderFiltered(param.Search.Value, param.SortOrder, param.Start, param.Length, out var totalCount);
            var finalResult = new DTResult<Profile>
            {
                Draw = param.Draw,
                Data = filtered.ToList(),
                RecordsFiltered = totalCount,
                RecordsTotal = filtered.Count
            };

            return Json(finalResult);
        }

        private List<Profile> GetOrderFiltered(string search, string sortOrder, int start, int length,
            out int totalCount)
        {
            var result = _context.Profiles.Where(p =>
                search == null || p.ProfileName != null &&
                p.ProfileName.ToLower().Contains(search.ToLower()) || p.ModifiedBy != null &&
                p.ModifiedBy.ToLower().Contains(search.ToLower()) || p.Author != null &&
                p.Author.ToString().ToLower().Contains(search.ToLower()) || p.Id != null &&
                p.Id.ToString().ToLower().Contains(search.ToLower())).ToList();
            totalCount = result.Count;

            result = result.Skip(start).Take(length).ToList();
            switch (sortOrder)
            {
                case "id":
                    result = result.OrderBy(a => a.Id).ToList();
                    break;

                case "profileName":
                    result = result.OrderBy(a => a.ProfileName).ToList();
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

                case "created":
                    result = result.OrderBy(a => a.Created).ToList();
                    break;

                case "isDeleted":
                    result = result.OrderBy(a => a.IsDeleted).ToList();
                    break;

                case "id DESC":
                    result = result.OrderByDescending(a => a.Id).ToList();
                    break;

                case "profileName DESC":
                    result = result.OrderByDescending(a => a.ProfileName).ToList();
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

                case "created DESC":
                    result = result.OrderByDescending(a => a.Created).ToList();
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
        /// Get view for Create a profiles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateProfileViewModel
            {
                Entities = _contextEntities.Table.AsEnumerable().Where(x => x.EntityType == "Profile")
            };
            return View(model);
        }

        /// <summary>
        /// Create a profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(CreateProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _context.Profiles.Add(model.Adapt<Profile>());
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), "Profile");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("fail", e.Message);
            }

            return View(model);
        }

        /// <summary>
        /// Get profile by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (id == null) return NotFound();
            var model = _context.Profiles.FirstOrDefault(x => x.Id == Guid.Parse(id));
            return View(model);
        }

        /// <summary>
        /// Save profile data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(Profile model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                _context.Profiles.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), "Profile");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return View(model);
        }

        /// <summary>
        /// Confirm Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            if (!id.HasValue)
            {
                return Json(false);
            }

            var isUsed = await _context.RoleProfiles.AnyAsync(x => x.ProfileId == id);
            if (isUsed)
            {
                return Json(false);
            }

            var group = await _context.Profiles.Where(x => x.Id == id).ToListAsync();
            if (!group.Any())
            {
                return Json(false);
            }

            try
            {
                _context.RemoveRange(group);
                _context.SaveChanges();
                return Json(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Json(false);
            }
        }
    }
}