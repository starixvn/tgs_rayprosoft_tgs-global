using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tgs.Global.ApplicationServices.Brands;
using Tgs.Global.ViewModels;
using TGS.Paging;
using TGS.ServiceUtility;

namespace Tgs.Global.Controllers
{
    [Route("global-api/[controller]")]
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class BrandController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("apikeys")]
        public async ValueTask<PagingResponseViewModel<BrandApiKeyViewModel>> GetBrandApiKeysAsync(
            [FromServices] IGetService<BrandApiKeyQuery, int> countService,
            [FromServices] IQueryService<BrandApiKeyQuery, BrandApiKeyInfo> queryService,
            [FromQuery] PagingViewModel paging)
        {
            if (paging.ShowAll == true)
            {
                var query = new BrandApiKeyQuery(null);

                var totalCount = await countService.GetAsync(query).ConfigureAwait(false);

                var items = queryService.QueryAsync(query)
                    .Select(row =>
                        new BrandApiKeyViewModel(
                            row.BrandCode,
                            row.ApiKey));

                return new PagingResponseViewModel<BrandApiKeyViewModel>
                {
                    TotalCount = totalCount,
                    Page = 1,
                    Rows = -1,
                    Items = items.ToEnumerable()
                };
            }
            else
            {
                var sourcePaging = new Paging<BrandApiKeyInfo>
                {
                    Page = paging.Page ?? 1,
                    RowCount = paging.Rows ?? 10
                };

                if (!string.IsNullOrWhiteSpace(paging.SortBy))
                    sourcePaging.SortByPropertyName(paging.SortBy, paging.IsDesc ?? false);

                var query = new BrandApiKeyQuery(sourcePaging);

                var totalCount = await countService.GetAsync(query).ConfigureAwait(false);

                var items = queryService.QueryAsync(query)
                    .Select(row =>
                        new BrandApiKeyViewModel(
                            row.BrandCode,
                            row.ApiKey));

                return new PagingResponseViewModel<BrandApiKeyViewModel>
                {
                    TotalCount = totalCount,
                    Page = sourcePaging.Page,
                    Rows = sourcePaging.RowCount,
                    Items = items.ToEnumerable()
                };
            }
        }

        //[HttpGet]
        //[AllowAnonymous]
        //[Route("players/mocksite")]
        //public async ValueTask<PagedResponse<BrandPlayersResult>> GetBrandPlayersForMockSiteAsync(
        //    [FromQuery] GetBrandPlayersRequest request)
        //    => await _mediator.Send(request).ConfigureAwait(false);

        ///// <summary>
        /////     This api will create a new player when the player is not exist, and login with desktop
        ///// </summary>
        //[HttpPost]
        //[AllowAnonymous]
        //[Route("player/createandlogin")]
        //public async ValueTask<CreatePlayerAndLoginResponse> CreatePlayerAndLoginAsync(
        //    CreatePlayerAndLoginRequest request)
        //    => await _mediator.Send(request).ConfigureAwait(false);
    }
}