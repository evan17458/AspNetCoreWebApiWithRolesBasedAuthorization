using WebApiWithRoleAuthentication.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using WebApiWithRoleAuthentication.Dtos;
using WebApiWithRoleAuthentication.ResourceParameters;
using WebApiWithRoleAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using WebApiWithRoleAuthentication.Helper;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using Microsoft.AspNetCore.JsonPatch;
namespace WebApiWithRoleAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper = default!;
        public TouristRoutesController(
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper,
             IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor
        )
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext!);
        }
        private string GenerateTouristRouteResourceURL(
        TouristRouteResourceParamaters paramaters,
        PaginationResourceParamaters paramaters2,
        ResourceUriType type
)
        {
            var link = type switch
            {
                ResourceUriType.PreviousPage => _urlHelper.Link("GetTouristRoutes", new
                {
                    keyword = paramaters.Keyword,
                    rating = paramaters.Rating,
                    pageNumber = paramaters2.PageNumber - 1,
                    pageSize = paramaters2.PageSize
                }),
                ResourceUriType.NextPage => _urlHelper.Link("GetTouristRoutes", new
                {
                    keyword = paramaters.Keyword,
                    rating = paramaters.Rating,
                    pageNumber = paramaters2.PageNumber + 1,
                    pageSize = paramaters2.PageSize
                }),
                _ => _urlHelper.Link("GetTouristRoutes", new
                {
                    keyword = paramaters.Keyword,
                    rating = paramaters.Rating,
                    pageNumber = paramaters2.PageNumber,
                    pageSize = paramaters2.PageSize
                })
            };

            if (link == null)
            {
                throw new InvalidOperationException("產生 URL 失敗，請確認 Route 名稱是否正確。");
            }

            return link;
        }

        [HttpGet(Name = "GetTouristRoutes")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GerTouristRoutes(
            [FromQuery] TouristRouteResourceParamaters paramaters,
            [FromQuery] PaginationResourceParamaters paramaters2
        )
        {
            var touristRoutesFromRepo = await _touristRouteRepository.GetTouristRoutesAsync(paramaters.Keyword, paramaters.RatingOperator, paramaters.RatingValue,
                    paramaters2.PageSize,
                    paramaters2.PageNumber,
                    paramaters.orderBy
                   );
            if (touristRoutesFromRepo == null || touristRoutesFromRepo.Count() <= 0)
            {
                return NotFound("没有旅游路線");
            }
            var touristRoutesDto = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo);

            var previousPageLink = touristRoutesFromRepo.HasPrevious
              ? GenerateTouristRouteResourceURL(
             paramaters, paramaters2, ResourceUriType.PreviousPage)
                 : null;

            var nextPageLink = touristRoutesFromRepo.HasNext
                ? GenerateTouristRouteResourceURL(
                    paramaters, paramaters2, ResourceUriType.NextPage)
                : null;

            // x-pagination
            var paginationMetadata = new
            {
                previousPageLink,
                nextPageLink,
                totalCount = touristRoutesFromRepo.TotalCount,
                pageSize = touristRoutesFromRepo.PageSize,
                currentPage = touristRoutesFromRepo.CurrentPage,
                totalPages = touristRoutesFromRepo.TotalPages
            };

            Response.Headers.Append("x-pagination", JsonConvert.SerializeObject(paginationMetadata));
            return Ok(touristRoutesDto);
        }

        // api/touristroutes/{touristRouteId}
        [HttpGet("{touristRouteId}", Name = "GetTouristRouteById")]
        public async Task<IActionResult> GetTouristRouteById(Guid touristRouteId)
        {
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            if (touristRouteFromRepo == null)
            {
                return NotFound($"旅游路線{touristRouteId}找不到");
            }
            var touristRouteDto = _mapper.Map<TouristRouteDto>(touristRouteFromRepo);
            return Ok(touristRouteDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreationDto)
        {
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteForCreationDto);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            await _touristRouteRepository.SaveAsync();
            var touristRouteToReture = _mapper.Map<TouristRouteDto>(touristRouteModel);
            return CreatedAtRoute(
                "GetTouristRouteById",
                new { touristRouteId = touristRouteToReture.Id },
                touristRouteToReture
            );
        }
        [HttpPut("{touristRouteId}")]

        public async Task<IActionResult> UpdateTouristRoute(
       [FromRoute] Guid touristRouteId,
        [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto
)
        {
            if (!await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
            {
                return NotFound("旅游路线找不到");
            }

            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);

            _mapper.Map(touristRouteForUpdateDto, touristRouteFromRepo);

            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }
        [HttpPatch("{touristRouteId}")]
        //  [Authorize(AuthenticationSchemes = "Bearer")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PartiallyUpdateTouristRoute(
        [FromRoute] Guid touristRouteId,
        [FromBody] JsonPatchDocument<TouristRouteForUpdateDto> patchDocument
)
        {
            if (!await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
            {
                return NotFound("旅游路线找不到");
            }

            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            var touristRouteToPatch = _mapper.Map<TouristRouteForUpdateDto>(touristRouteFromRepo);
            patchDocument.ApplyTo(touristRouteToPatch);
            if (!TryValidateModel(touristRouteToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(touristRouteToPatch, touristRouteFromRepo);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{touristRouteId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTouristRoute([FromRoute] Guid touristRouteId)
        {
            if (!await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
            {
                return NotFound("旅游路線找不到");
            }

            var touristRoute = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);

            if (touristRoute is not null)
            {
                _touristRouteRepository.DeleteTouristRoute(touristRoute);
                await _touristRouteRepository.SaveAsync();
            }


            return NoContent();
        }
    }
}