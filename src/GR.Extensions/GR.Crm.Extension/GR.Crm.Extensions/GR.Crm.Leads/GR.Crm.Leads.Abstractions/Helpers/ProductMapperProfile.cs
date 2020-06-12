using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Leads.Abstractions.ViewModels.ProductsViewModels;

namespace GR.Crm.Leads.Abstractions.Helpers
{
    public class ProductMapperProfile: Profile
    {
        public ProductMapperProfile()
        {
            // Add product
            CreateMap<Product, AddProductViewModel>()
                .ForMember(o => o.Sku, m => m.MapFrom(x => x.Sku))
                .ForMember(o => o.BankAccount, m => m.MapFrom(x => x.BankAccount))
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.Price, m => m.MapFrom(x => x.Price))
                .ForMember(o => o.Vat, m => m.MapFrom(x => x.Vat))
                .ForMember(o => o.Commission, m => m.MapFrom(x => x.Commission))
                .ReverseMap();

            //Map agreement with get viewmodel
            CreateMap<Product, GetProductViewModel>()
                .IncludeAllDerived()
                .ReverseMap();
        }
    }
}
