using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DataTransferObjectLayer.Dtos.AppUserDtos;
using RealEstate_Dapper_UI.Models;
using Microsoft.Extensions.Options;

namespace RealEstate_Dapper_UI.ViewComponents.PropertySingle
{
    public class _PropertySingleAppUserViewComponnentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public _PropertySingleAppUserViewComponnentPartial(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync(int i2)
        {
            ViewBag.id = i2;
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("AppUser?id=" + i2);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<GetByIdAppUserDto>(jsonData);
                return View(values);
            }
            return View();
        }
    }
}