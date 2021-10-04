using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TGS.ServiceUtility;

namespace Tgs.Global.ApplicationServices.Brands
{
    internal class BrandApiKeyQueryService :
        IQueryService<BrandApiKeyQuery, BrandApiKeyInfo>,
        IGetService<BrandApiKeyQuery, int>
    {
        private readonly ITgsRepository _repository;

        public BrandApiKeyQueryService(ITgsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public ValueTask<int> GetAsync(BrandApiKeyQuery query, CancellationToken cancellationToken = default)
            => _repository.CountBrandApiKeysAsync();

        public IAsyncEnumerable<BrandApiKeyInfo> QueryAsync(
            BrandApiKeyQuery query, CancellationToken cancellationToken = default)
            => query.Paging == null
                ? _repository.QueryBrandApiKeysAsync()
                : _repository.QueryBrandApiKeysAsync(query.Paging);
    }
}