using AutoMapper;
using WebApiWithRoleAuthentication.Dtos;
using WebApiWithRoleAuthentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace WebApiWithRoleAuthentication.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;

        public OrdersController(
            IHttpContextAccessor httpContextAccessor,
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper,
            IHttpClientFactory httpClientFactory
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrders()
        {
            // 1. 获得当前用户
            var userId = _httpContextAccessor
                .HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // 2. 使用用户id来获取订单历史记录

            var orders = await _touristRouteRepository.GetOrdersByUserId(userId ?? string.Empty);

            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }

        [HttpGet("{orderId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GerOrderById([FromRoute] Guid orderId)
        {
            // 1. 获得当前用户
            var userId = _httpContextAccessor
                .HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var order = await _touristRouteRepository.GetOrderById(orderId);

            return Ok(_mapper.Map<OrderDto>(order));
        }


        [HttpPost("{orderId}/placeOrder")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> placeOrder([FromRoute] Guid orderId)
        {
            // 1. 获得当前用户
            var userId = _httpContextAccessor
                .HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // 2. 开始处理支付
            var order = await _touristRouteRepository.GetOrderById(orderId);
            order?.PaymentProcessing();
            await _touristRouteRepository.SaveAsync();

            // 3. 向第三方提交支付请求
            var httpClient = _httpClientFactory.CreateClient();
            string url = @"https://localhost:3001/api/FakeVanderPaymentProcess?orderNumber={0}&returnFault={1}";
            var response = await httpClient.PostAsync(
                string.Format(url, order?.Id, false)
                , null);

            // 4. 提取支付结果，以及支付信息
            bool isApproved = false;
            string transactionMetadata = "";
            if (response.IsSuccessStatusCode)
            {
                transactionMetadata = await response.Content.ReadAsStringAsync();
                JObject? jsonObject = JsonConvert.DeserializeObject<JObject>(transactionMetadata);
                isApproved = jsonObject?["approved"]?.Value<bool>() ?? false;
            }

            // 5. 如果第三方支付成功. 完成订单
            if (isApproved)
            {
                order?.PaymentApprove();
            }
            else
            {
                order?.PaymentReject();
            }
            order = order ?? throw new ArgumentNullException(nameof(order), "訂單不存在");
            order.TransactionMetadata = transactionMetadata;
            await _touristRouteRepository.SaveAsync();

            return Ok(_mapper.Map<OrderDto>(order));
        }
    }
}
