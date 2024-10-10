using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DataTransferObjectLayer.Dtos.ServiceDtos;
using DataTransferObjectLayer.Dtos.WhoWeAreDtos;
using RealEstate_Dapper_UI.Models;
using Microsoft.Extensions.Options;

namespace RealEstate_Dapper_UI.ViewComponents.HomePage
{
    public class _DefaultWhoWeAreViewComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public _DefaultWhoWeAreViewComponentPartial(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var client2 = _httpClientFactory.CreateClient();
            client2.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("WhoWeAreDetail");
            var responseMessage2 = await client2.GetAsync("Service");

            if (responseMessage.IsSuccessStatusCode && responseMessage2.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var jsonData2 = await responseMessage2.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultWhoWeAreDetailDtos>>(jsonData);
                var values2 = JsonConvert.DeserializeObject<List<ResultServiceDtos>>(jsonData2);
                ViewBag.title = values.Select(x => x.title).FirstOrDefault();
                ViewBag.subtitle = values.Select(x => x.subtitle).FirstOrDefault();
                ViewBag.description1 = values.Select(x => x.description1).FirstOrDefault();
                ViewBag.description2 = values.Select(x => x.description2).FirstOrDefault();
                return View(values2);
            }
            return View();
        }
    }
}