using Microsoft.AspNetCore.Mvc;
using PeliculasWeb.Models;
using PeliculasWeb.Repositorio.IRepositorio;
using PeliculasWeb.Utilidades;

namespace PeliculasWeb.Controllers
{
    public class PeliculasController : Controller
    {
        private readonly ICategoriaRepositorio _repoCategoria;
        private readonly IPeliculaRepositorio _repoPelicula;

        public PeliculasController(ICategoriaRepositorio repoCategoria, IPeliculaRepositorio repoPelicula)
        {
            _repoCategoria = repoCategoria;
            _repoPelicula = repoPelicula;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(new Pelicula() { });
        }

        [HttpGet]
        public async Task<IActionResult> GetTodasPeliculas()
        {
            return Json(new { data = await _repoPelicula.GetPeliculasTodoAsync(CT.RutaPeliculasApi) });
        }
    }
}
