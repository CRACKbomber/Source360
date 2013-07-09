using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Source360.Common
{
    /// <summary>
    /// Contains all of Valve's Xbox 360 Releases with their corresponding zip sizes and RTE names
    /// </summary>
    public enum GameNames
    {
        NoGame = 0,
        // Zip Creation names
        CounterStrike = 0x4ED3C036,
        HalfLife2 = 0x3C58F836,
        Left4DeadGOTYZ0 = 0x256B4036,
        Left4DeadGOTYZ1 = 0x27E1B036,
        Left4DeadOrgZ0 = 0x247DF036,
        Left4DeadOrgZ1 = 0x275AB036,
        Left4Dead2Z0 = 0x40BCA836,
        Left4Dead2Z1 = 0x2E6C8836,
        Portal = 0x4ED3C036,
        Portal2Z0 = 0xE487836,
        Portal2Z1 = 0x4D9CE036,
        TeamFortress = 0x107B6836,
        // RTE game Names
        Left4Dead,
        Left4Dead2,
        OrangeBox,
        CSGO,
        Portal2
    }
}
