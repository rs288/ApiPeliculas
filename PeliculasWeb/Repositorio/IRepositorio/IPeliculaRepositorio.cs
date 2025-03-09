using PeliculasWeb.Models;

namespace PeliculasWeb.Repositorio.IRepositorio
{
    public interface IPeliculaRepositorio : IRepositorio<Pelicula>
    {
        Task<IEnumerable<Pelicula>> GetPeliculasTodoAsync(string url);
    }
}
