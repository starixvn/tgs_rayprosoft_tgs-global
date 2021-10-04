namespace Tgs.Global
{
    public record BrandPlayerQueryConditions(
        string BrandCode,
        string CurrencyCode,
        PlayerType PlayerType);
}