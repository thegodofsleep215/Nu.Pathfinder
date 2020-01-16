﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu.OfficerMiniGame
{
    [Serializable]
    public class Propulsion
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public PropulsionType PropulsionType { get; set; }
        public int ShipSpeed { get; set;}
        public int PropulsionHitPoints { get; set; }
    }

    public enum PropulsionType
    {
        Sails,
        Oars
    }
}
