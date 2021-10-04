using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tgs.Global.Repositories.Entities;
using TGS.Paging;

namespace Tgs.Global.Repositories
{
    internal class TgsRepository : ITgsRepository
    {
        private readonly TgsContext _dbContext;

        public TgsRepository(TgsContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IAsyncEnumerable<BrandApiKeyInfo> QueryBrandApiKeysAsync()
            => _dbContext.Brands.AsQueryable()
                .Select(b => new BrandApiKeyInfo(b.Code, b.ApiKey))
                .AsAsyncEnumerable();

        public IAsyncEnumerable<BrandApiKeyInfo> QueryBrandApiKeysAsync(Paging<BrandApiKeyInfo> paging)
        {
            var dataQuery = _dbContext.Brands.AsQueryable()
                .Select(b => new BrandApiKeyInfo(b.Code, b.ApiKey));

            dataQuery = dataQuery.ApplyOrderBy(paging).ApplyPaging(paging);

            return dataQuery.AsAsyncEnumerable();
        }

        public async ValueTask<int> CountBrandApiKeysAsync()
            => await _dbContext.Brands.AsQueryable()
                .CountAsync().ConfigureAwait(false);
    }
}