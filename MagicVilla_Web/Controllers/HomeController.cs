using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string villaUrl;
        private IMapper _mapper;

        public HomeController(IHttpClientFactory httpClient, IConfiguration configuration, IMapper mapper)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            villaUrl = _configuration.GetValue<string>("ServiceUrls:VillaAPI");
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClient.CreateClient("MagicAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(villaUrl + "/api/VillaAPI");
            message.Method = HttpMethod.Get;

            HttpResponseMessage httpResponse = await client.SendAsync(message);

            var apiContent = await httpResponse.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<APIResponse>(apiContent);
            var villaList = JsonConvert.DeserializeObject<List<VillaDTO>>(data.Result.ToString());
            return View(villaList);
        }

    }
}
