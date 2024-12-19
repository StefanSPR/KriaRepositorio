using AutoMapper;
using Repositorio.Aplicacao.Dto.Return;
using Repositorio.Dominio;

namespace Repositorio.Api.Profiles;

public class UsuarioProfile:Profile
{
    public UsuarioProfile()
    {
        CreateMap<MdlUsuario, RtnUsuario>();
    }
}
