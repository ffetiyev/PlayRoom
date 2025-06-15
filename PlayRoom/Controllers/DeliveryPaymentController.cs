using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using System.Threading.Tasks;

namespace PlayRoom.Controllers
{
    public class DeliveryPaymentController : Controller
    {
        private readonly IDeliveryPaymentService _service;
        public DeliveryPaymentController(IDeliveryPaymentService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAsync();
            return View(data);
        }
    }
}
