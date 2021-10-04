using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TGS.Paging;

namespace Tgs.Global
{
    public interface ITgsRepository
    {
        IAsyncEnumerable<BrandApiKeyInfo> QueryBrandApiKeysAsync();

        IAsyncEnumerable<BrandApiKeyInfo> QueryBrandApiKeysAsync(Paging<BrandApiKeyInfo> paging);

        ValueTask<int> CountBrandApiKeysAsync();

        ValueTask<int> CountBrandPlayersAsync(BrandPlayerQueryConditions conditions);

        IAsyncEnumerable<BrandPlayerInfo> QueryBrandPlayersAsync(
            BrandPlayerQueryConditions conditions, Paging<BrandPlayerInfo> paging);

        ValueTask<Guid> QueryBrandIdByCodeAsync(string brandCode);

        ValueTask<int> QueryDefaultBetLimitIdAsync(Guid brandId, string currencyCode, bool isDefault);
    }
}