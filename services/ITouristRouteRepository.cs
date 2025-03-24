using WebApiWithRoleAuthentication.Models;

namespace WebApiWithRoleAuthentication.Services
{
    public interface ITouristRouteRepository
    {

        Task<IEnumerable<TouristRoute?>> GetTouristRoutesAsync(string? keyword, string? ratingOperator, int? ratingValue, int pageSize, int pageNumber, string? orderBy);
        Task<TouristRoute?> GetTouristRouteAsync(Guid touristRouteId);
        Task<bool> TouristRouteExistsAsync(Guid touristRouteId);
        Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid touristRouteId);
        Task<TouristRoutePicture?> GetPictureAsync(int pictureId);
        Task<IEnumerable<TouristRoute>> GetTouristRoutesByIDListAsync(IEnumerable<Guid> ids);
        void AddTouristRoute(TouristRoute touristRoute);
        void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture);

        bool TouristRouteExists(Guid touristRouteId);
        void DeleteTouristRoute(TouristRoute touristRoute);
        Task<ShoppingCart?> GetShoppingCartByUserId(string? userId);
        Task CreateShoppingCart(ShoppingCart shoppingCart);

        Task AddShoppingCartItem(LineItem lineItem);

        Task<LineItem?> GetShoppingCartItemByItemId(int lineItemId);
        void DeleteShoppingCartItem(LineItem lineItem);
        Task<IEnumerable<LineItem>> GeshoppingCartsByIdListAsync(IEnumerable<int> ids);
        void DeleteShoppingCartItems(IEnumerable<LineItem> lineItems);
        Task AddOrderAsync(Order order);

        Task<IEnumerable<Order>> GetOrdersByUserId(string userId);
        Task<Order?> GetOrderById(Guid orderId);
        Task<bool> SaveAsync();
    }
}
