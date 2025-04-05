using AutoMapper;
using WebApiWithRoleAuthentication.Dtos;
using WebApiWithRoleAuthentication.Enums;
using WebApiWithRoleAuthentication.Models;

namespace WebApiWithRoleAuthentication.Profiles
{
  public class TouristRouteProfile : Profile
  {
    public TouristRouteProfile()
    {
      CreateMap<TouristRoute, TouristRouteDto>()
          .ForMember(
              dest => dest.Price,
              opt => opt.MapFrom(src => src.OriginalPrice * (decimal)(src.DiscountPresent ?? 1))
          )
          .ForMember(
              dest => dest.TravelDays,
              opt => opt.MapFrom(src => src.TravelDays.ToString())
          )
          .ForMember(
              dest => dest.TripType,
              opt => opt.MapFrom(src => src.TripType.ToString())
          )
          .ForMember(
              dest => dest.DepartureCity,
              opt => opt.MapFrom(src => src.DepartureCity.ToString())
          );
      CreateMap<TouristRouteForCreationDto, TouristRoute>()
         .ForMember(
          dest => dest.Id,// 指定目標物件（TouristRoute）的 Id 屬性
          opt => opt.MapFrom(src => Guid.NewGuid())// 指定來源物件（TouristRouteForCreationDto）映射時，給 Id 指定一個新的 GUID
      );
      //使用這段設定後，當你寫： 
      //var touristRoute = _mapper.Map<TouristRoute>(touristRouteDto);
      // touristRoute 原本有的Id 屬性,就會賦值（GUID），你不需要自己手動賦值。
      //不是「新增」屬性，而是 設定已有屬性的值要怎麼從來源物件（source）來計算或轉換而來。

      CreateMap<TouristRouteForUpdateDto, TouristRoute>()
      .ForMember(dest => dest.TravelDays,
         opt => opt.MapFrom(src => Enum.Parse<TravelDays>(src.TravelDays!, true)))
       .ForMember(dest => dest.TripType,
         opt => opt.MapFrom(src => Enum.Parse<TripType>(src.TripType!, true)))
       .ForMember(dest => dest.DepartureCity,
         opt => opt.MapFrom(src => Enum.Parse<DepartureCity>(src.DepartureCity!, true)));
      //DTO 中的 TravelDays（字串）轉換成 TouristRoute 中的 TravelDays
      //Enum.Parse 是 C# 中用來把 字串轉換成列舉（enum） 的方法

      CreateMap<TouristRoute, TouristRouteForUpdateDto>();

      CreateMap<TouristRouteForCreationDto, TouristRoute>()
       .ForMember(dest => dest.TravelDays,
         opt => opt.MapFrom(src => Enum.Parse<TravelDays>(src.TravelDays!, true)))
       .ForMember(dest => dest.TripType,
         opt => opt.MapFrom(src => Enum.Parse<TripType>(src.TripType!, true)))
       .ForMember(dest => dest.DepartureCity,
         opt => opt.MapFrom(src => Enum.Parse<DepartureCity>(src.DepartureCity!, true)));
    }
  }
}
