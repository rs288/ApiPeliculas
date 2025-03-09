using Microsoft.AspNetCore.Mvc;
using PeliculasWeb.Models;
using PeliculasWeb.Repositorio.IRepositorio;
using PeliculasWeb.Utilidades;

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

        [HttpGet]
        public async Task<IActionResult> GetTodasCategorias()
        {
            return Json(new { data = await _repoCategoria.GetTodoAsync(CT.RutaCategoriasApi) });
        }
    }
}
