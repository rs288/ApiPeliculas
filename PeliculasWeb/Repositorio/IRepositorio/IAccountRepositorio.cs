using PeliculasWeb.Models;

namespace PeliculasWeb.Repositorio.IRepositorio
{
    public interface IAccountRepositorio
    {
        Task<UsuarioAuth> LoginAsync(string url, UsuarioAuth itemCrear);
        Task<bool> RegisterAsync(string url, UsuarioAuth itemCrear);
    }
}
