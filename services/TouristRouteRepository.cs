using Microsoft.EntityFrameworkCore;
using WebApiWithRoleAuthentication.Data;
using WebApiWithRoleAuthentication.Models;

namespace WebApiWithRoleAuthentication.Services
{
    public class TouristRouteRepository : ITouristRouteRepository
    {
        private readonly AppDbContext _context;

        public TouristRouteRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<TouristRoute?> GetTouristRouteAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefaultAsync(n => n.Id == touristRouteId);
        }
        public bool TouristRouteExists(Guid touristRouteId)
        {
            return _context.TouristRoutes.Any(t => t.Id == touristRouteId);
        }
        public async Task<IEnumerable<TouristRoute?>> GetTouristRoutesAsync(
            string? keyword,
            string? ratingOperator,
            int? ratingValue,
            int pageSize,
            int pageNumber,
            string? orderBy
        )
        {
            IQueryable<TouristRoute> result = _context
                .TouristRoutes
                .Include(t => t.TouristRoutePictures);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                result = result.Where(t => (t.Title ?? string.Empty).Contains(keyword));
            }
            if (ratingValue >= 0)
            {
                result = ratingOperator switch
                {
                    "largerThan" => result.Where(t => t.Rating >= ratingValue),
                    "lessThan" => result.Where(t => t.Rating <= ratingValue),
                    _ => result.Where(t => t.Rating == ratingValue),
                };
            }


            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                if (orderBy.ToLowerInvariant() == "originalprice")
                {
                    result = result.OrderBy(t => t.OriginalPrice);
                }

                //result.ApplySort(orderBy, _mappingDictionary);
            }

            // pagination
            // skip
            var skip = (pageNumber - 1) * pageSize;
            result = result.Skip(skip);
            // 以pagesize為標準顯示一定量的資料
            result = result.Take(pageSize);
            // include vs join


            return await result.ToListAsync();
        }

        public async Task<bool> TouristRouteExistsAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.AnyAsync(t => t.Id == touristRouteId);
        }

        public async Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutePictures
                .Where(p => p.TouristRouteId == touristRouteId).ToListAsync();
        }

        public async Task<TouristRoutePicture?> GetPictureAsync(int pictureId)
        {
            return await _context.TouristRoutePictures.Where(p => p.Id == pictureId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesByIDListAsync(IEnumerable<Guid> ids)
        {
            return await _context.TouristRoutes.Where(t => ids.Contains(t.Id)).ToListAsync();
        }

        public void AddTouristRoute(TouristRoute touristRoute)
        {
            if (touristRoute == null)
            {
                throw new ArgumentNullException(nameof(touristRoute));
            }
            _context.TouristRoutes.Add(touristRoute);
            //_context.SaveChanges();
        }

        public void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture)
        {
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            if (touristRoutePicture == null)
            {
                throw new ArgumentNullException(nameof(touristRoutePicture));
            }
            touristRoutePicture.TouristRouteId = touristRouteId;
            _context.TouristRoutePictures.Add(touristRoutePicture);
        }

        public void DeleteTouristRoute(TouristRoute touristRoute)
        {
            _context.TouristRoutes.Remove(touristRoute);
        }
        public async Task<ShoppingCart?> GetShoppingCartByUserId(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            return await _context!.ShoppingCarts
                .Include(s => s.User)
                .Include(s => s.ShoppingCartItems!)
                .ThenInclude(li => li.TouristRoute)
                .Where(s => s.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task CreateShoppingCart(ShoppingCart shoppingCart)
        {
            await _context.ShoppingCarts.AddAsync(shoppingCart);
        }
        public async Task AddShoppingCartItem(LineItem lineItem)
        {
            await _context.LineItems.AddAsync(lineItem);
        }

        public async Task<LineItem?> GetShoppingCartItemByItemId(int lineItemId)
        {
            return await _context.LineItems
                .Where(li => li.Id == lineItemId)
                .FirstOrDefaultAsync();
        }

        public void DeleteShoppingCartItem(LineItem lineItem)
        {
            _context.LineItems.Remove(lineItem);
        }

        public async Task<IEnumerable<LineItem>> GeshoppingCartsByIdListAsync(
        IEnumerable<int> ids)
        {
            return await _context.LineItems
                .Where(li => ids.Contains(li.Id))
                .ToListAsync();
        }

        public void DeleteShoppingCartItems(IEnumerable<LineItem> lineItems)
        {
            _context.LineItems.RemoveRange(lineItems);
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserId(string userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<Order?> GetOrderById(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems!).ThenInclude(oi => oi.TouristRoute)
                .Where(o => o.Id == orderId)
                .FirstOrDefaultAsync();
        }


        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}