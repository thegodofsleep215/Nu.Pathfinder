﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu.OfficerMiniGame
{
    public class BaseResponse
    {
        public bool Success { get; set; } = false;

        public List<string> Messages { get; private set; } = new List<string>();
    }
}
