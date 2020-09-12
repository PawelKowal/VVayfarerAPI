using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Dtos;
using AutoMapper;
using VVayfarerApi.Models;

namespace VVayfarerApi.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserModel, UserReadDto>();
            CreateMap<UserUpdateDto, UserModel>();
            CreateMap<UserModel, UserUpdateDto>();
        }
    }
}
