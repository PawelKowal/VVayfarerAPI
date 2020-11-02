using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Dtos;
using AutoMapper;
using VVayfarerApi.Entities;

namespace VVayfarerApi.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserReadDto>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<User, UserUpdateDto>();
        }
    }
}
