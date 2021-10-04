using System;
using System.Threading;
using System.Threading.Tasks;
using TGS.ServiceUtility;

namespace Tgs.Global.ApplicationServices.Brands
{
    public class CreateBrandPlayerCommandService : ICommandService<CreateBrandPlayerCommand>
    {
        private readonly ITgsRepository _tgsRepository;

        public CreateBrandPlayerCommandService(
            ITgsRepository tgsRepository)
        {
            _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
        }

        public async ValueTask ExecuteAsync(
            CreateBrandPlayerCommand command, CancellationToken cancellationToken = default)
        {
            var brandId = await _tgsRepository.QueryBrandIdByCodeAsync(command.BrandCode)
                .ConfigureAwait(true);

            var betLimitId = await _tgsRepository.QueryDefaultBetLimitIdAsync(brandId, command.CurrencyCode, true);
        }
    }
}