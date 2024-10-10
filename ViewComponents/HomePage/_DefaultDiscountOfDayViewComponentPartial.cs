using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DataTransferObjectLayer.Dtos.PropertyDtos;
using RealEstate_Dapper_UI.Models;
using Microsoft.Extensions.Options;

namespace RealEstate_Dapper_UI.ViewComponents.HomePage
{
    public class _DefaultDiscountOfDayViewComponentPartial: ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public _DefaultDiscountOfDayViewComponentPartial(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("Property/GetLastThreePropertyList");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultLastThreePropertyWithCategoryDto>>(jsonData);
                return View(values);
            }
            return View();
        }
    }
}