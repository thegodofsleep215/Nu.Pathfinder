﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu.OfficerMiniGame.Dal.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShipType
    {
        Barkentine,
        Barque,
        Brig,
        Corvette,
        Cutter,
        Drakkar,
        Dromon,
        Frigate,
        Galley,
        Galleass,
        GrandFrigate,
        GreatKetch,
        Ketch,
        Junk,
        LargeSchooner,
        ManOfWar,
        MerchantShip,
        MerchantGalleon,
        Pinnace,
        PrisonShip,
        Quinquereme,
        Schooner,
        Scow,
        SlaveShip,
        Sloop,
        SloopOfWar,
        TurtleShip,
        Warship,
        WarGalley,
        WarGalleon,
        Yawl,
        Carrack,
        Caravel
    }
}
