using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Leads.Abstractions.ViewModels.AgreementsViewModels
{
    public class GetTableAgreementViewModel
    {
        public virtual Guid Id { get; set; }

        public virtual DateTime Created { get; set; }

        public virtual string Name { get; set; }

        public virtual string OrganizationName { get; set; }

        public virtual string LeadName { get; set; }

        public virtual string ProductName { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual string UserName { get; set; }

        public virtual Guid UserId { get; set; }
        
    }
}
