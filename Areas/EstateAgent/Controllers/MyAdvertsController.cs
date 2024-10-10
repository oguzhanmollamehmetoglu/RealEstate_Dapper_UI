using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using DataTransferObjectLayer.Dtos.CategoryDtos;
using DataTransferObjectLayer.Dtos.PropertyDtos;
using RealEstate_Dapper_UI.Services;
using System.Text;
using RealEstate_Dapper_UI.Models;
using Microsoft.Extensions.Options;

namespace RealEstate_Dapper_UI.Areas.EstateAgent.Controllers
{
    [Area("EstateAgent")]
    [Authorize]
    public class MyAdvertsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILoginService _loginService;
        private readonly ApiSettings _apiSettings;

        public MyAdvertsController(IHttpClientFactory httpClientFactory, ILoginService loginService, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _loginService = loginService;
            _apiSettings = apiSettings.Value;
        }

        public async Task<IActionResult> ActiveAdverts()
        {
            var Id = _loginService.GetUserId;
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("Property/PropertyAdvertsListByAppUserByTrue?id=" + Id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultPropertyAdvertListWithCategoryByAppUserDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        public async Task<IActionResult> PassiveAdverts()
        {
            var Id = _loginService.GetUserId;
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("Property/PropertyAdvertsListByAppUserByFalse?id=" + Id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultPropertyAdvertListWithCategoryByAppUserDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        public async Task<IActionResult> ApprovalAdverts()
        {
            var Id = _loginService.GetUserId;
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("Property/PropertyAdvertsListByAppUserByStatusFalse?id=" + Id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultPropertyAdvertListWithCategoryByAppUserDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> CreateAdverts()
        {
            var user = User.Claims;
            var userId = _loginService.GetUserId;

            var token = User.Claims.FirstOrDefault(x => x.Type == "realestatetoken")?.Value;
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_apiSettings.BaseUrl);
                var responseMessage = await client.GetAsync("Categories");
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultCategoryDtos>>(jsonData);

                List<SelectListItem> selectListItems = (from x in values.ToList()
                                                        select new SelectListItem
                                                        {
                                                            Text = x.CategoryName,
                                                            Value = x.CategoryID.ToString()
                                                        }).ToList();
                ViewData["PropertyCategory"] = selectListItems;
                return View();
            }
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> CreateAdverts(CreatePropertyDtos createPropertyDtos)
        {
            createPropertyDtos.DealOfTheDay = false;
            createPropertyDtos.AdvertisementDate = DateTime.Now;
            createPropertyDtos.PropertyStatus = false;

            var id = _loginService.GetUserId;
            createPropertyDtos.AppUserID = int.Parse(id);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var jsonData = JsonConvert.SerializeObject(createPropertyDtos);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("Property", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ActiveAdverts", "MyAdverts", new { area = "EstateAgent" });
            }
            return View();
        }

        public async Task<IActionResult> DeleteAdverts(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.DeleteAsync($"Property/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ActiveAdverts", "MyAdverts", new { area = "EstateAgent" });
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAdverts(int id)
        {
            var client2 = _httpClientFactory.CreateClient();
            client2.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage2 = await client2.GetAsync("Categories");
            var jsonData2 = await responseMessage2.Content.ReadAsStringAsync();
            var values2 = JsonConvert.DeserializeObject<List<ResultCategoryDtos>>(jsonData2);
            List<SelectListItem> selectListItems2 = (from x in values2.ToList()
                                                     select new SelectListItem
                                                     {
                                                         Text = x.CategoryName,
                                                         Value = x.CategoryID.ToString()
                                                     }).ToList();
            ViewData["PropertyCategory"] = selectListItems2;
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync($"Property/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdatePropertyDtos>(jsonData);
                return View(values);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAdverts(UpdatePropertyDtos updatePropertyDtos)
        {
            updatePropertyDtos.DealOfTheDay = false;
            updatePropertyDtos.AdvertisementDate = DateTime.Now;
            updatePropertyDtos.PropertyStatus = false;

            var id = _loginService.GetUserId;
            updatePropertyDtos.AppUserID = int.Parse(id);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var jsonData = JsonConvert.SerializeObject(updatePropertyDtos);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("Property", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ActiveAdverts", "MyAdverts", new { area = "EstateAgent" });
            }
            return View();
        }

        public async Task<IActionResult> AdvertsStatusChangeToFalse(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("Property/AdvertPropertyStatusChangeToFalse/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ActiveAdverts", "MyAdverts", new { area = "EstateAgent" });
            }
            return View();
        }
    }
}