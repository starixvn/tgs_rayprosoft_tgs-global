using TGS.Paging;

namespace Tgs.Global.ApplicationServices.Brands
{
    public record BrandApiKeyQuery(
        Paging<BrandApiKeyInfo>? Paging);
}