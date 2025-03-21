using WebApiWithRoleAuthentication.Models;

namespace WebApiWithRoleAuthentication.Services
{
    public interface ITouristRouteRepository
    {
        IEnumerable<TouristRoute> GetTouristRoutes(string? keyword, string? ratingOperator, int? ratingValue);
        TouristRoute? GetTouristRoute(Guid touristRouteId);
        bool TouristRouteExists(Guid touristRouteId);
        IEnumerable<TouristRoutePicture> GetPicturesByTouristRouteId(Guid touristRouteId);

        TouristRoutePicture? GetPicture(int pictureId);
    }
}
