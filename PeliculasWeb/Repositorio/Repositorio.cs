using Newtonsoft.Json;
using PeliculasWeb.Repositorio.IRepositorio;
using System.Collections;
using System.Text;

namespace PeliculasWeb.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        //Injección de dependencias se debe importar el IHttpClientFactory
        private readonly IHttpClientFactory _clientFactory;

        public Repositorio(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<bool> ActualizarAsync(string url, T itemActualizar)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Patch, url);

            if (itemActualizar != null)
            {
                peticion.Content = new StringContent(
                    JsonConvert.SerializeObject(itemActualizar), Encoding.UTF8, "application/json"
                    );
            }
            else
            {
                return false;
            }

            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se actualizo y retorna boleano
            if (respuesta.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Task<bool> ActualizarPeliculaAsync(string url, T peliculaActualizar)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable> Buscar(string url, string nombre)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CrearAsync(string url, T itemCrear)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CrearPeliculaAsync(string url, T peliculaCrear)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(string url, int Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable> GetPeliculasEnCategoriaAsync(string url, int categoriaId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable> GetTodoAsync(string url)
        {
            throw new NotImplementedException();
        }
    }
}
