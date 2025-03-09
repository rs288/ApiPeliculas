﻿using Microsoft.AspNetCore.Authorization;
using PeliculasWeb.Models;
using PeliculasWeb.Repositorio.IRepositorio;

namespace PeliculasWeb.Repositorio
{
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {
        //Injección de dependencias se debe importar el IHttpClientFactory
        private readonly IHttpClientFactory _clientFactory;

        public CategoriaRepositorio(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
