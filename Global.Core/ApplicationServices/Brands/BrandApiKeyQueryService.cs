using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TGS.ServiceUtility;

namespace Tgs.Global.ApplicationServices.Brands
{
    internal class BrandApiKeyQueryService :
        IQueryService<BrandApiKeyQuery, BrandApiKeyInfo>
    {
        private readonly ITgsRepository _repository;

        public BrandApiKeyQueryService(ITgsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IAsyncEnumerable<BrandApiKeyInfo> QueryAsync(
            BrandApiKeyQuery query, CancellationToken cancellationToken = default)
        {
            var licenseeCodes = query.LicenseeCodes.ToArray();
            var items = _repository.QueryBrandApiKeysAsync();

            if (licenseeCodes.Length == 0)
            {
                return items;
            }

            return items
                .Where(info => licenseeCodes.Contains(info.LicenseeCode))
                .Select(info => info);
        }
    }
}