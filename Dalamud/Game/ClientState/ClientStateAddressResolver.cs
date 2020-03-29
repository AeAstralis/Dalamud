using System;
using Dalamud.Game.Internal;

namespace Dalamud.Game.ClientState
{
    public sealed class ClientStateAddressResolver : BaseAddressResolver {
        // Static offsets
        public IntPtr ViewportActorTable { get; private set; }
        public IntPtr LocalContentId { get; private set; }
        public IntPtr JobGaugeData { get; private set; }
        public IntPtr KeyboardState { get; private set; }

        // Functions
        public IntPtr SetupTerritoryType { get; private set; }
        public IntPtr SomeActorTableAccess { get; private set; }
        
        protected override void Setup64Bit(SigScanner sig) {
            ViewportActorTable = sig.GetStaticAddressFromSig("48 8D 0D ?? ?? ?? ?? 85 ED", 0) + 0x148;
            SomeActorTableAccess = sig.ScanText("E8 ?? ?? ?? ?? 48 8D 55 A0 48 8D 8E ?? ?? ?? ??");

            LocalContentId = sig.GetStaticAddressFromSig("48 8B 05 ?? ?? ?? ?? 48 89 86 ?? ?? ?? ??", 0);
            JobGaugeData = sig.GetStaticAddressFromSig("E8 ?? ?? ?? ?? FF C6 48 8D 5B 0C", 0xB9) + 0x10;

            SetupTerritoryType = sig.ScanText("48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 48 8B F9 66 89 91 ?? ?? ?? ??");

            // This resolves to a fixed offset only, without the base address added in, so GetStaticAddressFromSig() can't be used
            KeyboardState = sig.ScanText("48 8D 0C 85 ?? ?? ?? ?? 8B 04 31 85 C2 0F 85") + 0x4;
        }
    }
}
