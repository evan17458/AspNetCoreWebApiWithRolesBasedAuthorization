
using WebApiWithRoleAuthentication.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using WebApiWithRoleAuthentication.Dtos;
using System.Text.RegularExpressions;
namespace WebApiWithRoleAuthentication.Controllers
{
    [Route("api/[controller]")] // api/touristroute
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;
        public TouristRoutesController(
      ITouristRouteRepository touristRouteRepository,
      IMapper mapper
  )
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GerTouristRoutes(
        [FromQuery] string keyword,
        string rating // 小于lessThan, 大于largerThan, 等于equalTo lessThan3, largerThan2, equalTo5 
         )// FromQuery vs FromBody
        {
            Regex regex = new Regex(@"([A-Za-z0-9\-]+)(\d+)");
            string operatorType = "";
            int raringVlaue = -1;
            Match match = regex.Match(rating);
            if (match.Success)
            {
                operatorType = match.Groups[1].Value;
                raringVlaue = Int32.Parse(match.Groups[2].Value);
            }
            var touristRoutesFromRepo = _touristRouteRepository.GetTouristRoutes(keyword, operatorType, raringVlaue);
            if (touristRoutesFromRepo == null || touristRoutesFromRepo.Count() <= 0)
            {
                return NotFound("没有旅游路线");
            }
            var touristRoutesDto = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo);
            return Ok(touristRoutesDto);
        }

        // api/touristroutes/{touristRouteId}
        [HttpGet("{touristRouteId}")]
        public IActionResult GetTouristRouteById(Guid touristRouteId)
        {
            var touristRouteFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);
            if (touristRouteFromRepo == null)
            {
                return NotFound($"旅游路线{touristRouteId}找不到");
            }
            var touristRouteDto = _mapper.Map<TouristRouteDto>(touristRouteFromRepo);
            return Ok(touristRouteDto);
        }

    }
}