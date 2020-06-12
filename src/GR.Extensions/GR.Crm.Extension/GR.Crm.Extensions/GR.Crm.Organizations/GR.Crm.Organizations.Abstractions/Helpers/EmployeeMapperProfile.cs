using AutoMapper;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.EmployeesViewModels;

namespace GR.Crm.Organizations.Abstractions.Helpers
{
    public class EmployeeMapperProfile: Profile
    {

        public EmployeeMapperProfile()
        {
            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(o => o.Interval, m => m.MapFrom(x => x.Interval))
                .ReverseMap();


            CreateMap<Employee, GetEmployeeViewModel>()
                .IncludeAllDerived()
                .ReverseMap();
        }
    }
}
