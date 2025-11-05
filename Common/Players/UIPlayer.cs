using Pokemod.Common.UI.MoveLearnUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace Pokemod.Common.Players
{
    internal class UIPlayer : ModPlayer
    {
        public UICatch MoveLearnUICatch;
        public struct UICatch(bool closure, MoveLearnUIState state)
        {
            public bool closure = closure;
            public MoveLearnUIState state = state;
        }

        public override void PreUpdate()
        {
            base.PreUpdate();
            if (MoveLearnUICatch.closure == true)
            {
                ModContent.GetInstance<MoveLearnUISystem>().CatchUIState(MoveLearnUICatch.state);
                MoveLearnUICatch.closure = false;
                MoveLearnUICatch.state = null;
            }
        }

        // Prevent Move Learn UI from closing when openning/closing inventory
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (ModContent.GetInstance<MoveLearnUISystem>().IsActive())
            {
                MoveLearnUIState UIstate = ModContent.GetInstance<MoveLearnUISystem>().MoveLearnUI;
                // Detect inventory key
                if (PlayerInput.Triggers.Current.Inventory)
                {
                    MoveLearnUICatch = new UICatch(true, UIstate);
                }
            }
            base.ProcessTriggers(triggersSet);
        }
    }
}
