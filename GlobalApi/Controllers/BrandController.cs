using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Tgs.BrandCertificate.Clients;
using Tgs.Global.ApplicationServices.Brands;
using Tgs.Global.ApplicationServices.Setting;
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
        public async ValueTask<ResponseViewModel<BrandApiKeyViewModel>> GetBrandApiKeysAsync(
            [FromServices] IWebHostEnvironment environment,
            [FromServices] IQueryService<SuperLicenseeCodesQuery, string> queryLicenseeCodesService,
            [FromServices] IQueryService<BrandApiKeyQuery, BrandApiKeyInfo> queryApiKeyService)
        {
            var licenseeCodes = environment.IsProduction()
                ? await queryLicenseeCodesService.QueryAsync(new SuperLicenseeCodesQuery())
                    .ToArrayAsync()
                    .ConfigureAwait(false)
                : Array.Empty<string>();

            var query = new BrandApiKeyQuery(licenseeCodes);

            var items = queryApiKeyService.QueryAsync(query)
                .Select(row =>
                    new BrandApiKeyViewModel(
                        row.BrandCode,
                        row.ApiKey));

            return new ResponseViewModel<BrandApiKeyViewModel>
            {
                Items = items.ToEnumerable()
            };
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("players/mocksite")]
        public async ValueTask<PagingResponseViewModel<BrandPlayerViewModel>> GetBrandPlayersForMockSiteAsync(
            [FromServices] IGetService<BrandPlayerQuery, int> countService,
            [FromServices] IQueryService<BrandPlayerQuery, BrandPlayerInfo> queryService,
            [FromServices] ICommandService<CreateBrandPlayerCommand> commandService,
            [FromQuery] BrandPlayerQueryViewModel playerQuery,
            [FromQuery] PagingViewModel paging)
        {
            var sourcePaging = new Paging<BrandPlayerInfo>();

            if (paging.ShowAll == true)
            {
                // if ShowAll = true, return the first page with default row counts
                sourcePaging.Page = 1;
                sourcePaging.RowCount = 10;
            }
            else
            {
                sourcePaging.Page = paging.Page ?? 1;
                sourcePaging.RowCount = paging.Rows ?? 10;
            }

            if (!string.IsNullOrWhiteSpace(paging.SortBy))
                sourcePaging.SortByPropertyName(paging.SortBy, paging.IsDesc ?? false);

            var query = new BrandPlayerQuery(
                playerQuery.BrandCode,
                playerQuery.CurrencyCode,
                playerQuery.PlayerType,
                sourcePaging);

            var totalCount = await countService.GetAsync(query).ConfigureAwait(false);

            if (totalCount == 0)
            {
                var command = new CreateBrandPlayerCommand(
                    playerQuery.BrandCode,
                    playerQuery.CurrencyCode,
                    playerQuery.PlayerType);

                await commandService.ExecuteAsync(command).ConfigureAwait(false);

                totalCount = await countService.GetAsync(query).ConfigureAwait(false);
            }

            var items = queryService.QueryAsync(query)
                .Select(row =>
                    new BrandPlayerViewModel
                    {
                        PlayerCode = row.PlayerCode,
                        UserName = row.UserName,
                        Status = row.Status
                    });

            return new PagingResponseViewModel<BrandPlayerViewModel>
            {
                TotalCount = totalCount,
                Page = sourcePaging.Page,
                Rows = sourcePaging.RowCount,
                Items = items.ToEnumerable()
            };
        }

        [HttpGet("token/{brandCode}")]
        public async Task<string?> MakeBrandTokenAsync(
            [FromServices] IGetService<GetBrandIdByCodeQuery, Guid> getBrandIdService,
            [FromServices] IBrandCertificateClient brandCertificateClient,
            string brandCode)
        {
            var brandId = await getBrandIdService
                .GetAsync(new GetBrandIdByCodeQuery(brandCode))
                .ConfigureAwait(false);

            return await brandCertificateClient
                .MakeJsonWebTokenAsync(brandId)
                .ConfigureAwait(false);
        }
    }
}