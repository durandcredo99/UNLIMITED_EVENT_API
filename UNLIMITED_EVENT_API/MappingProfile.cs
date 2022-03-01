﻿using AutoMapper;
using Entities.DataTransfertObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UNLIMITED_EVENT_API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Source -> Target
            CreateMap<AppUser, AppUserResponse>()
                .ForMember(dest => dest.Email, src => src.MapFrom(src => src.UserName));

            CreateMap<AppUserRequest, AppUser>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(src => src.Email))
                /*.ForMember(dest => dest.Name, src => src.MapFrom(src => src.Name))*/;


            CreateMap<LoginRequest, LoginModel>();
            CreateMap<Authentication, Authentication>();

            CreateMap<CategoryBlog, CategoryBlogResponse>().ReverseMap();
            CreateMap<CategoryBlogRequest, CategoryBlog>().ReverseMap();

            CreateMap<Blog, BlogResponse>().ReverseMap();
            CreateMap<BlogRequest, Blog>().ReverseMap();

            CreateMap<Command, CommandResponse>().ReverseMap();
            CreateMap<CommandRequest, Command>().ReverseMap();

            CreateMap<Comment, CommentResponse>().ReverseMap();
            CreateMap<CommentRequest, Comment>().ReverseMap();

            CreateMap<Commercial, CommercialResponse>().ReverseMap();
            CreateMap<CommercialRequest, Commercial>().ReverseMap();

            CreateMap<Category, CategoryResponse>().ReverseMap();
            CreateMap<CategoryRequest, Category>().ReverseMap();

            CreateMap<Event, EventResponse>().ReverseMap();
            CreateMap<EventRequest, Event>().ReverseMap();

            CreateMap<Payment, PaymentResponse>().ReverseMap();
            CreateMap<PaymentRequest, Payment>().ReverseMap();

            CreateMap<PaymentType, PaymentTypeResponse>().ReverseMap();
            CreateMap<PaymentTypeRequest, PaymentType>().ReverseMap();

            CreateMap<Place, PlaceResponse>().ReverseMap();
            CreateMap<PlaceRequest, Place>().ReverseMap();

            CreateMap<Sponsor, SponsorResponse>().ReverseMap();
            CreateMap<SponsorRequest, Sponsor>().ReverseMap();

            CreateMap<Workstation, WorkstationResponse>().ReverseMap();
            CreateMap<WorkstationRequest, Workstation>().ReverseMap();

            CreateMap<Partner, PartnerResponse>().ReverseMap();
            CreateMap<PartnerRequest, Partner>().ReverseMap();

            CreateMap<Rate, RateResponse>().ReverseMap();
            CreateMap<RateRequest, Rate>().ReverseMap();
            //CreateMap<University, UniversityResponse>()
            //    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));




        }
    }
}
