using TGS.Paging;

namespace Tgs.Global.ApplicationServices.Brands
{
    public record BrandPlayerQuery(
        string BrandCode,
        string CurrencyCode,
        PlayerType PlayerType,
        Paging<BrandPlayerInfo> Paging);
}