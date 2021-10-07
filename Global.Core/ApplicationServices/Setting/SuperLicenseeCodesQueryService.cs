using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using TGS.ServiceUtility;

namespace Tgs.Global.ApplicationServices.Setting
{
    internal class SuperLicenseeCodesQueryService : IQueryService<SuperLicenseeCodesQuery, string>
    {
        private readonly ITgsRepository _repository;

        public SuperLicenseeCodesQueryService(ITgsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async IAsyncEnumerable<string> QueryAsync(
            SuperLicenseeCodesQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var value = await _repository.QuerySettingAsync(new SettingQueryConditions("SuperLicenseeCodes"))
                .ConfigureAwait(false);

            if (string.IsNullOrEmpty(value))
                yield break;

            var values = JsonSerializer.Deserialize<string[]>(value);

            foreach (var item in values ?? Array.Empty<string>())
                yield return item;
        }
    }
}