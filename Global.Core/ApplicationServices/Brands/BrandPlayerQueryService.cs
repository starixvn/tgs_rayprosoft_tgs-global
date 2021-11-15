using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tgs.ServiceUtility;

namespace Tgs.Global.ApplicationServices.Brands
{
    public class BrandPlayerQueryService :
        IGetService<BrandPlayerQuery, int>,
        IQueryService<BrandPlayerQuery, BrandPlayerInfo>
    {
        private readonly ITgsRepository _repository;

        public BrandPlayerQueryService(ITgsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public ValueTask<int> GetAsync(
            BrandPlayerQuery query,
            CancellationToken cancellationToken = default)
            => _repository.CountBrandPlayersAsync(
                new BrandPlayerQueryConditions(
                    query.BrandCode,
                    query.CurrencyCode,
                    query.PlayerType));

        public IAsyncEnumerable<BrandPlayerInfo> QueryAsync(
            BrandPlayerQuery query, CancellationToken cancellationToken = default)
            => _repository.QueryBrandPlayersAsync(
                new BrandPlayerQueryConditions(
                    query.BrandCode,
                    query.CurrencyCode,
                    query.PlayerType),
                query.Paging);
    }
}
