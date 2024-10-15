using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DataTransferObjectLayer.Dtos.PropertyDtos;
using DataTransferObjectLayer.Dtos.PropertyDetailDtos;
using RealEstate_Dapper_UI.Models;
using Microsoft.Extensions.Options;

namespace RealEstate_Dapper_UI.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public PropertyController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("Property/PropertyListWithCategoryStatusByTrue");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultPropertyDtos>>(jsonData);
                return View(values);
            }
            return View();
        }

        public async Task<IActionResult> PropertyListWithSearch(string searchKeyValue, int propertyCategoryId, string city)
        {
            searchKeyValue = TempData["searchKeyValue"].ToString();
            propertyCategoryId = int.Parse(TempData["propertyCategoryId"].ToString());
            city = TempData["city"].ToString();

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync($"Property/GetPropertyWithSearchListAsync?searchKeyValue={searchKeyValue}&propertyCategoryId={propertyCategoryId}&city={city}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultPropertyWithSearchListDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpGet("propertysingle/{slug}/{id}")]
        public async Task<IActionResult> PropertySingle(string slug,int id)
        {
            ViewBag.i = id;
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("Property/GetPropertyByPropertyId?id=" + id);
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<ResultPropertyDtos>(jsonData);

            var client2 = _httpClientFactory.CreateClient();
            client2.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage2 = await client2.GetAsync("PropertyDetail/GetPropertyDetailByPropertyId?id=" + id);
            var jsonData2 = await responseMessage2.Content.ReadAsStringAsync();
            var values2 = JsonConvert.DeserializeObject<GetPropertyDetailDto>(jsonData2);

            ViewBag.PropertyID = values.PropertyID;
            ViewBag.i2 = values.AppUserID;
            ViewBag.Title1 = values.Title.ToString();
            ViewBag.Price = values.Price;
            ViewBag.Address = values.Address;
            ViewBag.City = values.City;
            ViewBag.District = values.District;
            ViewBag.Type = values.Type;
            ViewBag.Description = values.Description;
            ViewBag.AdvertisementDate = values.AdvertisementDate;
            ViewBag.SlugUrl = values.SlugUrl;
            DateTime date1 = DateTime.Now;
            DateTime date2 = values.AdvertisementDate;
            TimeSpan timeSpan = date1 - date2;
            int month = timeSpan.Days;
            ViewBag.Datediff = month / 30;

            ViewBag.BathCount = values2.BathCount;
            ViewBag.BedRoomCount = values2.BedRoomCount;
            ViewBag.GarageSize = values2.GarageSize;
            ViewBag.PropertySize = values2.PropertySize;
            ViewBag.RoomCount = values2.RoomCount;
            ViewBag.BuildYear = values2.BuildYear;
            ViewBag.Location = values2.Location;
            ViewBag.VideoUrl = values2.VideoUrl;

            string slugFromTitle = CreateSlug(values.Title);
            ViewBag.slugUrl = slugFromTitle;

            return View();
        }

        private string CreateSlug(string title)
        {
            title = title.ToLowerInvariant(); // Küçük harfe çevir
            title = title.Replace(" ", "-"); // Boşlukları tire ile değiştir
            title = System.Text.RegularExpressions.Regex.Replace(title, @"[^a-z0-9\s-]", ""); // Geçersiz karakterleri kaldır
            title = System.Text.RegularExpressions.Regex.Replace(title, @"\s+", " ").Trim(); // Birden fazla boşluğu tek boşluğa indir ve kenar boşluklarını kaldır
            title = System.Text.RegularExpressions.Regex.Replace(title, @"\s", "-"); // Boşlukları tire ile değiştir

            return title;
        }
    }
}