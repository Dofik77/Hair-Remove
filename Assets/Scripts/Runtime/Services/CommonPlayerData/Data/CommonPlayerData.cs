using System;
using System.Collections.Generic;
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Runtime.Services.CommonPlayerData.Data
{
    public class CommonPlayerData
    {
        public EScene Level;
        public int coins;

        public CommonPlayerData()
        {
            Level = EScene.Game0_1;
            coins = 0;
        }
    }
}