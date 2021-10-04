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
    }
}