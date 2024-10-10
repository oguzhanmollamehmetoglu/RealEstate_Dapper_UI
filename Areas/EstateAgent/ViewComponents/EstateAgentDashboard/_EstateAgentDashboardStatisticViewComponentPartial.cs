using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RealEstate_Dapper_UI.Models;
using RealEstate_Dapper_UI.Services;

namespace RealEstate_Dapper_UI.Areas.EstateAgent.ViewComponents.EstateAgentDashboard
{
    public class _EstateAgentDashboardStatisticViewComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILoginService _loginService;
        private readonly ApiSettings _apiSettings;

        public _EstateAgentDashboardStatisticViewComponentPartial(IHttpClientFactory httpClientFactory, ILoginService loginService, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _loginService = loginService;
            _apiSettings = apiSettings.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var id = _loginService.GetUserId;
            #region İstatistik1 - ToplamİlanSayısı
            var client1 = _httpClientFactory.CreateClient();
            client1.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage1 = await client1.GetAsync("EstateAgentDashboardStatistic/AllPropertyCount");
            var jsonData1 = await responseMessage1.Content.ReadAsStringAsync();
            ViewBag.PropertyCount = jsonData1;
            #endregion

            #region İstatistik12 - EmlakçınınToplamİlanSayısı
            var client2 = _httpClientFactory.CreateClient();
            client2.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage2 = await client2.GetAsync("EstateAgentDashboardStatistic/PropertyCountByEmployeeId?id=" + id);
            var jsonData2 = await responseMessage2.Content.ReadAsStringAsync();
            ViewBag.EmployeeByPropertyCount = jsonData2;
            #endregion

            #region İstatistik3 - AktifİlanSayısı
            var client3 = _httpClientFactory.CreateClient();
            client3.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage3 = await client3.GetAsync("EstateAgentDashboardStatistic/PropertyCountByStatusTrue?id=" + id);
            var jsonData3 = await responseMessage3.Content.ReadAsStringAsync();
            ViewBag.PropertyCountByStatusTrue = jsonData3;
            #endregion

            #region İstatistik4 - PasifİlanSayısı
            var client4 = _httpClientFactory.CreateClient();
            client4.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage4 = await client4.GetAsync("EstateAgentDashboardStatistic/PropertyCountByStatusFalse?id=" + id);
            var jsonData4 = await responseMessage4.Content.ReadAsStringAsync();
            ViewBag.PropertyCountByStatusFalse = jsonData4;
            #endregion

            return View();
        }
    }
}