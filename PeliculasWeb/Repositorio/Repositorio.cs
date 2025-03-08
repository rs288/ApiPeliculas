using Newtonsoft.Json;
using PeliculasWeb.Repositorio.IRepositorio;
using System.Collections;
using System.Net.Http;
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

        public async Task<bool> ActualizarPeliculaAsync(string url, T peliculaActualizar)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Patch, url);
            // Se instancia MultipartFormDataContent, que permite enviar datos en formato multipart/form-data, 
            // comúnmente usado para cargar archivos junto con datos de formulario.
            var multipartContent = new MultipartFormDataContent();

            if (peliculaActualizar != null)
            {
                // Serializar cada propiedad de peliculaActualizar y añadirla al contenido
                // multipart/form-data
                foreach (var property in typeof(T).GetProperties())
                {
                    var value = property.GetValue(peliculaActualizar);
                    if (value != null)
                    {
                        if (property.PropertyType == typeof(IFormFile))
                        {
                            var file = value as IFormFile;
                            if (file != null)
                            {
                                var streamContent = new StreamContent(file.OpenReadStream());
                                streamContent.Headers.ContentType =
                                    new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                                multipartContent.Add(streamContent, property.Name, file.FileName);
                            }
                        }
                        else
                        {
                            var stringContent = new StringContent(value.ToString());
                            multipartContent.Add(stringContent, property.Name);
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            peticion.Content = multipartContent;
            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se actualizo y retorna boleano
            if (respuesta.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
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
