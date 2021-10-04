namespace Tgs.Global.ApplicationServices.Brands
{
    public record CreateBrandPlayerCommand(
        string BrandCode,
        string CurrencyCode,
        PlayerType PlayerType);
}