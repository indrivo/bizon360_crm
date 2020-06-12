using System;
using System.ComponentModel.DataAnnotations;
using GR.Core;
using GR.Crm.Organizations.Abstractions.ViewModels.WebProfilesViewModels;

namespace GR.Crm.Organizations.Abstractions.ViewModels.ContactsViewModels
{
    public class ContactWebProfileViewModel : BaseModel
    {
        /// <summary>
        /// Web profile Id
        /// </summary>
        [Required]
        public virtual Guid WebProfileId { get; set; }

        /// <summary>
        /// user name
        /// </summary>
        [Required]
        public virtual string UserName { get; set; }


        /// <summary>
        /// url
        /// </summary>
        [Required]
        public virtual string Url { get; set; }


        /// <summary>
        /// Contact id
        /// </summary>
        [Required]
        public virtual Guid ContactId { get; set; }

    }
}