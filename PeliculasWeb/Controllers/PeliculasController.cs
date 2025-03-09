﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PeliculasWeb.Models;
using PeliculasWeb.Models.ViewModels;
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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            IEnumerable<Categoria> ctList = (IEnumerable<Categoria>)await _repoCategoria.GetTodoAsync(CT.RutaCategoriasApi);

            PeliculasVM objVM = new PeliculasVM()
            {
                ListaCategorias = ctList.Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Id.ToString()
                }),

                Pelicula = new Pelicula()
            };

            return View(objVM);
        }
    }
}
