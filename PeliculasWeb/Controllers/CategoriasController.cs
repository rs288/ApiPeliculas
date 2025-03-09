using Microsoft.AspNetCore.Mvc;
using PeliculasWeb.Models;
using PeliculasWeb.Repositorio.IRepositorio;

namespace PeliculasWeb.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ICategoriaRepositorio _repoCategoria;

        public CategoriasController(ICategoriaRepositorio repoCategoria)
        {
            _repoCategoria = repoCategoria;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new Categoria() { });
        }
    }
}
