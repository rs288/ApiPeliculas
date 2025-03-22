using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PeliculasWeb.Models;
using PeliculasWeb.Repositorio.IRepositorio;
using PeliculasWeb.Utilidades;
using PeliculasWeb.Models.ViewModels;

namespace PeliculasWeb.Controllers;

public class HomeController : Controller
{
    //private readonly ILogger<HomeController> _logger;
    private readonly IAccountRepositorio _accRepo;
    private readonly ICategoriaRepositorio _repoCategoria;
    private readonly IPeliculaRepositorio _repoPelicula;

    public HomeController(IAccountRepositorio accRepo, ICategoriaRepositorio repoCategoria, IPeliculaRepositorio repoPelicula)//ILogger<HomeController> logger)
    {
       // _logger = logger;
       _accRepo = accRepo;
       _repoCategoria = repoCategoria;
       _repoPelicula = repoPelicula;
    }

    //V1 de Controlador
    //[HttpGet]
    //public async Task<IActionResult> Index()
    //{
    //    IndexVM listaPeliculasCategorias = new IndexVM()
    //    {
    //        ListaCategorias = (IEnumerable<Categoria>)await _repoCategoria.GetTodoAsync(CT.RutaCategoriasApi),
    //        ListaPeliculas = (IEnumerable<Pelicula>)await _repoPelicula.GetPeliculasTodoAsync(CT.RutaPeliculasApi)
    //    };

    //    return View(listaPeliculasCategorias);
    //}

    //V2 con soporte para paginaci�n
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1)
    {
        const int pageSize = 6; // O el tama�o de p�gina que prefieras
        var url = $"{CT.RutaPeliculasApi}?pageNumber={page}&pageSize={pageSize}";

        var peliculaResponse = await _repoPelicula.GetPeliculasTodoAsync(url);

        IndexVM listaPeliculasCategorias = new IndexVM()
        {
            ListaCategorias = (IEnumerable<Categoria>)await _repoCategoria.GetTodoAsync(CT.RutaCategoriasApi),
            ListaPeliculas = peliculaResponse.Items,
            TotalPages = peliculaResponse.TotalPages,
            CurrentPage = page,
        };

        return View(listaPeliculasCategorias);
    }

    [HttpGet]
    public async Task<IActionResult> IndexCategoria(int id)
    {
        var pelisEnCategoria = await _repoPelicula.GetPeliculasEnCategoriaAsync(CT.RutaPeliculasEnCategoriaApi, id);

        return View(pelisEnCategoria);
    }

    [HttpPost]
    public async Task<IActionResult> IndexBusqueda(string nombre)
    {
        var pelisEncontradas = await _repoPelicula.Buscar(CT.RutaPeliculasBusquedaApi, nombre);

        return View(pelisEncontradas);
    }

    [HttpGet]
    public IActionResult Login()
    {
        UsuarioAuth usuario = new UsuarioAuth();
        return View(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Login(UsuarioAuth obj)
    {
        if (ModelState.IsValid)
        {
            UsuarioAuth objUser = await _accRepo.LoginAsync(CT.RutaUsuariosApi + "Login", obj);
            if (objUser.Token == null)
            {
                TempData["alert"] = "Los datos son incorrectos";
                return View();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Email, objUser.NombreUsuario));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            HttpContext.Session.SetString("JWToken", objUser.Token);
            HttpContext.Session.SetString("Usuario", objUser.NombreUsuario);

            return RedirectToAction("Index");
        }
        else
        {
            return View();
        }
    }

    [HttpGet]
    public IActionResult Registro()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registro(UsuarioAuth obj)
    {
        bool result = await _accRepo.RegisterAsync(CT.RutaUsuariosApi + "Registro", obj);
        if (result == false)
        {
            return View();
        }

        TempData["alert"] = "Registro Correcto";
        return RedirectToAction("Login");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        // Cierra la sesi�n de autenticaci�n
        await HttpContext.SignOutAsync();

        // Limpia la sesi�n de usuario, incluyendo cualquier token
        HttpContext.Session.Clear();

        // Elimina la cookie de sesi�n manualmente
        if (Request.Cookies.ContainsKey(".AspNetCore.Session"))
        {
            Response.Cookies.Delete(".AspNetCore.Session");
        }

        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
