using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApiWithRoleAuthentication.Dtos;
using WebApiWithRoleAuthentication.Models;

namespace WebApiWithRoleAuthentication.Profiles
{
    public class TouristRoutePictureProfile : Profile
    {
        public TouristRoutePictureProfile()
        {
            CreateMap<TouristRoutePicture, TouristRoutePictureDto>();
        }
    }
}