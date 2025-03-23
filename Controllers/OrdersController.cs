using AutoMapper;
using WebApiWithRoleAuthentication.Dtos;
using WebApiWithRoleAuthentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace WebApiWithRoleAuthentication.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public OrdersController(
            IHttpContextAccessor httpContextAccessor,
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
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

    }
}
