using System;

namespace Tgs.Global
{
    public record PlayerInfo(
        string PlayerCode,
        string UserName,
        PlayerType PlayerType,
        Guid BrandId,
        byte[] Salt,
        byte[] Password,
        int BetLimitId,
        string CurrencyCode,
        Guid WalletId);
}