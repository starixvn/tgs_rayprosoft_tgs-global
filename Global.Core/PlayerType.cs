using System;

namespace Tgs.Global
{
    [Flags]
    public enum PlayerType : byte
    {
        None = 0,
        RealPlayer = 1,
        TestPlayer = 2,
        All = RealPlayer | TestPlayer
    }
}