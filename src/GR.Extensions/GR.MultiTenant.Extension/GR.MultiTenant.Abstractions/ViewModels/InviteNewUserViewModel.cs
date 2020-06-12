using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GR.MultiTenant.Abstractions.ViewModels
{
    public class InviteNewUserViewModel
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// User roles
        /// </summary>
        public IEnumerable<string> Roles { get; set; } = new List<string>();

        [Required]
        public virtual string FirstName { get; set; }

        [Required]
        public virtual string LastName { get; set; }
    }
}