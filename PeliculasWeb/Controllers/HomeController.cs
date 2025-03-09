using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PeliculasWeb.Models;
using PeliculasWeb.Repositorio.IRepositorio;
using PeliculasWeb.Utilidades;

namespace PeliculasWeb.Controllers;

public class HomeController : Controller
{
    //private readonly ILogger<HomeController> _logger;
    private readonly IAccountRepositorio _accRepo;

    public HomeController(IAccountRepositorio accRepo)//ILogger<HomeController> logger)
    {
       // _logger = logger;
       _accRepo = accRepo;
    }

    public IActionResult Index()
    {
        return View();
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
        // Cierra la sesión de autenticación
        await HttpContext.SignOutAsync();

        // Limpia la sesión de usuario, incluyendo cualquier token
        HttpContext.Session.Clear();

        // Elimina la cookie de sesión manualmente
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
