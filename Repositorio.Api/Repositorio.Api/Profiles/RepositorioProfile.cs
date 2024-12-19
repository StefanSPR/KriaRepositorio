using AutoMapper;
using Repositorio.Aplicacao.Dto.Create;
using Repositorio.Aplicacao.Dto.Return;
using Repositorio.Aplicacao.Dto.Update;
using Repositorio.Dominio;

namespace Repositorio.Api.Profiles
{
    public class RepositorioProfile : Profile
    {
        public RepositorioProfile()
        {
            CreateMap<CrtRepositorio, MdlRepositorio>()
                .ForMember(dest => dest.DataAtualizacao, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<UpdRepositorio, MdlRepositorio>()
                .ForMember(dest => dest.DataAtualizacao, opt => opt.MapFrom(src => DateTime.Now)); ;
            CreateMap<MdlRepositorio, RtnRepositorio>()
                .ForMember(dest => dest.DonoRepositorio, opt => opt.MapFrom(src => (src.Usuario ?? new()).Nome)); ;

        }
    }
}
