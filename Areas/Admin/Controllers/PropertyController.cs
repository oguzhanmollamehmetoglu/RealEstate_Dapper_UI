﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using DataTransferObjectLayer.Dtos.CategoryDtos;
using DataTransferObjectLayer.Dtos.PropertyDtos;
using DataTransferObjectLayer.Dtos.PropertyDetailDtos;
using RealEstate_Dapper_UI.Models;
using Microsoft.Extensions.Options;
using System.Text;
using RealEstate_Dapper_UI.Services;

namespace RealEstate_Dapper_UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PropertyController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILoginService _loginService;
        private readonly ApiSettings _apiSettings;

        public PropertyController(IHttpClientFactory httpClientFactory, ILoginService loginService, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _loginService = loginService;
            _apiSettings = apiSettings.Value;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);

            var responseMessage = await client.GetAsync("Property/PropertyListWithCategory");
            var responseMessage2 = await client.GetAsync("PropertyDetail");

            if (responseMessage.IsSuccessStatusCode && responseMessage2.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultPropertyDtos>>(jsonData);
                ViewBag.Model1 = values;

                var jsonData2 = await responseMessage2.Content.ReadAsStringAsync();
                var values2 = JsonConvert.DeserializeObject<List<ResultPropertyDetailDto>>(jsonData2);
                ViewBag.Model2 = values2;

                // Convert ViewBag.Model2 to a dictionary for faster lookups
                var model2Dict = values2.ToDictionary(v => v.PropertyID);
                ViewBag.Model2Dict = model2Dict;

                return View();
            }

            // Handle error scenario (e.g., show an error message or view)
            ViewBag.ErrorMessage = "Veri alınırken bir hata oluştu.";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> CreateProperty()
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
        [HttpPost]
        public async Task<IActionResult> CreateProperty(CreatePropertyDtos createPropertyDtos)
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
                return RedirectToAction("Index", "Property", new { area = "Admin" });
            }
            return View();
        }
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);

            // İlk olarak PropertyDetail kaydını sil
            var deletePropertyDetailTask = client.DeleteAsync($"PropertyDetail/{id}");
            // PropertyDetail silme işleminin tamamlanmasını bekle
            var deletePropertyDetailResponse = await deletePropertyDetailTask;
            // PropertyDetail silme işlemi başarılı mı kontrol et
            if (!deletePropertyDetailResponse.IsSuccessStatusCode)
            {
                TempData["Error"] = "PropertyDetail silinirken bir hata oluştu.";
                return RedirectToAction("Index", "Property", new { area = "Admin" });
            }
            // PropertyDetail silme işlemi başarılı ise, Property kaydını sil
            var deletePropertyTask = client.DeleteAsync($"Property/{id}");

            // Property silme işleminin tamamlanmasını bekle
            var deletePropertyResponse = await deletePropertyTask;
            // Property silme işlemi başarılı mı kontrol et
            if (deletePropertyResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Property", new { area = "Admin" });
            }
            TempData["Error"] = "Property silinirken bir hata oluştu.";
            return RedirectToAction("Index", "Property", new { area = "Admin" });
        }
        [HttpGet]
        public async Task<IActionResult> UpdateProperty(int id)
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
        public async Task<IActionResult> UpdateProperty(UpdatePropertyDtos updatePropertyDtos)
        {
            //Burada kaldın devam edilicek

            //var client2 = _httpClientFactory.CreateClient();
            //client2.BaseAddress = new Uri(_apiSettings.BaseUrl);
            //var responseMessage2 = await client2.GetAsync("Property/PropertyListWithCategory");
            //var jsonData2 = await responseMessage2.Content.ReadAsStringAsync();
            //var values2 = JsonConvert.DeserializeObject<List<ResultPropertyDtos>>(jsonData2);

            updatePropertyDtos.DealOfTheDay = false;
            updatePropertyDtos.AdvertisementDate = DateTime.Now;
            updatePropertyDtos.PropertyStatus = false;

            //var id = _loginService.GetUserId;
            //updatePropertyDtos.AppUserID = int.Parse(id);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var jsonData = JsonConvert.SerializeObject(updatePropertyDtos);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("Property", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Property", new { area = "Admin" });
            }
            return View();
        }
        public async Task<IActionResult> PropertyDealOfTheDayStatusChangeToFalse(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("Property/PropertyDealofTheDayStatusChangeToFalse/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<IActionResult> PropertyDealOfTheDayStatusChangeToTrue(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("Property/PropertyDealOfTheDayStatusChangeToTrue/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<IActionResult> PropertyStatusChangeToFalse(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("Property/PropertyStatusChangeToFalse/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<IActionResult> PropertyStatusChangeToTrue(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("Property/PropertyStatusChangeToTrue/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        //PropertyDetail Controler//
        public async Task<IActionResult> PropertyDetail(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("PropertyDetail/GetPropertyDetailByPropertyId?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ResultPropertyDetailDto>(jsonData);
                return View(values);
            }
            return View();
        }
        [HttpGet]
        public IActionResult CreatePropertyDetail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreatePropertyDetail(CreatePropertyDetailDto createPropertyDetailDto, int id)
        {
            //Bu alanda property Status True Olarak günceller
            var client2 = _httpClientFactory.CreateClient();
            client2.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage2 = await client2.GetAsync($"Property/{id}");
            var jsonData2= await responseMessage2.Content.ReadAsStringAsync();
            var values2 = JsonConvert.DeserializeObject<UpdatePropertyDtos>(jsonData2);
            values2.PropertyStatus = true;
            var jsonData3 = JsonConvert.SerializeObject(values2);
            StringContent stringContent3 = new StringContent(jsonData3, Encoding.UTF8, "application/json");
            var responseMessage3 = await client2.PutAsync("Property", stringContent3);
            /////////////////////////////////////////////////////
            //Bu alanda Detay Oluşturur
            createPropertyDetailDto.PropertyID = id;
            createPropertyDetailDto.PropertyDetailsStatus = true;
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var jsonData = JsonConvert.SerializeObject(createPropertyDetailDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("PropertyDetail", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Property", new { area = "Admin" });
            }
            else
            {
                //Bu alanda eğer detay oluşmaz ise property de ki oluşturulan detayı olmayan ilanı siler
                var client4 = _httpClientFactory.CreateClient();
                client4.BaseAddress = new Uri(_apiSettings.BaseUrl);
                var responseMessage4 = await client4.DeleteAsync($"Property/{id}");
                if (responseMessage4.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Property", new { area = "Admin" });
                }
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> UpdatePropertyDetail(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync($"PropertyDetail/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdatePropertyDetailDto>(jsonData);
                return View(values);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePropertyDetail(UpdatePropertyDetailDto updatePropertyDetailDto)
        {
            updatePropertyDetailDto.PropertyDetailsStatus = true;

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var jsonData = JsonConvert.SerializeObject(updatePropertyDetailDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("PropertyDetail", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Property", new { area = "Admin" });
            }
            return View();
        }
    }
}