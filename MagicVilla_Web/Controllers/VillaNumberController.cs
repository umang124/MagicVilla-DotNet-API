using AutoMapper;
using MagicVillaAPI.Models;
using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MagicVilla_Web.Models.VM;
using Microsoft.AspNetCore.Mvc.Rendering;
using MagicVillaAPI.Models.Dto;
using System.Text;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string villaUrl;
        private IMapper _mapper;

        public VillaNumberController(IHttpClientFactory httpClient, IConfiguration configuration, IMapper mapper)
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
            message.RequestUri = new Uri(villaUrl + "/api/VillaNumberAPI");
            message.Method = HttpMethod.Get;

            HttpResponseMessage httpResponse = await client.SendAsync(message);

            var apiContent = await httpResponse.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<APIResponse>(apiContent);
            var villaNumberList = JsonConvert.DeserializeObject<List<Models.Dto.VillaNumberDTO>>(data.Result.ToString());
            return View(villaNumberList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var client = _httpClient.CreateClient("MagicAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(villaUrl + "/api/VillaAPI");
            message.Method = HttpMethod.Get;

            HttpResponseMessage httpResponse = await client.SendAsync(message);

            var apiContent = await httpResponse.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<APIResponse>(apiContent);
            var villaList = JsonConvert.DeserializeObject<List<Models.Dto.VillaDTO>>(data.Result.ToString());

            VillaNumberCreateVM villaNumberCreateVM = new VillaNumberCreateVM();
            villaNumberCreateVM.VillaList = villaList.Select(x => new SelectListItem
            {
                Text = x.VillaName,
                Value = x.Id.ToString()
            });
            return View(villaNumberCreateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VillaNumberCreateVM villaNumberCreateVM)
        {
            var client = _httpClient.CreateClient("MagicAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(villaUrl + "/api/VillaNumberAPI");
            message.Method = HttpMethod.Post;

            var model = _mapper.Map<VillaNumber>(villaNumberCreateVM.VillaNumber);

            message.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse = await client.SendAsync(message);
            var apiContent = await httpResponse.Content.ReadAsStringAsync();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Update(int villaNo)
        {
            var client = _httpClient.CreateClient("MagicAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(villaUrl + "/api/VillaNumberAPI/" + villaNo);
            message.Method = HttpMethod.Get;

            HttpResponseMessage httpResponse = await client.SendAsync(message);

            var apiContent = await httpResponse.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<APIResponse>(apiContent);
            VillaNumberCreateVM villaNumberCreateVM = new VillaNumberCreateVM();
            var villaNumber = JsonConvert.DeserializeObject<Models.Dto.VillaNumberDTO>(data.Result.ToString());

            villaNumberCreateVM.VillaNumber = villaNumber;


            HttpRequestMessage message1 = new HttpRequestMessage();
            message1.Headers.Add("Accept", "application/json");
            message1.RequestUri = new Uri(villaUrl + "/api/VillaAPI");
            message1.Method = HttpMethod.Get;

            HttpResponseMessage httpResponse1 = await client.SendAsync(message1);

            var apiContent1 = await httpResponse1.Content.ReadAsStringAsync();
            var data1 = JsonConvert.DeserializeObject<APIResponse>(apiContent1);
            var villaList = JsonConvert.DeserializeObject<List<Models.Dto.VillaDTO>>(data1.Result.ToString());

            villaNumberCreateVM.VillaList = villaList.Select(x => new SelectListItem
            {
                Text = x.VillaName,
                Value = x.Id.ToString()
            });

            return View(villaNumberCreateVM);
        } 

        [HttpPost]
        public async Task<IActionResult> Update(VillaNumberCreateVM villaNumberCreateVM)
        {
            var client = _httpClient.CreateClient("MagicAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(villaUrl + "/api/VillaNumberAPI/" + villaNumberCreateVM.VillaNumber.VillaNo);
            message.Method = HttpMethod.Put;

            var model = _mapper.Map<VillaNumber>(villaNumberCreateVM.VillaNumber);

            message.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse = await client.SendAsync(message);
            var apiContent = await httpResponse.Content.ReadAsStringAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int villaNo)
        {
            var client = _httpClient.CreateClient("MagicAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(villaUrl + "/api/VillaNumberAPI/" + villaNo);
            message.Method = HttpMethod.Get;

            HttpResponseMessage httpResponse = await client.SendAsync(message);

            var apiContent = await httpResponse.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<APIResponse>(apiContent);
            VillaNumberCreateVM villaNumberCreateVM = new VillaNumberCreateVM();
            var villaNumber = JsonConvert.DeserializeObject<Models.Dto.VillaNumberDTO>(data.Result.ToString());

            villaNumberCreateVM.VillaNumber = villaNumber;


            HttpRequestMessage message1 = new HttpRequestMessage();
            message1.Headers.Add("Accept", "application/json");
            message1.RequestUri = new Uri(villaUrl + "/api/VillaAPI");
            message1.Method = HttpMethod.Get;

            HttpResponseMessage httpResponse1 = await client.SendAsync(message1);

            var apiContent1 = await httpResponse1.Content.ReadAsStringAsync();
            var data1 = JsonConvert.DeserializeObject<APIResponse>(apiContent1);
            var villaList = JsonConvert.DeserializeObject<List<Models.Dto.VillaDTO>>(data1.Result.ToString());

            villaNumberCreateVM.VillaList = villaList.Select(x => new SelectListItem
            {
                Text = x.VillaName,
                Value = x.Id.ToString()
            });

            return View(villaNumberCreateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(VillaNumberCreateVM villaNumberCreateVM)
        {
            var client = _httpClient.CreateClient("MagicAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(villaUrl + "/api/VillaNumberAPI/" + villaNumberCreateVM.VillaNumber.VillaNo);
            message.Method = HttpMethod.Delete;

            var model = _mapper.Map<VillaNumber>(villaNumberCreateVM.VillaNumber);

            message.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse = await client.SendAsync(message);
            var apiContent = await httpResponse.Content.ReadAsStringAsync();

            return RedirectToAction("Index");
        }
    }
}
