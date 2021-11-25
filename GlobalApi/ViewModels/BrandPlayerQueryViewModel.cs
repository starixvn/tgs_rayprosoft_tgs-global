using Microsoft.AspNetCore.Mvc;

namespace Tgs.Global.ViewModels
{
    public class BrandPlayerQueryViewModel
    {
        [FromQuery(Name = "brandcode")]
        public string BrandCode { get; init; } = default!;

        [FromQuery(Name = "currency")]
        public string CurrencyCode { get; init; } = default!;

        [FromQuery(Name = "playertype")]
        public PlayerType PlayerType { get; init; }
    }
}