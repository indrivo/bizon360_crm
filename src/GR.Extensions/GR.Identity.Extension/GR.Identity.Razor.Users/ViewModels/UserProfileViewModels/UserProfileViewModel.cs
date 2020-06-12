using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GR.Audit.Abstractions.Attributes;
using GR.Audit.Abstractions.Enums;
using GR.Crm.Abstractions.Models;
using GR.Crm.Abstractions.ViewModels.JobPositionViewModels;
using GR.Identity.Abstractions;

namespace GR.Identity.Razor.Users.ViewModels.UserProfileViewModels
{
    public class UserProfileViewModel
    {
        public UserProfileViewModel()
        {
            Roles = new HashSet<string>();
            Groups = new HashSet<string>();
        }

        public string  Id { get; set; }

        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }

        [Display(Name = "User Name", Description = "user name", Prompt = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Email Address", Description = "email", Prompt = "email")]
        public string Email { get; set; }

        [MaxLength(50)]
        [Display(Name = "First name", Description = "first name", Prompt = "ex: John")]
        public string UserFirstName { get; set; }

        [MaxLength(50)]
        [Display(Name = "Last name", Description = "last name", Prompt = "ex: Doe")]
        public string UserLastName { get; set; }

        [MaxLength(20)]
        [Display(Name = "PHONE number", Description = "phone number ", Prompt = "0123456789")]
        public string UserPhoneNumber { get; set; }

        [Display(Name = "Birthday", Description = "birthday")]
        public DateTime Birthday { get; set; }

        [MaxLength(500)]
        [Display(Name = "About me", Description = "same description")]
        public string AboutMe { get; set; }


        /// <inheritdoc />
        /// <summary>
        /// Stores state of the Object. True if object is deleted and false otherwise
        /// </summary>
        [TrackField(Option = TrackFieldOption.Allow)]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Is disabled field status
        /// </summary>
        [TrackField(Option = TrackFieldOption.Allow)]
        public bool IsDisabled { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<GearRole> UserRoles { get; set; } = new List<GearRole>();

        public IEnumerable<string> UserRolesId { get; set; } = new List<string>();


        public IEnumerable<string> Groups { get; set; }

        public virtual Guid? JobPositionId { get; set; }
        public virtual JobPositionViewModel JobPosition { get; set; }

        public string PhoneNumber { get; set; }
    }
}