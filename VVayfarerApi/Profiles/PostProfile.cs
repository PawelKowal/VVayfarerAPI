using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Dtos;
using VVayfarerApi.Entities;

namespace VVayfarerApi.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Entity, PostReadDto>();
            CreateMap<PostAddDto, Post>();
            CreateMap<PostUpdateDto, Post>();
            CreateMap<Post, PostUpdateDto>();
        }
    }
}
