using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RealEstate_Dapper_UI.Models;

namespace RealEstate_Dapper_UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StatisticsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public StatisticsController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
        }

        public async Task<IActionResult> Index()
        {
            #region İstatistik1
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("Statistics/ActiveCategoryCount");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            ViewBag.ActiveCategoryCount = jsonData;
            #endregion

            #region İstatistik2
            var client2 = _httpClientFactory.CreateClient();
            client2.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage2 = await client2.GetAsync("Statistics/ActiveEmployeeCount");
            var jsonData2 = await responseMessage2.Content.ReadAsStringAsync();
            ViewBag.ActiveEmployeeCount = jsonData2;
            #endregion

            #region İstatistik3
            var client3 = _httpClientFactory.CreateClient();
            client3.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage3 = await client3.GetAsync("Statistics/ApartmentCount");
            var jsonData3 = await responseMessage3.Content.ReadAsStringAsync();
            ViewBag.ApartmentCount = jsonData3;
            #endregion

            #region İstatistik4
            var client4 = _httpClientFactory.CreateClient();
            client4.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage4 = await client4.GetAsync("Statistics/AveragePropertyPriceByRent");
            var jsonData4 = await responseMessage4.Content.ReadAsStringAsync();
            ViewBag.AveragePropertyPriceByRent = jsonData4;
            #endregion

            #region İstatistik5
            var client5 = _httpClientFactory.CreateClient();
            client5.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage5 = await client5.GetAsync("Statistics/AveragePropertyPriceBySale");
            var jsonData5 = await responseMessage5.Content.ReadAsStringAsync();
            ViewBag.AveragePropertyPriceBySale = jsonData5;
            #endregion

            #region İstatistik6
            var client6 = _httpClientFactory.CreateClient();
            client6.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage6 = await client6.GetAsync("Statistics/AverageRoomCount");
            var jsonData6 = await responseMessage6.Content.ReadAsStringAsync();
            ViewBag.AverageRoomCount = jsonData6;
            #endregion

            #region İstatistik7
            var client7 = _httpClientFactory.CreateClient();
            client7.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage7 = await client7.GetAsync("Statistics/CategoryCount");
            var jsonData7 = await responseMessage7.Content.ReadAsStringAsync();
            ViewBag.CategoryCount = jsonData7;
            #endregion

            #region İstatistik8
            var client8 = _httpClientFactory.CreateClient();
            client8.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage8 = await client8.GetAsync("Statistics/CategoryNameByMaxPropertyCount");
            var jsonData8 = await responseMessage8.Content.ReadAsStringAsync();
            ViewBag.CategoryNameByMaxPropertyCount = jsonData8;
            #endregion

            #region İstatistik9
            var client9 = _httpClientFactory.CreateClient();
            client9.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage9 = await client9.GetAsync("Statistics/CityNameByMaxPropertyCount");
            var jsonData9 = await responseMessage9.Content.ReadAsStringAsync();
            ViewBag.CityNameByMaxPropertyCount = jsonData9;
            #endregion

            #region İstatistik10
            var client10 = _httpClientFactory.CreateClient();
            client10.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage10 = await client10.GetAsync("Statistics/DifferehtCityCount");
            var jsonData10 = await responseMessage10.Content.ReadAsStringAsync();
            ViewBag.DifferehtCityCount = jsonData10;
            #endregion

            #region İstatistik11
            var client11 = _httpClientFactory.CreateClient();
            client11.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage11 = await client11.GetAsync("Statistics/EmployeeNameByMaxPropertyCount");
            var jsonData11 = await responseMessage11.Content.ReadAsStringAsync();
            ViewBag.EmployeeNameByMaxPropertyCount = jsonData11;
            #endregion

            #region İstatistik12
            var client12 = _httpClientFactory.CreateClient();
            client12.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage12 = await client12.GetAsync("Statistics/LastPropertyPrice");
            var jsonData12 = await responseMessage12.Content.ReadAsStringAsync();
            ViewBag.LastPropertyPrice = jsonData12;
            #endregion

            #region İstatistik13
            var client13 = _httpClientFactory.CreateClient();
            client13.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage13 = await client13.GetAsync("Statistics/NewestBuildingYear");
            var jsonData13 = await responseMessage13.Content.ReadAsStringAsync();
            ViewBag.NewestBuildingYear = jsonData13;
            #endregion

            #region İstatistik14
            var client14 = _httpClientFactory.CreateClient();
            client14.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage14 = await client14.GetAsync("Statistics/OldestBuildingYear");
            var jsonData14 = await responseMessage14.Content.ReadAsStringAsync();
            ViewBag.OldestBuildingYear = jsonData14;
            #endregion

            #region İstatistik15
            var client15 = _httpClientFactory.CreateClient();
            client15.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage15 = await client15.GetAsync("Statistics/PassiveCategoryCount");
            var jsonData15 = await responseMessage15.Content.ReadAsStringAsync();
            ViewBag.PassiveCategoryCount = jsonData15;
            #endregion

            #region İstatistik16
            var client16 = _httpClientFactory.CreateClient();
            client16.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage16 = await client16.GetAsync("Statistics/PropertyCount");
            var jsonData16 = await responseMessage16.Content.ReadAsStringAsync();
            ViewBag.PropertyCount = jsonData16;
            #endregion

            return View();
        }
    }
}