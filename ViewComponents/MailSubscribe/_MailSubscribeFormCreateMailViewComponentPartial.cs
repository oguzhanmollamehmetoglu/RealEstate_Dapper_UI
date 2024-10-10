using DataTransferObjectLayer.Dtos.MailSubscribeDtos;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate_Dapper_UI.ViewComponents.MailSubscribe
{
    public class _MailSubscribeFormCreateMailViewComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Modeli buraya ekle ve View'e bu modeli geçir
            var model = new CreateMailSubscribeDto();
            return View(model);
        }
    }
}