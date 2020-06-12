using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GR.Crm.Organizations.Abstractions.ViewModels.HelpersViewModels
{
    public class ListSelectorsForOrganization
    {
        public virtual IEnumerable<SelectListItem> ListEmployees { get; set; } = new List<SelectListItem>();

        public virtual IEnumerable<SelectListItem> ListIndustry { get; set; } = new List<SelectListItem>();

        public virtual IEnumerable<SelectListItem> WorkCategories { get; set; } = new List<SelectListItem>();
    }
}
