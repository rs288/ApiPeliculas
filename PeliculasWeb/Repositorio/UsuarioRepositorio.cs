using Microsoft.AspNetCore.Authorization;
using PeliculasWeb.Models;
using PeliculasWeb.Repositorio.IRepositorio;

namespace PeliculasWeb.Repositorio
{
    public class UsuarioRepositorio : Repositorio<Usuario>, IUsuarioRepositorio
    {
        //Injección de dependencias se debe importar el IHttpClientFactory
        private readonly IHttpClientFactory _clientFactory;

        public UsuarioRepositorio(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
