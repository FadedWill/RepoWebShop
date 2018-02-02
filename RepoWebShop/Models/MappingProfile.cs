﻿using AutoMapper;   
using Microsoft.AspNetCore.Identity;
using RepoWebShop.Extensions;
using RepoWebShop.ViewModels;
using System;
using System.Security.Claims;

namespace RepoWebShop.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ExternalLoginInfo, ApplicationUser>()
                .ForMember(x => x.UserName, opt =>
                {
                    if (opt.DestinationMember != null)
                        opt.UseDestinationValue();
                    else
                    {
                        opt.PreCondition(w => !string.IsNullOrEmpty(w.Principal.GetClaimValue(ClaimTypes.Email)));
                        opt.MapFrom(y => y.Principal.GetClaimValue(ClaimTypes.Email));
                    }
                })
                .ForMember(x => x.Email, opt =>
                {
                    opt.PreCondition(w => !string.IsNullOrEmpty(w.Principal.GetClaimValue(ClaimTypes.Email)));
                    opt.MapFrom(y => y.Principal.GetClaimValue(ClaimTypes.Email));
                })
                .ForMember(x => x.NameIdentifier, opt => opt.MapFrom(y => y.Principal.GetClaimValue(ClaimTypes.NameIdentifier)))
                .ForMember(x => x.EmailConfirmed, opt => opt.MapFrom(y => !string.IsNullOrEmpty(y.Principal.GetClaimValue(ClaimTypes.Email))))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(y => y.Principal.GetClaimValue(ClaimTypes.GivenName)))
                .ForMember(x => x.LastName, opt => opt.MapFrom(y => y.Principal.GetClaimValue(ClaimTypes.Surname)))
                .ForMember(x => x.AddressLine1, opt => opt.UseDestinationValue())
                //.ForMember(x => x.StreetName, opt => opt.MapFrom(y => y.Principal.GetClaimValue(ClaimTypes.StreetAddress)))
                //.ForMember(x => x.State, opt => opt.MapFrom(y => y.Principal.GetClaimValue(ClaimTypes.StateOrProvince)))
                //.ForMember(x => x.ZipCode, opt => opt.MapFrom(y => y.Principal.GetClaimValue(ClaimTypes.PostalCode)))
                //.ForMember(x => x.Country, opt => opt.MapFrom(y => y.Principal.GetClaimValue(ClaimTypes.Country)))
                //.ForMember(x => x.PhoneNumber, opt => opt.MapFrom(y => y.Principal.GetClaimValue(ClaimTypes.MobilePhone)))
                .ForMember(x => x.Gender, opt => opt.MapFrom(y => y.Principal.GetClaimValue(ClaimTypes.Gender)))
                .ForMember(x => x.DateOfBirth, opt =>
                {
                    opt.PreCondition(w =>
                    {
                        var date = w.Principal.GetClaimValue(ClaimTypes.DateOfBirth);
                        DateTime result;
                        return DateTime.TryParse(date, out result);
                    });
                    opt.MapFrom(z => DateTime.Parse(z.Principal.GetClaimValue(ClaimTypes.DateOfBirth)));
                });

            CreateMap<ApplicationUser, RegisterProviderWithMailViewModel>();
            CreateMap<RegisterProviderWithMailViewModel, ApplicationUser>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(y => y.Email));
            CreateMap<PieDetailCreateViewModel, PieDetail>();
            CreateMap<RegisterViewModel, ApplicationUser>();
            CreateMap<ApplicationUser, EmailValidationViewModel>();
            CreateMap<PieDetail, PieDetailCreateViewModel>();
            CreateMap<PieDetail, PieDetail>().ForMember(x => x.PieDetailId, opt => opt.Ignore());
            CreateMap<Order, OrderStatusViewModel>();
            CreateMap<PaymentNotice, Order>();
            CreateMap<IdentityUser, ApplicationUser>();
            CreateMap<WorkingHours, OpenHours>();
            CreateMap<WorkingHours, ProcessingHours>();
            CreateMap<ApplicationUser, ApplicationUserViewModel>();
            CreateMap<ApplicationUserViewModel, ApplicationUser>();
            CreateMap<AddSpecialDateViewModel, PublicHoliday>()
                .ForMember(x => x.OpenHours,
                    opt => opt.MapFrom(src => src.OpenHoursStartingAt.HasValue && src.OpenHoursFinishingAt.HasValue  ? 
                    new OpenHours()
                    {
                        StartingAt = src.OpenHoursStartingAt.Value,
                        Duration = src.OpenHoursFinishingAt.Value.Subtract(src.OpenHoursStartingAt.Value),
                        DayId = 8
                    } : null))
                .ForMember(x => x.ProcessingHours,
                    opt => opt.MapFrom(src => src.ProcessingHoursStartingAt.HasValue && src.ProcessingHoursFinishingAt.HasValue ?
                    new ProcessingHours()
                    {
                        StartingAt = src.ProcessingHoursStartingAt.Value,
                        Duration = src.ProcessingHoursFinishingAt.Value.Subtract(src.ProcessingHoursStartingAt.Value),
                        DayId = 8
                    } : null));
        }
    }
}
