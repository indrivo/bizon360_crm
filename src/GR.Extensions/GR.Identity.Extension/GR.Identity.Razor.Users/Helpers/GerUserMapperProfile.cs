using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using GR.Identity.Abstractions;
using GR.Identity.Razor.Users.ViewModels.UserProfileViewModels;

namespace GR.Identity.Razor.Users.Helpers
{
    public class GerUserMapperProfile: Profile
    {

        public GerUserMapperProfile()
        {

            //Map create agreement 
            CreateMap<GearUser, UserProfileViewModel>()
                .ForMember(o => o.UserFirstName, m => m.MapFrom(x => x.UserFirstName))
                .ForMember(o => o.UserLastName, m => m.MapFrom(x => x.UserLastName))
                .ForMember(o => o.Id, m => m.MapFrom(x => x.Id))
                .ForMember(o => o.UserName, m => m.MapFrom(x => x.UserName))
                .ForMember(o => o.Email, m => m.MapFrom(x => x.Email))
                .ForMember(o => o.Birthday, m => m.MapFrom(x => x.Birthday))
                .ForMember(o => o.UserPhoneNumber, m => m.MapFrom(x => x.PhoneNumber))
                .ForMember(o => o.IsDeleted, m => m.MapFrom(x => x.IsDeleted))
                .ForMember(o => o.IsDisabled, m => m.MapFrom(x => x.IsDisabled))
                .ReverseMap();
        }
    }
}
