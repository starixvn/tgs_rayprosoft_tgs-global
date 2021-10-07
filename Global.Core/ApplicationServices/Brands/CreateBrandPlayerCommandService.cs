using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tgs.Core.Infrastructures.Security;
using Tgs.Global.Utilities;
using TGS.ServiceUtility;

namespace Tgs.Global.ApplicationServices.Brands
{
    internal class CreateBrandPlayerCommandService : ICommandService<CreateBrandPlayerCommand>
    {
        private static readonly int _NewPlayersCount = 2;
        private readonly IPasswordHandler _passwordHandler;

        private readonly ITgsRepository _repository;

        public CreateBrandPlayerCommandService(
            ITgsRepository repository,
            IPasswordHandler passwordHandler)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _passwordHandler = passwordHandler ?? throw new ArgumentNullException(nameof(passwordHandler));
        }

        public async ValueTask ExecuteAsync(
            CreateBrandPlayerCommand command, CancellationToken cancellationToken = default)
        {
            var brandId = await _repository.QueryBrandIdByCodeAsync(command.BrandCode)
                .ConfigureAwait(true);

            var betLimitId = await _repository.QueryDefaultBetLimitIdAsync(brandId, command.CurrencyCode, true)
                .ConfigureAwait(false);

            var prefix = command.PlayerType == PlayerType.RealPlayer ? "RP" : "TP";

            var players = Enumerable.Range(0, _NewPlayersCount).Select(idx =>
                CreatePlayer(brandId, command.CurrencyCode, command.PlayerType, betLimitId, prefix, idx + 1));

            await _repository.CreatePlayersAsync(players).ConfigureAwait(false);
        }

        private PlayerCreateInfo CreatePlayer(
            Guid brandId,
            string currencyCode,
            PlayerType playerType,
            int betLimit,
            string prefix,
            int num)
        {
            const string pwd = "TgsP@ssw0rd";

            var walletId = GuidExtensions.NewSequentialGuid();
            var (salt, password) = _passwordHandler.GenerateCredential(pwd);
            var (withdrawSalt, withdrawPassword) = _passwordHandler.GenerateCredential(pwd);
            var userName = $"{prefix}{num:00}_{currencyCode.ToLower()}";
            var playerCode = PlayerCodeGenerator.NewId();

            return new PlayerCreateInfo(
                playerCode,
                userName,
                playerType,
                brandId,
                salt,
                password,
                withdrawSalt,
                withdrawPassword,
                betLimit,
                currencyCode,
                walletId);
        }
    }
}