using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Leads.Abstractions.ViewModels
{
    public sealed class OrderLeadStatesViewModel
    {
        [Required]
        public Guid StateId { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
