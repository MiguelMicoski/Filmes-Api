namespace FilmesAPI.Profiles;
using AutoMapper;
using FilmesAPI.Data.DTOS;
using FilmesAPI.Models;

public class FilmeProfile : Profile 
{
    public FilmeProfile()
    {
        CreateMap<CreateFilmeDto, Filme>();
        CreateMap<UpdateFilmeDto, Filme>();
        CreateMap<Filme, UpdateFilmeDto>();
        CreateMap<Filme, ReadFilmeDto>();
        CreateMap<Cinema, ReadCinemaDto>().ForMember(filmedto => filmedto.Sessoes, opt => opt.MapFrom(filme => filme.Sessoes));
    }
}
