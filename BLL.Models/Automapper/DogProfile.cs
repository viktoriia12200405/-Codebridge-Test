using AutoMapper;
using BLL.Models.Models;
using Domain.Models;

namespace BLL.Models.Automapper;
public class DogProfile : Profile
{
    public DogProfile()
    {
        CreateMap<Dog, DogDTO>()
            .ForMember(dst => dst.Tail_length,
                opt => opt.MapFrom(src => src.TailLength))
            .ReverseMap()
            .ForMember(dst => dst.TailLength,
                opt => opt.MapFrom(src => src.Tail_length));
    }
}
