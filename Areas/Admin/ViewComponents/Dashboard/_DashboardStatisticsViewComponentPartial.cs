using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RealEstate_Dapper_UI.Models;

namespace RealEstate_Dapper_UI.Areas.Admin.ViewComponents.Dashboard
{
    public class _DashboardStatisticsViewComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public _DashboardStatisticsViewComponentPartial(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            #region İstatistik1 - ToplamİlanSayısı
            var client1 = _httpClientFactory.CreateClient();
            client1.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage1 = await client1.GetAsync("Statistics/PropertyCount");
            var jsonData1 = await responseMessage1.Content.ReadAsStringAsync();
            ViewBag.PropertyCount = jsonData1;
            #endregion

            #region İstatistik12 - EnBaşarılıPersonel
            var client2 = _httpClientFactory.CreateClient();
            client2.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage2 = await client2.GetAsync("Statistics/EmployeeNameByMaxPropertyCount");
            var jsonData2 = await responseMessage2.Content.ReadAsStringAsync();
            ViewBag.EmployeeNameByMaxPropertyCount = jsonData2;
            #endregion

            #region İstatistik3 - İlandakiŞheirSayıları
            var client3 = _httpClientFactory.CreateClient();
            client3.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage3 = await client3.GetAsync("Statistics/DifferehtCityCount");
            var jsonData3 = await responseMessage3.Content.ReadAsStringAsync();
            ViewBag.DifferehtCityCount = jsonData3;
            #endregion

            #region İstatistik4 - OrtalamaKiraFiyatı
            var client4 = _httpClientFactory.CreateClient();
            client4.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage4 = await client4.GetAsync("Statistics/AveragePropertyPriceByRent");
            var jsonData4 = await responseMessage4.Content.ReadAsStringAsync();
            ViewBag.AveragePropertyPriceByRent = jsonData4;
            #endregion

            return View();
        }
    }
}