using DataTransferObjectLayer.Dtos.MailSubscribeDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RealEstate_Dapper_UI.Models;
using System.Text;

namespace RealEstate_Dapper_UI.Controllers
{
    public class MailSubscribeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _settings;

        public MailSubscribeController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> settings)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings.Value;
        }

        [HttpGet]
        public IActionResult CreateMailSubscribe()
        {
            return View();
        }
        [HttpPost("MailSubscribe/CreateMailSubscribe")]
        public async Task<IActionResult> CreateMailSubscribe(CreateMailSubscribeDto createMailSubscribeDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createMailSubscribeDto); // Hatalı model durumunu yönet.
            }
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_settings.BaseUrl);
            var jsonData = JsonConvert.SerializeObject(createMailSubscribeDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("MailSubscribe", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index","Default");
            }
            return View(createMailSubscribeDto);
        }
    }
}