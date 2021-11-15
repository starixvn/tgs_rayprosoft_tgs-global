using System;
using System.Threading;
using System.Threading.Tasks;
using Tgs.ServiceUtility;

namespace Tgs.Global.ApplicationServices.Brands
{
    internal class GetBrandIdByCodeQueryService : IGetService<GetBrandIdByCodeQuery, Guid>
    {
        private readonly ITgsRepository _tgsRepository;

        public GetBrandIdByCodeQueryService(ITgsRepository tgsRepository)
        {
            _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
        }

        public ValueTask<Guid> GetAsync(GetBrandIdByCodeQuery query, CancellationToken cancellationToken = default)
            => _tgsRepository.QueryBrandIdByCodeAsync(query.BrandCode);
    }
}
