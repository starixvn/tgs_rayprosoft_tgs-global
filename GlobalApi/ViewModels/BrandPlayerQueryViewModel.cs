using Microsoft.AspNetCore.Mvc;

namespace Tgs.Global.ViewModels
{
    public class BrandPlayerQueryViewModel
    {
        [FromQuery(Name = "brandcode")]
        public string BrandCode { get; init; }

        [FromQuery(Name = "currency")]
        public string CurrencyCode { get; init; }

        [FromQuery(Name = "playertype")]
        public PlayerType PlayerType { get; init; }
    }
}