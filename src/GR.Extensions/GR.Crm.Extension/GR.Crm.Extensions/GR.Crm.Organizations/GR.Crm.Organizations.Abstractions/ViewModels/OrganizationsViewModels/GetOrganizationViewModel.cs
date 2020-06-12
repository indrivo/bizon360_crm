using System;
using System.Collections.Generic;
using GR.Crm.Organizations.Abstractions.Models;

namespace GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels
{
   public  class GetOrganizationViewModel : Organization
    {
       public IEnumerable<Guid> ListWorkCategory { get; set; }
    }
}
