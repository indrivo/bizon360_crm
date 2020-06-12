using AutoMapper;
using GR.Crm.Payments.Abstractions.Models;
using GR.Crm.Payments.Abstractions.ViewModels.PaymentsViewModels;

namespace GR.Crm.Payments.Abstractions.Helpers
{
    public class PaymentMapperProfile: Profile
    {

        public PaymentMapperProfile()
        {
            // Add payment
            CreateMap<Payment, AddPaymentViewModel>()
                .ForMember(o => o.DateTransaction, m => m.MapFrom(x => x.DateTransaction))
                .ForMember(o => o.TotalPrice, m => m.MapFrom(x => x.TotalPrice))
                .ForMember(o => o.TotalTVA, m => m.MapFrom(x => x.TotalTVA))
                .ForMember(o => o.TVA, m => m.MapFrom(x => x.TVA))
                .ForMember(o => o.TotalPriceWithoutTVA, m => m.MapFrom(x => x.TotalPriceWithoutTVA))
                .ForMember(o => o.UnitPriceWithoutTVA, m => m.MapFrom(x => x.UnitPriceWithoutTVA))
                .ForMember(o => o.Quantity, m => m.MapFrom(x => x.Quantity))
                .ForMember(o => o.Currency, m => m.MapFrom(x => x.Currency))
                .ForMember(o => o.PaymentDestination, m => m.MapFrom(x => x.PaymentDestination))
                .ForMember(o => o.DocumentNumber, m => m.MapFrom(x => x.DocumentNumber))
                .ReverseMap();

            //Map payment with get viewmodel
            CreateMap<PaymentMapped, GetPaymentViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

            // Add payment code
            CreateMap<PaymentCode, PaymentCodeViewModel>()
                .ForMember(o => o.Code, m => m.MapFrom(x => x.Code))
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ReverseMap();

            // Map  get payment code
            CreateMap<PaymentCode, GetPaymentCodeViewModel>()
                .IncludeAllDerived()
                .ReverseMap();
        }
    }
}
