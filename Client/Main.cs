﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using ManagedAPI.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Main : BaseScript
    {
        public Main()
        {
            Tick += OnTick;
        }

        [Tick]
        private async Task OnTick()
        {
            //stfu until I find something to put here
            await new Task<bool>(() => true);
        }
    }
}
