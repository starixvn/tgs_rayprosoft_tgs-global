using System;

namespace Tgs.Global
{
    public record PlayerCreateInfo(
        string PlayerCode,
        string UserName,
        PlayerType Type,
        Guid BrandId,
        byte[] Salt,
        byte[] Password,
        byte[] WithdrawSalt,
        byte[] WithdrawPassword,
        int BetLimitId,
        string CurrencyCode,
        Guid WalletId);
}