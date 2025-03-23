using Microsoft.AspNetCore.Mvc;


namespace WebApiWithRoleAuthentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FakeVanderPaymentProcessController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> ProcessPayment(
            [FromQuery] Guid orderNumber,
            [FromQuery] bool returnFault = false
        )
        {
            // 假装在处理
            await Task.Delay(3000);

            // if returnFault is true, 返回支付失败
            if (returnFault)
            {
                return Ok(new
                {
                    id = Guid.NewGuid(),
                    created = DateTime.UtcNow,
                    approved = false,
                    message = "Reject",
                    payment_metohd = "信用卡支付",
                    order_number = orderNumber,
                    card = new
                    {
                        card_type = "信用卡",
                        last_four = "1234"
                    }
                });
            }

            return Ok(new
            {
                id = Guid.NewGuid(),
                created = DateTime.UtcNow,
                approved = true,
                message = "Reject",
                payment_metohd = "信用卡支付",
                order_number = orderNumber,
                card = new
                {
                    card_type = "信用卡",
                    last_four = "1234"
                }
            });
        }
    }
}
