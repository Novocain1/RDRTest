using CitizenFX.Core;
using CitizenFX.Core.Native;
using ManagedAPI.Server;
using ManagedAPI.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
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
