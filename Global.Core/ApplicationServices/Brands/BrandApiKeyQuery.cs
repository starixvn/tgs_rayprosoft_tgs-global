using System.Collections.Generic;

namespace Tgs.Global.ApplicationServices.Brands
{
    public record BrandApiKeyQuery(
        IEnumerable<string> LicenseeCodes);
}