using MagicVilla_API.Models;
using MagicVilla_Utils;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService //Aplicamos interfaz base
    {
        public APIResponse ResponseModel { get; set; } 
        public IHttpClientFactory _httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.ResponseModel = new(); //Inicializamos nuestro objeto APIResponse
            this._httpClient = httpClient; //Inyección de dependencias de HttpClientFactory
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest) //Método que se va a encargar de tramitar todas nuestras request, recibe un objeto APIRequest por parametro
        {
            try
            {
                var client = _httpClient.CreateClient("MagicAPI"); //Estamos obligados a ponerle un nombre al cliente
                HttpRequestMessage message = new HttpRequestMessage(); //Se crea un objeto HttpRequestMessage para enviar una solicitud HTTP
                message.Headers.Add("Accept", "application/json"); //Se indica en el header de la solicitud que acepte una respuesta en formato JSON

                if (apiRequest.Parameters == null)
                    message.RequestUri = new Uri(apiRequest.Url); //Se indica la URL de la solicitud
                else
                {
                    var builder = new UriBuilder(apiRequest.Url);
                    var query = HttpUtility.ParseQueryString(builder.Query);
                    query["PageNumber"] = apiRequest.Parameters.PageNumber.ToString();
                    query["PageSize"] = apiRequest.Parameters.PageSize.ToString();
                    builder.Query = query.ToString();
                    string url = builder.ToString(); // api/Villa/VillaPaginated/PageNumber=1&PageSize=4
                    message.RequestUri = new Uri(url);
                }

                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");

                    /* "StringContent" es una clase en .NET que representa el contenido de una solicitud o respuesta HTTP como una cadena de texto. 
                     * Se utiliza para enviar o recibir datos en formato de texto plano, como JSON, XML o texto sin formato. */
                    //JsonConvert.SerializeObject(apiRequest.Data) = Serializamos el objeto apiRequest.Data a una cadena en formato JSON
                    //Encoding.UTF8 = Codificación de caracteres UTF-8. Asegura que los caracteres especiales y multibyte sean representados correctamente en la cadena JSON
                    //"application/json" = Establecemos que el contenido de la solicitud HTTP es de tipo JSON
                }

                switch (apiRequest.APIType) //Se agrega el tipo correspondiente
                {
                    case DS.APIType.POST:
                        message.Method = HttpMethod.Post;
                        break;

                    case DS.APIType.PUT:
                        message.Method = HttpMethod.Put;
                        break;

                    case DS.APIType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;

                    default:
                        message.Method = HttpMethod.Get; //Si no se especifica por defecto es GET
                        break;
                }

                HttpResponseMessage apiResponse = null; //Se crea un objeto HTTP que posee la respuesta de la API.
                if (!string.IsNullOrEmpty(apiRequest.Token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
                apiResponse = await client.SendAsync(message); //Se envia la solicitud HTTP. El resultado de la llamada es asignado a la variable apiResponse.
                var apiContent = await apiResponse.Content.ReadAsStringAsync(); //ReadAsStringAsync() se utiliza para obtener el contenido como una cadena de texto.
                //var APIResponse = JsonConvert.DeserializeObject<T>(apiContent); //Deseralizamos para pasar de una cadena JSON a un objeto del tipo especificado genericamente (T)

                try
                {
                    APIResponse response = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if(response != null && (apiResponse.StatusCode == HttpStatusCode.BadRequest || apiResponse.StatusCode == HttpStatusCode.NotFound))
                    {
                        response.statusCode = HttpStatusCode.BadRequest;
                        response.IsSuccessful = false;
                        var res = JsonConvert.SerializeObject(response);
                        var obj = JsonConvert.DeserializeObject<T>(res);
                        return obj;
                    }
                }
                catch (Exception ex)
                {
                    var errorResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return errorResponse;
                }

                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;
            }
            catch (Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccessful = false
                };

                var res = JsonConvert.SerializeObject(dto);
                var responseEx = JsonConvert.DeserializeObject<T>(res);
                return responseEx;
            }
        }
    }
}
