using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebApi.Data.DomainObjects;

namespace WebApi1.Controllers
{
    public class ProxyController : ControllerBase
    {
        private readonly RedirectConfigSection _config;
        private readonly ILogger _logger;

        public ProxyController(IConfiguration configuration, ILogger logger)
        {
            _logger = logger;

            try
            {
                _config = configuration.GetSection("RedirectTo").Get<RedirectConfigSection>();
            }
            catch (Exception e)
            {
                _logger.Log("Unable to load configuration", e);
            } 
        }


        [HttpGet("api/user/{id}")]
        public async Task<IActionResult> GetUser(long id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    _logger.Log("Start executing get request...");
                    string url = new UriBuilder(
                        "http",
                        _config.Http.Host,
                        _config.Http.Port,
                        $"api/user/{id}").Uri.ToString();
                    _logger.Log($"Senging get request to {url} ...");
                    return await ProxyResponse<User>(await client.GetAsync(url));
                }
                catch (Exception e)
                {
                    _logger.Log($"Sending get request failed." , e);
                    return BadRequest("Request failed. See log for additional information.");
                }
                finally
                {
                    _logger.Log("Finish executing get request.");
                }
            }
        }
        
        [HttpPost("api/user")]
        public async Task<IActionResult> SaveUser([FromBody] User user)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    _logger.Log("Start executing get request...");
                    string url = new UriBuilder(
                        "http",
                        _config.Http.Host,
                        _config.Http.Port,
                        "api/user")
                        .Uri.ToString();
                    _logger.Log($"Senging post request to {url} ...");

                    var obj = SerializeObject(user);
                    return await ProxyResponse(await client.PostAsync(url, obj));
                }
                catch (Exception e)
                {
                    _logger.Log($"Sending get request failed." , e);
                    return BadRequest("Request failed. See log for additional information.");
                }
                finally
                {
                    _logger.Log("Finish executing post request.");
                }
            }
        }

        private async Task<IActionResult> ProxyResponse(HttpResponseMessage responseMessage)
        {
            switch (responseMessage.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(await responseMessage.Content.ReadAsStringAsync());
                default:
                    return BadRequest(await responseMessage.Content.ReadAsStringAsync());
            }
        }
        
        private async Task<IActionResult> ProxyResponse<T>(HttpResponseMessage responseMessage) where T : class, new()
        {
            switch (responseMessage.StatusCode)
            {
                case HttpStatusCode.OK:
                    var content = await responseMessage.Content.ReadAsStringAsync();
                    return Ok(JsonConvert.DeserializeObject<T>(content));
                default:
                    return BadRequest(await responseMessage.Content.ReadAsStringAsync());
            }
        }

        private static StringContent SerializeObject(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
    }
}