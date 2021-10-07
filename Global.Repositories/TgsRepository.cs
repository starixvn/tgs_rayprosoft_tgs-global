using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tgs.Global.Repositories.Entities;
using Tgs.Global.Repositories.Entities.Models;
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

        public IAsyncEnumerable<BrandPlayerInfo> QueryBrandPlayersAsync(
            BrandPlayerQueryConditions conditions,
            Paging<BrandPlayerInfo> paging)
        {
            var dataQuery = MakeBrandPlayersQueryAsync(conditions)
                .Select(p => new BrandPlayerInfo(p.PlayerCode, p.Username, (PlayerStatus)p.Status));

            dataQuery = dataQuery.ApplyOrderBy(paging).ApplyPaging(paging);

            return dataQuery.AsAsyncEnumerable();
        }

        public ValueTask<int> CountBrandPlayersAsync(
            BrandPlayerQueryConditions conditions)
        {
            var dataQuery = MakeBrandPlayersQueryAsync(conditions);

            return dataQuery.CountAsync();
        }

        public async ValueTask<Guid> QueryBrandIdByCodeAsync(string brandCode)
        {
            var brand = await _dbContext.Brands.AsQueryable()
                .FirstOrDefaultAsync(b => b.Code == brandCode)
                .ConfigureAwait(false);

            return brand.Id;
        }

        public async ValueTask<int> QueryDefaultBetLimitIdAsync(Guid brandId, string currencyCode, bool isDefault)
        {
            var betLimit =
                await _dbContext.BrandBetLimits.AsQueryable()
                    .FirstOrDefaultAsync(bl =>
                        bl.BrandId == brandId &&
                        bl.BetLimit.CurrencyCode == currencyCode &&
                        bl.IsDefault == isDefault).ConfigureAwait(false);

            return betLimit.BetLimitId;
        }

        public IAsyncEnumerable<BrandApiKeyInfo> QueryBrandApiKeysAsync()
            => _dbContext.Brands.AsQueryable()
                .Select(b => new BrandApiKeyInfo(b.Code, b.Licensee.Code, b.ApiKey))
                .AsAsyncEnumerable();

        public async ValueTask<string> QuerySettingAsync(SettingQueryConditions conditions)
            => await _dbContext.Settings.AsQueryable()
                .Where(s => s.Key == conditions.Key)
                .Select(s => s.Value)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

        private IAsyncEnumerable<Player> MakeBrandPlayersQueryAsync(
            BrandPlayerQueryConditions conditions)
        {
            var dataQuery = _dbContext.Players.AsQueryable()
                .Where(p => p.Brand.Code == conditions.BrandCode &&
                    p.DefaultWalletCurrencyCode == conditions.CurrencyCode &&
                    p.Type == (short)conditions.PlayerType);

            return dataQuery.AsAsyncEnumerable();
        }
    }
}