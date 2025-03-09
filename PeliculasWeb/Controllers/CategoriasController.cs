﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeliculasWeb.Models;
using PeliculasWeb.Repositorio.IRepositorio;
using PeliculasWeb.Utilidades;

namespace PeliculasWeb.Controllers
{
    [Authorize]
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await _repoCategoria.CrearAsync(CT.RutaCategoriasApi, categoria, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            Categoria itemCategoria = new Categoria();

            if (id == null)
            {
                return NotFound();
            }

            itemCategoria = await _repoCategoria.GetAsync(CT.RutaCategoriasApi, id.GetValueOrDefault());

            if (itemCategoria == null)
            {
                return NotFound();
            }

            return View(itemCategoria);

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await _repoCategoria.ActualizarAsync(CT.RutaCategoriasApi + categoria.Id, categoria, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _repoCategoria.BorrarAsync(CT.RutaCategoriasApi, id, HttpContext.Session.GetString("JWToken"));

            if (status)
            {
                return Json(new { success = true, message = "Borrado correctamente" });
            }

            return Json(new { success = false, message = "No se pudo borrar" });
        }
    }
}
