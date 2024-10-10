using DataTransferObjectLayer.Dtos.ToDoListDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RealEstate_Dapper_UI.Models;

namespace RealEstate_Dapper_UI.Areas.Admin.ViewComponents.Dashboard
{
    public class _DashboardCalenderViewComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public _DashboardCalenderViewComponentPartial(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("ToDoList/GetToDoListStatusAsyncByTrue/");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultToDoListDto>>(jsonData);

                // Tarihleri formatla ve listeyi oluştur
                var datesToHighlight = values.Select(x => x.ToDoListDate.ToString("yyyy-MM-dd")).ToList();
                
                // Tarih listesini View'a gönder
                ViewBag.DatesToHighlight = datesToHighlight;
                return View(values);
            }
            return View();
        }
    }
}