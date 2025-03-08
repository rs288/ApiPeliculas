using System;
using System.Collections;
using System.Collections.Generic; 

namespace PeliculasWeb.Repositorio.IRepositorio
{
    public interface IRepositorio<T> where T : class
    {
        Task<IEnumerable> GetTodoAsync(string url); // metodo generico
        Task<IEnumerable> GetPeliculasEnCategoriaAsync(string url, int categoriaId); //metodo con subida de archivos
        Task<IEnumerable> Buscar(string url, string nombre);
        Task<T> GetAsync(string url, int Id);
        Task<bool> CrearAsync(string url, T itemCrear);
        Task<bool> CrearPeliculaAsync(string url, T peliculaCrear);
        Task<bool> ActualizarAsync(string url, T itemActualizar);
        Task<bool> ActualizarPeliculaAsync(string url, T peliculaActualizar);
    }
}
