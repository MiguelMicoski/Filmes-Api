using AutoMapper;
using FilmesAPI.Data.DTOS;

namespace FilmesAPI.Models
{
    public class SessaoProfile : Profile
    {
        public SessaoProfile()
        {
            CreateMap<CreateSessaoDto, Sessao>();
            CreateMap<Sessao, ReadSessaoDto>();

        }
    }
}
