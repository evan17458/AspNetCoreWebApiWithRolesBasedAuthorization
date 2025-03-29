using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiWithRoleAuthentication.Dtos;
using WebApiWithRoleAuthentication.Helper;
using WebApiWithRoleAuthentication.Models;
using WebApiWithRoleAuthentication.Services;


namespace WebApiWithRoleAuthentication.Controllers
{
    [ApiController]
    [Route("api/shoppingCart")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public ShoppingCartController(
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
        public async Task<IActionResult> GetShoppingCart()
        {
            // 1 取得當前用户
            var userId = _httpContextAccessor
             .HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // 2 使用userid取得購物車
            var shoppingCart = await _touristRouteRepository.GetShoppingCartByUserId(userId);

            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCart));
        }

        [HttpPost("items")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddShoppingCartItem(
        [FromBody] AddShoppingCartItemDto addShoppingCartItemDto
)
        {
            // 1 取得當前用户
            var userId = _httpContextAccessor
                .HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (userId == null)
            {
                return NotFound("使用者id不存在在");
            }
            // 2 使用userid取得購物車
            var shoppingCart = await _touristRouteRepository
                .GetShoppingCartByUserId(userId);
            if (shoppingCart == null)
            {
                return NotFound($"用戶 {userId} 的購物車不存在");
            }

            // 3 創建lineItem
            var touristRoute = await _touristRouteRepository
                .GetTouristRouteAsync(addShoppingCartItemDto.TouristRouteId);
            if (touristRoute == null)
            {
                return NotFound("旅游路線不存在");
            }

            var lineItem = new LineItem()
            {
                TouristRouteId = addShoppingCartItemDto.TouristRouteId,
                ShoppingCartId = shoppingCart?.Id,
                OriginalPrice = touristRoute.OriginalPrice,
                DiscountPresent = touristRoute.DiscountPresent
            };

            // 4 添加lineitem，並保存資料庫
            await _touristRouteRepository.AddShoppingCartItem(lineItem);
            await _touristRouteRepository.SaveAsync();
            Console.WriteLine($"盛建雄ShoppingCart: Id={shoppingCart?.Id}, UserId={shoppingCart?.UserId} ");
            return Ok(shoppingCart);
        }

        [HttpDelete("items/{itemId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteShoppingCartItem([FromRoute] int itemId)
        {
            // 1 取lineitem資料
            var lineItem = await _touristRouteRepository
                .GetShoppingCartItemByItemId(itemId);
            if (lineItem == null)
            {
                return NotFound("購物車商品找不到");
            }

            _touristRouteRepository.DeleteShoppingCartItem(lineItem);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("items/({itemIDs})")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveShoppingCartItems(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))]
         [FromRoute] IEnumerable<int> itemIDs
)
        {
            var lineitems = await _touristRouteRepository
                .GeshoppingCartsByIdListAsync(itemIDs);

            _touristRouteRepository.DeleteShoppingCartItems(lineitems);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        [HttpPost("checkout")]//購物車商品加入訂單
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Checkout()
        {
            // 1 取得當前用户
            var userId = _httpContextAccessor
                .HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return NotFound("使用者id不存在在");
            }
            // 2 使用userid取得購物車
            var shoppingCart = await _touristRouteRepository.GetShoppingCartByUserId(userId);

            if (shoppingCart == null)
            {
                return NotFound($"用戶 {userId} 的購物車不存在");
            }

            if (shoppingCart.ShoppingCartItems == null)
            {
                return NotFound($"用戶 {shoppingCart.ShoppingCartItems} 的購物車item不存在");
            }
            // 3 創建訂單
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                State = OrderStateEnum.Pending,
                OrderItems = shoppingCart?.ShoppingCartItems, // 如果 shoppingCart 為空，OrderItems 會是 null
                CreateDateUTC = DateTime.UtcNow,
            };
            // 4 保存資料
            await _touristRouteRepository.AddOrderAsync(order);
            if (shoppingCart != null)
            {
                shoppingCart.ShoppingCartItems = null;//清空購物車
            }
            await _touristRouteRepository.SaveAsync();


            return Ok(_mapper.Map<OrderDto>(order));
        }




    }
}
