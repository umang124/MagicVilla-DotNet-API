using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVillaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using APIResponse = MagicVilla_Web.Models.APIResponse;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string villaUrl;
        private IMapper _mapper;

        public VillaController(IHttpClientFactory httpClient, IConfiguration configuration, IMapper mapper)
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

            if(HttpContext.Session.GetString("SessionToken") != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", 
                                                    HttpContext.Session.GetString("SessionToken"));
            }

            HttpResponseMessage httpResponse = await client.SendAsync(message);

            var apiContent = await httpResponse.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<APIResponse>(apiContent);
            if (data == null)
            {
                return View(new List<VillaDTO>());
            }
            var villaList = JsonConvert.DeserializeObject<List<VillaDTO>>(data.Result.ToString());
            return View(villaList);
        }
        [HttpGet]
        [Authorize (Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVilla(VillaDTO villaDTO)
        {
            var client = _httpClient.CreateClient("MagicAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(villaUrl + "/api/VillaAPI");
            message.Method = HttpMethod.Post;
            if (HttpContext.Session.GetString("SessionToken") != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                                                    HttpContext.Session.GetString("SessionToken"));
            }

            var model = _mapper.Map<Villa>(villaDTO);
       
            message.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse = await client.SendAsync(message);
            var apiContent = await httpResponse.Content.ReadAsStringAsync();

            TempData["success"] = "Villa added successfully";

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            var client = _httpClient.CreateClient("MagicAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(villaUrl + "/api/VillaAPI/" + villaId);
            message.Method = HttpMethod.Get;
            if (HttpContext.Session.GetString("SessionToken") != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                                                    HttpContext.Session.GetString("SessionToken"));
            }

            HttpResponseMessage httpResponse = await client.SendAsync(message);

            var apiContent = await httpResponse.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<APIResponse>(apiContent);
            var villa = JsonConvert.DeserializeObject<VillaDTO>(data.Result.ToString());

            return View(villa);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(VillaDTO villaDTO)
        {
            var client = _httpClient.CreateClient("MagicAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(villaUrl + "/api/VillaAPI/" + villaDTO.Id);
            message.Method = HttpMethod.Put;
            if (HttpContext.Session.GetString("SessionToken") != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                                                    HttpContext.Session.GetString("SessionToken"));
            }

            var model = _mapper.Map<Villa>(villaDTO);

            message.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse = await client.SendAsync(message);
            var apiContent = await httpResponse.Content.ReadAsStringAsync();

            TempData["success"] = "Villa updated successfully";

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            var client = _httpClient.CreateClient("MagicAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(villaUrl + "/api/VillaAPI/" + villaId);
            message.Method = HttpMethod.Get;
            if (HttpContext.Session.GetString("SessionToken") != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                                                    HttpContext.Session.GetString("SessionToken"));
            }

            HttpResponseMessage httpResponse = await client.SendAsync(message);

            var apiContent = await httpResponse.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<APIResponse>(apiContent);
            var villa = JsonConvert.DeserializeObject<VillaDTO>(data.Result.ToString());

            return View(villa);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVilla(VillaDTO villaDTO)
        {
            var client = _httpClient.CreateClient("MagicAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(villaUrl + "/api/VillaAPI/" + villaDTO.Id);
            message.Method = HttpMethod.Delete;
            if (HttpContext.Session.GetString("SessionToken") != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                                                    HttpContext.Session.GetString("SessionToken"));
            }

            var model = _mapper.Map<Villa>(villaDTO);

            message.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse = await client.SendAsync(message);
            var apiContent = await httpResponse.Content.ReadAsStringAsync();

            TempData["success"] = "Villa deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
