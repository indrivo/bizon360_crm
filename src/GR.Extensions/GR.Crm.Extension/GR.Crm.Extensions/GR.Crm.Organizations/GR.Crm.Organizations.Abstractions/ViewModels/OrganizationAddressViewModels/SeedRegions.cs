using System;
using System.Collections.Generic;
using System.Text;
using GR.Crm.Organizations.Abstractions.Enums;

namespace GR.Crm.Organizations.Abstractions.ViewModels.OrganizationAddressViewModels
{
    public class SeedRegions
    {
        public virtual string Name { get; set; }
       
        public virtual IEnumerable<SeedCity>  Cities { get; set; } = new List<SeedCity>();
    }

    public class SeedCity
    {
        public virtual string Name { get; set; }
    }
}
