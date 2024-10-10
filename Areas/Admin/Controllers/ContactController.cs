using DataTransferObjectLayer.Dtos.ContactDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RealEstate_Dapper_UI.Models;

namespace RealEstate_Dapper_UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public ContactController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("Contact");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultContactDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        public async Task<IActionResult> DeleteContact(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.DeleteAsync($"Contact/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SendMessageContact(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("Contact/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<GetByIdContactDto>(jsonData);
                ViewBag.Message = values.Message;
                ViewBag.Subject = values.Subject;
                return View(values);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessageContact(AnswerMessageDto answerMessageDto)
        {
            //var user = _contactUsService.TGetByID(replyMessageDTO.ReplyId);
            //if (answerMessageDto.MessageBody != null)
            //{
            //    MimeMessage mimeMessage = new MimeMessage();
            //    MailboxAddress mailboxAddressFrom = new MailboxAddress("Admin", "traversalseyahatturizim@gmail.com");
            //    mimeMessage.From.Add(mailboxAddressFrom);
            //    MailboxAddress mailboxAddressTo = new MailboxAddress("User", user.Email);
            //    mimeMessage.To.Add(mailboxAddressTo);
            //    var bodybuilder = new BodyBuilder();
            //    bodybuilder.TextBody = answerMessageDto.Title;
            //    mimeMessage.Body = bodybuilder.ToMessageBody();
            //    mimeMessage.Subject = "Admin Cevap";
            //    SmtpClient smtpClient = new SmtpClient();
            //    smtpClient.Connect("smtp.gmail.com", 587, false);
            //    smtpClient.Authenticate("traversalseyahatturizim@gmail.com", "qguavhgmewqlbwrq");
            //    smtpClient.Send(mimeMessage);
            //    smtpClient.Disconnect(true);
            //    user.ContactUsStatus = false;
            //    _contactUsService.TUpdate(user);
            //    return RedirectToAction("Dashboard", "Admin");
            //}
            return View();
        }
    }
}